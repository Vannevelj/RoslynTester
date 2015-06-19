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
        public void Analyzer_With_LocationDiagnostic()
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
        public void Analyzer_With_MessageDiagnostic()
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

            VerifyDiagnostic(original, string.Format(TestAnalyzer.Message, "Method"));
            VerifyFix(original, result);
        }

        [TestMethod]
        public void Analyzer_Without_Diagnostic()
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
    }
}