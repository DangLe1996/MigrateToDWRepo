namespace MigrateToDW1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FixtureSetupCodes.Parameters")]
    public partial class Parameter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Parameter()
        {
            ParameterAtCategoryAtFixtures = new HashSet<ParameterAtCategoryAtFixture>();
            Timing_Option = new HashSet<Timing_Option>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(25)]
        public string Code { get; set; }

        [StringLength(75)]
        public string Description { get; set; }

        [Required]
        [StringLength(250)]
        public string Footnote { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParameterAtCategoryAtFixture> ParameterAtCategoryAtFixtures { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Timing_Option> Timing_Option { get; set; }


        public override bool Equals(object obj)
        {
            Parameter q = obj as Parameter;
            return q != null && q.Code == this.Code;

        }

        public override int GetHashCode()
        {
            return this.Code.GetHashCode();

        }
        public static int GetHashCode(ICollection<Parameter> array)
        {
            // if non-null array then go into unchecked block to avoid overflow
            if (array != null)
            {
                unchecked
                {
                    int hash = 17;

                    // get hash code for all items in array
                    foreach (var item in array)
                    {
                        hash = hash * 23 + ((item != null) ? item.GetHashCode() : 0);
                    }

                    return hash;
                }
            }

            // if null, hash code is zero
            return 0;


        }
    }
}
