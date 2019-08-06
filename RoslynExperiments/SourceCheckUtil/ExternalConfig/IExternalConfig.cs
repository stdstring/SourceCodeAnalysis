using System;

namespace SourceCheckUtil.ExternalConfig
{
    public interface IExternalConfig
    {
        ExternalConfigData LoadDefault();

        ExternalConfigData Load(String projectName);
    }
}
