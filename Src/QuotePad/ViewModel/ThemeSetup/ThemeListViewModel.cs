using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using QuotePad.Model;
using QuotePad.Infrastructure.Names;
using QuotePad.Infrastructure.Services;
using QuotePad.Data;

namespace QuotePad.ViewModel
{
    public class ThemeListViewModel : ItemListViewModelBase<ThemeViewModel>
    {
        public ThemeListViewModel(IMessenger messenger, IDataProvider dataProvider, IFileService fileService)
            :base(messenger,dataProvider,fileService)
        {
        }

        protected override void InitializeItemList()
        {
            IList<Theme> themes = DataProvider.GetThemes();
            Items.Clear();
            foreach (var theme in themes)
            {
                Items.Add(new ThemeViewModel(theme));
            }
        }

        protected override string GetDataSourceName()
        {
            return DataSourceNames.ThemeDataSource;
        }
    }
}
