using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuotePad.Infrastructure.Services
{
    public interface IMessageService
    {
        bool? YesNoCancelQuestion(string question, string caption = null);
        bool YesNoQuestion(string question, string caption = null);

    }
}
