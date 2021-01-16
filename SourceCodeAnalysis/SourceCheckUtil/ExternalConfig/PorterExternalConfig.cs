using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SourceCheckUtil.ExternalConfig
{
    internal class PorterExternalConfig : IExternalConfig
    {
        public PorterExternalConfig(String config)
        {
            if (!CheckConfig(config))
                throw new FileNotFoundException(config);
            _configFilename = IsFile(config) ? config : Path.Combine(config, DefaultConfigName);
        }

        public ExternalConfigData LoadDefault()
        {
            XDocument document = XDocument.Load(_configFilename);
            return LoadImpl(document.Root, _configFilename);
        }

        public ExternalConfigData Load(String projectName)
        {
            String projectConfig = Path.Combine(Path.GetDirectoryName(_configFilename), String.Concat(projectName, ConfigExtension));
            if (!IsFile(projectConfig))
                return LoadDefault();
            XDocument document = XDocument.Load(projectConfig);
            return LoadImpl(document.Root, projectConfig);
        }

        public static Boolean CheckConfig(String config)
        {
            if (String.IsNullOrEmpty(config))
                throw new ArgumentNullException(nameof(config));
            if (IsFile(config))
                return true;
            if (IsDirectory(config) && IsFile(Path.Combine(config, DefaultConfigName)))
                return true;
            return false;
        }

        // TODO (std_string) : think about non-recursive version of this impl
        private ExternalConfigData LoadImpl(String importName, String parentConfig)
        {
            importName = importName.Replace("/", "\\");
            // TODO (std_string) : we must know how to process case when importName will be relative to porter directory (if 'use_porter_home_directory_while_resolving_path' option is enabled)
            String importConfigName = Path.IsPathRooted(importName) ? importName : Path.Combine(Path.GetDirectoryName(parentConfig), importName);
            XDocument document = XDocument.Load(importConfigName);
            return LoadImpl(document.Root, importConfigName);
        }

        // TODO (std_string) : think about non-recursive version of this impl
        private ExternalConfigData LoadImpl(XElement root, String currentConfig)
        {
            IList<AttributeData> attributes = PorterExternalConfigHelper.GetAttributes(root);
            IList<FileProcessingData> fileProcessing = PorterExternalConfigHelper.GetFileProcessing(root);
            ExternalConfigData[] configs = PorterExternalConfigHelper.GetImports(root).Select(import => LoadImpl(import, currentConfig)).ToArray();
            return ExternalConfigData.Merge(new ExternalConfigData(attributes, fileProcessing), configs);
        }

        // TODO (std_string) : probably move this into the specialized place
        private static Boolean IsDirectory(String path)
        {
            return Directory.Exists(path);
        }

        // TODO (std_string) : probably move this into the specialized place
        private static Boolean IsFile(String path)
        {
            return File.Exists(path);
        }

        private readonly String _configFilename;

        private const String DefaultConfigName = "porter.config";
        private const String ConfigExtension = ".config";
    }
}
