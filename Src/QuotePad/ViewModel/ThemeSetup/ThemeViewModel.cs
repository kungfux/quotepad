using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using QuotePad.Model;
using QuotePad.Infrastructure.Services;

namespace QuotePad.ViewModel
{
    public class ThemeViewModel : EditableItemViewModelBase<Theme>
    {
        private Theme _theme;
        private Theme _themeBackup;

        public ThemeViewModel (Theme theme)
        {
            _theme = theme;
        }

        public override Theme ItemData
        {
            get { return _theme; }
            protected set
            {
                _theme = value;
                RaisePropertyChanged("ThemeName");
            }
        }
        protected override Theme ItemDataBackup
        {
            get { return _themeBackup; }
            set
            {
                _themeBackup = value;
                RaisePropertyChanged("ThemeNameOriginal");
            }
        }
        public override int GetItemDataID()
        {
            return ItemData.Id;
        }

        public string ThemeName
        {
            get { return _theme.Name; }
            set
            {
                _theme.Name = value;
                IsDirty = true;
                RaisePropertyChanged("ThemeName");
            }
        }

        public string ThemeNameOriginal
        {
            get { return ItemDataBackup == null ? ThemeName : ItemDataBackup.Name; }
        }

        public string Description
        {
            get { return _theme.Description; }
            set
            {
                _theme.Description = value;
                IsDirty = true;
                RaisePropertyChanged("Description");
            }
        }
    }
}
