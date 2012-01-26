using GalaSoft.MvvmLight;
using QuotePad.Infrastructure;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.Unity;
using QuotePad.Infrastructure.Names;
using QuotePad.Infrastructure.Messages;
using QuotePad.Infrastructure.Services;
using QuotePad.Data;
using QuotePad.Model;

namespace QuotePad.ViewModel
{
    public class AuthorSetupViewModel : EditableModuleViewModelBase<AuthorViewModel>
    {

        public AuthorSetupViewModel (IUnityContainer container) : base (container)
        {
            ItemListViewModel = container.Resolve<ViewModelBase>("AuthorListViewModel", new ParameterOverride("messenger", MessengerInstance));
        }

        public override string Header
        {
            get { return "Авторы"; }
        }

        public override string HeaderExtended
        {
            get
            {
                if (CurrentSelection != null && IsEditMode && CurrentSelection.IsNew)
                    return "Создание автора";
                if (IsEditMode)
                    return "Редактирование автора";
                return "Просмотр автора";
            }
        }

        protected override AuthorViewModel CreateItemInstance()
        {
            return new AuthorViewModel(new Author(), Container.Resolve<IFileService>());
        }

        protected override string GetDataSourceName()
        {
            return DataSourceNames.AuthorDataSource;
        }

        protected override bool RemoveItemFromStorage(AuthorViewModel currentSelection)
        {
            return DataProvider.RemoveAuthor(currentSelection.ItemData);
        }

        protected override bool SaveItemToStorage(AuthorViewModel currentSelection)
        {
            return DataProvider.SaveAuthor(currentSelection.ItemData);
        }
    }
}
