using System;

namespace SourceCheckUtil.Config
{
    public interface IConfig
    {
        ConfigData LoadDefault();

        ConfigData Load(String projectName);
    }
}
