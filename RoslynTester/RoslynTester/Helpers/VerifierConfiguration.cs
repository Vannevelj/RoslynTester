using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace RoslynTester.Helpers
{
    public interface IConfigureVerifier
    {
        IConfigureVerifier WithAssemblyReference(Assembly assembly);
    }

    public class VerifierConfiguration : IConfigureVerifier
    {
        private static readonly MetadataReference CodeAnalysisReference = MetadataReference.CreateFromFile(typeof(Compilation).Assembly.Location);
        private static readonly MetadataReference CorlibReference = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
        private static readonly MetadataReference CSharpSymbolsReference = MetadataReference.CreateFromFile(typeof(CSharpCompilation).Assembly.Location);
        private static readonly MetadataReference VisualBasicSymbolsReference = MetadataReference.CreateFromFile(typeof(VisualBasicCompilation).Assembly.Location);
        private static readonly MetadataReference VisualBasicStandardModuleAttributeReference = MetadataReference.CreateFromFile(typeof(StandardModuleAttribute).Assembly.Location);
        private static readonly MetadataReference SystemCoreReference = MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location);

        public IEnumerable<MetadataReference> AssemblyReferences { get; private set; }

        public string LanguageName { get; private set; }

        public VerifierConfiguration(string language = LanguageNames.CSharp)
        {
            LanguageName = language;

            if (language == LanguageNames.CSharp)
            {
                AssemblyReferences = new[]
                {
                    CorlibReference,
                    SystemCoreReference,
                    CSharpSymbolsReference,
                    CodeAnalysisReference
                };
            }
            else
            {
                AssemblyReferences = new[]
                {
                    CorlibReference,
                    SystemCoreReference,
                    VisualBasicSymbolsReference,
                    VisualBasicStandardModuleAttributeReference,
                    CodeAnalysisReference
                };
            }
        }

        public IConfigureVerifier WithAssemblyReference(Assembly assembly)
        {
            AssemblyReferences = AssemblyReferences.Union(new[] {MetadataReference.CreateFromFile(assembly.Location)}).ToList();
            return this;
        }
    }
}
