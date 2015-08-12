using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynTester.Helpers.CSharp;
using Tests.SampleAnalyzer_FixIntroducesNewDiagnostic;

namespace Tests.Tests
{
    [TestClass]
    public class SampleAnalyzerFixIntroducesNewDiagnosticTests : CSharpCodeFixVerifier
    {
        protected override CodeFixProvider CodeFixProvider => new SampleAnalyzerFixIntroducesNewDiagnosticCodeFix();

        protected override DiagnosticAnalyzer DiagnosticAnalyzer => new SampleAnalyzerFixIntroducesNewDiagnosticAnalyzer();

        [TestMethod]
        public void Analyzer_FixIntroducesNewDiagnostic()
        {
            var original = @"
using System;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
            Int16 i16 = 9;
        }
    }
}";

            var result = @"
using System;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
            short i16 = 9;
        }
    }
}";
            VerifyDiagnostic(original, string.Format(SampleAnalyzerFixIntroducesNewDiagnosticAnalyzer.Rule.MessageFormat.ToString(), "short", "Int16"));
            VerifyFix(original, result, allowedNewCompilerDiagnosticsId: "CS8019");
        }
    }
}