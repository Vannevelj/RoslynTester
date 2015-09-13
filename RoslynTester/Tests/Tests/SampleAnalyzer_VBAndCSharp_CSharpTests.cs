using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynTester.Helpers.CSharp;
using Tests.SampleAnalyzer_VBAndCSharp;

namespace Tests.Tests
{
    [TestClass]
    public class SampleAnalyzer_VBAndCSharp_CSharpTests : CSharpCodeFixVerifier
    {
        protected override DiagnosticAnalyzer DiagnosticAnalyzer => new SampleAnalyzer_VBAndCSharpAnalyzer();

        protected override CodeFixProvider CodeFixProvider => new SampleAnalyzer_VBAndCSharpCodeFix();

        [TestMethod]
        public void SampleAnalyzer_AddsFlagsAttribute()
        {
            var original = 
@"namespace ConsoleApplication1
{
    enum Foo
    {
    }
}";

            var result = 
@"using System;

namespace ConsoleApplication1
{
    [Flags]
    enum Foo
    {
    }
}";

            VerifyDiagnostic(original, SampleAnalyzer_VBAndCSharpAnalyzer.Rule.MessageFormat.ToString());
            VerifyFix(original, result);
        }

        [TestMethod]
        public void SampleAnalyzer_AddsFlagsAttribute_DoesNotAddDuplicateUsingSystem()
        {
            var original =
@"using System;

namespace ConsoleApplication1
{
    enum Foo
    {
    }
}";

            var result =
@"using System;

namespace ConsoleApplication1
{
    [Flags]
    enum Foo
    {
    }
}";

            VerifyDiagnostic(original, SampleAnalyzer_VBAndCSharpAnalyzer.Rule.MessageFormat.ToString());
            VerifyFix(original, result);
        }

        [TestMethod]
        public void SampleAnalyzer_AddsFlagsAttribute_OnlyAddsFlagsAttribute()
        {
            var original =
@"using System;

namespace ConsoleApplication1
{
    [Obsolete(""I'm obsolete"")]
    enum Foo
    {
        Goo = 0,
        Hoo,
        Joo,
        Koo,
        Loo
    }
}";

            var result =
@"using System;

namespace ConsoleApplication1
{
    [Obsolete(""I'm obsolete"")]
    [Flags]
    enum Foo
    {
        Goo = 0,
        Hoo,
        Joo,
        Koo,
        Loo
    }
}";

            VerifyDiagnostic(original, SampleAnalyzer_VBAndCSharpAnalyzer.Rule.MessageFormat.ToString());
            VerifyFix(original, result);
        }

        [TestMethod]
        public void SampleAnalyzer_EnumHasXmlDocComment_OnlyAddsFlagsAttribute()
        {
            var original =
@"namespace ConsoleApplication1
{
    /// <summary>
    /// Doc comment for Foo...
    /// </summary>
    enum Foo
    {
        Goo = 0,
        Hoo,
        Joo,
        Koo,
        Loo
    }
}";

            var result =
@"using System;

namespace ConsoleApplication1
{
    /// <summary>
    /// Doc comment for Foo...
    /// </summary>
    [Flags]
    enum Foo
    {
        Goo = 0,
        Hoo,
        Joo,
        Koo,
        Loo
    }
}";

            VerifyDiagnostic(original, SampleAnalyzer_VBAndCSharpAnalyzer.Rule.MessageFormat.ToString());
            VerifyFix(original, result);
        }

        [TestMethod]
        public void SampleAnalyzer_AddsFlagsAttribute_AddsUsingSystemWhenUsingSystemDotAnything()
        {
            var original =
@"using System.Text;

namespace ConsoleApplication1
{
    enum Foo
    {
    }
}";

            var result =
@"using System.Text;
using System;

namespace ConsoleApplication1
{
    [Flags]
    enum Foo
    {
    }
}";

            VerifyDiagnostic(original, SampleAnalyzer_VBAndCSharpAnalyzer.Rule.MessageFormat.ToString());
            VerifyFix(original, result);
        }

        [TestMethod]
        public void SampleAnalyzer_InspectionDoesNotReturnWhenFlagsAlreadyApplied()
        {
            var original = @"
using System;

namespace ConsoleApplication1
{
    [Flags]
    enum Foo
    {
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void SampleAnalyzer_InspectionDoesNotReturnWhenFlagsAttributeAlreadyApplied()
        {
            var original = @"
using System;

namespace ConsoleApplication1
{
    [FlagsAttribute]
    enum Foo
    {
    }
}";

            VerifyDiagnostic(original);
        }

        [TestMethod]
        public void SampleAnalyzer_InspectionDoesNotReturnWhenFlagsAlreadyAppliedAsChain()
        {
            var original = @"
using System;

namespace ConsoleApplication1
{
    [Obsolete(""I'm obsolete""), Flags]
    enum Foo
    {
    }
}";

            VerifyDiagnostic(original);
        }
    }
}