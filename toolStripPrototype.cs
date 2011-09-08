using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuotePad
{
    public class ToolStripPrototype : ToolStrip
    {
        // Perform setting correct visibility on load
        protected override void SetDisplayedItems()
        {
            SetVisibility();
            base.SetDisplayedItems();
        }

        // Set Visible property for each item in main menu
        public void SetVisibility()
        {
            bool isSuperUser = true; // Here is should be external call to check isAdminLoggedIn?
            foreach (ToolStripButtonPrototype item in this.Items)
            {
                if (item.isForSupervisorOnly)
                {
                    item.Visible = isSuperUser;
                }
            }
        }
    }
}
