using Microsoft.CodeAnalysis;

namespace RoslynTester.Helpers.VisualBasic
{
    public abstract class VisualBasicDiagnosticVerifier : DiagnosticVerifier
    {
        public VisualBasicDiagnosticVerifier() : base(LanguageNames.VisualBasic)
        {
        }
    }
}