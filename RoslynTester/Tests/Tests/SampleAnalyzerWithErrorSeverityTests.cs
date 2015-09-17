using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynTester.Helpers.CSharp;
using Tests.SampleAnalyzerWithErrorSeverity;

namespace Tests.Tests
{
    [TestClass]
    public class SampleAnalyzerWithErrorSeverityTests : CSharpCodeFixVerifier
    {
        protected override CodeFixProvider CodeFixProvider => new TestCodeFixWithErrorSeverity();

        protected override DiagnosticAnalyzer DiagnosticAnalyzer => new TestAnalyzerWithErrorSeverity();

        [TestMethod]
        public void Analyzer_With_MessageDiagnostic_DoesNotThrowUncompilable()
        {
            var original = @"
    using System;
    using System.Text;
    using System.Threading.Tasks;

    namespace ConsoleApplication1
    {
        class MyClass
        {   
            static void Main() {}

            async Task Method()
            {
                
            }
        }
    }";

            var result = @"
    using System;
    using System.Text;
    using System.Threading.Tasks;

    namespace ConsoleApplication1
    {
        class MyClass
        {   
            static void Main() {}

            async Task MethodAsync()
            {
                
            }
        }
    }";

            VerifyDiagnostic(original, string.Format(TestAnalyzerWithErrorSeverity.Message, "Method"));
            VerifyFix(original, result);
        }
    }
}