using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using CSharpSyntaxKind = Microsoft.CodeAnalysis.CSharp.SyntaxKind;
using VisualBasicSyntaxKind = Microsoft.CodeAnalysis.VisualBasic.SyntaxKind;

namespace Tests.SampleAnalyzer_VBAndCSharp
{
    [DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    public class SampleAnalyzer_VBAndCSharpAnalyzer : DiagnosticAnalyzer
    {
        private const string DiagnosticId = nameof(SampleAnalyzer_VBAndCSharpAnalyzer);
        private const DiagnosticSeverity Severity = DiagnosticSeverity.Hidden;

        private static readonly string Category = "";
        private static readonly string Message = "";
        private static readonly string Title = "";

        internal static DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, Severity, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeCSharpSymbol, CSharpSyntaxKind.EnumDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeVisualBasicSymbol, VisualBasicSyntaxKind.EnumStatement);
        }

        private void AnalyzeCSharpSymbol(SyntaxNodeAnalysisContext context)
        {
            var enumDeclaration = (EnumDeclarationSyntax)context.Node;

            // enum must not already have flags attribute
            if (enumDeclaration.AttributeLists.Any(
                a => a.Attributes.Any(
                    t => {
                            var symbol = context.SemanticModel.GetSymbolInfo(t).Symbol;

                            return symbol == null || symbol.ContainingType.MetadataName == typeof(FlagsAttribute).Name;
                        })))
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, enumDeclaration.GetLocation()));
        }

        private void AnalyzeVisualBasicSymbol(SyntaxNodeAnalysisContext context)
        {
            var enumDeclaration = (EnumStatementSyntax)context.Node;

            // enum must not already have flags attribute
            if (enumDeclaration.AttributeLists.Any(
                a => a.Attributes.Any(
                    t => {
                        var symbol = context.SemanticModel.GetSymbolInfo(t).Symbol;

                        return symbol == null || symbol.ContainingType.MetadataName == typeof(FlagsAttribute).Name;
                    })))
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, enumDeclaration.GetLocation()));
        }
    }
}