using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace QuotePad.ViewModel
{
    public class ViewModelLocator
    {
        private static readonly Bootstrapper Bootstrapper;

        static ViewModelLocator()
        {
            if (Bootstrapper == null)
                Bootstrapper = new Bootstrapper();
        }

        public MainViewModel Main
        {
            get { return Bootstrapper.Container.Resolve<MainViewModel>(); }
        }
    }
}
