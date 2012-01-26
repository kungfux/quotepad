using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace QuotePad.Infrastructure.Interfaces
{
    public interface IEditableItem : IEditableObject
    {
        bool IsDirty { get; set; }
        bool IsNew { get; set; }
        int GetItemDataID();
    }
}
