using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Tests.SampleAnalyzerWithAssemblyReference
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TestAnalyzerWithAssemblyReference : DiagnosticAnalyzer
    {
        public const string DiagnosticId = nameof(SampleAnalyzerWithErrorSeverity.TestAnalyzerWithErrorSeverity);
        internal const string Category = "DateTime";
        internal const string Message = "Don't use the Add method.";
        internal const DiagnosticSeverity Severity = DiagnosticSeverity.Error;
        internal const string Title = "Verifies that the AddPlusOneHour method is used.";
        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, Severity, isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.InvocationExpression);
        }

        private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var invocationExpr = (InvocationExpressionSyntax)context.Node;
            var memberAccessExpr = invocationExpr.Expression as MemberAccessExpressionSyntax;
            if (memberAccessExpr?.Name.ToString() != "Add")
            {
                return;
            }
                
            var memberSymbol = context.SemanticModel.GetSymbolInfo(memberAccessExpr).Symbol as IMethodSymbol;

            if (memberSymbol == null)
            {
                return;
            }

            if (!memberSymbol.ToString().StartsWith("System.DateTime.Add"))
            {
                return;
            }

            var diagnostic = Diagnostic.Create(Rule, memberAccessExpr.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }
    }
}
