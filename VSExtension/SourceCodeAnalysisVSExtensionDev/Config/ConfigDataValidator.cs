using System;
using System.Windows.Forms;

namespace SourceCodeAnalysisVSExtensionDev.Config
{
    internal static class ConfigDataValidator
    {
        public static Boolean ValidateAppLocation(String appPath, ErrorProvider errorProvider, Control control)
        {
            if (String.IsNullOrEmpty(appPath))
            {
                SetError(errorProvider, control, "Empty app path");
                return false;
            }
            return true;
        }

        public static Boolean ValidateSourceEntry(String sourcePath, String configPath, ErrorProvider errorProviderSourcePath, Control controlSourcePath)
        {
            if (String.IsNullOrEmpty(sourcePath))
            {
                SetError(errorProviderSourcePath, controlSourcePath, "Empty source path");
                return false;
            }
            return true;
        }

        private static void SetError(ErrorProvider errorProvider, Control control, String errorText)
        {
            errorProvider.SetError(control, errorText);
            errorProvider.SetIconPadding(control, 4);
        }
    }
}