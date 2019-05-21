using ContentPlatform.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace ContentPlatform.Domain.Tests
{
    public class ContentPlatformContextTests
    {
        private DbContextOptionsBuilder<ContentPlatformContext> optionsBuilder
            = new DbContextOptionsBuilder<ContentPlatformContext>();

        [Fact]
        public void HasNoSeedData()
        {
            optionsBuilder.UseInMemoryDatabase("HasNoSeedData");
            using (var context = new ContentPlatformContext(optionsBuilder.Options))
            {
                var locations = context.Publishers.ToList();
                Assert.Empty(locations);
            }
        }

        [Fact]
        public void HasSeedData()
        {
            optionsBuilder.UseInMemoryDatabase("HasSeedData");
            using (var context = new ContentPlatformContext(optionsBuilder.Options))
            {
                context.Database.EnsureCreated();
                Assert.NotEqual(0, context.Publishers.Count());
            }
        }

        [Fact]
        public void RetainChanges()
        {
            optionsBuilder.UseInMemoryDatabase("RetainChanges");
            using (var context = new ContentPlatformContext(optionsBuilder.Options))
            {
                context.Database.EnsureCreated();
            }
            using (var newContextSameDbName = new ContentPlatformContext(optionsBuilder.Options))
            {
                Assert.NotEqual(0, newContextSameDbName.Publishers.Count());
            }
        }

        [Fact]
        public void ResetWithNoSeedData()
        {
            optionsBuilder.UseInMemoryDatabase("ResetWithSeedData");
            using (var context = new ContentPlatformContext(optionsBuilder.Options))
            {
                context.Database.EnsureCreated();
            }
            optionsBuilder.UseInMemoryDatabase("NewInMemory");
            using (var newContextNewDbName = new ContentPlatformContext(optionsBuilder.Options))
            {
                Assert.Equal(0, newContextNewDbName.Publishers.Count());
            }
        }
    }
}