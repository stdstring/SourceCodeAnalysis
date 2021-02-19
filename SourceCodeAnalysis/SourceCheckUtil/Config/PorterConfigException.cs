using System;

namespace SourceCheckUtil.Config
{
    public class PorterConfigException : Exception
    {
        public PorterConfigException()
        {
        }

        public PorterConfigException(String message) : base(message)
        {
        }

        public PorterConfigException(String message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
