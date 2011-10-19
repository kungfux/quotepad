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
        public readonly List<ToolStripButtonPrototype> toolStripItemsB = new List<ToolStripButtonPrototype>();
        public readonly List<ToolStripComboBoxPrototype> toolStripItemsC = new List<ToolStripComboBoxPrototype>();
        public readonly List<ToolStripLabelPrototype> toolStripItemsL = new List<ToolStripLabelPrototype>();
        public readonly List<ToolStripSeparatorPrototype> toolStripItemsS = new List<ToolStripSeparatorPrototype>();
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
            toolStripItemsB.Add(item);
        }

        public void AddToolStripItem(ToolStripComboBoxPrototype item)
        {
            item.Tag = tagText;
            toolStripItemsC.Add(item);
        }

        public void AddToolStripItem(ToolStripLabelPrototype item)
        {
            item.Tag = tagText;
            toolStripItemsL.Add(item);
        }

        public void AddToolStripItem(ToolStripSeparatorPrototype item)
        {
            item.Tag = tagText;
            toolStripItemsS.Add(item);
        }
    }
}
