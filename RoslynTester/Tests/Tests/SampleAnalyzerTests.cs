using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynTester.DiagnosticResults;
using RoslynTester.Helpers.CSharp;
using Tests.SampleAnalyzer;

namespace Tests.Tests
{
    [TestClass]
    public class SampleAnalyzerTests : CSharpCodeFixVerifier
    {
        protected override CodeFixProvider CodeFixProvider => new TestCodeFix();
        protected override DiagnosticAnalyzer DiagnosticAnalyzer => new TestAnalyzer();

        [TestMethod]
        public void AsyncMethodWithoutAsyncSuffixAnalyzer_WithAsyncKeywordAndNoSuffix_InvokesWarning()
        {
            var original = @"
    using System;
    using System.Text;
    using System.Threading.Tasks;

    namespace ConsoleApplication1
    {
        class MyClass
        {   
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
            async Task MethodAsync()
            {
                
            }
        }
    }";

            var expectedDiagnostic = new DiagnosticResult
            {
                Id = TestAnalyzer.DiagnosticId,
                Message = string.Format(TestAnalyzer.Message, "Method"),
                Severity = TestAnalyzer.Severity,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 10, 24)
                    }
            };

            VerifyDiagnostic(original, expectedDiagnostic);
            VerifyFix(original, result);
        }

        [TestMethod]
        public void AsyncMethodWithoutAsyncSuffixAnalyzer_WithAsyncKeywordAndSuffix_DoesNotDisplayWarning()
        {
            var original = @"
    using System;
    using System.Text;
    using System.Threading.Tasks;

    namespace ConsoleApplication1
    {
        class MyClass
        {   
            async Task MethodAsync()
            {
                
            }
        }
    }";
            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void AsyncMethodWithoutAsyncSuffixAnalyzer_WithoutAsyncKeywordAndSuffix_DoesNotDisplayWarning()
        {
            var original = @"
    using System;
    using System.Text;
    using System.Threading.Tasks;

    namespace ConsoleApplication1
    {
        class MyClass
        {   
            void Method()
            {
                
            }
        }
    }";
            VerifyDiagnostic(original);
        }
    }
}
