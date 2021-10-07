using BeDoHave.Data.AccessLayer.UserDefinedTables;
using BeDoHave.Data.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BeDoHave.ElasticSearch.Entities;

namespace BeDoHave.Data.AccessLayer.Context
{
    public class DocumentDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Organisation> Organisations { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<OrganisationInvite> OrganisationInvites { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }


        // USER DEFINED TABLES
        public virtual DbSet<PageTree> PageTrees { get; set; }
        public virtual DbSet<TagWeight> TagWeights { get; set; }

        
        public DocumentDbContext(DbContextOptions<DocumentDbContext> options) : base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Page>()
                .HasOne(p => p.User)
                .WithMany(u => u.Pages)
                .HasForeignKey(p => p.UserId);
                
            modelBuilder.Entity<Page>()
            .HasMany(page => page.Descendants)
            .WithMany(page => page.Ancestors)
            .UsingEntity<PageLink>(
                pageLink => pageLink
                                .HasOne<Page>(pl => pl.DescendantPage)
                                .WithMany(p => p.AncestorsLinks)
                                .HasForeignKey(pl => pl.DescendantPageId)
                                .OnDelete(DeleteBehavior.NoAction),
                pageLink => pageLink
                                .HasOne<Page>(pl => pl.AncestorPage)
                                .WithMany(p => p.DescendantsLinks)
                                .HasForeignKey(pl => pl.AncestorPageId)
                                .OnDelete(DeleteBehavior.Cascade));



            modelBuilder.Entity<Organisation>()
                .HasMany(organisation => organisation.Pages)
                .WithOne(page => page.Organisation)
                .HasForeignKey(p => p.OrganisationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Organisation>()
                .HasMany(o => o.Members)
                .WithMany(u => u.Organisations)
                .UsingEntity<OrganisationMember>(
                    organisationMember => organisationMember
                        .HasOne(om => om.Member)
                        .WithMany(o => o.OrganisationMembers)
                        .HasForeignKey(om => om.MemberId)
                        .OnDelete(DeleteBehavior.NoAction),
                    organisationMember => organisationMember
                        .HasOne(om => om.Organisation)
                        .WithMany(o => o.OrganisationMembers)
                        .HasForeignKey(om => om.OrganisationId)
                    );


            modelBuilder.Entity<OrganisationInvite>()
                .HasOne(p => p.Organisation)
                .WithMany(u => u.OrganisationInvites)
                .HasForeignKey(p => p.OrganisationId);
            
            
            modelBuilder.Entity<OrganisationInvite>()
                .HasOne(p => p.Invited)
                .WithMany(u => u.OrganisationInvites)
                .HasForeignKey(p => p.InvitedId)
                .OnDelete(DeleteBehavior.NoAction);
            
            
            modelBuilder.Entity<OrganisationInvite>()
                .HasOne(p => p.Inviter)
                .WithMany(u => u.OrganisationInviters)
                .HasForeignKey(p => p.InviterId)
                .OnDelete(DeleteBehavior.NoAction);
            
            
            modelBuilder.Entity<Organisation>()
                .HasOne(o => o.Author)
                .WithMany(a => a.OrganisationsAuthor)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tag>()
                .HasMany(page => page.Pages)
                .WithMany(page => page.Tags)
                .UsingEntity<TagPage>(
                    tagPage => tagPage
                        .HasOne(tp => tp.Page)
                        .WithMany(p => p.TagPages)
                        .HasForeignKey(tp => tp.PageId)
                        .OnDelete(DeleteBehavior.NoAction),
                    tagPage => tagPage
                        .HasOne(tp => tp.Tag)
                        .WithMany(t => t.TagPages)
                        .HasForeignKey(tp => tp.TagId));

            modelBuilder.Entity<PageTree>().HasNoKey();
            modelBuilder.Entity<TagWeight>().HasNoKey();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow;

                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreatedAt = now;
                }

                ((BaseEntity)entity.Entity).UpdatedAt = now;
            }
        }

    }
}
