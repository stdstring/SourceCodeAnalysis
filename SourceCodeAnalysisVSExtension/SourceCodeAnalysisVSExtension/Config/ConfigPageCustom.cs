using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace SourceCodeAnalysisVSExtension.Config
{
    [Guid("4137B12F-9307-47DF-9B23-D528E50CDDE4")]
    public class ConfigPageCustom : DialogPage
    {
        protected override IWin32Window Window
        {
            get
            {
                ConfigUserControl page = new ConfigUserControl(() => new AppDataConfigDataProvider());
                return page;
            }
        }
    }
}
