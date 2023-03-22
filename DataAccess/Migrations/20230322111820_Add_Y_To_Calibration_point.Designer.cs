﻿// <auto-generated />
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20230322111820_Add_Y_To_Calibration_point")]
    partial class Add_Y_To_Calibration_point
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.30");

            modelBuilder.Entity("DataAccess.Models.CalibrationCell", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ParameterId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ParameterIdY")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("UseCastomValue")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("UseCastomValueY")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Value")
                        .HasColumnType("REAL");

                    b.Property<float>("ValueY")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("CalibrationCells");
                });

            modelBuilder.Entity("DataAccess.Models.ConnectSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Baudrate")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ComName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("CommandFlag")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ConnectionTimeout")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Ip")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("ModbAddr")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Parity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PortNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StartReg")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Way")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ConnectSettingses");
                });

            modelBuilder.Entity("DataAccess.Models.Parameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCyclic")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Length")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Notification")
                        .HasColumnType("TEXT");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RegNum")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RegType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Parameters");
                });

            modelBuilder.Entity("DataAccess.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Login")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
