﻿// <auto-generated />
using System;
using BeDoHave.Data.AccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BeDoHave.Data.AccessLayer.Migrations
{
    [DbContext(typeof(DocumentDbContext))]
    [Migration("20210928182138_RemoveTitlesFromPageLink")]
    partial class RemoveTitlesFromPageLink
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BeDoHave.Data.AccessLayer.UserDefinedTables.PageTree", b =>
                {
                    b.Property<string>("Children")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IconColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IconName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("QueryRoot")
                        .HasColumnType("bit");

                    b.Property<int>("RootId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("PageTrees");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.Organisation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Organisations");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.OrganisationInvite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("Accepted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("InvitedId")
                        .HasColumnType("int");

                    b.Property<int>("InviterId")
                        .HasColumnType("int");

                    b.Property<int>("OrganisationId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("InvitedId");

                    b.HasIndex("InviterId");

                    b.HasIndex("OrganisationId");

                    b.ToTable("OrganisationInvites");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.OrganisationMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<int>("OrganisationId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("OrganisationId");

                    b.ToTable("OrganisationMember");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.Page", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Archived")
                        .HasColumnType("bit");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("IconColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IconName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("OrganisationDirect")
                        .HasColumnType("bit");

                    b.Property<int?>("OrganisationId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationId");

                    b.HasIndex("UserId");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.PageLink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AncestorPageId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Depth")
                        .HasColumnType("int");

                    b.Property<int>("DescendantPageId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AncestorPageId");

                    b.HasIndex("DescendantPageId");

                    b.ToTable("PageLink");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.TagPage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("PageId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PageId");

                    b.HasIndex("TagId");

                    b.ToTable("TagPage");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentityId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BeDoHave.ElasticSearch.Entities.TagWeight", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.ToTable("TagWeights");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.Organisation", b =>
                {
                    b.HasOne("BeDoHave.Data.Core.Entities.User", "Author")
                        .WithMany("OrganisationsAuthor")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.OrganisationInvite", b =>
                {
                    b.HasOne("BeDoHave.Data.Core.Entities.User", "Invited")
                        .WithMany("OrganisationInvites")
                        .HasForeignKey("InvitedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BeDoHave.Data.Core.Entities.User", "Inviter")
                        .WithMany("OrganisationInviters")
                        .HasForeignKey("InviterId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BeDoHave.Data.Core.Entities.Organisation", "Organisation")
                        .WithMany("OrganisationInvites")
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Invited");

                    b.Navigation("Inviter");

                    b.Navigation("Organisation");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.OrganisationMember", b =>
                {
                    b.HasOne("BeDoHave.Data.Core.Entities.User", "Member")
                        .WithMany("OrganisationMembers")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BeDoHave.Data.Core.Entities.Organisation", "Organisation")
                        .WithMany("OrganisationMembers")
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");

                    b.Navigation("Organisation");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.Page", b =>
                {
                    b.HasOne("BeDoHave.Data.Core.Entities.Organisation", "Organisation")
                        .WithMany("Pages")
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("BeDoHave.Data.Core.Entities.User", "User")
                        .WithMany("Pages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organisation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.PageLink", b =>
                {
                    b.HasOne("BeDoHave.Data.Core.Entities.Page", "AncestorPage")
                        .WithMany("DescendantsLinks")
                        .HasForeignKey("AncestorPageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeDoHave.Data.Core.Entities.Page", "DescendantPage")
                        .WithMany("AncestorsLinks")
                        .HasForeignKey("DescendantPageId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AncestorPage");

                    b.Navigation("DescendantPage");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.TagPage", b =>
                {
                    b.HasOne("BeDoHave.Data.Core.Entities.Page", "Page")
                        .WithMany("TagPages")
                        .HasForeignKey("PageId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BeDoHave.Data.Core.Entities.Tag", "Tag")
                        .WithMany("TagPages")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Page");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.Organisation", b =>
                {
                    b.Navigation("OrganisationInvites");

                    b.Navigation("OrganisationMembers");

                    b.Navigation("Pages");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.Page", b =>
                {
                    b.Navigation("AncestorsLinks");

                    b.Navigation("DescendantsLinks");

                    b.Navigation("TagPages");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.Tag", b =>
                {
                    b.Navigation("TagPages");
                });

            modelBuilder.Entity("BeDoHave.Data.Core.Entities.User", b =>
                {
                    b.Navigation("OrganisationInviters");

                    b.Navigation("OrganisationInvites");

                    b.Navigation("OrganisationMembers");

                    b.Navigation("OrganisationsAuthor");

                    b.Navigation("Pages");
                });
#pragma warning restore 612, 618
        }
    }
}
