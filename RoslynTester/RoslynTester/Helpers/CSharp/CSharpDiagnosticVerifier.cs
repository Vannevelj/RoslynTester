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
    }
}