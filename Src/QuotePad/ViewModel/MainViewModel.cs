using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.Unity;
using QuotePad.Infrastructure.Interfaces;

namespace QuotePad.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IUnityContainer _container;
        private readonly ObservableCollection<IModule> _applications;
        private IModule _selectedApplication;
        private bool _isAdminMode = true;
        

        public MainViewModel (IUnityContainer container)
        {
            _container = container;
            _applications = new ObservableCollection<IModule>();

            if (IsInDesignMode)
                SelectedApplication = _container.Resolve<IModule>("AuthorSetupModule");
        }

        public ObservableCollection<IModule> Applications
        {
            get { return _applications; }
        }

        public IModule SelectedApplication
        {
            get { return _selectedApplication; }
            set
            {
                _selectedApplication = value;
                RaisePropertyChanged("SelectedApplication");
                RaisePropertyChanged("IsSelectedApplicationEditable");
                RaisePropertyChanged("IsSelectedApplicationNavigatable");
            }
        }

        public bool IsAdminMode
        {
            get { return _isAdminMode; }
            set
            {
                _isAdminMode = value;
                RaisePropertyChanged("IsAdminMode");
            }
        }

        public bool IsSelectedApplicationEditable
        {
            get { return SelectedApplication as IEditableModule != null; }
        }

        public bool IsSelectedApplicationNavigatable
        {
            get { return SelectedApplication as IEditableItem != null; }
        }

        #region ShowAuthorSetupCommand
        private RelayCommand _showAuthorSetupCommand;
        public RelayCommand ShowAuthorSetupCommand
        {
            get
            {
                return _showAuthorSetupCommand ??
                       (_showAuthorSetupCommand = new RelayCommand(ShowAuthorSetup));
            }
        }
        private void ShowAuthorSetup()
        {
            var authorSetupModule = _container.Resolve<IModule>("AuthorSetupModule");
            Applications.Add(authorSetupModule);
            SelectedApplication = authorSetupModule;
        } 
        #endregion

    }
}
