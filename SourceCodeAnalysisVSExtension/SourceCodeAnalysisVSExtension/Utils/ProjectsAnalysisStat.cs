using System;

namespace SourceCodeAnalysisVSExtension.Utils
{
    internal class ProjectsAnalysisStat
    {
        public ProjectsAnalysisStat(String projectNames, Int32 totalCount)
        {
            ProjectNames = projectNames;
            TotalCount = totalCount;
            SuccessCount = 0;
            FailedCount = 0;
            SkippedCount = 0;
        }

        public String ProjectNames { get; }

        public Int32 TotalCount { get; }

        public Int32 SuccessCount { get; set; }

        public Int32 FailedCount { get; set; }

        public Int32 SkippedCount { get; set; }

        public String TargetType => TotalCount > 1 ? "projects" : "project";
    }
}