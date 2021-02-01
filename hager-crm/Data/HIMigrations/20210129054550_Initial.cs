﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hager_crm.Data.HIMigrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "HG");

            migrationBuilder.CreateTable(
                name: "BillingTerms",
                schema: "HG",
                columns: table => new
                {
                    BillingTermID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Terms = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingTerms", x => x.BillingTermID);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "HG",
                columns: table => new
                {
                    CategoriesID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoriesID);
                });

            migrationBuilder.CreateTable(
                name: "ContractorTypes",
                schema: "HG",
                columns: table => new
                {
                    ContractorTypeID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractorTypes", x => x.ContractorTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "HG",
                columns: table => new
                {
                    CountryID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountryName = table.Column<string>(maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryID);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                schema: "HG",
                columns: table => new
                {
                    CurrencyID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurrencyName = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.CurrencyID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTypes",
                schema: "HG",
                columns: table => new
                {
                    CustomerTypeID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTypes", x => x.CustomerTypeID);
                });

            migrationBuilder.CreateTable(
                name: "EmploymentTypes",
                schema: "HG",
                columns: table => new
                {
                    EmploymentTypeID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentTypes", x => x.EmploymentTypeID);
                });

            migrationBuilder.CreateTable(
                name: "JobPositions",
                schema: "HG",
                columns: table => new
                {
                    JobPositionID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Position = table.Column<string>(maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPositions", x => x.JobPositionID);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                schema: "HG",
                columns: table => new
                {
                    ProvinceID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProvinceName = table.Column<string>(maxLength: 30, nullable: false),
                    CountryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.ProvinceID);
                });

            migrationBuilder.CreateTable(
                name: "VendorTypes",
                schema: "HG",
                columns: table => new
                {
                    VendorTypeID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorTypes", x => x.VendorTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "HG",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    JobPositionID = table.Column<int>(nullable: false),
                    EmploymentTypeID = table.Column<int>(nullable: false),
                    EmployeeAddress1 = table.Column<string>(maxLength: 100, nullable: false),
                    EmployeeAddress2 = table.Column<string>(maxLength: 50, nullable: true),
                    EmployeeProvinceID = table.Column<int>(nullable: false),
                    EmployeePostalCode = table.Column<string>(nullable: false),
                    EmployeeCountryID = table.Column<int>(nullable: false),
                    CellPhone = table.Column<long>(nullable: false),
                    WorkPhone = table.Column<long>(nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    DOB = table.Column<DateTime>(nullable: false),
                    Wage = table.Column<decimal>(nullable: false),
                    Expense = table.Column<decimal>(nullable: false),
                    DateJoined = table.Column<DateTime>(nullable: false),
                    KeyFob = table.Column<string>(maxLength: 7, nullable: false),
                    EmergencyContactName = table.Column<string>(maxLength: 50, nullable: false),
                    EmergencyContactPhone = table.Column<long>(nullable: false),
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
                        principalSchema: "HG",
                        principalTable: "Countries",
                        principalColumn: "CountryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Provinces_EmployeeProvinceID",
                        column: x => x.EmployeeProvinceID,
                        principalSchema: "HG",
                        principalTable: "Provinces",
                        principalColumn: "ProvinceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_EmploymentTypes_EmploymentTypeID",
                        column: x => x.EmploymentTypeID,
                        principalSchema: "HG",
                        principalTable: "EmploymentTypes",
                        principalColumn: "EmploymentTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_JobPositions_JobPositionID",
                        column: x => x.JobPositionID,
                        principalSchema: "HG",
                        principalTable: "JobPositions",
                        principalColumn: "JobPositionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                schema: "HG",
                columns: table => new
                {
                    CompanyID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Location = table.Column<string>(maxLength: 50, nullable: false),
                    CreditCheck = table.Column<bool>(nullable: false),
                    DateChecked = table.Column<DateTime>(nullable: false),
                    BillingTermID = table.Column<int>(nullable: false),
                    CurrencyID = table.Column<int>(nullable: false),
                    Phone = table.Column<long>(nullable: false),
                    Website = table.Column<string>(maxLength: 50, nullable: true),
                    BillingAddress1 = table.Column<string>(maxLength: 100, nullable: false),
                    BillingAddress2 = table.Column<string>(maxLength: 50, nullable: true),
                    BillingProvinceID = table.Column<int>(nullable: false),
                    BillingPostalCode = table.Column<string>(nullable: false),
                    BillingCountryID = table.Column<int>(nullable: false),
                    ShippingAddress1 = table.Column<string>(maxLength: 100, nullable: false),
                    ShippingAddress2 = table.Column<string>(maxLength: 50, nullable: true),
                    ShippingProvinceID = table.Column<int>(nullable: false),
                    ShippingPostalCode = table.Column<string>(nullable: false),
                    ShippingCountryID = table.Column<int>(nullable: false),
                    Customer = table.Column<bool>(nullable: false),
                    CustomerTypeID = table.Column<int>(nullable: true),
                    Vendor = table.Column<bool>(nullable: false),
                    VendorTypeID = table.Column<int>(nullable: true),
                    Contractor = table.Column<bool>(nullable: false),
                    ContractorTypeID = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyID);
                    table.ForeignKey(
                        name: "FK_Companies_Countries_BillingCountryID",
                        column: x => x.BillingCountryID,
                        principalSchema: "HG",
                        principalTable: "Countries",
                        principalColumn: "CountryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_Provinces_BillingProvinceID",
                        column: x => x.BillingProvinceID,
                        principalSchema: "HG",
                        principalTable: "Provinces",
                        principalColumn: "ProvinceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_BillingTerms_BillingTermID",
                        column: x => x.BillingTermID,
                        principalSchema: "HG",
                        principalTable: "BillingTerms",
                        principalColumn: "BillingTermID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_ContractorTypes_ContractorTypeID",
                        column: x => x.ContractorTypeID,
                        principalSchema: "HG",
                        principalTable: "ContractorTypes",
                        principalColumn: "ContractorTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalSchema: "HG",
                        principalTable: "Currencies",
                        principalColumn: "CurrencyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Companies_CustomerTypes_CustomerTypeID",
                        column: x => x.CustomerTypeID,
                        principalSchema: "HG",
                        principalTable: "CustomerTypes",
                        principalColumn: "CustomerTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Companies_Countries_ShippingCountryID",
                        column: x => x.ShippingCountryID,
                        principalSchema: "HG",
                        principalTable: "Countries",
                        principalColumn: "CountryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Companies_Provinces_ShippingProvinceID",
                        column: x => x.ShippingProvinceID,
                        principalSchema: "HG",
                        principalTable: "Provinces",
                        principalColumn: "ProvinceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Companies_VendorTypes_VendorTypeID",
                        column: x => x.VendorTypeID,
                        principalSchema: "HG",
                        principalTable: "VendorTypes",
                        principalColumn: "VendorTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                schema: "HG",
                columns: table => new
                {
                    ContactID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    JobTitle = table.Column<string>(maxLength: 50, nullable: false),
                    CellPhone = table.Column<long>(nullable: false),
                    WorkPhone = table.Column<long>(nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
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
                        principalSchema: "HG",
                        principalTable: "Companies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactCategories",
                schema: "HG",
                columns: table => new
                {
                    ContactCategoriesID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoriesID = table.Column<int>(nullable: false),
                    ContactID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactCategories", x => x.ContactCategoriesID);
                    table.ForeignKey(
                        name: "FK_ContactCategories_Categories_CategoriesID",
                        column: x => x.CategoriesID,
                        principalSchema: "HG",
                        principalTable: "Categories",
                        principalColumn: "CategoriesID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactCategories_Contacts_ContactID",
                        column: x => x.ContactID,
                        principalSchema: "HG",
                        principalTable: "Contacts",
                        principalColumn: "ContactID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_BillingCountryID",
                schema: "HG",
                table: "Companies",
                column: "BillingCountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_BillingProvinceID",
                schema: "HG",
                table: "Companies",
                column: "BillingProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_BillingTermID",
                schema: "HG",
                table: "Companies",
                column: "BillingTermID");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ContractorTypeID",
                schema: "HG",
                table: "Companies",
                column: "ContractorTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CurrencyID",
                schema: "HG",
                table: "Companies",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CustomerTypeID",
                schema: "HG",
                table: "Companies",
                column: "CustomerTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ShippingCountryID",
                schema: "HG",
                table: "Companies",
                column: "ShippingCountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ShippingProvinceID",
                schema: "HG",
                table: "Companies",
                column: "ShippingProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_VendorTypeID",
                schema: "HG",
                table: "Companies",
                column: "VendorTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ContactCategories_CategoriesID",
                schema: "HG",
                table: "ContactCategories",
                column: "CategoriesID");

            migrationBuilder.CreateIndex(
                name: "IX_ContactCategories_ContactID",
                schema: "HG",
                table: "ContactCategories",
                column: "ContactID");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CompanyID",
                schema: "HG",
                table: "Contacts",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                schema: "HG",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeCountryID",
                schema: "HG",
                table: "Employees",
                column: "EmployeeCountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeProvinceID",
                schema: "HG",
                table: "Employees",
                column: "EmployeeProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmploymentTypeID",
                schema: "HG",
                table: "Employees",
                column: "EmploymentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobPositionID",
                schema: "HG",
                table: "Employees",
                column: "JobPositionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactCategories",
                schema: "HG");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "HG");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "HG");

            migrationBuilder.DropTable(
                name: "Contacts",
                schema: "HG");

            migrationBuilder.DropTable(
                name: "EmploymentTypes",
                schema: "HG");

            migrationBuilder.DropTable(
                name: "JobPositions",
                schema: "HG");

            migrationBuilder.DropTable(
                name: "Companies",
                schema: "HG");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "HG");

            migrationBuilder.DropTable(
                name: "Provinces",
                schema: "HG");

            migrationBuilder.DropTable(
                name: "BillingTerms",
                schema: "HG");

            migrationBuilder.DropTable(
                name: "ContractorTypes",
                schema: "HG");

            migrationBuilder.DropTable(
                name: "Currencies",
                schema: "HG");

            migrationBuilder.DropTable(
                name: "CustomerTypes",
                schema: "HG");

            migrationBuilder.DropTable(
                name: "VendorTypes",
                schema: "HG");
        }
    }
}