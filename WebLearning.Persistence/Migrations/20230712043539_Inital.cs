using Microsoft.EntityFrameworkCore.Migrations;


namespace WebLearning.Persistence.Migrations
{
    public partial class Inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CatCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetDepartment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetDepartment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetMovedStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetMovedStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetSupplier",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyTaxCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetSupplier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notify = table.Column<bool>(type: "bit", nullable: false),
                    DescNotify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalWatched = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistorySubmitFinal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCompoleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Submit = table.Column<bool>(type: "bit", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorySubmitFinal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistorySubmitLession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCompoleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Submit = table.Column<bool>(type: "bit", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorySubmitLession", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistorySubmitMonthly",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCompoleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Submit = table.Column<bool>(type: "bit", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorySubmitMonthly", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationResponse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetNotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FatherTargetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FatherAlias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notify = table.Column<bool>(type: "bit", nullable: false),
                    DescNotify = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationResponse", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportUserScore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportUserScore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportUserScoreFinal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportUserScoreFinal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportUserScoreMonthly",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportUserScoreMonthly", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Asset",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    SeriNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetsCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetsSupplierId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AssetSubCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AssetsDepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manager = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetsStatusId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateBuyed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateExpired = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUsed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpireDay = table.Column<int>(type: "int", nullable: false),
                    DateMoved = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateChecked = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateRepaired = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepairLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Spec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessModel = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asset_AssetCategory_AssetsCategoryId",
                        column: x => x.AssetsCategoryId,
                        principalTable: "AssetCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Asset_AssetDepartment_AssetsDepartmentId",
                        column: x => x.AssetsDepartmentId,
                        principalTable: "AssetDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Asset_AssetStatus_AssetsStatusId",
                        column: x => x.AssetsStatusId,
                        principalTable: "AssetStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Asset_AssetSupplier_AssetsSupplierId",
                        column: x => x.AssetsSupplierId,
                        principalTable: "AssetSupplier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseImageVideo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseImageVideo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseImageVideo_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notify = table.Column<bool>(type: "bit", nullable: false),
                    DescNotify = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lession_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizCourse",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeToDo = table.Column<int>(type: "int", nullable: false),
                    ScorePass = table.Column<int>(type: "int", nullable: false),
                    Notify = table.Column<bool>(type: "bit", nullable: false),
                    DescNotify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HistorySubmitFinalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReportUserScoreFinalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizCourse", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QuizCourse_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizCourse_HistorySubmitFinal_HistorySubmitFinalId",
                        column: x => x.HistorySubmitFinalId,
                        principalTable: "HistorySubmitFinal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuizCourse_ReportUserScoreFinal_ReportUserScoreFinalId",
                        column: x => x.ReportUserScoreFinalId,
                        principalTable: "ReportUserScoreFinal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHased = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorizeRole = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseRole",
                columns: table => new
                {
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumWatched = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseRole", x => new { x.CourseId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_CourseRole_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizMonthly",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeToDo = table.Column<int>(type: "int", nullable: false),
                    ScorePass = table.Column<int>(type: "int", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    Notify = table.Column<bool>(type: "bit", nullable: false),
                    DescNotify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HistorySubmitMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReportUserScoreMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizMonthly", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QuizMonthly_HistorySubmitMonthly_HistorySubmitMonthlyId",
                        column: x => x.HistorySubmitMonthlyId,
                        principalTable: "HistorySubmitMonthly",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuizMonthly_ReportUserScoreMonthly_ReportUserScoreMonthlyId",
                        column: x => x.ReportUserScoreMonthlyId,
                        principalTable: "ReportUserScoreMonthly",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuizMonthly_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HistoryAddSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    CodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Editor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypedSubmit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendMail = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryAddSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryAddSlots_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssetMoved",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssestsId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AssetsMovedStatusId = table.Column<int>(type: "int", nullable: false),
                    AssetsDepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Receiver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DateUsed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateMoved = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetMoved", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetMoved_Asset_AssestsId",
                        column: x => x.AssestsId,
                        principalTable: "Asset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AssetMoved_AssetDepartment_AssetsDepartmentId",
                        column: x => x.AssetsDepartmentId,
                        principalTable: "AssetDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssetMoved_AssetMovedStatus_AssetsMovedStatusId",
                        column: x => x.AssetsMovedStatusId,
                        principalTable: "AssetMovedStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetRepaired",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssestsId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LocationRepaired = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateRepaired = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetRepaired", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetRepaired_Asset_AssestsId",
                        column: x => x.AssestsId,
                        principalTable: "Asset",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LessionVideoImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkVideo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notify = table.Column<bool>(type: "bit", nullable: false),
                    DescNotify = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessionVideoImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessionVideoImage_Lession_LessionId",
                        column: x => x.LessionId,
                        principalTable: "Lession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OtherFileUpload",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherFileUpload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtherFileUpload_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OtherFileUpload_Lession_LessionId",
                        column: x => x.LessionId,
                        principalTable: "Lession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizLession",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeToDo = table.Column<int>(type: "int", nullable: false),
                    ScorePass = table.Column<int>(type: "int", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    SortItem = table.Column<int>(type: "int", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notify = table.Column<bool>(type: "bit", nullable: false),
                    DescNotify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HistorySubmitLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReportUserScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizLession", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QuizLession_HistorySubmitLession_HistorySubmitLessionId",
                        column: x => x.HistorySubmitLessionId,
                        principalTable: "HistorySubmitLession",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuizLession_Lession_LessionId",
                        column: x => x.LessionId,
                        principalTable: "Lession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizLession_ReportUserScore_ReportUserScoreId",
                        column: x => x.ReportUserScoreId,
                        principalTable: "ReportUserScore",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestionFinal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuizCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Point = table.Column<int>(type: "int", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionFinal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionFinal_QuizCourse_QuizCourseId",
                        column: x => x.QuizCourseId,
                        principalTable: "QuizCourse",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountDetail_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Avatar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avatar_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionMonthly",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuizMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Point = table.Column<int>(type: "int", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionMonthly", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionMonthly_QuizMonthly_QuizMonthlyId",
                        column: x => x.QuizMonthlyId,
                        principalTable: "QuizMonthly",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionLession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuizLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Point = table.Column<int>(type: "int", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionLession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionLession_QuizLession_QuizLessionId",
                        column: x => x.QuizLessionId,
                        principalTable: "QuizLession",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerCourse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckBoxId = table.Column<int>(type: "int", nullable: false),
                    Checked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerCourse_QuestionFinal_QuestionCourseId",
                        column: x => x.QuestionCourseId,
                        principalTable: "QuestionFinal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CorrectAnswerCourse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionFinalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrectAnswerCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorrectAnswerCourse_QuestionFinal_QuestionFinalId",
                        column: x => x.QuestionFinalId,
                        principalTable: "QuestionFinal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OptionCourse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionFinalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionCourse_QuestionFinal_QuestionFinalId",
                        column: x => x.QuestionFinalId,
                        principalTable: "QuestionFinal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerMonthly",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckBoxId = table.Column<int>(type: "int", nullable: false),
                    Checked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerMonthly", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerMonthly_QuestionMonthly_QuestionMonthlyId",
                        column: x => x.QuestionMonthlyId,
                        principalTable: "QuestionMonthly",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CorrectAnswerMonthly",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrectAnswerMonthly", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorrectAnswerMonthly_QuestionMonthly_QuestionMonthlyId",
                        column: x => x.QuestionMonthlyId,
                        principalTable: "QuestionMonthly",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OptionMonthly",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionMonthlyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionMonthly", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionMonthly_QuestionMonthly_QuestionMonthlyId",
                        column: x => x.QuestionMonthlyId,
                        principalTable: "QuestionMonthly",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerLession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckBoxId = table.Column<int>(type: "int", nullable: false),
                    Checked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerLession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerLession_QuestionLession_QuestionLessionId",
                        column: x => x.QuestionLessionId,
                        principalTable: "QuestionLession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CorrectAnswerLession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrectAnswerLession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorrectAnswerLession_QuestionLession_QuestionLessionId",
                        column: x => x.QuestionLessionId,
                        principalTable: "QuestionLession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OptionLessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionLessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionLessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionLessions_QuestionLession_QuestionLessionId",
                        column: x => x.QuestionLessionId,
                        principalTable: "QuestionLession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Hội trường" });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Lầu 2" });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Lầu 3" });

            migrationBuilder.CreateIndex(
                name: "IX_Account_RoleId",
                table: "Account",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountDetail_AccountId",
                table: "AccountDetail",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnswerCourse_QuestionCourseId",
                table: "AnswerCourse",
                column: "QuestionCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerLession_QuestionLessionId",
                table: "AnswerLession",
                column: "QuestionLessionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerMonthly_QuestionMonthlyId",
                table: "AnswerMonthly",
                column: "QuestionMonthlyId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_RoomId",
                table: "Appointments",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Asset_AssetsCategoryId",
                table: "Asset",
                column: "AssetsCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Asset_AssetsDepartmentId",
                table: "Asset",
                column: "AssetsDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Asset_AssetsStatusId",
                table: "Asset",
                column: "AssetsStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Asset_AssetsSupplierId",
                table: "Asset",
                column: "AssetsSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMoved_AssestsId",
                table: "AssetMoved",
                column: "AssestsId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMoved_AssetsDepartmentId",
                table: "AssetMoved",
                column: "AssetsDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetMoved_AssetsMovedStatusId",
                table: "AssetMoved",
                column: "AssetsMovedStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetRepaired_AssestsId",
                table: "AssetRepaired",
                column: "AssestsId");

            migrationBuilder.CreateIndex(
                name: "IX_Avatar_AccountId",
                table: "Avatar",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CorrectAnswerCourse_QuestionFinalId",
                table: "CorrectAnswerCourse",
                column: "QuestionFinalId");

            migrationBuilder.CreateIndex(
                name: "IX_CorrectAnswerLession_QuestionLessionId",
                table: "CorrectAnswerLession",
                column: "QuestionLessionId");

            migrationBuilder.CreateIndex(
                name: "IX_CorrectAnswerMonthly_QuestionMonthlyId",
                table: "CorrectAnswerMonthly",
                column: "QuestionMonthlyId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseImageVideo_CourseId",
                table: "CourseImageVideo",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRole_RoleId",
                table: "CourseRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryAddSlots_RoomId",
                table: "HistoryAddSlots",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Lession_CourseId",
                table: "Lession",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_LessionVideoImage_LessionId",
                table: "LessionVideoImage",
                column: "LessionId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionCourse_QuestionFinalId",
                table: "OptionCourse",
                column: "QuestionFinalId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionLessions_QuestionLessionId",
                table: "OptionLessions",
                column: "QuestionLessionId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionMonthly_QuestionMonthlyId",
                table: "OptionMonthly",
                column: "QuestionMonthlyId");

            migrationBuilder.CreateIndex(
                name: "IX_OtherFileUpload_CourseId",
                table: "OtherFileUpload",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_OtherFileUpload_LessionId",
                table: "OtherFileUpload",
                column: "LessionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionFinal_QuizCourseId",
                table: "QuestionFinal",
                column: "QuizCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionLession_QuizLessionId",
                table: "QuestionLession",
                column: "QuizLessionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionMonthly_QuizMonthlyId",
                table: "QuestionMonthly",
                column: "QuizMonthlyId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizCourse_CourseId",
                table: "QuizCourse",
                column: "CourseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuizCourse_HistorySubmitFinalId",
                table: "QuizCourse",
                column: "HistorySubmitFinalId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizCourse_ReportUserScoreFinalId",
                table: "QuizCourse",
                column: "ReportUserScoreFinalId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizLession_HistorySubmitLessionId",
                table: "QuizLession",
                column: "HistorySubmitLessionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizLession_LessionId",
                table: "QuizLession",
                column: "LessionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizLession_ReportUserScoreId",
                table: "QuizLession",
                column: "ReportUserScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizMonthly_HistorySubmitMonthlyId",
                table: "QuizMonthly",
                column: "HistorySubmitMonthlyId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizMonthly_ReportUserScoreMonthlyId",
                table: "QuizMonthly",
                column: "ReportUserScoreMonthlyId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizMonthly_RoleId",
                table: "QuizMonthly",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountDetail");

            migrationBuilder.DropTable(
                name: "AnswerCourse");

            migrationBuilder.DropTable(
                name: "AnswerLession");

            migrationBuilder.DropTable(
                name: "AnswerMonthly");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "AssetMoved");

            migrationBuilder.DropTable(
                name: "AssetRepaired");

            migrationBuilder.DropTable(
                name: "Avatar");

            migrationBuilder.DropTable(
                name: "CorrectAnswerCourse");

            migrationBuilder.DropTable(
                name: "CorrectAnswerLession");

            migrationBuilder.DropTable(
                name: "CorrectAnswerMonthly");

            migrationBuilder.DropTable(
                name: "CourseImageVideo");

            migrationBuilder.DropTable(
                name: "CourseRole");

            migrationBuilder.DropTable(
                name: "HistoryAddSlots");

            migrationBuilder.DropTable(
                name: "LessionVideoImage");

            migrationBuilder.DropTable(
                name: "NotificationResponse");

            migrationBuilder.DropTable(
                name: "OptionCourse");

            migrationBuilder.DropTable(
                name: "OptionLessions");

            migrationBuilder.DropTable(
                name: "OptionMonthly");

            migrationBuilder.DropTable(
                name: "OtherFileUpload");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "AssetMovedStatus");

            migrationBuilder.DropTable(
                name: "Asset");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "QuestionFinal");

            migrationBuilder.DropTable(
                name: "QuestionLession");

            migrationBuilder.DropTable(
                name: "QuestionMonthly");

            migrationBuilder.DropTable(
                name: "AssetCategory");

            migrationBuilder.DropTable(
                name: "AssetDepartment");

            migrationBuilder.DropTable(
                name: "AssetStatus");

            migrationBuilder.DropTable(
                name: "AssetSupplier");

            migrationBuilder.DropTable(
                name: "QuizCourse");

            migrationBuilder.DropTable(
                name: "QuizLession");

            migrationBuilder.DropTable(
                name: "QuizMonthly");

            migrationBuilder.DropTable(
                name: "HistorySubmitFinal");

            migrationBuilder.DropTable(
                name: "ReportUserScoreFinal");

            migrationBuilder.DropTable(
                name: "HistorySubmitLession");

            migrationBuilder.DropTable(
                name: "Lession");

            migrationBuilder.DropTable(
                name: "ReportUserScore");

            migrationBuilder.DropTable(
                name: "HistorySubmitMonthly");

            migrationBuilder.DropTable(
                name: "ReportUserScoreMonthly");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Course");
        }
    }
}
