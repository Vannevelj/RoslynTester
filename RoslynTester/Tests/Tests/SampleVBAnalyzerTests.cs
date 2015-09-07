using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynTester.Helpers.VisualBasic;
using Tests.SampleVBAnalyzer;

namespace Tests.Tests
{
    [TestClass]
    public class AttributeWithEmptyArgumentListTests : VisualBasicCodeFixVerifier
    {
        protected override DiagnosticAnalyzer DiagnosticAnalyzer => new VBTestAnalyzer();

        protected override CodeFixProvider CodeFixProvider => new VBTestCodeFix();

        [TestMethod]
        public void Analyzer_WithVisualBasicCode_WithCodeFix()
        {
            var original = @"
Module Module1

    <Obsolete()>
    Sub Foo()

    End Sub

End Module";

            var result = @"
Module Module1

    <Obsolete>
    Sub Foo()

    End Sub

End Module";

            VerifyDiagnostic(original, VBTestAnalyzer.Rule.MessageFormat.ToString());
            VerifyFix(original, result);
        }

        [TestMethod]
        public void Analyzer_WithVisualBasicCode_WithDiagnostic()
        {
            var original = @"
Module Module1

    <Obsolete>
    Sub Foo()

    End Sub

End Module";

            VerifyDiagnostic(original);
        }
    }
}