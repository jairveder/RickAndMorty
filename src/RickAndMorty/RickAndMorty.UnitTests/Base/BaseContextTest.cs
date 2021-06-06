using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.EntityFrameworkCore;

namespace RickAndMorty.UnitTests.Base
{
    public class BaseContextTest<T> where T : DbContext
    {
        public IFixture Fixture { get; } = GetFixture();
        
        public DbContextOptions<T> Options { get; } = new DbContextOptionsBuilder<T>()
            .UseInMemoryDatabase($"{nameof(T)}-{new Random().Next()}")
            .Options;

        private static IFixture GetFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
            return fixture;
        }
    }
}