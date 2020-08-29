using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models
{
    public partial class NTT_DBContext : DbContext
    {
        public NTT_DBContext()
        {
        }

        public NTT_DBContext(DbContextOptions<NTT_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<NttEmployeeRecords> NttEmployeeRecords { get; set; }
        public virtual DbSet<NttEmployeeTimeDetails> NttEmployeeTimeDetails { get; set; }
        public virtual DbSet<NttRoles> NttRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //                optionsBuilder.UseSqlServer("Server=LAPTOP-NE79USQD\\SQLEXPRESS;Database=NTT_DB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NttEmployeeRecords>(entity =>
            {
                entity.HasKey(e => e.NttEmployeeId)
                    .HasName("PK__Ntt_Empl__EE5518D92A8BFB6D");

                entity.ToTable("Ntt_Employee_Records");

                entity.HasIndex(e => e.EmployeeId)
                    .HasName("UQ__Ntt_Empl__7AD04F1062787924")
                    .IsUnique();

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<NttEmployeeTimeDetails>(entity =>
            {
                entity.HasKey(e => e.EmployeeTimeDetailId)
                    .HasName("PK__Ntt_Empl__CEF11F0DEA8B01DC");

                entity.ToTable("Ntt_Employee_Time_Details");

                entity.Property(e => e.EmployeeTimeDetailId).HasColumnName("EmployeeTImeDetailId");

                entity.Property(e => e.InTime).HasColumnType("datetime");

                entity.Property(e => e.OutTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<NttRoles>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__Ntt_Role__8AFACE1A903E4BA5");

                entity.ToTable("Ntt_Roles");

                entity.Property(e => e.Role)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
