using System.Linq;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RickAndMorty.DataAccess.Contexts;
using RickAndMorty.DataAccess.Models;
using RickAndMorty.UnitTests.Base;
using Xunit;

namespace RickAndMorty.UnitTests.Tests
{
    public class CharacterContextsTests : BaseContextTest<CharacterContext>
    {
        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(100)]
        public void Test1(int numberOfCustomers)
        {
            using var context = new CharacterContext(Options);

            var characters = Fixture.Build<Character>().CreateMany(numberOfCustomers).ToList();

            context.Character.AddRange(characters);
            context.SaveChanges();

            var entities = context.Character
                .Include(character => character.Location)
                .Include(character => character.Origin)
                .Include(character => character.CharacterEpisodes)
                .ThenInclude(characterEpisode => characterEpisode.Episode).ToList();

            entities.Count.Should().Be(characters.Count);
        }
    }
}
