using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations.Schema;
using Tank.PaymentAPI.Models;

namespace Tank.PaymentAPI.Datas
{
    public partial class MyDbWebContext : DbContext
    {
        public MyDbWebContext(DbContextOptions<MyDbWebContext> options) : base(options) { }
        public MyDbWebContext()
        {

        }

        #region DbSet
        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }
        public virtual DbSet<LogCardModel> LogCards { get; set; }
        public virtual DbSet<ServerListModel> ServerLists { get; set; }
        public virtual DbSet<UserModel> Users { get; set; }
        public DbSet<MBBankModel> MBBanks { get; set; }
        public DbSet<MomoModel> Momos { get; set; }
        public DbSet<ChargeValueModel> ChargeValues { get; set; }
        public DbSet<PaymentCodeModel> PaymentCodes { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<PaymentCodeModel>(e =>
            {
                e.ToTable("Log_PaymentCode");
                e.HasKey(e => e.Code);
                e.Property(e => e.Amount).HasDefaultValue(10000);
                e.Property(e => e.EndTime);
            });

            modelBuilder.Entity<ChargeValueModel>(e =>
            {
                e.ToTable("Charge_Value");
                e.HasKey(e => e.ID);
                e.Property(e => e.RealAmount).IsRequired();
                e.Property(e => e.GameAmount).IsRequired();
                e.Property(e => e.BonusRate).HasDefaultValue(0.00);

            });

            modelBuilder.Entity<MomoModel>(e =>
            {
                e.ToTable("Log_Momo");
                e.HasKey(e => e.TranID).HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);
                e.Property(e => e.UserID).IsRequired().HasMaxLength(255);
                e.Property(e => e.PartnerID).IsRequired().HasMaxLength(15);
                e.Property(e => e.PartnerName).IsRequired().HasMaxLength(50);
                e.Property(e => e.Amount).IsRequired().HasDefaultValue(10000);
                e.Property(e => e.Comment).IsRequired().IsUnicode(true).HasDefaultValue("Nạp game bằng MOMO").HasMaxLength(160);
                e.Property(e => e.PayTime).HasColumnType("smalldatetime");
                e.Property(e => e.Checked).HasDefaultValue(false);
            });

            modelBuilder.Entity<MBBankModel>(e =>
            {
                e.ToTable("Log_MBBank");
                e.HasKey(o => o.TransactionID);
                e.Property(o => o.Amount).IsRequired().HasDefaultValue(20000).HasMaxLength(9);
                e.Property(o => o.Description).IsRequired().HasDefaultValue("NOI DUNG CHUYEN KHOAN").HasMaxLength(90);
                e.Property(o => o.TransactionDate).HasDefaultValueSql("getutcdate()");
                e.Property(o => o.Type).HasDefaultValue("IN").HasMaxLength(10);
                e.Property(o => o.Checked).HasDefaultValue(false);
            });

            modelBuilder.Entity<LogCardModel>(entity =>
            {
                entity.ToTable("log_card");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CardCode)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("card_code");

                entity.Property(e => e.CardName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("card_name");

                entity.Property(e => e.CardSeri)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("card_seri");

                entity.Property(e => e.CardType)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("card_type");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("note");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LogCards)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__log_card__user_i__286302EC");
            });

            modelBuilder.Entity<ServerListModel>(entity =>
            {
                entity.ToTable("server_list");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Catalog)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.ConfigUrl)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.DataSource)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.FlashUrl)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.KeyRequest)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.LinkCenter).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.RequestUrl)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("UserID");
            });

            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("email");

                entity.Property(e => e.IsExist).HasColumnName("is_exist");

                entity.Property(e => e.Money).HasColumnName("money");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("phone_number");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("username");

                entity.Property(e => e.VipExp).HasColumnName("vip_exp");

                entity.Property(e => e.VipLevel).HasColumnName("vip_level");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
