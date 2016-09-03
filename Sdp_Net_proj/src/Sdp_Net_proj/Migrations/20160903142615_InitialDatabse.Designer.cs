using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Sdp_Net_proj.Models;

namespace Sdp_Net_proj.Migrations
{
    [DbContext(typeof(WorldContext))]
    [Migration("20160903142615_InitialDatabse")]
    partial class InitialDatabse
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sdp_Net_proj.Models.Game", b =>
                {
                    b.Property<string>("ID");

                    b.Property<string>("UserID");

                    b.Property<string>("player1");

                    b.Property<int>("player1_score");

                    b.Property<string>("player2");

                    b.Property<int>("player2_score");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Sdp_Net_proj.Models.Step", b =>
                {
                    b.Property<string>("ID");

                    b.Property<string>("GameID");

                    b.Property<string>("Letters_Indexs");

                    b.Property<int>("Player1_Score");

                    b.Property<int>("Player2_Score");

                    b.Property<int>("Step_Number");

                    b.Property<string>("Word");

                    b.Property<int>("Word_Score");

                    b.HasKey("ID");

                    b.HasIndex("GameID");

                    b.ToTable("Step");
                });

            modelBuilder.Entity("Sdp_Net_proj.Models.User", b =>
                {
                    b.Property<string>("ID");

                    b.Property<string>("Email");

                    b.Property<string>("Passowrd");

                    b.Property<string>("User_Name");

                    b.Property<int>("User_score");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Sdp_Net_proj.Models.Game", b =>
                {
                    b.HasOne("Sdp_Net_proj.Models.User")
                        .WithMany("Games")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("Sdp_Net_proj.Models.Step", b =>
                {
                    b.HasOne("Sdp_Net_proj.Models.Game")
                        .WithMany("Game_Steps")
                        .HasForeignKey("GameID");
                });
        }
    }
}
