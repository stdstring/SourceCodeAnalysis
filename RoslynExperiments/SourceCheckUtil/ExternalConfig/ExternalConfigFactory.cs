using System;

namespace SourceCheckUtil.ExternalConfig
{
    internal static class ExternalConfigFactory
    {
        public static IExternalConfig Create(String config)
        {
            if (String.IsNullOrEmpty(config))
                return new EmptyExternalConfig();
            return new PorterExternalConfig(config);
        }
    }
}
