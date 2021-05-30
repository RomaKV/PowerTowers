using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EntitySql.Entity
{
    public partial class Tower
    {
        public Tower()
        {
            TowerValues = new HashSet<TowerValue>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        [InverseProperty(nameof(TowerValue.Tower))]
        public virtual ICollection<TowerValue> TowerValues { get; set; }
    }
}
