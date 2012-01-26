using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using QuotePad.Model;
using QuotePad.Infrastructure.Names;
using QuotePad.Infrastructure.Services;
using QuotePad.Data;

namespace QuotePad.ViewModel
{
    public class AuthorListViewModel : ItemListViewModelBase<AuthorViewModel>
    {
        public AuthorListViewModel(IMessenger messenger, IDataProvider dataProvider, IFileService fileService)
            : base(messenger, dataProvider,fileService)
        {
        }

        protected override void InitializeItemList()
        {
            IList<Author> authors = DataProvider.GetAuthors();
            Items.Clear();
            foreach (var author in authors)
            {
                Items.Add(new AuthorViewModel(author, FileService));
            }
        }

        protected override string GetDataSourceName()
        {
            return DataSourceNames.AuthorDataSource;
        }

        /*private ObservableCollection<AuthorViewModel> Authors { get; set; }
        private readonly IDataProvider _dataProvider;
        private readonly IFileService _fileService;

        #region ctor
        public AuthorListViewModel(IMessenger messenger, IDataProvider dataProvider, IFileService fileService)
            : base(messenger)
        {
            _dataProvider = dataProvider;
            _fileService = fileService;
            Authors = new ObservableCollection<AuthorViewModel>();
            AuthorsView = CollectionViewSource.GetDefaultView(Authors);
            AuthorsView.CurrentChanged +=
                (s, e) => MessengerInstance.Send(new SelectionChangedMessage<AuthorViewModel>(this, SelectedAuthor));
            InitilazeMessages();
            InitializeAuthorList();
        } 
        #endregion

        #region Public Properties
        public ICollectionView AuthorsView { get; private set; }

        public AuthorViewModel SelectedAuthor
        {
            get { return (AuthorViewModel)AuthorsView.CurrentItem; }
        } 
        #endregion

        #region Private Methods
        private void ChangeSelectedAuthor(object sender, AuthorViewModel targetSelection)
        {
            if (sender == this || SelectedAuthor == targetSelection) return;
            DispatcherHelper.UIDispatcher.BeginInvoke(new Action(() => AuthorsView.MoveCurrentTo(targetSelection)));
        }

        private void InitializeAuthorList()
        {
            IList<Author> authors = _dataProvider.GetAuthors();
            Authors.Clear();
            foreach (var author in authors)
            {
                Authors.Add(new AuthorViewModel(author, _fileService));
            }
            if (Authors.Count > 0)
                AuthorsView.MoveCurrentToFirst();
        }

        private void InitilazeMessages()
        {
            MessengerInstance.Register<SelectionChangedMessage<AuthorViewModel>>(this,
                                                                                 m => ChangeSelectedAuthor(m.Sender, m.TargetValue));
        } 
        #endregion*/


    }
}
