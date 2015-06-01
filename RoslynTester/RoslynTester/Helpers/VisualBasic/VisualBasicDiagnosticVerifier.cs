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
    }
}