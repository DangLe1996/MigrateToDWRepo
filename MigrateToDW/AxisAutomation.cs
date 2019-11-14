namespace MigrateToDW
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AxisAutomation : DbContext
    {
        public AxisAutomation()
            : base("name=AxisAutomation")
        {
        }

        public virtual DbSet<Timing_Option> Timing_Option { get; set; }
        public virtual DbSet<Timing_WorkStations> Timing_WorkStations { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Fixture> Fixtures { get; set; }
        public virtual DbSet<Parameter> Parameters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Timing_Option>()
                .Property(e => e.Name)
                .IsFixedLength();

            //modelBuilder.Entity<Timing_Option>()
            //    .HasOptional(e => e.Timing_Option1)
            //    .WithRequired(e => e.Timing_Option2);

            modelBuilder.Entity<Timing_WorkStations>()
                .Property(e => e.OpCode)
                .IsFixedLength();
        }
    }
}
