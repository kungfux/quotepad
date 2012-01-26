using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.Unity;
using QuotePad.Data;
using QuotePad.Infrastructure.Interfaces;
using QuotePad.Infrastructure.Names;
using QuotePad.Infrastructure.Messages;
using QuotePad.Infrastructure.Services;

namespace QuotePad.ViewModel
{
    public abstract class EditableModuleViewModelBase<T> : ViewModelBase, IEditableModule where T : class, IEditableItem
    {
        #region Private properties
        private ViewModelBase _itemListViewModel;
        private T _currentSelectionViewModel;
        private bool _isEditMode; 
        #endregion

        #region Inherited properties
        protected readonly IMessageService MessageService;
        protected readonly IDataProvider DataProvider;
        protected readonly IUnityContainer Container; 
        #endregion

        #region Constructor
        protected EditableModuleViewModelBase(IUnityContainer container)
        {
            Container = container;
            MessageService = container.Resolve<IMessageService>();
            DataProvider = container.Resolve<IDataProvider>();
            MessengerInstance = container.Resolve<IMessenger>();
            InitializeMessages();
            AddItemCommand = new RelayCommand(AddItem, () => !IsEditMode);
            EditSaveItemCommand = new RelayCommand(EditSaveItem, () => CurrentSelection != null);
            RemoveItemCommand = new RelayCommand(RemoveItem, () => CurrentSelection != null && !IsEditMode);
        } 
        #endregion

        #region Public Properties
        public RelayCommand AddItemCommand { get; private set; }
        public RelayCommand EditSaveItemCommand { get; private set; }
        public RelayCommand RemoveItemCommand { get; private set; }

        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                _isEditMode = value;
                RaisePropertyChanged("IsEditMode");
                RaisePropertyChanged("HeaderExtended");
                AddItemCommand.RaiseCanExecuteChanged();
                RemoveItemCommand.RaiseCanExecuteChanged();
            }
        }

        public T CurrentSelection
        {
            get { return _currentSelectionViewModel; }
            set
            {
                _currentSelectionViewModel = value;
                RaisePropertyChanged("CurrentSelection");
                EditSaveItemCommand.RaiseCanExecuteChanged();
                RemoveItemCommand.RaiseCanExecuteChanged();

            }
        }

        public ViewModelBase ItemListViewModel
        {
            get { return _itemListViewModel; }
            protected set
            {
                _itemListViewModel = value;
                RaisePropertyChanged("ItemListViewModel");
            }
        } 
        #endregion

        #region Abstract Methods & Properties
        protected abstract T CreateItemInstance();
        protected abstract bool RemoveItemFromStorage(T currentSelection);
        protected abstract bool SaveItemToStorage(T currentSelection);
        protected abstract string GetDataSourceName();
        public abstract string Header { get; }
        public abstract string HeaderExtended { get; }
        #endregion

        #region Internal Methods
        private void AddItem()
        {
            CurrentSelection = CreateItemInstance();
            CurrentSelection.IsNew = true;
            MessengerInstance.Send(new SelectionChangedMessage<T>(this, null));
            IsEditMode = true;
        }

        private void EditSaveItem()
        {
            if (IsEditMode)
            {
                if (CanLeaveCurrentSelection(CurrentSelection))
                {
                    IsEditMode = false;
                }
            }
            else
            {
                CurrentSelection.BeginEdit();
                IsEditMode = true;
            }
        }

        private void RemoveItem()
        {
            if (MessageService.YesNoQuestion("Уверены, что хотите удалить?",Header))
            {
                if (RemoveItemFromStorage(CurrentSelection))
                {
                    Messenger.Default.Send(new DataSourceChangedMessage(
                                               GetDataSourceName(), this, ItemListViewModel,
                                               CurrentSelection));
                    //TODO Send message to refresh the list
                }
                else
                {
                    //TODO show error message
                }
            }
        }

        private bool CanLeaveCurrentSelection(T targetSelection)
        {
            if (CurrentSelection == null || !IsEditMode || !CurrentSelection.IsDirty) return true;
            var userResponse = MessageService.YesNoCancelQuestion("Хотите сохранить изменения в настройках?",Header);
            var result = false;
            switch (userResponse)
            {
                case true:
                    if (SaveItemToStorage(CurrentSelection))
                    {
                        CurrentSelection.EndEdit();
                        CurrentSelection.IsDirty = false;
                        CurrentSelection.IsNew = false;
                        Messenger.Default.Send(new DataSourceChangedMessage(
                                                   GetDataSourceName(), this, ItemListViewModel,
                                                   targetSelection));
                        result = true;
                    }
                    else
                    {
                        //TODO warning that can not save
                    }
                    break;
                case false:
                    CurrentSelection.CancelEdit();
                    CurrentSelection.IsDirty = false;
                    CurrentSelection.IsNew = false;
                    result = true;
                    break;
            }
            return result;
        }

        private void InitializeMessages()
        {
            MessengerInstance.Register<SelectionChangedMessage<T>>(
                this,
                m =>
                {
                    if (m.Sender == this || m.TargetValue == CurrentSelection
                        || (m.TargetValue == null && CurrentSelection.IsNew)) return;
                    if (CanLeaveCurrentSelection(m.TargetValue))
                    {
                        CurrentSelection = m.TargetValue;
                        IsEditMode = false;
                    }
                    else
                    {
                        MessengerInstance.Send(new SelectionChangedMessage<T>(this, CurrentSelection));
                    }
                });
        } 
        #endregion
    }
}
