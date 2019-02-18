using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizIt.Models;

namespace QuizIt.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Quiz> Questions { get; set; }
        public DbSet<Quiz> Tracks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuizQuestion>()
                .HasKey(x => new { x.QuizId, x.QuestionId });
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Battle>()
            //    .HasOne(p => p.BattleLog)
            //    .WithOne(i => i.Battle)
            //    .HasForeignKey<BattleLog>(b => b.BattleId);


            //modelBuilder.Entity<Samurai>()
            //    .HasOne(p => p.SecretIdentity)
            //    .WithOne(i => i.Samurai)
            //    .HasForeignKey<SecretIdentity>(s => s.SamuraiId);

            //modelBuilder.Entity<Battle>()
            //    .HasOne(p => p.BattleLog)
            //    .WithOne(i => i.Battle)
            //    .HasForeignKey<BattleLog>(b => b.BattleId);

            //modelBuilder.Entity<SamuraiBattle>().
            //    HasKey(x => new { x.SamuraiId, x.BattleId });
            //base.OnModelCreating;
        }

    }
}
