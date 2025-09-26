using System;
using System.Windows.Forms;
using ScriptPortal.Vegas;

namespace YTPMVScript
{
    public class EntryPoint
    {
        public void FromVegas(Vegas vegas)
        {
            // Entry point called by Vegas when script runs (FromVegas signature)
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                using (var form = new Ui.MainForm(vegas))
                {
                    form.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error launching YTPMVScript UI:\n" + ex.Message, "YTPMVScript", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}