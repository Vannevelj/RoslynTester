using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynTester.Helpers.CSharp;
using RoslynTester.Helpers.Testing;
using Tests.SampleAnalyzer;
using Tests.SampleAnalyzerWithAssemblyReference;
using Tests.SampleAnalyzerWithErrorSeverity;
using Tests.SampleAnalyzer_VBAndCSharp;

namespace Tests.Tests
{
    [TestClass]
    public class SampleAnalyzerWithAssemblyReferenceTests : CSharpCodeFixVerifier
    {
        protected override DiagnosticAnalyzer DiagnosticAnalyzer => new TestAnalyzerWithAssemblyReference();

        protected override CodeFixProvider CodeFixProvider => new TestCodeFixWithAssemblyReference();

        private void TestCodeFix()
        {
            var original =
@"using System;
namespace ConsoleApplication1
{
public class Program {
    public static void Main() {
        var dt = DateTime.Now.Add(TimeSpan.FromMinutes(10));
    }
}
}";

            var result =
@"using System;
using Tests;

namespace ConsoleApplication1
{
public class Program {
    public static void Main() {
        var dt = DateTime.Now.AddPlusOneHour(TimeSpan.FromMinutes(10));
    }
}
}";

            VerifyDiagnostic(original, TestAnalyzerWithAssemblyReference.Rule.MessageFormat.ToString());
            VerifyFix(original, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCodeException))]
        public void SampleAnalyzer_FailsOnMissingAssemblyReference()
        {
            TestCodeFix();
        }

        [TestMethod]        
        public void SampleAnalyzer_SucceedsOnCorrectAssemblyReference()
        {
            Configure.WithAssemblyReference(Assembly.GetExecutingAssembly()); //Add the assembly containing the code required to apply the code fix

            TestCodeFix();
        }
    }
}
