using Microsoft.EntityFrameworkCore;
using RickAndMorty.DataAccess.Models;

namespace RickAndMorty.DataAccess.Contexts
{
    public class CharacterContext : DbContext
    {
        public static readonly string[] Tables = { "Character", "Planet", "Episode", "CharacterEpisode" };
        public CharacterContext(DbContextOptions<CharacterContext> options) : base(options) { }
        public DbSet<Character> Character { get; set; }
        public DbSet<Planet> Planet { get; set; }
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
