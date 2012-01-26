using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using QuotePad.Utils;
using QuotePad.Infrastructure;
using PetaPoco;

namespace QuotePad.Model
{
    [TableName("Authors")]
    [PrimaryKey("Id",autoIncrement = true)]
    public class Author : ICloneable
    {
        [Column("ID")] public int Id { get; set; }
        [Column("Name")] public string Name { get; set; }
        [Column("Info")] public string Description { get; set; }
        [Column("Photo")] public byte[] Photo { get; set; }


        public object Clone()
        {
            return new Author { Id = Id, Name = Name, Description = Description,Photo = Photo};
        }
    }
}
