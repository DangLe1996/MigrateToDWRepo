namespace MigrateToDW1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("[Timing.Option]")]
    public partial class Timing_Option
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Timing_Option()
        {
            Categories = new HashSet<Category>();
            Parameters = new HashSet<Parameter>();
        }

        [Key]
        public int OptionID { get; set; }

        [Required]
        [StringLength(10)]
        public string Name { get; set; }

        public double Time { get; set; }

        public int FixtureID { get; set; }

        public int WorkStationID { get; set; }

        public virtual Fixture Fixture { get; set; }

        public virtual Timing_WorkStations Timing_WorkStations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Categories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Parameter> Parameters { get; set; }


        public override bool Equals(object obj)
        {
            Timing_Option q = obj as Timing_Option;
            return q != null && q.Name == this.Name
                //&& q.Time == this.Time
                && q.Timing_WorkStations == this.Timing_WorkStations
                && q.Parameters.SequenceEqual(this.Parameters)
                && q.Categories.SequenceEqual(this.Categories)
                && q.Fixture == this.Fixture;
             
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode() 
                ^ this.Timing_WorkStations.GetHashCode() 
                ^ Parameter.GetHashCode(this.Parameters)
                ^ Category.GetHashCode(this.Categories)
                //^ this.Time.GetHashCode() 
                ^ this.Fixture.GetHashCode();
        }


    }
}
