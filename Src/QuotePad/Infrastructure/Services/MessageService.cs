using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MessageBox = Microsoft.Windows.Controls.MessageBox;

namespace QuotePad.Infrastructure.Services
{
    public class MessageService : IMessageService
    {
        public bool? YesNoCancelQuestion(string question, string caption = null)
        {
            bool? result;
            var response = MessageBox.Show(question, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (response == MessageBoxResult.Yes)
                result = true;
            else if (response == MessageBoxResult.No)
                result = false;
            else result = null;
            return result;

        }


        public bool YesNoQuestion(string question, string caption = null)
        {
            var response = MessageBox.Show(question, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return response == MessageBoxResult.Yes;
        }
    }
}
