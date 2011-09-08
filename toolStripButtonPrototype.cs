using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuotePad
{
    public class ToolStripButtonPrototype : ToolStripButton
    {
        public ToolStripButtonPrototype(string text)
        {
            this.Text = text;
        }

        public bool isForSupervisorOnly = false;
    }
}
