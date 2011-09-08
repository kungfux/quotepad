using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuotePad
{
    public class pageQuoteView : TabPagePrototype
    {
        public pageQuoteView()
        {
            this.Text = "Просмотр";
            this.AddToolStripItem(new ToolStripButtonPrototype("Редактировать цитату", Resources._1315515132_edit));
        }
    }
}
