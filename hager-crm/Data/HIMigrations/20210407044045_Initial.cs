using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hager_crm.Data.HIMigrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    AnnouncementID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(maxLength: 64, nullable: false),
                    Message = table.Column<string>(maxLength: 256, nullable: false),
                    PostedAt = table.Column<DateTime>(nullable: false),
                    Severity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.AnnouncementID);
                });

            migrationBuilder.CreateTable(
                name: "BillingTerms",
                columns: table => new
                {
                    BillingTermID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order = table.Column<int>(nullable: true),
                    Terms = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingTerms", x => x.BillingTermID);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order = table.Column<int>(nullable: true),
                    Category = table.Column<string>(maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ContractorTypes",
                columns: table => new
                {
                    ContractorTypeID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order = table.Column<int>(nullable: true),
                    Type = table.Column<string>(maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractorTypes", x => x.ContractorTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountryName = table.Column<string>(maxLength: 40, nullable: false),
                    CountryAbbr = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryID);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    CurrencyID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order = table.Column<int>(nullable: true),
                    CurrencyName = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.CurrencyID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTypes",
                columns: table => new
                {
                    CustomerTypeID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order = table.Column<int>(nullable: true),
                    Type = table.Column<string>(maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTypes", x => x.CustomerTypeID);
                });

            migrationBuilder.CreateTable(
                name: "EmploymentTypes",
                columns: table => new
                {
                    EmploymentTypeID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order = table.Column<int>(nullable: true),
                    Type = table.Column<string>(maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentTypes", x => x.EmploymentTypeID);
                });

            migrationBuilder.CreateTable(
                name: "JobPositions",
                columns: table => new
                {
                    JobPositionID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order = table.Column<int>(nullable: true),
                    Position = table.Column<string>(maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPositions", x => x.JobPositionID);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    ProvinceID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProvinceName = table.Column<string>(maxLength: 30, nullable: false),
                    ProvinceAbbr = table.Column<string>(maxLength: 10, nullable: false),
                    CountryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.ProvinceID);
                });

            migrationBuilder.CreateTable(
                name: "VendorTypes",
                columns: table => new
                {
                    VendorTypeID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order = table.Column<int>(nullable: true),
                    Type = table.Column<string>(maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorTypes", x => x.VendorTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    JobPositionID = table.Column<int>(nullable: true),
                    EmploymentTypeID = table.Column<int>(nullable: true),
                    EmployeeAddress1 = table.Column<string>(maxLength: 100, nullable: true),
                    EmployeeAddress2 = table.Column<string>(maxLength: 50, nullable: true),
                    EmployeeCity = table.Column<string>(nullable: true),
                    EmployeeProvinceID = table.Column<int>(nullable: true),
                    EmployeePostalCode = table.Column<string>(nullable: true),
                    EmployeeCountryID = table.Column<int>(nullable: true),
                    CellPhone = table.Column<long>(nullable: true),
                    WorkPhone = table.Column<long>(nullable: true),
                    Email = table.Column<string>(maxLength: 255, nullable: true),
                    DOB = table.Column<DateTime>(nullable: true),
                    Wage = table.Column<decimal>(nullable: true),
                    Expense = table.Column<decimal>(nullable: true),
                    DateJoined = table.Column<DateTime>(nullable: true),
                    KeyFob = table.Column<int>(nullable: true),
                    EmergencyContactName = table.Column<string>(maxLength: 50, nullable: true),
                    EmergencyContactPhone = table.Column<long>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(maxLength: 200, nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_Employees_Countries_EmployeeCountryID",
                        column: x => x.EmployeeCountryID,
                        principalTable: "Countries",
                        principalColumn: "CountryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Provinces_EmployeeProvinceID",
                        column: x => x.EmployeeProvinceID,
                        principalTable: "Provinces",
                        principalColumn: "ProvinceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_EmploymentTypes_EmploymentTypeID",
                        column: x => x.EmploymentTypeID,
                        principalTable: "EmploymentTypes",
                        principalColumn: "EmploymentTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_JobPositions_JobPositionID",
                        column: x => x.JobPositionID,
                        principalTable: "JobPositions",
                        principalColumn: "JobPositionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Location = table.Column<string>(maxLength: 50, nullable: true),
                    CreditCheck = table.Column<bool>(nullable: false),
                    DateChecked = table.Column<DateTime>(nullable: true),
                    BillingTermID = table.Column<int>(nullable: true),
                    CurrencyID = table.Column<int>(nullable: true),
                    Phone = table.Column<long>(nullable: true),
                    Website = table.Column<string>(maxLength: 50, nullable: true),
                    BillingAddress1 = table.Column<string>(maxLength: 100, nullable: true),
                    BillingAddress2 = table.Column<string>(maxLength: 50, nullable: true),
                    BillingCity = table.Column<string>(nullable: true),
                    BillingProvinceID = table.Column<int>(nullable: true),
                    BillingPostalCode = table.Column<string>(nullable: true),
                    BillingCountryID = table.Column<int>(nullable: true),
                    ShippingAddress1 = table.Column<string>(maxLength: 100, nullable: true),
                    ShippingAddress2 = table.Column<string>(maxLength: 50, nullable: true),
                    ShippingCity = table.Column<string>(nullable: true),
                    ShippingProvinceID = table.Column<int>(nullable: true),
                    ShippingPostalCode = table.Column<string>(nullable: true),
                    ShippingCountryID = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ContractorTypeID = table.Column<int>(nullable: true),
                    CustomerTypeID = table.Column<int>(nullable: true),
                    VendorTypeID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyID);
                    table.ForeignKey(
                        name: "FK_Companies_Countries_BillingCountryID",
                        column: x => x.BillingCountryID,
                        principalTable: "Countries",
                        principalColumn: "CountryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_Provinces_BillingProvinceID",
                        column: x => x.BillingProvinceID,
                        principalTable: "Provinces",
                        principalColumn: "ProvinceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_BillingTerms_BillingTermID",
                        column: x => x.BillingTermID,
                        principalTable: "BillingTerms",
                        principalColumn: "BillingTermID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_ContractorTypes_ContractorTypeID",
                        column: x => x.ContractorTypeID,
                        principalTable: "ContractorTypes",
                        principalColumn: "ContractorTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_CustomerTypes_CustomerTypeID",
                        column: x => x.CustomerTypeID,
                        principalTable: "CustomerTypes",
                        principalColumn: "CustomerTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_Countries_ShippingCountryID",
                        column: x => x.ShippingCountryID,
                        principalTable: "Countries",
                        principalColumn: "CountryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_Provinces_ShippingProvinceID",
                        column: x => x.ShippingProvinceID,
                        principalTable: "Provinces",
                        principalColumn: "ProvinceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_VendorTypes_VendorTypeID",
                        column: x => x.VendorTypeID,
                        principalTable: "VendorTypes",
                        principalColumn: "VendorTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementEmployees",
                columns: table => new
                {
                    AnnouncementEmployeeID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeID = table.Column<int>(nullable: false),
                    AnnouncementID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementEmployees", x => x.AnnouncementEmployeeID);
                    table.ForeignKey(
                        name: "FK_AnnouncementEmployees_Announcements_AnnouncementID",
                        column: x => x.AnnouncementID,
                        principalTable: "Announcements",
                        principalColumn: "AnnouncementID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnouncementEmployees_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    CalendarId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.CalendarId);
                    table.ForeignKey(
                        name: "FK_Calendars_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyContractors",
                columns: table => new
                {
                    CompanyID = table.Column<int>(nullable: false),
                    ContractorTypeID = table.Column<int>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyContractors", x => new { x.CompanyID, x.ContractorTypeID });
                    table.ForeignKey(
                        name: "FK_CompanyContractors_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyContractors_ContractorTypes_ContractorTypeID",
                        column: x => x.ContractorTypeID,
                        principalTable: "ContractorTypes",
                        principalColumn: "ContractorTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyCustomers",
                columns: table => new
                {
                    CompanyID = table.Column<int>(nullable: false),
                    CustomerTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyCustomers", x => new { x.CompanyID, x.CustomerTypeID });
                    table.ForeignKey(
                        name: "FK_CompanyCustomers_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyCustomers_CustomerTypes_CustomerTypeID",
                        column: x => x.CustomerTypeID,
                        principalTable: "CustomerTypes",
                        principalColumn: "CustomerTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyVendors",
                columns: table => new
                {
                    CompanyID = table.Column<int>(nullable: false),
                    VendorTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyVendors", x => new { x.CompanyID, x.VendorTypeID });
                    table.ForeignKey(
                        name: "FK_CompanyVendors_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyVendors_VendorTypes_VendorTypeID",
                        column: x => x.VendorTypeID,
                        principalTable: "VendorTypes",
                        principalColumn: "VendorTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ContactID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    JobTitle = table.Column<string>(maxLength: 50, nullable: true),
                    CellPhone = table.Column<long>(nullable: true),
                    WorkPhone = table.Column<long>(nullable: true),
                    Email = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(maxLength: 200, nullable: true),
                    CompanyID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ContactID);
                    table.ForeignKey(
                        name: "FK_Contacts_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactCategories",
                columns: table => new
                {
                    ContactID = table.Column<int>(nullable: false),
                    CategoriesID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactCategories", x => new { x.CategoriesID, x.ContactID });
                    table.ForeignKey(
                        name: "FK_ContactCategories_Categories_CategoriesID",
                        column: x => x.CategoriesID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactCategories_Contacts_ContactID",
                        column: x => x.ContactID,
                        principalTable: "Contacts",
                        principalColumn: "ContactID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementEmployees_AnnouncementID",
                table: "AnnouncementEmployees",
                column: "AnnouncementID");

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementEmployees_EmployeeID",
                table: "AnnouncementEmployees",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_CompanyId",
                table: "Calendars",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_BillingCountryID",
                table: "Companies",
                column: "BillingCountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_BillingProvinceID",
                table: "Companies",
                column: "BillingProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_BillingTermID",
                table: "Companies",
                column: "BillingTermID");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ContractorTypeID",
                table: "Companies",
                column: "ContractorTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CurrencyID",
                table: "Companies",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CustomerTypeID",
                table: "Companies",
                column: "CustomerTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ShippingCountryID",
                table: "Companies",
                column: "ShippingCountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ShippingProvinceID",
                table: "Companies",
                column: "ShippingProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_VendorTypeID",
                table: "Companies",
                column: "VendorTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyContractors_ContractorTypeID",
                table: "CompanyContractors",
                column: "ContractorTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCustomers_CustomerTypeID",
                table: "CompanyCustomers",
                column: "CustomerTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyVendors_VendorTypeID",
                table: "CompanyVendors",
                column: "VendorTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ContactCategories_ContactID",
                table: "ContactCategories",
                column: "ContactID");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CompanyID",
                table: "Contacts",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeCountryID",
                table: "Employees",
                column: "EmployeeCountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeProvinceID",
                table: "Employees",
                column: "EmployeeProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmploymentTypeID",
                table: "Employees",
                column: "EmploymentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobPositionID",
                table: "Employees",
                column: "JobPositionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnouncementEmployees");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "CompanyContractors");

            migrationBuilder.DropTable(
                name: "CompanyCustomers");

            migrationBuilder.DropTable(
                name: "CompanyVendors");

            migrationBuilder.DropTable(
                name: "ContactCategories");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "EmploymentTypes");

            migrationBuilder.DropTable(
                name: "JobPositions");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "BillingTerms");

            migrationBuilder.DropTable(
                name: "ContractorTypes");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "CustomerTypes");

            migrationBuilder.DropTable(
                name: "VendorTypes");
        }
    }
}
