﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD392_BE.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AreaId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "date", nullable: true),
                    DeletedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Area__3214EC079CAF76E2", x => x.Id);
                    table.UniqueConstraint("AK_Area_AreaId", x => x.AreaId);
                });

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "date", nullable: true),
                    DeletedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Session__3214EC079ABC5FB3", x => x.Id);
                    table.UniqueConstraint("AK_Session_SessionId", x => x.SessionId);
                });

            migrationBuilder.CreateTable(
                name: "Campus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampusId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    AreaId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "date", nullable: true),
                    DeletedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Campus__3214EC077B2628DA", x => x.Id);
                    table.UniqueConstraint("AK_Campus_CampusId", x => x.CampusId);
                    table.ForeignKey(
                        name: "FK_Campus_Area",
                        column: x => x.AreaId,
                        principalTable: "Area",
                        principalColumn: "AreaId");
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    AreaId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<int>(type: "int", nullable: false),
                    OpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "date", nullable: true),
                    DeletedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Store__3214EC0776229960", x => x.Id);
                    table.UniqueConstraint("AK_Store_StoreId", x => x.StoreId);
                    table.ForeignKey(
                        name: "FK_Store_Area",
                        column: x => x.AreaId,
                        principalTable: "Area",
                        principalColumn: "AreaId");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    UserName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Email = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    CampusId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Phone = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "date", nullable: true),
                    DeletedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    AccessToken = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    RefreshToken = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__3214EC078230A9E0", x => x.Id);
                    table.UniqueConstraint("AK_User_UserId", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Campus",
                        column: x => x.CampusId,
                        principalTable: "Campus",
                        principalColumn: "CampusId");
                });

            migrationBuilder.CreateTable(
                name: "Food",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StoreId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cate = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "date", nullable: true),
                    DeletedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Food__3214EC07C7E84DA7", x => x.Id);
                    table.UniqueConstraint("AK_Food_FoodId", x => x.FoodId);
                    table.ForeignKey(
                        name: "FK_Food_Store",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "StoreId");
                });

            migrationBuilder.CreateTable(
                name: "StoreSession",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreSessionId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    SessionId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    StoreId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StoreSes__3214EC07C073EE8D", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreSession_Session",
                        column: x => x.SessionId,
                        principalTable: "Session",
                        principalColumn: "SessionId");
                    table.ForeignKey(
                        name: "FK_StoreSession_Store",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "StoreId");
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransationId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    UserId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Amonut = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Transact__3214EC07466B0EBC", x => x.Id);
                    table.UniqueConstraint("AK_Transaction_TransationId", x => x.TransationId);
                    table.ForeignKey(
                        name: "FK_Transaction_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    UserId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    SessionId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    TransationId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order__3214EC07CCDC7598", x => x.Id);
                    table.UniqueConstraint("AK_Order_OrderId", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_Session",
                        column: x => x.SessionId,
                        principalTable: "Session",
                        principalColumn: "SessionId");
                    table.ForeignKey(
                        name: "FK_Order_Store",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "StoreId");
                    table.ForeignKey(
                        name: "FK_Order_Transaction",
                        column: x => x.TransationId,
                        principalTable: "Transaction",
                        principalColumn: "TransationId");
                    table.ForeignKey(
                        name: "FK_Order_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDetailId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    OrderId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    FoodId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderDet__3214EC0711CB4869", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Food",
                        column: x => x.FoodId,
                        principalTable: "Food",
                        principalColumn: "FoodId");
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Area__70B820491680D86A",
                table: "Area",
                column: "AreaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Campus_AreaId",
                table: "Campus",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "UQ__Campus__FD598DD7B20764EA",
                table: "Campus",
                column: "CampusId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Food_StoreId",
                table: "Food",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "UQ__Food__856DB3EAC803D9F9",
                table: "Food",
                column: "FoodId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_SessionId",
                table: "Order",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_StoreId",
                table: "Order",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_TransationId",
                table: "Order",
                column: "TransationId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__Order__C3905BCE483BB568",
                table: "Order",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_FoodId",
                table: "OrderDetail",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "UQ__OrderDet__D3B9D36D5AF1F417",
                table: "OrderDetail",
                column: "OrderDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Session__C9F49291EA42D802",
                table: "Session",
                column: "SessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Store_AreaId",
                table: "Store",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "UQ__Store__3B82F1006731C5ED",
                table: "Store",
                column: "StoreId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreSession_SessionId",
                table: "StoreSession",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreSession_StoreId",
                table: "StoreSession",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "UQ__StoreSes__6E52FC495720368B",
                table: "StoreSession",
                column: "StoreSessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_UserId",
                table: "Transaction",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__Transact__B1E731541DD0AF25",
                table: "Transaction",
                column: "TransationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_CampusId",
                table: "User",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "UQ__User__1788CC4D67B058A3",
                table: "User",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "StoreSession");

            migrationBuilder.DropTable(
                name: "Food");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Campus");

            migrationBuilder.DropTable(
                name: "Area");
        }
    }
}