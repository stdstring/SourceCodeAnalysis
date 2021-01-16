using System.Collections.Generic;
using SourceCheckUtil.Utils;

namespace SourceCheckUtil.Analyzers
{
    internal static class AnalyzersFactory
    {
        public static IList<IFileAnalyzer> Create(OutputImpl output)
        {
            return new IFileAnalyzer[]
            {
                new BadFilenameCaseAnalyzer(output),
                new CastToSameTypeAnalyzer(output),
                new NonAsciiIdentifiersAnalyzer(output)
            };
        }
    }
}