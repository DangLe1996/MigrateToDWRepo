namespace TimeCalculator
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DWModel : DbContext
    {
        public DWModel()
            : base("name=DWModel2")
        {
        }
        public DWModel(bool useDefault = true)
        {
           
            this.Database.Connection.ConnectionString = @"data source=VAULT\DRIVEWORKS;initial catalog=AXIS Automation;;persist security info=True;user id=epicoradmin;password=Ep1c0r4Life!;MultipleActiveResultSets=True;App=EntityFramework";
        }

        public virtual DbSet<Timing_Option> Timing_Option { get; set; }
        public virtual DbSet<Timing_WorkStations> Timing_WorkStations { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryAtFixture> CategoryAtFixtures { get; set; }
        public virtual DbSet<Fixture> Fixtures { get; set; }
        public virtual DbSet<ParameterAtCategoryAtFixture> ParameterAtCategoryAtFixtures { get; set; }
        public virtual DbSet<Parameter> Parameters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Timing_Option>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Timing_WorkStations>()
                .Property(e => e.OpCode)
                .IsFixedLength();

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Timing_Option)
                .WithMany(e => e.Categories)
                .Map(m => m.ToTable("TimingOptionAtCategory").MapLeftKey("CategoriesID").MapRightKey("TimingOptionID"));

            modelBuilder.Entity<CategoryAtFixture>()
                .HasMany(e => e.ParameterAtCategoryAtFixtures)
                .WithRequired(e => e.CategoryAtFixture)
                .HasForeignKey(e => e.CategoryAtFixtureId);

            modelBuilder.Entity<ParameterAtCategoryAtFixture>()
                .HasMany(e => e.CategoryAtFixtures)
                .WithOptional(e => e.ParameterAtCategoryAtFixture)
                .HasForeignKey(e => e.DefaultFallbackSelection);
                 
            modelBuilder.Entity<Parameter>()
                .HasMany(e => e.Timing_Option)
                .WithMany(e => e.Parameters)
                .Map(m => m.ToTable("TimingOptionAtParameter").MapLeftKey("ParameterID").MapRightKey("TimingOptionID"));
        }
    }
}
