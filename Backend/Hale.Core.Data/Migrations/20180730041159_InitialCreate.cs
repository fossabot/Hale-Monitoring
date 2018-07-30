using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hale.Core.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(maxLength: 450, nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    OldPassword = table.Column<string>(nullable: true),
                    PasswordChanged = table.Column<DateTimeOffset>(nullable: true),
                    Activated = table.Column<bool>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    Modified = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgentConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Identifier = table.Column<string>(maxLength: 32, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    Modified = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgentConfigSetCheckActions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Action = table.Column<string>(nullable: true),
                    Module = table.Column<string>(nullable: true),
                    Target = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentConfigSetCheckActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Identifier = table.Column<string>(nullable: true),
                    Major = table.Column<int>(nullable: false),
                    Minor = table.Column<int>(nullable: false),
                    Revision = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FriendlyName = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    Domain = table.Column<string>(nullable: true),
                    OperatingSystem = table.Column<string>(nullable: true),
                    NicSummary = table.Column<string>(nullable: true),
                    HardwareSummary = table.Column<string>(nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    LastConnected = table.Column<DateTimeOffset>(nullable: true),
                    Modified = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedBy = table.Column<int>(nullable: true),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    ConfiguredBy = table.Column<int>(nullable: true),
                    Guid = table.Column<Guid>(nullable: false),
                    RsaKey = table.Column<byte[]>(nullable: true),
                    Configured = table.Column<bool>(nullable: false),
                    Blocked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HostId = table.Column<int>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false),
                    FunctionId = table.Column<int>(nullable: false),
                    Target = table.Column<string>(nullable: true),
                    ExecutionTime = table.Column<DateTimeOffset>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Exception = table.Column<string>(nullable: true),
                    AboveWarning = table.Column<bool>(nullable: false),
                    AboveCritical = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    AccountId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountDetails_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AgentConfigSetTasks",
                columns: table => new
                {
                    Enabled = table.Column<bool>(nullable: false),
                    Interval = table.Column<TimeSpan>(nullable: false),
                    Startup = table.Column<bool>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    AgentConfigSetId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentConfigSetTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgentConfigSetTasks_AgentConfigs_AgentConfigSetId",
                        column: x => x.AgentConfigSetId,
                        principalTable: "AgentConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AgentConfigSetFuncSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<byte>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false),
                    Function = table.Column<string>(nullable: true),
                    Interval = table.Column<TimeSpan>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    Startup = table.Column<bool>(nullable: false),
                    WarningThreshold = table.Column<float>(nullable: false),
                    CriticalThreshold = table.Column<float>(nullable: false),
                    WarningActionId = table.Column<int>(nullable: true),
                    CriticalActionId = table.Column<int>(nullable: true),
                    AgentConfigSetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentConfigSetFuncSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgentConfigSetFuncSettings_AgentConfigs_AgentConfigSetId",
                        column: x => x.AgentConfigSetId,
                        principalTable: "AgentConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgentConfigSetFuncSettings_AgentConfigSetCheckActions_CriticalActionId",
                        column: x => x.CriticalActionId,
                        principalTable: "AgentConfigSetCheckActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AgentConfigSetFuncSettings_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgentConfigSetFuncSettings_AgentConfigSetCheckActions_WarningActionId",
                        column: x => x.WarningActionId,
                        principalTable: "AgentConfigSetCheckActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Functions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Identifier = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<byte>(nullable: false),
                    ModuleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Functions_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NodeComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NodeId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodeComments_Nodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NodeComments_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NodeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HostId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    NodeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodeDetails_Nodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResultId = table.Column<int>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckRecords_Results_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Results",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InfoRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResultId = table.Column<int>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfoRecords_Results_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Results",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AgentConfigSetFunctionSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Target = table.Column<string>(nullable: true),
                    AgentConfigSetFunctionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentConfigSetFunctionSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgentConfigSetFunctionSettings_AgentConfigSetFuncSettings_AgentConfigSetFunctionId",
                        column: x => x.AgentConfigSetFunctionId,
                        principalTable: "AgentConfigSetFuncSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Activated", "Created", "CreatedBy", "Email", "Enabled", "FullName", "IsAdmin", "Modified", "ModifiedBy", "OldPassword", "Password", "PasswordChanged", "UserName" },
                values: new object[] { 4, true, new DateTimeOffset(new DateTime(2018, 7, 30, 6, 11, 58, 838, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), null, "foobar@acmegroup.com", false, "Test User 04", false, null, null, null, "$2a$05$hD7kYpARPJ5qd9tXM5UgnO/.41H.2CJRIaWepPHcjcub5tDyheU8q", null, "test04" });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Activated", "Created", "CreatedBy", "Email", "Enabled", "FullName", "IsAdmin", "Modified", "ModifiedBy", "OldPassword", "Password", "PasswordChanged", "UserName" },
                values: new object[] { 2, true, new DateTimeOffset(new DateTime(2018, 7, 30, 6, 11, 58, 832, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), null, "nils@piksel.se", true, "Test User 02", false, null, null, null, "$2a$05$nPh.H6huDCRI28dg0nwER.TLTy.cbrk9tVlIB/7iT.aYiMtIavHEe", null, "test02" });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Activated", "Created", "CreatedBy", "Email", "Enabled", "FullName", "IsAdmin", "Modified", "ModifiedBy", "OldPassword", "Password", "PasswordChanged", "UserName" },
                values: new object[] { 1, true, new DateTimeOffset(new DateTime(2018, 7, 30, 6, 11, 58, 711, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), null, "simon.aronsson@outlook.com", true, "Test User 01", true, null, null, null, "$2a$05$OS9Z/x.AqZbG86U10wtSBumXefJ.X8i47SReZdDLFeeY5bITZ3PLy", null, "test01" });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Activated", "Created", "CreatedBy", "Email", "Enabled", "FullName", "IsAdmin", "Modified", "ModifiedBy", "OldPassword", "Password", "PasswordChanged", "UserName" },
                values: new object[] { 3, false, new DateTimeOffset(new DateTime(2018, 7, 30, 6, 11, 58, 835, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), null, "foo@bar.com", true, "Test User 03", false, null, null, null, "$2a$05$1wnTBWxmpZKToBd9AI7T4OHtHoFL9iPKKENlfNYp8l4Pd/KeIAhNi", null, "test03" });

            migrationBuilder.InsertData(
                table: "AgentConfigs",
                columns: new[] { "Id", "Created", "CreatedBy", "Identifier", "Modified", "ModifiedBy", "Name" },
                values: new object[] { 1, new DateTimeOffset(new DateTime(2018, 7, 30, 6, 11, 58, 844, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), null, "seed_config01", null, null, null });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "Identifier", "Major", "Minor", "Revision" },
                values: new object[] { 1, "com.itshale.core.memory", 0, 0, 0 });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Blocked", "Configured", "ConfiguredBy", "Created", "Domain", "FriendlyName", "Guid", "HardwareSummary", "HostName", "Ip", "LastConnected", "Modified", "ModifiedBy", "NicSummary", "OperatingSystem", "RsaKey", "Status" },
                values: new object[] { 4, false, true, null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "domain.com", "TestHost04", new Guid("057449e7-e7f1-47b6-80a7-b21ed8dea051"), null, "test-host-04", "10.1.2.4", null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, 1 });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Blocked", "Configured", "ConfiguredBy", "Created", "Domain", "FriendlyName", "Guid", "HardwareSummary", "HostName", "Ip", "LastConnected", "Modified", "ModifiedBy", "NicSummary", "OperatingSystem", "RsaKey", "Status" },
                values: new object[] { 5, false, true, null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "domain.com", "TestHost05", new Guid("faf16de0-b8e4-4a0f-8c1b-eb410725c6d2"), null, "test-host-05", "10.1.2.5", null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, 0 });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Blocked", "Configured", "ConfiguredBy", "Created", "Domain", "FriendlyName", "Guid", "HardwareSummary", "HostName", "Ip", "LastConnected", "Modified", "ModifiedBy", "NicSummary", "OperatingSystem", "RsaKey", "Status" },
                values: new object[] { 6, false, true, null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "domain.com", "TestHost06", new Guid("844fa1ab-3b54-45bf-ab5c-9bedfcd6afa3"), null, "test-host-06", "10.1.2.6", null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, 0 });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Blocked", "Configured", "ConfiguredBy", "Created", "Domain", "FriendlyName", "Guid", "HardwareSummary", "HostName", "Ip", "LastConnected", "Modified", "ModifiedBy", "NicSummary", "OperatingSystem", "RsaKey", "Status" },
                values: new object[] { 7, false, true, null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "domain.com", "TestHost07", new Guid("057449e7-e7f1-47b6-80a7-b21ed8dea044"), null, "test-host-01", "10.1.2.7", null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, 1 });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Blocked", "Configured", "ConfiguredBy", "Created", "Domain", "FriendlyName", "Guid", "HardwareSummary", "HostName", "Ip", "LastConnected", "Modified", "ModifiedBy", "NicSummary", "OperatingSystem", "RsaKey", "Status" },
                values: new object[] { 2, false, true, null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "domain.com", "TestHost02", new Guid("faf16de0-b8e4-4a0f-8c1b-eb410725c6da"), null, "test-host-02", "10.1.2.2", null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, 0 });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Blocked", "Configured", "ConfiguredBy", "Created", "Domain", "FriendlyName", "Guid", "HardwareSummary", "HostName", "Ip", "LastConnected", "Modified", "ModifiedBy", "NicSummary", "OperatingSystem", "RsaKey", "Status" },
                values: new object[] { 9, false, false, null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "domain.com", "TestHost09", new Guid("844fa1ab-3b54-45bf-ab5c-9bedfcd6afa6"), null, "test-host-01", "10.1.2.9", null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Microsoft Windows NT 10.0.14393.0", null, 0 });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Blocked", "Configured", "ConfiguredBy", "Created", "Domain", "FriendlyName", "Guid", "HardwareSummary", "HostName", "Ip", "LastConnected", "Modified", "ModifiedBy", "NicSummary", "OperatingSystem", "RsaKey", "Status" },
                values: new object[] { 10, false, false, null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "domain.com", "TestHost10", new Guid("057449e7-e7f1-47b6-80a7-b21ed8dea05b"), null, "test-host-01", "10.1.2.10", null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Microsoft Windows NT 10.0.14393.0", null, 1 });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Blocked", "Configured", "ConfiguredBy", "Created", "Domain", "FriendlyName", "Guid", "HardwareSummary", "HostName", "Ip", "LastConnected", "Modified", "ModifiedBy", "NicSummary", "OperatingSystem", "RsaKey", "Status" },
                values: new object[] { 11, false, false, null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "domain.com", "TestHost11", new Guid("faf16de0-b8e4-4a0f-8c1b-eb410725c6dd"), null, "test-host-01", "10.1.2.11", null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Microsoft Windows NT 10.0.14393.0", null, 0 });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Blocked", "Configured", "ConfiguredBy", "Created", "Domain", "FriendlyName", "Guid", "HardwareSummary", "HostName", "Ip", "LastConnected", "Modified", "ModifiedBy", "NicSummary", "OperatingSystem", "RsaKey", "Status" },
                values: new object[] { 12, false, false, null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "domain.com", "TestHost12", new Guid("844fa1ab-3b54-45bf-ab5c-9bedfcd6afbb"), null, "test-host-01", "10.1.2.12", null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Microsoft Windows NT 10.0.14393.0", null, 0 });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Blocked", "Configured", "ConfiguredBy", "Created", "Domain", "FriendlyName", "Guid", "HardwareSummary", "HostName", "Ip", "LastConnected", "Modified", "ModifiedBy", "NicSummary", "OperatingSystem", "RsaKey", "Status" },
                values: new object[] { 13, false, false, null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "domain.com", "TestHost13", new Guid("844fa1ab-3b54-45bf-ab5c-9bedfcd6afcc"), null, "test-host-01", "10.1.2.13", null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Microsoft Windows NT 10.0.14393.0", null, 0 });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Blocked", "Configured", "ConfiguredBy", "Created", "Domain", "FriendlyName", "Guid", "HardwareSummary", "HostName", "Ip", "LastConnected", "Modified", "ModifiedBy", "NicSummary", "OperatingSystem", "RsaKey", "Status" },
                values: new object[] { 1, false, true, null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "domain.com", "TestHost01", new Guid("057449e7-e7f1-47b6-80a7-b21ed8dea058"), null, "test-host-01", "127.0.0.1", null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, 1 });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Blocked", "Configured", "ConfiguredBy", "Created", "Domain", "FriendlyName", "Guid", "HardwareSummary", "HostName", "Ip", "LastConnected", "Modified", "ModifiedBy", "NicSummary", "OperatingSystem", "RsaKey", "Status" },
                values: new object[] { 3, false, true, null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "domain.com", "TestHost03", new Guid("844fa1ab-3b54-45bf-ab5c-9bedfcd6afa9"), null, "test-host-03", "10.1.2.3", null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, 0 });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Blocked", "Configured", "ConfiguredBy", "Created", "Domain", "FriendlyName", "Guid", "HardwareSummary", "HostName", "Ip", "LastConnected", "Modified", "ModifiedBy", "NicSummary", "OperatingSystem", "RsaKey", "Status" },
                values: new object[] { 8, false, true, null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "domain.com", "TestHost08", new Guid("faf16de0-b8e4-4a0f-8c1b-eb410725c6d5"), null, "test-host-01", "10.1.2.8", null, new DateTimeOffset(new DateTime(2018, 7, 30, 4, 11, 58, 842, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, null, 0 });

            migrationBuilder.InsertData(
                table: "AgentConfigSetFuncSettings",
                columns: new[] { "Id", "AgentConfigSetId", "CriticalActionId", "CriticalThreshold", "Enabled", "Function", "Interval", "ModuleId", "Startup", "Type", "WarningActionId", "WarningThreshold" },
                values: new object[] { 1, 0, null, 1f, true, "default", new TimeSpan(0, 0, 10, 0, 0), 1, true, (byte)1, null, 0.5f });

            migrationBuilder.InsertData(
                table: "AgentConfigSetFunctionSettings",
                columns: new[] { "Id", "AgentConfigSetFunctionId", "Key", "Target", "Value" },
                values: new object[] { 1, 1, "foo", "default", "bar" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountDetails_AccountId",
                table: "AccountDetails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserName",
                table: "Accounts",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgentConfigs_Identifier",
                table: "AgentConfigs",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgentConfigSetFuncSettings_AgentConfigSetId",
                table: "AgentConfigSetFuncSettings",
                column: "AgentConfigSetId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentConfigSetFuncSettings_CriticalActionId",
                table: "AgentConfigSetFuncSettings",
                column: "CriticalActionId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentConfigSetFuncSettings_ModuleId",
                table: "AgentConfigSetFuncSettings",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentConfigSetFuncSettings_WarningActionId",
                table: "AgentConfigSetFuncSettings",
                column: "WarningActionId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentConfigSetFunctionSettings_AgentConfigSetFunctionId",
                table: "AgentConfigSetFunctionSettings",
                column: "AgentConfigSetFunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentConfigSetTasks_AgentConfigSetId",
                table: "AgentConfigSetTasks",
                column: "AgentConfigSetId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckRecords_ResultId",
                table: "CheckRecords",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Functions_ModuleId",
                table: "Functions",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoRecords_ResultId",
                table: "InfoRecords",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeComments_NodeId",
                table: "NodeComments",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeComments_UserId",
                table: "NodeComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeDetails_NodeId",
                table: "NodeDetails",
                column: "NodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountDetails");

            migrationBuilder.DropTable(
                name: "AgentConfigSetFunctionSettings");

            migrationBuilder.DropTable(
                name: "AgentConfigSetTasks");

            migrationBuilder.DropTable(
                name: "CheckRecords");

            migrationBuilder.DropTable(
                name: "Functions");

            migrationBuilder.DropTable(
                name: "InfoRecords");

            migrationBuilder.DropTable(
                name: "NodeComments");

            migrationBuilder.DropTable(
                name: "NodeDetails");

            migrationBuilder.DropTable(
                name: "AgentConfigSetFuncSettings");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Nodes");

            migrationBuilder.DropTable(
                name: "AgentConfigs");

            migrationBuilder.DropTable(
                name: "AgentConfigSetCheckActions");

            migrationBuilder.DropTable(
                name: "Modules");
        }
    }
}
