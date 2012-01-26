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
    public class AuthorViewModel : EditableItemViewModelBase<Author>
    {
        private Author _author;
        private Author _authorBackup;
        private readonly IFileService _fileService;

        public AuthorViewModel (Author author, IFileService fileService)
        {
            _author = author;
            _fileService = fileService;
            RemovePhotoCommand = new RelayCommand(() => AuthorPhoto = null);
            ModifyPhotoCommand = new RelayCommand(ModifyPhoto);
        }

        public RelayCommand RemovePhotoCommand { get; private set; }
        public RelayCommand ModifyPhotoCommand { get; private set; }

        public override Author ItemData
        {
            get { return _author; }
            protected set
            {
                _author = value;
                RaisePropertyChanged("AuthorName");
            }
        }
        protected override Author ItemDataBackup
        {
            get { return _authorBackup; }
            set
            {
                _authorBackup = value;
                RaisePropertyChanged("AuthorNameOriginal");
            }
        }
        public override int GetItemDataID()
        {
            return ItemData.Id;
        }

        public string AuthorName
        {
            get { return _author.Name; }
            set
            {
                _author.Name = value;
                IsDirty = true;
                RaisePropertyChanged("AuthorName");
            }
        }

        public byte[] AuthorPhoto
        {
            get { return _author.Photo; }
            set
            {
                _author.Photo = value;
                IsDirty = true;
                RaisePropertyChanged("AuthorPhoto");
            }
        }

        public string AuthorNameOriginal
        {
            get { return ItemDataBackup == null ? AuthorName : ItemDataBackup.Name; }
        }

        public string Description
        {
            get { return _author.Description; }
            set
            {
                _author.Description = value;
                IsDirty = true;
                RaisePropertyChanged("Description");
            }
        }

        private void ModifyPhoto()
        {
            string selectedFile;
            const string fileFilter = "Image files (jpg, jpeg, bmp, png, tiff)|*.jpg;*.jpeg;*.bmp;*.png;*.tiff|All files (*.*)|*.*";
            if (_fileService.SelectFile(out selectedFile,fileFilter))
            {
                FileStream fs = File.OpenRead(selectedFile);
                var data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                AuthorPhoto = data;
            }
        }
    }
}
