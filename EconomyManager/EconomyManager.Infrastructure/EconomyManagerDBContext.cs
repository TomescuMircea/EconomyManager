using EconomyManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EconomyManager.Infrastructure
{
    public class EconomyManagerDBContext : DbContext
    {
        public string DbPath { get; set; }
        public EconomyManagerDBContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            string path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}EconomyManager.db";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder for User
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(x => x.Name)
                .HasMaxLength(256);

            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<User>()
                .Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<User>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(10);

            // modelBuilder for Wallet
            modelBuilder.Entity<Wallet>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Wallet>()
                .HasIndex(x => x.Name)
                .IsUnique();

            modelBuilder.Entity<Wallet>()
                .Property(x => x.Name)
                .IsRequired();

            modelBuilder.Entity<Wallet>()
                .Property(x => x.Description)
                .HasMaxLength(10000);

            // model for UserHasWallet & ManytoMany Relationship

            modelBuilder.Entity<UserHasWallet>()
                .HasKey(x => new { x.UserId, x.WalletId });

            // ManytoMany Relationship
            modelBuilder.Entity<UserHasWallet>()
                .HasOne<User>(x => x.User)
                .WithMany(x => x.UserHasWallets)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<UserHasWallet>()
                .HasOne<Wallet>(x => x.Wallet)
                .WithMany(x => x.UserHasWallets)
                .HasForeignKey(x => x.WalletId);

            // model for Movements && OneToMany relationship

            modelBuilder.Entity<Movement>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Movement>()
                .HasIndex(x => x.Id);

            modelBuilder.Entity<Movement>()
                .Property(x => x.Quantity)
                .IsRequired();

            modelBuilder.Entity<Movement>()
                .Property(x => x.IsIncome)
                .IsRequired();


            modelBuilder.Entity<Movement>()
                .Property(x => x.MovementDate)
                .IsRequired();

            
            /*modelBuilder.Entity<Movement>()
                .Property(x => x.Wallet)
                .IsRequired();*/

            modelBuilder.Entity<Movement>()
                .Property(x => x.WalletId)
                .IsRequired();

            // OneToMany relationship
            
            modelBuilder.Entity<Movement>()
                .HasOne(x => x.Wallet)
                .WithMany(x => x.Movements)
                .HasForeignKey(x => x.WalletId);
            
            
            modelBuilder.Entity<Movement>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Movements)
                .HasForeignKey(x => x.CategoryId);

            // model for Plan & OneToMany relationship

            modelBuilder.Entity<Plan>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Plan>()
                .Property(x => x.Name)
                .IsRequired();

            modelBuilder.Entity<Plan>()
                .Property(x => x.Name)
                .HasMaxLength(256);

            modelBuilder.Entity<Plan>()
                .Property(x =>x.Objective)
                .IsRequired();

            // OneToMany relationship

            modelBuilder.Entity<Plan>()
                .HasOne(x => x.Wallet)
                .WithMany(x => x.Plans)
                .HasForeignKey(x => x.WalletId);

            //model for Categories
            modelBuilder.Entity<Category>()
                .HasIndex(x => new { x.Name, x.UserId})
                .IsUnique();

            modelBuilder.Entity<Category>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(256);

            //model for Automatic Movements
            modelBuilder.Entity<AutomaticMovement>()
                .Property(x => x.Quantity)
                .IsRequired();

            modelBuilder.Entity<AutomaticMovement>()
                .Property(x => x.Active)
                .IsRequired();

            modelBuilder.Entity<AutomaticMovement>()
                .Property(x => x.IsIncome)
                .IsRequired();

            modelBuilder.Entity<Attachment>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Attachment>()
                .Property(x => x.Url)
                .IsRequired();


            modelBuilder.Entity<Attachment>()
                .HasKey(x => x.MovementId);

        }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<AutomaticMovement> AutomaticMovements { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserHasWallet> UserHasWallets { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
    }
}
