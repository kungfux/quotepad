using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using Microsoft.Practices.Unity;
using QuotePad.ViewModel;
using QuotePad.Infrastructure.Interfaces;
using QuotePad.Data;
using GalaSoft.MvvmLight.Messaging;
using QuotePad.Infrastructure.Services;

namespace QuotePad
{
    public class Bootstrapper
    {
        public IUnityContainer Container { get; set; }

        public Bootstrapper()
        {
            Container = new UnityContainer();
            ConfigureContainer();
        }

        private void ConfigureContainer()
        {
            Container.RegisterInstance(typeof (IUnityContainer), Container);
            Container.RegisterType<IMessenger, Messenger>();
            Container.RegisterType<MainViewModel>();
            Container.RegisterType<ViewModelBase, AuthorListViewModel>("AuthorListViewModel"); // TODO сделать чтобы по дефолту null передавался
            Container.RegisterType<ViewModelBase, ThemeListViewModel>("ThemeListViewModel"); // TODO сделать чтобы по дефолту null передавался
            Container.RegisterInstance(typeof (IDataProvider), new DataProvider());
            Container.RegisterType<IMessageService,MessageService>();
            Container.RegisterType<IFileService, FileService>();
            Container.RegisterType<IModule, AuthorSetupViewModel>("AuthorSetupModule");
            Container.RegisterType<IModule, ThemeSetupViewModel>("ThemeSetupModule");
        }
    }
}
