﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using eHospitalServer.DataAccess.Context;

#nullable disable

namespace eHospitalServer.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("text")
                        .HasColumnName("normalized_name");

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.ToTable("roles", (string)null);
                });

            modelBuilder.Entity("eHospitalServer.Entities.Models.Appointment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("DoctorId")
                        .HasColumnType("uuid")
                        .HasColumnName("doctor_id");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_date");

                    b.Property<string>("EpicrisisReport")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("epicrisis_report");

                    b.Property<bool>("IsItFinished")
                        .HasColumnType("boolean")
                        .HasColumnName("is_it_finished");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("uuid")
                        .HasColumnName("patient_id");

                    b.Property<decimal>("Price")
                        .HasColumnType("money")
                        .HasColumnName("price");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_date");

                    b.HasKey("Id")
                        .HasName("pk_appointments");

                    b.HasIndex("DoctorId")
                        .HasDatabaseName("ix_appointments_doctor_id");

                    b.HasIndex("PatientId")
                        .HasDatabaseName("ix_appointments_patient_id");

                    b.ToTable("appointments", (string)null);
                });

            modelBuilder.Entity("eHospitalServer.Entities.Models.DoctorDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal>("AppointmentPrice")
                        .HasColumnType("money")
                        .HasColumnName("appointment_price");

                    b.Property<int>("Specialty")
                        .HasColumnType("integer")
                        .HasColumnName("specialty");

                    b.Property<List<string>>("WorkingDays")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("working_days");

                    b.HasKey("Id")
                        .HasName("pk_doctor_details");

                    b.ToTable("doctor_details", (string)null);
                });

            modelBuilder.Entity("eHospitalServer.Entities.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer")
                        .HasColumnName("access_failed_count");

                    b.Property<string>("BloodType")
                        .HasColumnType("text")
                        .HasColumnName("blood_type");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("date")
                        .HasColumnName("date_of_birth");

                    b.Property<Guid?>("DoctorDetailId")
                        .HasColumnType("uuid")
                        .HasColumnName("doctor_detail_id");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<int>("EmailConfirmCode")
                        .HasColumnType("integer")
                        .HasColumnName("email_confirm_code");

                    b.Property<DateTime>("EmailConfirmCodeSendDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("email_confirm_code_send_date");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("email_confirmed");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<int?>("ForgotPasswordCode")
                        .HasColumnType("integer")
                        .HasColumnName("forgot_password_code");

                    b.Property<DateTime?>("ForgotPasswordCodeSendDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("forgot_password_code_send_date");

                    b.Property<string>("FullAddress")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("full_address");

                    b.Property<string>("IdentityNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("identity_number");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("lockout_enabled");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("lockout_end");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text")
                        .HasColumnName("normalized_email");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text")
                        .HasColumnName("normalized_user_name");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("phone_number_confirmed");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text")
                        .HasColumnName("refresh_token");

                    b.Property<DateTime?>("RefreshTokenExpires")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("refresh_token_expires");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text")
                        .HasColumnName("security_stamp");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("two_factor_enabled");

                    b.Property<string>("UserName")
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.Property<int>("UserType")
                        .HasColumnType("integer")
                        .HasColumnName("user_type");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("DoctorDetailId")
                        .HasDatabaseName("ix_users_doctor_detail_id");

                    b.HasIndex("EmailConfirmCode")
                        .IsUnique()
                        .HasDatabaseName("ix_users_email_confirm_code");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("eHospitalServer.Entities.Models.Appointment", b =>
                {
                    b.HasOne("eHospitalServer.Entities.Models.User", "Doctor")
                        .WithMany()
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_appointments_users_doctor_id");

                    b.HasOne("eHospitalServer.Entities.Models.User", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_appointments_users_patient_id");

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("eHospitalServer.Entities.Models.User", b =>
                {
                    b.HasOne("eHospitalServer.Entities.Models.DoctorDetail", "DoctorDetail")
                        .WithMany()
                        .HasForeignKey("DoctorDetailId")
                        .HasConstraintName("fk_users_doctor_details_doctor_detail_id");

                    b.Navigation("DoctorDetail");
                });
#pragma warning restore 612, 618
        }
    }
}
