using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Rename;

namespace Tests.SampleAnalyzerWithAssemblyReference
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(TestCodeFixWithAssemblyReference)), Shared]
    public class TestCodeFixWithAssemblyReference : CodeFixProvider
    {
        private const string title = "Replace with AddPlusOneHour";

        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(TestAnalyzerWithAssemblyReference.DiagnosticId);

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var declaration = root.FindToken(diagnosticSpan.Start).Parent
                .AncestorsAndSelf()
                .OfType<InvocationExpressionSyntax>()
                .FirstOrDefault(e => ((MemberAccessExpressionSyntax)e.Expression).Name.ToString() == "Add");

            if (declaration != null)
            {
                context.RegisterCodeFix(CodeAction.Create(title, c => FixDateTimeOffsetAddAsync(context.Document, declaration, c), title), diagnostic);
            }
        }

        private async Task<Document> FixDateTimeOffsetAddAsync(Document document, InvocationExpressionSyntax invocationExpr, CancellationToken cancellationToken)
        {
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var memberAccessExpr = invocationExpr.Expression as MemberAccessExpressionSyntax;
            var memberSymbol = semanticModel.GetSymbolInfo(memberAccessExpr).Symbol as IMethodSymbol;
            var argumentList = invocationExpr.ArgumentList as ArgumentListSyntax;

            var root = await document.GetSyntaxRootAsync();
            var incrementMethod = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, memberAccessExpr.Expression, SyntaxFactory.IdentifierName("AddPlusOneHour"))
                .WithLeadingTrivia(invocationExpr.GetLeadingTrivia())
                .WithTrailingTrivia(invocationExpr.GetTrailingTrivia())
                .WithAdditionalAnnotations(Formatter.Annotation);
            var newRoot = root.ReplaceNode(invocationExpr, SyntaxFactory.InvocationExpression(incrementMethod, argumentList));


            var extensionsNamespace = "Tests";
            var usingStatement = String.Format("using {0};", extensionsNamespace);

            CompilationUnitSyntax compilationUnit = (CompilationUnitSyntax)newRoot;
            var extensionsUsing = SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName(extensionsNamespace));

            if (compilationUnit.Usings.All(x => x.ToString() != usingStatement))
            {
                newRoot = compilationUnit.AddUsings(extensionsUsing);
            }

            var newDocument = document.WithSyntaxRoot(newRoot);
            return newDocument;
        }
    }
}
