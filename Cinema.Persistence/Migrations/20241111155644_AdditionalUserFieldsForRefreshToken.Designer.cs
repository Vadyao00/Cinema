﻿// <auto-generated />
using System;
using Cinema.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Cinema.Persistence.Migrations
{
    [DbContext(typeof(CinemaContext))]
    [Migration("20241111155644_AdditionalUserFieldsForRefreshToken")]
    partial class AdditionalUserFieldsForRefreshToken
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Cinema.Domain.Entities.Actor", b =>
                {
                    b.Property<Guid>("ActorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ActorID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ActorId")
                        .HasName("PK__Actors__57B3EA2BCF3E9D69");

                    b.ToTable("Actors");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.Employee", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("EmployeeID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("EmployeeId")
                        .HasName("PK__Employee__7AD04FF117D3D12D");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.Event", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("EventID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time");

                    b.Property<decimal>("TicketPrice")
                        .HasColumnType("money");

                    b.HasKey("EventId")
                        .HasName("PK__Events__7944C8707A234F27");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.Genre", b =>
                {
                    b.Property<Guid>("GenreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("GenreID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("GenreId")
                        .HasName("PK__Genres__0385055ED3A445FA");

                    b.HasIndex(new[] { "Name" }, "UQ__Genres__737584F6D2AF413C")
                        .IsUnique();

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.Movie", b =>
                {
                    b.Property<Guid>("MovieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("MovieID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<int?>("AgeRestriction")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<TimeOnly>("Duration")
                        .HasColumnType("time");

                    b.Property<Guid>("GenreId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("GenreID");

                    b.Property<string>("ProductionCompany")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("MovieId")
                        .HasName("PK__Movies__4BD2943A46A6CE43");

                    b.HasIndex("GenreId");

                    b.HasIndex(new[] { "Title" }, "UQ__Movies__2CB664DCFFA8EF0F")
                        .IsUnique();

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.Seat", b =>
                {
                    b.Property<Guid>("SeatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("SeatID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<Guid?>("EventId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("EventID");

                    b.Property<bool>("IsOccupied")
                        .HasColumnType("bit");

                    b.Property<int?>("SeatNumber")
                        .HasColumnType("int");

                    b.Property<Guid?>("ShowtimeId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ShowtimeID");

                    b.HasKey("SeatId")
                        .HasName("PK__Seats__311713D382054537");

                    b.HasIndex("EventId");

                    b.HasIndex("ShowtimeId");

                    b.HasIndex(new[] { "SeatNumber", "ShowtimeId", "EventId" }, "UQ_SeatNumber_ShowtimeEvent")
                        .IsUnique()
                        .HasFilter("[SeatNumber] IS NOT NULL AND [ShowtimeID] IS NOT NULL AND [EventID] IS NOT NULL");

                    b.ToTable("Seats");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.Showtime", b =>
                {
                    b.Property<Guid>("ShowtimeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ShowtimeID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time");

                    b.Property<Guid>("MovieId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("MovieID");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time");

                    b.Property<decimal>("TicketPrice")
                        .HasColumnType("money");

                    b.HasKey("ShowtimeId")
                        .HasName("PK__Showtime__32D31FC0DEA066F3");

                    b.HasIndex("MovieId");

                    b.ToTable("Showtimes");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.Ticket", b =>
                {
                    b.Property<Guid>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("TicketID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<DateOnly>("PurchaseDate")
                        .HasColumnType("date");

                    b.Property<Guid>("SeatId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("SeatID");

                    b.HasKey("TicketId")
                        .HasName("PK__Tickets__712CC627F89A3261");

                    b.HasIndex("SeatId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Cinema.Domain.Entities.WorkLog", b =>
                {
                    b.Property<Guid>("WorkLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("WorkLogID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("EmployeeID");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.Property<int>("WorkExperience")
                        .HasColumnType("int");

                    b.Property<decimal>("WorkHours")
                        .HasColumnType("decimal(5, 2)");

                    b.HasKey("WorkLogId")
                        .HasName("PK__WorkLog__FE542DC20A525C37");

                    b.HasIndex("EmployeeId");

                    b.ToTable("WorkLog", (string)null);
                });

            modelBuilder.Entity("EventEmployee", b =>
                {
                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("EventID");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("EmployeeID");

                    b.HasKey("EventId", "EmployeeId")
                        .HasName("PK__EventEmp__7EE9CC8FE1CC6E80");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EventEmployees", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "964ce008-c88a-479c-aaad-0f7539aac2ac",
                            Name = "Manager",
                            NormalizedName = "MANAGER"
                        },
                        new
                        {
                            Id = "ef4bfc91-6cff-4130-b9d1-98da015acc5d",
                            Name = "Administrator",
                            NormalizedName = "ADMINISTRATOR"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("MovieActor", b =>
                {
                    b.Property<Guid>("MovieId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("MovieID");

                    b.Property<Guid>("ActorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ActorID");

                    b.HasKey("MovieId", "ActorId")
                        .HasName("PK__MovieAct__EEA9AA98553E3589");

                    b.HasIndex("ActorId");

                    b.ToTable("MovieActors", (string)null);
                });

            modelBuilder.Entity("ShowtimeEmloyee", b =>
                {
                    b.Property<Guid>("ShowtimeId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ShowtimeID");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("EmployeeID");

                    b.HasKey("ShowtimeId", "EmployeeId")
                        .HasName("PK__Showtime__357E1B3F23177BD1");

                    b.HasIndex("EmployeeId");

                    b.ToTable("ShowtimeEmloyees", (string)null);
                });

            modelBuilder.Entity("Cinema.Domain.Entities.Movie", b =>
                {
                    b.HasOne("Cinema.Domain.Entities.Genre", "Genre")
                        .WithMany("Movies")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Movies__GenreID__3F466844");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.Seat", b =>
                {
                    b.HasOne("Cinema.Domain.Entities.Event", "Event")
                        .WithMany("Seats")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK__Seats__EventID__60A75C0F");

                    b.HasOne("Cinema.Domain.Entities.Showtime", "Showtime")
                        .WithMany("Seats")
                        .HasForeignKey("ShowtimeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK__Seats__ShowtimeI__5FB337D6");

                    b.Navigation("Event");

                    b.Navigation("Showtime");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.Showtime", b =>
                {
                    b.HasOne("Cinema.Domain.Entities.Movie", "Movie")
                        .WithMany("Showtimes")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Showtimes__Movie__49C3F6B7");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.Ticket", b =>
                {
                    b.HasOne("Cinema.Domain.Entities.Seat", "Seat")
                        .WithMany("Tickets")
                        .HasForeignKey("SeatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Tickets__SeatID__66603565");

                    b.Navigation("Seat");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.WorkLog", b =>
                {
                    b.HasOne("Cinema.Domain.Entities.Employee", "Employee")
                        .WithMany("WorkLogs")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__WorkLog__Employe__6A30C649");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("EventEmployee", b =>
                {
                    b.HasOne("Cinema.Domain.Entities.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__EventEmpl__Emplo__5AEE82B9");

                    b.HasOne("Cinema.Domain.Entities.Event", null)
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__EventEmpl__Event__59FA5E80");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Cinema.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Cinema.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cinema.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Cinema.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MovieActor", b =>
                {
                    b.HasOne("Cinema.Domain.Entities.Actor", null)
                        .WithMany()
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__MovieActo__Actor__45F365D3");

                    b.HasOne("Cinema.Domain.Entities.Movie", null)
                        .WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__MovieActo__Movie__44FF419A");
                });

            modelBuilder.Entity("ShowtimeEmloyee", b =>
                {
                    b.HasOne("Cinema.Domain.Entities.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__ShowtimeE__Emplo__52593CB8");

                    b.HasOne("Cinema.Domain.Entities.Showtime", null)
                        .WithMany()
                        .HasForeignKey("ShowtimeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__ShowtimeE__Showt__5165187F");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.Employee", b =>
                {
                    b.Navigation("WorkLogs");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.Event", b =>
                {
                    b.Navigation("Seats");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.Genre", b =>
                {
                    b.Navigation("Movies");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.Movie", b =>
                {
                    b.Navigation("Showtimes");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.Seat", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("Cinema.Domain.Entities.Showtime", b =>
                {
                    b.Navigation("Seats");
                });
#pragma warning restore 612, 618
        }
    }
}