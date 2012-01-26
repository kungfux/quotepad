using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;

namespace QuotePad.Infrastructure.Messages
{
    public class DataSourceChangedMessage : MessageBase
    {
        public object TargetSelection { get; private set; }
        public string DataSourceName { get; private set; }

        public DataSourceChangedMessage(string dataSourceName, object sender, object target, object targetSelection)
            : base (sender, target)
        {
            DataSourceName = dataSourceName;
            TargetSelection = targetSelection;
        }

        public DataSourceChangedMessage(string dataSourceName, object target, object targetSelection)
            : base(null, target)
        {
            DataSourceName = dataSourceName;
            TargetSelection = targetSelection;
        }
    }
}
