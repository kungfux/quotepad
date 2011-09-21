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
            this.Text = text;
            this.Image = image;
            this.DisplayStyle = ToolStripItemDisplayStyle.Image;
        }

        public bool isForSupervisorOnly = false;
    }
}
