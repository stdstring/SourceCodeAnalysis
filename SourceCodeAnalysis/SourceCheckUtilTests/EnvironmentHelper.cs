using System;
using System.IO;
using System.Reflection;
using SourceCheckUtil.Config;

namespace SourceCheckUtilTests
{
    internal static class EnvironmentHelper
    {
        public static String GetContainedDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? String.Empty;
        }

        public static void RemoveAuxiliaryEntities()
        {
            RemoveDefaultConfig();
        }

        public static void RemoveDefaultConfig()
        {
            File.Delete(Path.Combine(GetContainedDirectory(), PorterConfigPathProvider.DefaultConfigName));
        }

        public static void CreateDefaultConfig(String innerContent)
        {
            String configPath = Path.Combine(GetContainedDirectory(), PorterConfigPathProvider.DefaultConfigName);
            String configContent = $"<?xml version=\"1.0\" encoding=\"utf-8\" ?><porter>{innerContent}</porter>";
            File.WriteAllText(configPath, configContent);
        }
    }
}
