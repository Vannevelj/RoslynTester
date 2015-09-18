using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace Tests.SampleAnalyzer_FixIntroducesNewDiagnostic
{
    [ExportCodeFixProvider("SampleAnalyzer_FixIntroducesNewDiagnostic", LanguageNames.CSharp), Shared]
    public class SampleAnalyzerFixIntroducesNewDiagnosticCodeFix : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(SampleAnalyzerFixIntroducesNewDiagnosticAnalyzer.Rule.Id);

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var statement = root.FindNode(diagnosticSpan);
            context.RegisterCodeFix(CodeAction.Create("Use alias", x => AsToCastAsync(context.Document, root, statement), nameof(SampleAnalyzerFixIntroducesNewDiagnosticAnalyzer)), diagnostic);
        }

        private async Task<Solution> AsToCastAsync(Document document, SyntaxNode root, SyntaxNode statement)
        {
            var semanticModel = await document.GetSemanticModelAsync();
            var typeName = semanticModel.GetSymbolInfo((IdentifierNameSyntax)statement).Symbol.MetadataName;
            
            var aliasToken =
                MapConcreteTypeToPredefinedTypeAlias.First(
                    kvp => kvp.Key == typeName).Value;

            var newExpression = SyntaxFactory.PredefinedType(SyntaxFactory.Token(aliasToken)).WithAdditionalAnnotations(Formatter.Annotation);
            var newRoot = root.ReplaceNode(statement, newExpression);
            var newDocument = document.WithSyntaxRoot(newRoot);

            return newDocument.Project.Solution;
        }

        public static readonly Dictionary<string, SyntaxKind> MapConcreteTypeToPredefinedTypeAlias =
            new Dictionary<string, SyntaxKind>
            {
                {"Int16", SyntaxKind.ShortKeyword},
                {"Int32", SyntaxKind.IntKeyword},
                {"Int64", SyntaxKind.LongKeyword},
                {"UInt16", SyntaxKind.UShortKeyword},
                {"UInt32", SyntaxKind.UIntKeyword},
                {"UInt64", SyntaxKind.ULongKeyword},
                {"Object", SyntaxKind.ObjectKeyword},
                {"Byte", SyntaxKind.ByteKeyword},
                {"SByte", SyntaxKind.SByteKeyword},
                {"Char", SyntaxKind.CharKeyword},
                {"Boolean", SyntaxKind.BoolKeyword},
                {"Single", SyntaxKind.FloatKeyword},
                {"Double", SyntaxKind.DoubleKeyword},
                {"Decimal", SyntaxKind.DecimalKeyword},
                {"String", SyntaxKind.StringKeyword}
            };
    }
}