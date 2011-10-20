using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace QuotePad
{
    /// <summary>
    /// Main menu prototype
    /// </summary>
    public class menuStripPrototype : MenuStrip
    {
        public readonly string adminTag = "superUserOnly"; // If item require superuser it should has this Tag value

        // Perform setting correct visibility on load
        protected override void OnVisibleChanged(EventArgs e)
        {
            SetVisibility();
            base.OnVisibleChanged(e);
        }

        // Set Visible property for each item in main menu
        public void SetVisibility()
        {
            bool isSuperUser = (Authorization.userType == UserType.Editor);
            foreach (ToolStripMenuItem item in this.Items)
            {
                if (item.Tag != null && item.Tag.ToString() == adminTag)
                {
                    item.Visible = isSuperUser;
                }
                if (item.Visible)
                {
                    foreach (ToolStripMenuItem subitem in item.DropDownItems)
                    {
                        if (subitem.Tag != null && subitem.Tag.ToString() == adminTag)
                            subitem.Visible = isSuperUser;
                    }
                }
            }
        }
    }
}
