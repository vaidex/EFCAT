﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sample.Model.Configuration;

#nullable disable

namespace Sample.Model.Migrations
{
    [DbContext(typeof(TestDbContext))]
    partial class TestDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Sample.Model.Entity.AdvancedEmailVerificationCode", b =>
                {
                    b.Property<Guid>("CODE_ID")
                        .HasColumnType("char(36)");

                    b.Property<int>("Random")
                        .HasColumnType("int(5)")
                        .HasColumnName("RANDOM");

                    b.HasKey("CODE_ID", "Random");

                    b.ToTable("ADVANCED_EMAIL_CODES", (string)null);
                });

            modelBuilder.Entity("Sample.Model.Entity.BadPerson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.HasKey("Id");

                    b.ToTable("BAD_PEOPLE", (string)null);
                });

            modelBuilder.Entity("Sample.Model.Entity.Code", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("ID");

                    b.Property<string>("DISCRIMINATOR")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ExpiresAt")
                        .IsRequired()
                        .HasColumnType("datetime(6)")
                        .HasColumnName("EXPIRES_AT");

                    b.Property<int?>("USER_ID")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("VALUE");

                    b.HasKey("Id");

                    b.HasIndex("USER_ID");

                    b.ToTable("USER_HAS_CODES", (string)null);

                    b.HasDiscriminator<string>("DISCRIMINATOR").HasValue("Code");
                });

            modelBuilder.Entity("Sample.Model.Entity.ForeignMultiKey", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("NAME");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("DATE");

                    b.HasKey("Id", "Name", "Date");

                    b.ToTable("FOREIGN_MULTI_KEYS", (string)null);
                });

            modelBuilder.Entity("Sample.Model.Entity.ForeignMultiKeyCustomized", b =>
                {
                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.Property<string>("NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<DateOnly>("DATE")
                        .HasColumnType("date");

                    b.HasKey("ID", "NAME");

                    b.HasIndex("ID", "NAME", "DATE")
                        .IsUnique();

                    b.ToTable("FOREIGN_MULTI_KEYS_CUSTOMIZED", (string)null);
                });

            modelBuilder.Entity("Sample.Model.Entity.ForeignMultiKeyDefault", b =>
                {
                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.Property<string>("NAME")
                        .HasColumnType("varchar(255)");

                    b.Property<DateOnly>("DATE")
                        .HasColumnType("date");

                    b.HasKey("ID", "NAME", "DATE");

                    b.ToTable("FOREIGN_MULTI_KEYS_DEFAULT", (string)null);
                });

            modelBuilder.Entity("Sample.Model.Entity.NicePerson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(32)")
                        .HasColumnName("FIRST_NAME");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("GENDER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar(32)")
                        .HasColumnName("LAST_NAME");

                    b.HasKey("Id");

                    b.ToTable("NICE_PEOPLE", (string)null);
                });

            modelBuilder.Entity("Sample.Model.Entity.NotGeneratedKeyImplement", b =>
                {
                    b.Property<int>("SecondId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("SECOND_ID");

                    b.HasKey("SecondId");

                    b.ToTable("NOT_GENERATED_KEY_IMPLEMENT", (string)null);
                });

            modelBuilder.Entity("Sample.Model.Entity.NotGeneratedKeyInherit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(32)")
                        .HasColumnName("NAME");

                    b.Property<int>("SecondId")
                        .HasColumnType("int")
                        .HasColumnName("SECOND_ID");

                    b.HasKey("Id");

                    b.ToTable("NOT_GENERATED_KEY_INHERIT", (string)null);
                });

            modelBuilder.Entity("Sample.Model.Entity.NotGeneratedKeyInherit2nd", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(32)")
                        .HasColumnName("NAME");

                    b.Property<int>("SecondId")
                        .HasColumnType("int")
                        .HasColumnName("SECOND_ID");

                    b.HasKey("Id");

                    b.ToTable("NOT_GENERATED_KEY_INHERIT_2ND", (string)null);
                });

            modelBuilder.Entity("Sample.Model.Entity.Role", b =>
                {
                    b.Property<int>("USER_ID")
                        .HasColumnType("int");

                    b.Property<string>("RoleName")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("ROLE_NAME");

                    b.HasKey("USER_ID", "RoleName");

                    b.ToTable("ROLES", (string)null);
                });

            modelBuilder.Entity("Sample.Model.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<decimal>("Balance")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)")
                        .HasColumnName("BALANCE");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("EMAIL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(16)")
                        .HasColumnName("NAME");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasColumnName("PASSWORD");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Email" }, "UQ_USER_EMAIL")
                        .IsUnique();

                    b.HasIndex(new[] { "Name" }, "UQ_USER_NAME")
                        .IsUnique();

                    b.ToTable("USERS", (string)null);
                });

            modelBuilder.Entity("Sample.Model.Entity.ZMail", b =>
                {
                    b.Property<int>("USER_ID")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("varchar(32)")
                        .HasColumnName("VALUE");

                    b.HasKey("USER_ID");

                    b.ToTable("ZMAILS", (string)null);
                });

            modelBuilder.Entity("Sample.Model.Entity.EmailVerificationCode", b =>
                {
                    b.HasBaseType("Sample.Model.Entity.Code");

                    b.HasDiscriminator().HasValue("EMAIL");
                });

            modelBuilder.Entity("Sample.Model.Entity.AdvancedEmailVerificationCode", b =>
                {
                    b.HasOne("Sample.Model.Entity.EmailVerificationCode", "Code")
                        .WithMany()
                        .HasForeignKey("CODE_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("EFCAT.Model.Data.Image", "Image", b1 =>
                        {
                            b1.Property<Guid>("AdvancedEmailVerificationCodeCODE_ID")
                                .HasColumnType("char(36)");

                            b1.Property<int>("AdvancedEmailVerificationCodeRandom")
                                .HasColumnType("int(5)");

                            b1.Property<byte[]>("Content")
                                .IsRequired()
                                .HasColumnType("longblob")
                                .HasColumnName("IMAGE_CONTENT");

                            b1.Property<string>("Type")
                                .IsRequired()
                                .HasColumnType("varchar(32)")
                                .HasColumnName("IMAGE_TYPE");

                            b1.HasKey("AdvancedEmailVerificationCodeCODE_ID", "AdvancedEmailVerificationCodeRandom");

                            b1.ToTable("ADVANCED_EMAIL_CODES");

                            b1.WithOwner()
                                .HasForeignKey("AdvancedEmailVerificationCodeCODE_ID", "AdvancedEmailVerificationCodeRandom");
                        });

                    b.Navigation("Code");

                    b.Navigation("Image")
                        .IsRequired();
                });

            modelBuilder.Entity("Sample.Model.Entity.BadPerson", b =>
                {
                    b.OwnsOne("Sample.Model.Entity.Person", "Person", b1 =>
                        {
                            b1.Property<int>("BadPersonId")
                                .HasColumnType("int");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasColumnType("varchar(32)")
                                .HasColumnName("PERSON_FIRST_NAME");

                            b1.Property<string>("Gender")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("PERSON_GENDER");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasColumnType("varchar(32)")
                                .HasColumnName("PERSON_LAST_NAME");

                            b1.HasKey("BadPersonId");

                            b1.ToTable("BAD_PEOPLE");

                            b1.WithOwner()
                                .HasForeignKey("BadPersonId");
                        });

                    b.Navigation("Person")
                        .IsRequired();
                });

            modelBuilder.Entity("Sample.Model.Entity.Code", b =>
                {
                    b.HasOne("Sample.Model.Entity.User", "User")
                        .WithMany("Codes")
                        .HasForeignKey("USER_ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sample.Model.Entity.ForeignMultiKeyCustomized", b =>
                {
                    b.HasOne("Sample.Model.Entity.ForeignMultiKey", "Key")
                        .WithOne()
                        .HasForeignKey("Sample.Model.Entity.ForeignMultiKeyCustomized", "ID", "NAME", "DATE")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Key");
                });

            modelBuilder.Entity("Sample.Model.Entity.ForeignMultiKeyDefault", b =>
                {
                    b.HasOne("Sample.Model.Entity.ForeignMultiKey", "Key")
                        .WithOne()
                        .HasForeignKey("Sample.Model.Entity.ForeignMultiKeyDefault", "ID", "NAME", "DATE")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Key");
                });

            modelBuilder.Entity("Sample.Model.Entity.NotGeneratedKeyImplement", b =>
                {
                    b.OwnsOne("Sample.Model.Entity.NotGeneratedKey", "NotGeneratedKey", b1 =>
                        {
                            b1.Property<int>("NotGeneratedKeyImplementSecondId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasColumnName("NOT_GENERATED_KEY_ID");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("varchar(32)")
                                .HasColumnName("NOT_GENERATED_KEY_NAME");

                            b1.HasKey("NotGeneratedKeyImplementSecondId");

                            b1.ToTable("NOT_GENERATED_KEY_IMPLEMENT");

                            b1.WithOwner()
                                .HasForeignKey("NotGeneratedKeyImplementSecondId");
                        });

                    b.Navigation("NotGeneratedKey")
                        .IsRequired();
                });

            modelBuilder.Entity("Sample.Model.Entity.Role", b =>
                {
                    b.HasOne("Sample.Model.Entity.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("USER_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sample.Model.Entity.User", b =>
                {
                    b.OwnsOne("Sample.Model.Entity.Implemented", "Impl", b1 =>
                        {
                            b1.Property<int>("UserId")
                                .HasColumnType("int");

                            b1.Property<string>("Text")
                                .IsRequired()
                                .HasColumnType("varchar(32)")
                                .HasColumnName("IMPL_TEXT");

                            b1.HasKey("UserId");

                            b1.ToTable("USERS");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("EFCAT.Model.Data.Image", "Image", b1 =>
                        {
                            b1.Property<int>("UserId")
                                .HasColumnType("int");

                            b1.Property<byte[]>("Content")
                                .IsRequired()
                                .HasColumnType("longblob")
                                .HasColumnName("IMAGE_CONTENT");

                            b1.Property<string>("Type")
                                .IsRequired()
                                .HasColumnType("varchar(32)")
                                .HasColumnName("IMAGE_TYPE");

                            b1.HasKey("UserId");

                            b1.ToTable("USERS");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Image");

                    b.Navigation("Impl");
                });

            modelBuilder.Entity("Sample.Model.Entity.ZMail", b =>
                {
                    b.HasOne("Sample.Model.Entity.User", "User")
                        .WithOne("Mail")
                        .HasForeignKey("Sample.Model.Entity.ZMail", "USER_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sample.Model.Entity.User", b =>
                {
                    b.Navigation("Codes");

                    b.Navigation("Mail");

                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
