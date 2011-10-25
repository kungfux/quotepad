using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace QuotePad
{
    public class ToolStripButtonPrototype : ToolStripButton
    {
        public ToolStripButtonPrototype(string text, Image image)
        {
            this.AutoSize = false;
            this.Size = new Size(66, 66);
            this.ImageScaling = ToolStripItemImageScaling.None;
            this.Text = text;
            this.Image = image;
            this.DisplayStyle = ToolStripItemDisplayStyle.Image;
        }

        public bool isForSupervisorOnly = false;
    }

    public class ToolStripComboBoxPrototype : ToolStripComboBox
    {
        public bool isForSupervisorOnly = false;

        public ToolStripComboBoxPrototype()
        {
            this.Font = new Font(this.Font.FontFamily, 14);
        }
    }

    public class ToolStripSeparatorPrototype : ToolStripSeparator
    {
        public bool isForSupervisorOnly = false;
    }

    public class ToolStripLabelPrototype : ToolStripLabel
    {
        public ToolStripLabelPrototype(string text)
        {
            this.Text = text;
            this.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.Font = new Font(this.Font.FontFamily, 14);
        }

        public bool isForSupervisorOnly = false;
    }
}
