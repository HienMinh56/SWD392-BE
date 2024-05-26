using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD392_BE.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class CampusFoodSystem : Migration
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
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Area__3214EC07B26BFC36", x => x.Id);
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
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Session__3214EC07E8C8F427", x => x.Id);
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
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Campus__3214EC07A5723796", x => x.Id);
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
                    Phone = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Store__3214EC07F087FC19", x => x.Id);
                    table.UniqueConstraint("AK_Store_StoreId", x => x.StoreId);
                    table.ForeignKey(
                        name: "FK_Store_Area",
                        column: x => x.AreaId,
                        principalTable: "Area",
                        principalColumn: "AreaId");
                });

            migrationBuilder.CreateTable(
                name: "AreaSession",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AreaSessionId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    SessionId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    AreaId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AreaSess__3214EC076D60CD96", x => x.Id);
                    table.UniqueConstraint("AK_AreaSession_AreaSessionId", x => x.AreaSessionId);
                    table.ForeignKey(
                        name: "FK_AreaSession_Area",
                        column: x => x.AreaId,
                        principalTable: "Area",
                        principalColumn: "AreaId");
                    table.ForeignKey(
                        name: "FK_AreaSession_Session",
                        column: x => x.SessionId,
                        principalTable: "Session",
                        principalColumn: "SessionId");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    CampusId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Phone = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AccessToken = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    RefreshToken = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__3214EC07644E0073", x => x.Id);
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
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Food__3214EC078EFC4B87", x => x.Id);
                    table.UniqueConstraint("AK_Food_FoodId", x => x.FoodId);
                    table.ForeignKey(
                        name: "FK_Food_Store",
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
                    Time = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Transact__3214EC0711B818F5", x => x.Id);
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
                    AreaSessionId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    TransationId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order__3214EC07C8EDD9E8", x => x.Id);
                    table.UniqueConstraint("AK_Order_OrderId", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_AreaSession",
                        column: x => x.AreaSessionId,
                        principalTable: "AreaSession",
                        principalColumn: "AreaSessionId");
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
                    table.PrimaryKey("PK__OrderDet__3214EC07531AB5AA", x => x.Id);
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
                name: "UQ__Area__70B82049341DE850",
                table: "Area",
                column: "AreaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AreaSession_AreaId",
                table: "AreaSession",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_AreaSession_SessionId",
                table: "AreaSession",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "UQ__AreaSess__E80F50256BAAC6F0",
                table: "AreaSession",
                column: "AreaSessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Campus_AreaId",
                table: "Campus",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "UQ__Campus__FD598DD7CC2976B3",
                table: "Campus",
                column: "CampusId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Food_StoreId",
                table: "Food",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "UQ__Food__856DB3EA0A736D4B",
                table: "Food",
                column: "FoodId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_AreaSessionId",
                table: "Order",
                column: "AreaSessionId");

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
                name: "UQ__Order__C3905BCED0AE1AF1",
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
                name: "UQ__OrderDet__D3B9D36D90383D15",
                table: "OrderDetail",
                column: "OrderDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Session__C9F49291B4A69BBB",
                table: "Session",
                column: "SessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Store_AreaId",
                table: "Store",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "UQ__Store__3B82F100A9787477",
                table: "Store",
                column: "StoreId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_UserId",
                table: "Transaction",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__Transact__B1E7315490DC15CB",
                table: "Transaction",
                column: "TransationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_CampusId",
                table: "User",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "UQ__User__1788CC4D63AAACC1",
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
                name: "Food");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "AreaSession");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Campus");

            migrationBuilder.DropTable(
                name: "Area");
        }
    }
}
