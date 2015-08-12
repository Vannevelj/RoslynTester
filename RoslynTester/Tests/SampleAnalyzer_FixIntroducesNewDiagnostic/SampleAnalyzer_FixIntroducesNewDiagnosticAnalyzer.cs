using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Tests.SampleAnalyzer_FixIntroducesNewDiagnostic
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SampleAnalyzerFixIntroducesNewDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        private const string DiagnosticId = nameof(SampleAnalyzerFixIntroducesNewDiagnosticAnalyzer);
        private const DiagnosticSeverity Severity = DiagnosticSeverity.Warning;

        private const string Category = "General";
        private const string Message = "Use alias {0} instead of concrete type {1}.";
        private const string Title = "Use alias instead of concrete type.";

        internal static DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, Severity, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSymbol, SyntaxKind.VariableDeclaration);
        }

        private void AnalyzeSymbol(SyntaxNodeAnalysisContext context)
        {
            var expression = (VariableDeclarationSyntax)context.Node;
            var typeExpression = expression.Type;

            if (!(typeExpression is IdentifierNameSyntax))
            {
                return;
            }

            if (typeExpression.IsVar)
            {
                return;
            }

            var symbol = context.SemanticModel.GetSymbolInfo(typeExpression).Symbol;
            var typeName = symbol.MetadataName;
            var namespaceName = symbol.ContainingNamespace.Name;

            if (namespaceName != "System")
            {
                return;
            }

            if (MapConcreteTypeToPredefinedTypeAlias.ContainsKey(typeName))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, typeExpression.GetLocation(),
                    MapConcreteTypeToPredefinedTypeAlias.First(kvp => kvp.Key == typeName).Value, typeName));
            }
        }

        public static readonly Dictionary<string, string> MapConcreteTypeToPredefinedTypeAlias =
            new Dictionary<string, string>
            {
                {"Int16", "short"},
                {"Int32", "int"},
                {"Int64", "long"},
                {"UInt16", "ushort"},
                {"UInt32", "uint"},
                {"UInt64", "ulong"},
                {"Object", "object"},
                {"Byte", "byte"},
                {"SByte", "sbyte"},
                {"Char", "char"},
                {"Boolean", "bool"},
                {"Single", "float"},
                {"Double", "double"},
                {"Decimal", "decimal"},
                {"String", "string"}
            };
    }
}