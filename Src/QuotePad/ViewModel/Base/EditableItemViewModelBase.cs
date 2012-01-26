using System;
using GalaSoft.MvvmLight;
using QuotePad.Infrastructure.Interfaces;

namespace QuotePad.ViewModel
{
    public abstract class EditableItemViewModelBase<T> : ViewModelBase, IEditableItem where T : class, ICloneable
    {
        private bool _isDirty;

        protected abstract T ItemDataBackup { get; set; }
        public abstract T ItemData { get; protected set; }
        public abstract int GetItemDataID();

        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                RaisePropertyChanged("IsDirty");
            }
        }

        public bool IsNew { get; set; }

        public void BeginEdit()
        {
            ItemDataBackup = ItemData;
            ItemData = (T)ItemData.Clone();
        }

        public void CancelEdit()
        {
            if (ItemDataBackup == null) return;
            ItemData = ItemDataBackup;
            ItemDataBackup = null;
        }

        public void EndEdit()
        {
            ItemDataBackup = null;
        }
    }
}
