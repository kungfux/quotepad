using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Command;

namespace QuotePad.Infrastructure.Interfaces
{
    public interface IEditableModule : IModule
    {
        RelayCommand AddItemCommand { get; }
        RelayCommand EditSaveItemCommand { get; }
        RelayCommand RemoveItemCommand { get; }
        bool IsEditMode { get; set; }
    }
}
