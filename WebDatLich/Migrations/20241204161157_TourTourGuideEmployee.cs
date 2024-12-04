using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDatLich.Migrations
{
    /// <inheritdoc />
    public partial class TourTourGuideEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    customer_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    full_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    phone_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    address = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__CD65CB859277D034", x => x.customer_id);
                });

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    destination_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    destination_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Destinat__5501539108393012", x => x.destination_id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    full_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    position = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    hire_date = table.Column<DateOnly>(type: "date", nullable: true),
                    phone_number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Employee__C52E0BA8EDF39BEC", x => x.employee_id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    username = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    customer_id = table.Column<int>(type: "int", nullable: true),
                    employee_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Accounts__F3DBC57334CD2C4B", x => x.username);
                    table.ForeignKey(
                        name: "FK__Accounts__custom__60A75C0F",
                        column: x => x.customer_id,
                        principalTable: "Customers",
                        principalColumn: "customer_id");
                    table.ForeignKey(
                        name: "FK__Accounts__employ__619B8048",
                        column: x => x.employee_id,
                        principalTable: "Employees",
                        principalColumn: "employee_id");
                });

            migrationBuilder.CreateTable(
                name: "TourGuides",
                columns: table => new
                {
                    guide_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employee_id = table.Column<int>(type: "int", nullable: false),
                    experience_years = table.Column<int>(type: "int", nullable: true),
                    language_spoken = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TourGuid__04A822417F30DD0C", x => x.guide_id);
                    table.ForeignKey(
                        name: "FK__TourGuide__emplo__4D94879B",
                        column: x => x.employee_id,
                        principalTable: "Employees",
                        principalColumn: "employee_id");
                });

            migrationBuilder.CreateTable(
                name: "Tours",
                columns: table => new
                {
                    tour_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tour_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    start_day = table.Column<DateOnly>(type: "date", nullable: true),
                    end_day = table.Column<DateOnly>(type: "date", nullable: true),
                    destination_id = table.Column<int>(type: "int", nullable: false),
                    guide_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tours__4B16B9E668FB5C36", x => x.tour_id);
                    table.ForeignKey(
                        name: "FK__Tours__destinati__5070F446",
                        column: x => x.destination_id,
                        principalTable: "Destinations",
                        principalColumn: "destination_id");
                    table.ForeignKey(
                        name: "FK__Tours__guide_id__5165187F",
                        column: x => x.guide_id,
                        principalTable: "TourGuides",
                        principalColumn: "guide_id");
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    booking_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    tour_id = table.Column<int>(type: "int", nullable: false),
                    booking_date = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    total_price = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bookings__5DE3A5B1E7DB8E54", x => x.booking_id);
                    table.ForeignKey(
                        name: "FK__Bookings__custom__5629CD9C",
                        column: x => x.customer_id,
                        principalTable: "Customers",
                        principalColumn: "customer_id");
                    table.ForeignKey(
                        name: "FK__Bookings__tour_i__571DF1D5",
                        column: x => x.tour_id,
                        principalTable: "Tours",
                        principalColumn: "tour_id");
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    feedback_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    tour_id = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: true),
                    comments = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__7A6B2B8C16B8AD1F", x => x.feedback_id);
                    table.ForeignKey(
                        name: "FK__Feedbacks__custo__5CD6CB2B",
                        column: x => x.customer_id,
                        principalTable: "Customers",
                        principalColumn: "customer_id");
                    table.ForeignKey(
                        name: "FK__Feedbacks__tour___5DCAEF64",
                        column: x => x.tour_id,
                        principalTable: "Tours",
                        principalColumn: "tour_id");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    payment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    payment_date = table.Column<DateOnly>(type: "date", nullable: true),
                    amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    payment_method = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payments__ED1FC9EA1BC8DF3B", x => x.payment_id);
                    table.ForeignKey(
                        name: "FK__Payments__bookin__59FA5E80",
                        column: x => x.booking_id,
                        principalTable: "Bookings",
                        principalColumn: "booking_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_customer_id",
                table: "Accounts",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_employee_id",
                table: "Accounts",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_customer_id",
                table: "Bookings",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_tour_id",
                table: "Bookings",
                column: "tour_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_customer_id",
                table: "Feedbacks",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_tour_id",
                table: "Feedbacks",
                column: "tour_id");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_booking_id",
                table: "Payments",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_TourGuides_employee_id",
                table: "TourGuides",
                column: "employee_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tours_destination_id",
                table: "Tours",
                column: "destination_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_guide_id",
                table: "Tours",
                column: "guide_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Tours");

            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "TourGuides");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
