using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using QuotePad.Data;
using QuotePad.Infrastructure.Interfaces;
using QuotePad.Infrastructure.Names;
using QuotePad.Infrastructure.Messages;
using QuotePad.Infrastructure.Services;

namespace QuotePad.ViewModel
{
    public abstract class ItemListViewModelBase<T> : ViewModelBase where T: class, IEditableItem
    {
        #region Inherited Properties
        protected readonly ObservableCollection<T> Items;
        protected readonly IDataProvider DataProvider;
        protected readonly IFileService FileService;
        
        protected T SelectedItem
        {
            get { return (T)ItemsView.CurrentItem; }
        } 
        #endregion

        #region ctor
        protected ItemListViewModelBase(IMessenger messenger, IDataProvider dataProvider, IFileService fileService)
            : base(messenger)
        {
            DataProvider = dataProvider;
            FileService = fileService;
            Items = new ObservableCollection<T>();
            ItemsView = CollectionViewSource.GetDefaultView(Items);
            ItemsView.CurrentChanged += 
                                (s, e) => MessengerInstance.Send(new SelectionChangedMessage<T>(this, SelectedItem));
            InitilazeMessages();
            InitializeItemList(false,default(T));
        } 
        #endregion

        #region Public Properties
        public ICollectionView ItemsView { get; private set; }
        #endregion

        #region Abstract Methods
        protected abstract void InitializeItemList();
        protected abstract string GetDataSourceName();
        #endregion

        #region Private Methods
        private void ChangeSelectedItem(object sender, T targetSelection)
        {
            if (sender == this || SelectedItem == targetSelection) return;
            DispatcherHelper.UIDispatcher.BeginInvoke(new Action(() => ItemsView.MoveCurrentTo(targetSelection)));
        }

        private void InitilazeMessages()
        {
            MessengerInstance.Register<SelectionChangedMessage<T>>(this,
                                                                   m => ChangeSelectedItem(m.Sender, m.TargetValue));
            Messenger.Default.Register<DataSourceChangedMessage>(this,m => OnDataSourceChanged(m.DataSourceName,m.Target,m.TargetSelection));
        } 

        private void OnDataSourceChanged (string dataSourceName, object target, object targetSelection)
        {
            if (dataSourceName == GetDataSourceName())
            {
                if (target == this)
                {
                    InitializeItemList(false, targetSelection as T);
                }
                else
                {
                    InitializeItemList(true, null);
                }
            }
        }

        private void InitializeItemList (bool restoreSelection, T targetSelection, bool moveToFirst = true)
        {
            int targetSelectionID = -1;
            if (restoreSelection)
            {
                targetSelectionID = SelectedItem.GetItemDataID();
            }
            else if (targetSelection != null)
            {
                targetSelectionID = targetSelection.GetItemDataID();
            }
            InitializeItemList();
            var targetItem = Items.FirstOrDefault(t => t.GetItemDataID() == targetSelectionID);
            if (targetItem == null && moveToFirst)
            {
                targetItem = Items.FirstOrDefault();
            }
            ChangeSelectedItem(null,targetItem);
        }
        #endregion
    }
}
