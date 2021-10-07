using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace BeDoHave.Shared.Factories
{
    public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext>
        where TContext : DbContext
    {
        public TContext CreateDbContext(string[] args)
        {
            return Create(Path.Combine(Directory.GetCurrentDirectory()), Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        }

        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

        public TContext Create()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var basePath = Assembly.GetExecutingAssembly().Location;

            return Create(basePath, environmentName);
        }

        private TContext Create(string basePath, string environmentName)
        {
            Console.WriteLine(basePath);
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            var connectionString = config.GetConnectionString("DocumentDatabaseConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Could not find a connection string named 'DocumentDatabaseConnection'.");
            }

            return Create(connectionString);
        }

        private TContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException(
                     $"{nameof(connectionString)} is null or empty.",
                     nameof(connectionString));
            }

            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            Console.WriteLine("DesignTimeDbContextFactory.Create(string): Connection string: {0}", connectionString);

            optionsBuilder.UseSqlServer(connectionString);

            var options = optionsBuilder.Options;

            return CreateNewInstance(options);
        }
    }
}
