using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace QuotePad.Utils
{
    public static class NotificationObject
    {
        public static void RaisePropertyChanged(this PropertyChangedEventHandler handler,object sender, string propertyName)
        {
            if (handler != null)
            {
                handler(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
