using System;
using Microsoft.VisualStudio.Shell;

namespace SourceCodeAnalysisVSExtensionDev.Utils
{
    internal class ProjectData
    {
        public ProjectData(EnvDTE.Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Name = project.Name;
            FileName = project.FileName;
            LanguageId = project.CodeModel?.Language;
        }

        public String Name { get; }

        public String FileName { get; }

        public String LanguageId { get; }
    }
}