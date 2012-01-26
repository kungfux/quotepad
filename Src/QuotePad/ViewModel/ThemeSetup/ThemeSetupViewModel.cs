using GalaSoft.MvvmLight;
using QuotePad.Infrastructure;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.Unity;
using QuotePad.Infrastructure.Messages;
using QuotePad.Infrastructure.Names;
using QuotePad.Infrastructure.Services;
using QuotePad.Data;
using QuotePad.Model;

namespace QuotePad.ViewModel
{
    public class ThemeSetupViewModel : EditableModuleViewModelBase<ThemeViewModel>
    {

        public ThemeSetupViewModel (IUnityContainer container) : base (container)
        {
            ItemListViewModel = container.Resolve<ViewModelBase>("ThemeListViewModel", new ParameterOverride("messenger", MessengerInstance));
        }

        public override string Header
        {
            get { return "Темы"; }
        }

        public override string HeaderExtended
        {
            get
            {
                if (CurrentSelection != null && IsEditMode && CurrentSelection.IsNew)
                    return "Создание темы";
                if (IsEditMode)
                    return "Редактирование темы";
                return "Просмотр тем";
            }
        }

        protected override string GetDataSourceName()
        {
            return DataSourceNames.ThemeDataSource;
        }

        protected override ThemeViewModel CreateItemInstance()
        {
            return new ThemeViewModel(new Theme());
        }

        protected override bool RemoveItemFromStorage(ThemeViewModel currentSelection)
        {
            return DataProvider.RemoveTheme(currentSelection.ItemData);
        }

        protected override bool SaveItemToStorage(ThemeViewModel currentSelection)
        {
            return DataProvider.SaveTheme(currentSelection.ItemData);
        }
    }
}
