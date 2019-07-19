using System;

namespace SourceCheckUtil.ExternalConfig
{
    internal static class ExternalConfigFactory
    {
        public static IExternalConfig Create(String config)
        {
            return new EmptyExternalConfig();
        }
    }
}
