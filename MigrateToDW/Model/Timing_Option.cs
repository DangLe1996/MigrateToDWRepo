namespace MigrateToDW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("[Timing.Option]")]
    public partial class Timing_Option
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OptionID { get; set; }

        [Required]
        [StringLength(10)]
        public string Name { get; set; }

        public double Time { get; set; }

        //public virtual Timing_Option Timing_Option1 { get; set; }

        //public virtual Timing_Option Timing_Option2 { get; set; }
    }
}
