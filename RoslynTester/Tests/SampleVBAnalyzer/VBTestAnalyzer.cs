using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace Tests.SampleVBAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.VisualBasic)]
    public class VBTestAnalyzer : DiagnosticAnalyzer
    {
        private const string DiagnosticId = nameof(VBTestAnalyzer);
        private const DiagnosticSeverity Severity = DiagnosticSeverity.Warning;

        private static readonly string Category = "Attributes";
        private static readonly string Message = "TestMessage";
        private static readonly string Title = "TestTitle";

        internal static DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, Severity, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeVisualBasicSymbol, SyntaxKind.Attribute);
        }

        private void AnalyzeVisualBasicSymbol(SyntaxNodeAnalysisContext context)
        {
            var attributeExpression = context.Node as AttributeSyntax;

            // attribute must have arguments
            // if there are no parenthesis, the ArgumentList is null
            // if there are empty parenthesis, the ArgumentList is empty
            if (attributeExpression?.ArgumentList == null || attributeExpression.ArgumentList.Arguments.Any())
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, attributeExpression.GetLocation()));
        }
    }
}