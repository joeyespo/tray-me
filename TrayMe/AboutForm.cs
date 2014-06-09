using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace TrayMe
{
    /// <summary>
    /// Represents the about dialog.
    /// </summary>
    public partial class AboutForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutForm"/> class.
        /// </summary>
        public AboutForm()
        {
            InitializeComponent();

            labelTitle.Text = AppTitle;
            labelDescription.Text = AppDescription;
        }

        #region Event Handler

        /// <summary>
        /// Handles the Load event of the AboutForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AboutForm_Load(object sender, System.EventArgs e)
        {
            string m_strProductVersion;
            string m_strVersion;
            string m_strRevision;
            int i;

            m_strProductVersion = System.Windows.Forms.Application.ProductVersion;

            i = m_strProductVersion.IndexOf(".");
            if (i >= 0)
                i = m_strProductVersion.IndexOf(".", (i + 1));
            m_strVersion = ((i >= 0) ? (m_strProductVersion.Substring(0, i)) : ("0"));

            if (i >= 0)
                i = m_strProductVersion.IndexOf(".", (i + 1));
            m_strRevision = ((i >= 0) ? (m_strProductVersion.Substring(i + 1)) : ("0"));

            labelVersion.Text = "Version: " + m_strVersion;
            labelRevision.Text = "[Revision: " + m_strRevision + "]";
        }

        /// <summary>
        /// Handles the LinkClicked event of the linkHomepageLink control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void linkHomepageLink_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // Call the Process.Start method to open the default browser with a URL:
                System.Diagnostics.Process.Start("http://www.uber-ware.com");
            }
            catch (Win32Exception)
            {
                // Ignore false Windows exceptions since the browser still opens.
            }
            catch
            {
                // Failsafe
                MessageBox.Show(this, "Could not open the link.", (AppTitle + " Error").TrimStart(' '), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the MouseEnter event of the linkHomepageLink control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void linkHomepageLink_MouseEnter(object sender, System.EventArgs e)
        {
            linkHomepageLink.LinkColor = linkHomepageLink.ActiveLinkColor;
        }

        /// <summary>
        /// Handles the MouseLeave event of the linkHomepageLink control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void linkHomepageLink_MouseLeave(object sender, System.EventArgs e)
        {
            linkHomepageLink.LinkColor = linkHomepageLink.ForeColor;
        }

        /// <summary>
        /// Handles the Resize event of the labelVersion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void labelVersion_Resize(object sender, System.EventArgs e)
        {
            labelRevision.Left = labelVersion.Left + labelVersion.Width;
        }

        #endregion

        private const string AppTitle = "TrayMe [Stand-Alone Version]";
        private const string AppDescription = "TrayMe is a Windows Tool that easily helps a user organize his or her windows and save them from window congestion by use of the System Tray.";
        // Note: the icon is located on the window.
    }
}
