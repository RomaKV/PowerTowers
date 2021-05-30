using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EntitySql.Entity
{
    public partial class TowerValue
    {
        [Key]
        public int Id { get; set; }
        public int TowerId { get; set; }
        public int Value { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        [ForeignKey(nameof(TowerId))]
        [InverseProperty("TowerValues")]
        public virtual Tower Tower { get; set; }
    }
}
