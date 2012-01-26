using System;
using GalaSoft.MvvmLight.Messaging;

namespace QuotePad.Infrastructure.Messages
{
    public class PropertyChangedMessageAction<T,TCallbackParam> : PropertyChangedMessage<T>
    {
        private readonly Action<TCallbackParam> _callback;

        public PropertyChangedMessageAction(object sender, T oldValue, T newValue, 
            string propertyName, Action<TCallbackParam> callback ) 
            : base(sender, oldValue, newValue, propertyName)
        {
            CheckCallback(callback);
            _callback = callback;
        }

        public PropertyChangedMessageAction(T oldValue, T newValue, 
            string propertyName, Action<TCallbackParam> callback) 
            : base(oldValue, newValue, propertyName)
        {
            CheckCallback(callback);
            _callback = callback;
        }

        public PropertyChangedMessageAction(object sender, object target, T oldValue, T newValue, 
            string propertyName, Action<TCallbackParam> callback) 
            : base(sender, target, oldValue, newValue, propertyName)
        {
            CheckCallback(callback);
            _callback = callback;

        }

        public void Execute(TCallbackParam parameter)
        {
            _callback.Invoke(parameter);
        }

        private static void CheckCallback(Action<TCallbackParam> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback", "Callback may not be null");
            }
        }
    }
}
