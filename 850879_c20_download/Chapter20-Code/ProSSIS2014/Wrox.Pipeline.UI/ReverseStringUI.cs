using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Pipeline.Design;
using Microsoft.SqlServer.Dts.Runtime;
using System.Windows.Forms;

namespace Wrox.Pipeline.UI
{
    public class ReverseStringUI : IDtsComponentUI 
    {
        #region Private Variables
        private IDTSComponentMetaData100 _dtsComponentMetaData;
        private IServiceProvider _serviceProvider;
        #endregion

        [CLSCompliant(false)]
        public void Initialize(IDTSComponentMetaData100 dtsComponentMetadata, IServiceProvider serviceProvider)
        {
            // Store ComponentMetaData for later use
            _dtsComponentMetaData = dtsComponentMetadata;
            _serviceProvider = serviceProvider;
        }

        public void Delete(System.Windows.Forms.IWin32Window parentWindow)
        {
        }

       public bool Edit(IWin32Window parentWindow, Variables variables, Connections connections)
        {
            try
            {
                // Create UI form and display
                ReverseStringUIForm ui = new ReverseStringUIForm(_dtsComponentMetaData, _serviceProvider, connections);
                DialogResult result = ui.ShowDialog(parentWindow);
                // Set return value to represent DialogResult. This tells the
                // managed wrapper to persist any changes made
                // on the component input and/or output, or properties.
                if (result == DialogResult.OK)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }

        public void Help(System.Windows.Forms.IWin32Window parentWindow)
        {
            throw new NotImplementedException();
        }

       

        public void New(System.Windows.Forms.IWin32Window parentWindow)
        {
         }
    }
}
