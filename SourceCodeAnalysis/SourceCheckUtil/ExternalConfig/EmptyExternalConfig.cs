using System;

namespace SourceCheckUtil.ExternalConfig
{
    internal class EmptyExternalConfig : IExternalConfig
    {
        public ExternalConfigData LoadDefault()
        {
            return new ExternalConfigData();
        }

        public ExternalConfigData Load(String projectName)
        {
            return new ExternalConfigData();
        }
    }
}
