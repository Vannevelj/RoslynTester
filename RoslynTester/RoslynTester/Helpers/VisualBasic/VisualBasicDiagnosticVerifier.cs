using Microsoft.CodeAnalysis;
using RoslynTester.DiagnosticResults;

namespace RoslynTester.Helpers.VisualBasic
{
    public abstract class VisualBasicDiagnosticVerifier : DiagnosticVerifier
    {
        protected override void VerifyDiagnostic(string[] sources, params DiagnosticResult[] expected)
        {
            VerifyDiagnostics(sources, LanguageNames.VisualBasic, expected);
        }

        protected override void VerifyDiagnostic(string source, params DiagnosticResult[] expected)
        {
            VerifyDiagnostics(new[] { source }, LanguageNames.VisualBasic, expected);
        }
    }
}