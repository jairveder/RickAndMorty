using Microsoft.EntityFrameworkCore;
using RickAndMorty.DataAccess.Models;

namespace RickAndMorty.DataAccess
{
    public class CharacterContext : DbContext
    {
        public static string[] Tables = new string[] { "Character", "Location", "Origin", "Episode", "CharacterEpisode" };
        public CharacterContext(DbContextOptions<CharacterContext> options) : base(options) { }
        public DbSet<Character> Character { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Origin> Origin { get; set; }
        public DbSet<Episode> Episode { get; set; }
        public DbSet<CharacterEpisode> CharacterEpisode { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterEpisode>()
                .HasKey(bc => new { bc.CharacterId, bc.EpisodeId });
            modelBuilder.Entity<CharacterEpisode>()
                .HasOne(bc => bc.Episode)
                .WithMany(b => b.CharacterEpisodes)
                .HasForeignKey(bc => bc.EpisodeId);
            modelBuilder.Entity<CharacterEpisode>()
                .HasOne(bc => bc.Character)
                .WithMany(c => c.CharacterEpisodes)
                .HasForeignKey(bc => bc.CharacterId);
        }
    }
}
