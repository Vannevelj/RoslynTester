using Microsoft.CodeAnalysis;
using RoslynTester.DiagnosticResults;

namespace RoslynTester.Helpers.CSharp
{
    public abstract class CSharpDiagnosticVerifier : DiagnosticVerifier
    {
        protected override void VerifyDiagnostic(string[] sources, params DiagnosticResult[] expected)
        {
            VerifyDiagnostics(sources, LanguageNames.CSharp, expected);
        }

        protected override void VerifyDiagnostic(string source, params DiagnosticResult[] expected)
        {
            VerifyDiagnostics(new[] { source }, LanguageNames.CSharp, expected);
        }
    }
}