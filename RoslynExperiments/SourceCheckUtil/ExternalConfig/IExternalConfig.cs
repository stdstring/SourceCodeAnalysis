using System;
using System.Collections.Generic;

namespace SourceCheckUtil.ExternalConfig
{
    public class AttributeData
    {
        public AttributeData(String name, IDictionary<String, String> data)
        {
            Name = name;
            Data = data;
        }

        public String Name { get; }

        public IDictionary<String, String> Data { get; }
    }

    public class ExternalConfigData
    {
        public ExternalConfigData()
        {
            Attributes = new List<AttributeData>();
        }


        public ExternalConfigData(IList<AttributeData> attributes)
        {
            Attributes = attributes;
        }

        public IList<AttributeData> Attributes { get; }

        public static ExternalConfigData Merge(ExternalConfigData mainConfig, params ExternalConfigData[] configs)
        {
            List<AttributeData> attributes = new List<AttributeData>();
            attributes.AddRange(mainConfig.Attributes);
            foreach (ExternalConfigData config in configs)
            {
                attributes.AddRange(config.Attributes);
            }
            return new ExternalConfigData(attributes);
        }
    }

    public interface IExternalConfig
    {
        ExternalConfigData LoadDefault();

        ExternalConfigData Load(String projectName);
    }
}
