using System.Collections.Generic;
using System.IO;

namespace SourceCheckUtil.Analyzers
{
    internal static class AnalyzersFactory
    {
        public static IList<IFileAnalyzer> Create(TextWriter output)
        {
            return new IFileAnalyzer[]
            {
                new BadFilenameCaseAnalyzer(output),
                new CastToSameTypeAnalyzer(output),
                new NonAsciiIdentifiersAnalyzer(output),
                new VirtualInheritanceAnalyzer(output)
            };
        }
    }
}