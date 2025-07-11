﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamiq.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MakeEmailVerificationTokenUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EmailVerifications_Token",
                table: "EmailVerifications",
                column: "Token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmailVerifications_Token",
                table: "EmailVerifications");
        }
    }
}
