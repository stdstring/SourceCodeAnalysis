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
            CheckConfig(config);
            _configDir = IsDirectory(config) ? config : Path.GetDirectoryName(config);
            _configFilename = IsFile(config) ? config : Path.Combine(config, DefaultConfigName);
        }

        public ExternalConfigData LoadDefault()
        {
            XDocument document = XDocument.Load(_configFilename);
            XElement root = document.Root;
            IList<AttributeData> attributes = GetAttributes(root);
            ExternalConfigData[] configs = GetImports(root).Select(LoadImpl).ToArray();
            return ExternalConfigData.Merge(new ExternalConfigData(attributes), configs);
        }

        public ExternalConfigData Load(String projectName)
        {
            String projectConfig = Path.Combine(_configDir, String.Concat(projectName, ConfigExtension));
            if (!IsFile(projectConfig))
                return LoadDefault();
            XDocument document = XDocument.Load(projectConfig);
            XElement root = document.Root;
            IList<AttributeData> attributes = GetAttributes(root);
            ExternalConfigData[] configs = GetImports(root).Select(LoadImpl).ToArray();
            return ExternalConfigData.Merge(new ExternalConfigData(attributes), configs);
        }

        private void CheckConfig(String config)
        {
            if (String.IsNullOrEmpty(config))
                throw new ArgumentNullException(nameof(config));
            if (IsFile(config))
                return;
            if (IsDirectory(config) && IsFile(Path.Combine(config, DefaultConfigName)))
                return;
            throw new FileNotFoundException(config);
        }

        // TODO (std_string) : think about non-recursive version of thix impl
        private ExternalConfigData LoadImpl(String importName)
        {
            // TODO (std_string) : we must know how to process case when importName will be relative to porter directory (if 'use_porter_home_directory_while_resolving_path' option is enabled)
            String importConfigName = Path.IsPathRooted(importName) ? importName : Path.Combine(_configDir, importName);
            XDocument document = XDocument.Load(importConfigName);
            XElement root = document.Root;
            IList<AttributeData> attributes = GetAttributes(root);
            ExternalConfigData[] configs = GetImports(root).Select(LoadImpl).ToArray();
            return ExternalConfigData.Merge(new ExternalConfigData(attributes), configs);
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
            return root.Elements().Where(element => String.Equals(element.Name.LocalName, "attribute")).Select(CreateAttributeData).ToList();
        }

        private AttributeData CreateAttributeData(XElement attributeElement)
        {
            String name = attributeElement.Attribute("name").Value;
            IDictionary<String, String> data = attributeElement.Attributes()
                .Where(attr => !String.Equals(attr.Name.LocalName, "name"))
                .ToDictionary(attr => attr.Name.LocalName, attr => attr.Value);
            return new AttributeData(name, data);
        }

        // TODO (std_string) : probably move this into the specialized place
        private Boolean IsDirectory(String path)
        {
            return Directory.Exists(path);
        }

        // TODO (std_string) : probably move this into the specialized place
        private Boolean IsFile(String path)
        {
            return File.Exists(path);
        }

        private readonly String _configDir;
        private readonly String _configFilename;

        private const String DefaultConfigName = "porter.config";
        private const String ConfigExtension = ".config";
    }
}
