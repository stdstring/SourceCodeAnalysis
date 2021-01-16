using System;
using System.Collections.Generic;
using SourceCheckUtil.Analyzers;

namespace SourceCheckUtil.Processors
{
    public interface ISourceProcessor
    {
        Boolean Process(IList<IFileAnalyzer> analyzers);
    }
}