using System.Collections.Generic;
using System.IO;

namespace SourceCheckUtil.Analyzers
{
    internal static class AnalyzersFactory
    {
        public static IList<IFileAnalyzer> Create(TextWriter output)
        {
            return new List<IFileAnalyzer>();
        }
    }
}