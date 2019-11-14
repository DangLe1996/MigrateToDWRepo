namespace MigrateToDW
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FixtureSetupCodes.Categories")]
    public partial class Category
    {
        public int id { get; set; }

        [Required]
        [StringLength(75)]
        public string Name { get; set; }

        public int TypeID { get; set; }

        public bool IsMultiselect { get; set; }

        [Required]
        [StringLength(250)]
        public string Footnote { get; set; }

        public bool IsOptional { get; set; }
    }
}
