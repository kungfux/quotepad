using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace QuotePad.Model
{
    [TableName("Themese")]
    [PrimaryKey("Id",autoIncrement = true)]
    public class Theme : ICloneable
    {
        [Column("Id")] public int Id { get; set; }
        [Column("Name")] public string Name { get; set; }
        [Column("Info")] public string Description { get; set; }

        public object Clone()
        {
            return new Theme { Id = Id, Name = Name, Description = Description };
        }
    }
}
