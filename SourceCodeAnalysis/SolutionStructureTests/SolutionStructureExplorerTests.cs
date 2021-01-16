using System;
using System.IO;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using NUnit.Framework;

namespace SolutionStructureTests
{
    [TestFixture]
    public class SolutionStructureExplorerTests
    {
        public SolutionStructureExplorerTests()
        {
            MSBuildLocator.RegisterDefaults();
        }

        [Test]
        public void ShowThisSolutionStructure()
        {
            const String solutionRelativePath = "..//..//..//SourceCodeAnalysis.sln";
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution solution = workspace.OpenSolutionAsync(solutionRelativePath).Result;
            foreach (ProjectId projectId in solution.GetProjectDependencyGraph().GetTopologicallySortedProjects())
            {
                Project project = solution.GetProject(projectId);
                String projectRoot = Path.GetDirectoryName(project.FilePath) + "\\";
                Console.WriteLine("project : {0}, path : {1}", project.Name, project.FilePath);
                foreach (Document document in project.Documents)
                    Console.WriteLine("    file : {0}", document.FilePath.Substring(projectRoot.Length));
                Console.WriteLine();
            }
        }
    }
}
