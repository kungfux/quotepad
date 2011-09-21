using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace QuotePad
{
    /// <summary>
    /// All tab pages should be created by inheritance of this class
    /// </summary>
    public class TabPagePrototype : TabPage
    {
        public readonly List<ToolStripButtonPrototype> toolStripItems = new List<ToolStripButtonPrototype>();
        public bool cancelClosing = false; // Flag which disable Dispose() method once
        private string tagText = "text";

        /// <summary>
        /// This method should remove TabPage from TabControl
        /// Signal will be transmitted by TabControl.MouseUp event
        /// </summary>
        public void Destroy()
        {
            if (BeforeDestroy != null) BeforeDestroy();
            if (!cancelClosing) this.Dispose();
            else cancelClosing = !cancelClosing;
        }

        /// <summary>
        /// Event which called before TabPage will be destroyed
        /// </summary>
        public event Delete BeforeDestroy;
        public delegate void Delete();

        public void AddToolStripItem(ToolStripButtonPrototype item)
        {
            item.Tag = tagText;
            toolStripItems.Add(item);
        }
    }
}
