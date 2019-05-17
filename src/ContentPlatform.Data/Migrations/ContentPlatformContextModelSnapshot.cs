﻿// <auto-generated />
using System;
using ContentPlatform.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ContentPlatform.Data.Migrations
{
    [DbContext(typeof(ContentPlatformContext))]
    partial class ContentPlatformContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ContentPlatform.Domain.Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("PublisherId");

                    b.HasKey("AuthorId");

                    b.HasIndex("PublisherId");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            AuthorId = 1,
                            Email = "a@a.a",
                            FirstName = "John",
                            LastName = "Snow",
                            PublisherId = 1
                        },
                        new
                        {
                            AuthorId = 2,
                            Email = "b@b.b",
                            FirstName = "Arya",
                            LastName = "Stark",
                            PublisherId = 1
                        },
                        new
                        {
                            AuthorId = 3,
                            Email = "c@c.c",
                            FirstName = "Margaery",
                            LastName = "Tyrell",
                            PublisherId = 1
                        });
                });

            modelBuilder.Entity("ContentPlatform.Domain.Blog", b =>
                {
                    b.Property<int>("BlogId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BlogType")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<int>("PublisherId");

                    b.Property<string>("Title");

                    b.Property<string>("Url");

                    b.HasKey("BlogId");

                    b.HasIndex("PublisherId");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("ContentPlatform.Domain.Contribution", b =>
                {
                    b.Property<int>("AuthorId");

                    b.Property<int>("PostId");

                    b.HasKey("AuthorId", "PostId");

                    b.HasIndex("PostId");

                    b.ToTable("Contributions");
                });

            modelBuilder.Entity("ContentPlatform.Domain.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.HasKey("LocationId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("ContentPlatform.Domain.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BlogId");

                    b.Property<string>("Content");

                    b.Property<string>("Title");

                    b.Property<string>("TitleBackgroundColor")
                        .IsRequired();

                    b.Property<Guid>("Version")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("NEWID()");

                    b.HasKey("PostId");

                    b.HasIndex("BlogId");

                    b.ToTable("BlogPosts");
                });

            modelBuilder.Entity("ContentPlatform.Domain.Publisher", b =>
                {
                    b.Property<int>("PublisherId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MainWebsite")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("PublisherId");

                    b.ToTable("Publishers");

                    b.HasData(
                        new
                        {
                            PublisherId = 1,
                            MainWebsite = "http://a.a.a",
                            Name = "G.R.R.M."
                        },
                        new
                        {
                            PublisherId = 2,
                            MainWebsite = "http://contoso.com",
                            Name = "Contoso"
                        });
                });

            modelBuilder.Entity("ContentPlatform.Domain.Author", b =>
                {
                    b.HasOne("ContentPlatform.Domain.Publisher", "Publisher")
                        .WithMany("Authors")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ContentPlatform.Domain.Blog", b =>
                {
                    b.HasOne("ContentPlatform.Domain.Publisher", "Publisher")
                        .WithMany("Blogs")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ContentPlatform.Domain.Contribution", b =>
                {
                    b.HasOne("ContentPlatform.Domain.Author", "Author")
                        .WithMany("Contributions")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ContentPlatform.Domain.Post", "Post")
                        .WithMany("Contributions")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ContentPlatform.Domain.Post", b =>
                {
                    b.HasOne("ContentPlatform.Domain.Blog", "Blog")
                        .WithMany("Posts")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
