using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuotePad
{
    public class pageQuoteEditor: TabPagePrototype
    {
        public pageQuoteEditor()
        {
            this.Text = "Редактирование";
            ToolStripButtonPrototype test = new ToolStripButtonPrototype("2");
            test.isForSupervisorOnly = true;
            this.AddToolStripItem(test);
        }
    }
}
