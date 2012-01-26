using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;

namespace QuotePad.Infrastructure.Messages
{ 
    public class SelectionChangedMessage<T> : MessageBase
    {
        public T TargetValue { get; private set; }

        public SelectionChangedMessage(object sender, T targetValue) : base(sender)
        {
            TargetValue = targetValue;
        }
    }
}
