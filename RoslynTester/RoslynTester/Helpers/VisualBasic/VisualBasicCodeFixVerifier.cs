﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;

namespace RoslynTester.Helpers.VisualBasic
{
    public abstract class VisualBasicCodeFixVerifier : VisualBasicDiagnosticVerifier
    {
        private readonly CodeFixVerifier _codeFixVerifier = new CodeFixVerifier();

        /// <summary>
        ///     Returns the codefix being tested - to be implemented in non-abstract class
        /// </summary>
        /// <returns>The CodeFixProvider to be used</returns>
        protected abstract CodeFixProvider CodeFixProvider { get; }

        /// <summary>
        ///     Called to test a VB codefix when applied on the inputted string as a source
        /// </summary>
        /// <param name="oldSource">A class in the form of a string before the CodeFix was applied to it</param>
        /// <param name="newSource">A class in the form of a string after the CodeFix was applied to it</param>
        /// <param name="codeFixIndex">Index determining which codefix to apply if there are multiple</param>
        /// <param name="allowNewCompilerDiagnostics">
        ///     A bool controlling whether or not the test will fail if the CodeFix
        ///     introduces other warnings after being applied
        /// </param>
        protected void VerifyFix(string oldSource, string newSource, int? codeFixIndex = null, bool allowNewCompilerDiagnostics = false)
        {
            _codeFixVerifier.VerifyFix(CodeFixProvider, DiagnosticAnalyzer, LanguageNames.VisualBasic, oldSource, newSource, codeFixIndex, allowNewCompilerDiagnostics);
        }

        protected void VerifyFix(string oldSource, string newSource, int? codeFixIndex = null,
            params string[] allowedNewCompilerDiagnosticsId)
        {
            _codeFixVerifier.VerifyFix(CodeFixProvider, DiagnosticAnalyzer, LanguageNames.VisualBasic, oldSource, newSource, codeFixIndex, allowedNewCompilerDiagnosticsId);
        }
    }
}