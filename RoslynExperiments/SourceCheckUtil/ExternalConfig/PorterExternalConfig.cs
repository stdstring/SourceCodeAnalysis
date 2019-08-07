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
            IList<AttributeData> attributes = GetAttributes(root);
            IList<FileProcessingData> fileProcessing = GetFileProcessing(root);
            ExternalConfigData[] configs = GetImports(root).Select(import => LoadImpl(import, currentConfig)).ToArray();
            return ExternalConfigData.Merge(new ExternalConfigData(attributes, fileProcessing), configs);
        }

        private IList<String> GetImports(XElement root)
        {
            return root.Elements()
                .Where(element => String.Equals(element.Name.LocalName, "import"))
                .Select(element => element.Attribute("config").Value)
                .ToList();
        }

        private IList<AttributeData> GetAttributes(XElement root)
        {
            return root.Elements()
                .Where(element => String.Equals(element.Name.LocalName, "attribute"))
                .Select(CreateAttributeData)
                .ToList();
        }

        private AttributeData CreateAttributeData(XElement attributeElement)
        {
            String name = attributeElement.Attribute("name").Value;
            IDictionary<String, String> data = attributeElement.Attributes()
                .Where(attr => !String.Equals(attr.Name.LocalName, "name"))
                .ToDictionary(attr => attr.Name.LocalName, attr => attr.Value);
            return new AttributeData(name, data);
        }

        private IList<FileProcessingData> GetFileProcessing(XElement root)
        {
            return root.Elements()
                .Where(element => String.Equals(element.Name.LocalName, "files"))
                .SelectMany(element => element.Elements())
                .Select(CreateFileProcessingData)
                .OrderByDescending(data => data.Mode)
                .ToList();
        }

        private FileProcessingData CreateFileProcessingData(XElement fileProcessingElement)
        {
            const String includeElement = "include";
            const String excludeElement = "exclude";
            const String onlyElement = "only";
            IDictionary<String, FileProcessingMode> fileProcessModeMap = new Dictionary<String, FileProcessingMode>
            {
                {includeElement, FileProcessingMode.Include},
                {excludeElement, FileProcessingMode.Exclude},
                {onlyElement, FileProcessingMode.Only}
            };
            const String maskAttribute = "file";
            FileProcessingMode mode = fileProcessModeMap[fileProcessingElement.Name.LocalName];
            String mask = fileProcessingElement.Attribute(maskAttribute).Value;
            return new FileProcessingData(mode, mask);
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
