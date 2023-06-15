using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WebLearning.Domain;
using WebLearning.Domain.Entites;

namespace WebLearning.Persistence.ApplicationContext
{
    public class WebLearningContext : DbContext
    {
        //private readonly IConfiguration configuration;
        public WebLearningContext(DbContextOptions<WebLearningContext> options) : base(options)
        {
        }
        public class BloggingContextFactory : IDesignTimeDbContextFactory<WebLearningContext>
        {
            public WebLearningContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<WebLearningContext>();
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=VXHAPP;User ID=sa;Password=12345",
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));

                return new WebLearningContext(optionsBuilder.Options);
            }
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AnswerLession> AnswerLessions { get; set; }

        public DbSet<AnswerCourse> AnswerCourses { get; set; }

        public DbSet<AnswerMonthly> AnswerMonthlies { get; set; }

        public DbSet<AccountDetail> AccountDetails { get; set; }

        public DbSet<Avatar> Avatars { get; set; }

        public DbSet<Assests> Assests { get; set; }
        public DbSet<AssetsCategory> AssetsCategories { get; set; }
        public DbSet<AssetsDepartment> AssetsDepartments { get; set; }
        public DbSet<AssetsStatus> AssetsStatuses { get; set; }


        public DbSet<Role> Roles { get; set; }

        public DbSet<CourseRole> CourseRoles { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseImageVideo> CourseImageVideos { get; set; }

        public DbSet<CorrectAnswerLession> CorrectAnswerLessions { get; set; }

        public DbSet<CorrectAnswerCourse> CorrectAnswerCourses { get; set; }

        public DbSet<CorrectAnswerMonthly> CorrectAnswerMonthlies { get; set; }

        public DbSet<Lession> Lessions { get; set; }

        public DbSet<LessionVideoImage> LessionVideoImages { get; set; }

        public DbSet<QuizLession> QuizLessions { get; set; }

        public DbSet<QuizCourse> QuizCourses { get; set; }

        public DbSet<NotificationResponse> NotificationResponses { get; set; }

        public DbSet<QuizMonthly> QuizMonthlies { get; set; }

        public DbSet<QuestionLession> QuestionLessions { get; set; }

        public DbSet<QuestionFinal> QuestionFinals { get; set; }

        public DbSet<QuestionMonthly> QuestionMonthlies { get; set; }

        public DbSet<HistorySubmitFinal> HistorySubmitFinals { get; set; }

        public DbSet<HistorySubmitLession> HistorySubmitLessions { get; set; }
        public DbSet<HistorySubmitMonthly> HistorySubmitMonthlies { get; set; }

        public DbSet<ReportUserScore> ReportUsersScore { get; set; }

        public DbSet<ReportUserScoreFinal> ReportUserScoreFinals { get; set; }

        public DbSet<ReportUserScoreMonthly> ReportUserScoreMonthlies { get; set; }

        public DbSet<OtherFileUpload> OtherFileUploads { get; set; }

        public DbSet<OptionLession> OptionLessions { get; set; }

        public DbSet<OptionCourse> OptionCourses { get; set; }

        public DbSet<OptionMonthly> OptionMonthlies { get; set; }

        //============= BOOKING =====================================

        public DbSet<AppointmentSlot> Appointments { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<HistoryAddSlot> HistoryAddSlots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Account>().ToTable("Account")
                                        .Property(e => e.AuthorizeRole)
                                        .HasConversion(
                                        v => v.ToString(),
                                        v => (AuthorizeRoles)Enum.Parse(typeof(AuthorizeRoles), v));

            modelBuilder.Entity<CourseRole>().ToTable("CourseRole").HasKey(sc => new { sc.CourseId, sc.RoleId });

            modelBuilder.Entity<AnswerLession>().ToTable("AnswerLession");

            modelBuilder.Entity<AnswerCourse>().ToTable("AnswerCourse");

            modelBuilder.Entity<AnswerMonthly>().ToTable("AnswerMonthly");

            modelBuilder.Entity<AccountDetail>().ToTable("AccountDetail");

            modelBuilder.Entity<Avatar>().ToTable("Avatar");

            modelBuilder.Entity<Assests>().ToTable("Assets");
            modelBuilder.Entity<AssetsCategory>().ToTable("AssetsCategory");
            modelBuilder.Entity<AssetsDepartment>().ToTable("AssetsDepartment");
            modelBuilder.Entity<AssetsStatus>().ToTable("AssetsStatus").Property(f => f.Id)
            .ValueGeneratedOnAdd();


            modelBuilder.Entity<Role>().ToTable("Role");

            modelBuilder.Entity<RefreshToken>().ToTable("RefreshToken");

            modelBuilder.Entity<Course>().ToTable("Course");

            modelBuilder.Entity<CourseImageVideo>().ToTable("CourseImageVideo");

            modelBuilder.Entity<CorrectAnswerLession>().ToTable("CorrectAnswerLession");

            modelBuilder.Entity<CorrectAnswerCourse>().ToTable("CorrectAnswerCourse");

            modelBuilder.Entity<CorrectAnswerMonthly>().ToTable("CorrectAnswerMonthly");


            modelBuilder.Entity<OptionLession>().ToTable("OptionLessions");

            modelBuilder.Entity<OptionCourse>().ToTable("OptionCourse");

            modelBuilder.Entity<OptionMonthly>().ToTable("OptionMonthly");

            modelBuilder.Entity<NotificationResponse>().ToTable("NotificationResponse");

            modelBuilder.Entity<Lession>().ToTable("Lession");

            modelBuilder.Entity<LessionVideoImage>().ToTable("LessionVideoImage");

            modelBuilder.Entity<QuizLession>().ToTable("QuizLession");

            modelBuilder.Entity<QuizCourse>().ToTable("QuizCourse");

            modelBuilder.Entity<QuizLession>().ToTable("QuizLession");

            modelBuilder.Entity<QuizMonthly>().ToTable("QuizMonthly");

            modelBuilder.Entity<QuestionLession>().ToTable("QuestionLession");

            modelBuilder.Entity<QuestionFinal>().ToTable("QuestionFinal");

            modelBuilder.Entity<QuestionMonthly>().ToTable("QuestionMonthly");

            modelBuilder.Entity<HistorySubmitLession>().ToTable("HistorySubmitLession");

            modelBuilder.Entity<HistorySubmitFinal>().ToTable("HistorySubmitFinal");

            modelBuilder.Entity<HistorySubmitMonthly>().ToTable("HistorySubmitMonthly");

            modelBuilder.Entity<ReportUserScore>().ToTable("ReportUserScore");

            modelBuilder.Entity<ReportUserScoreFinal>().ToTable("ReportUserScoreFinal");

            modelBuilder.Entity<ReportUserScoreMonthly>().ToTable("ReportUserScoreMonthly");

            //modelBuilder.Entity<QuizLession>().ToTable("Quiz");

            modelBuilder.Entity<OtherFileUpload>().ToTable("OtherFileUpload");

            //======================BOOKING==================================

            modelBuilder.Entity<Room>().HasData(new Room { Id = 1, Name = "Hội trường" });
            modelBuilder.Entity<Room>().HasData(new Room { Id = 2, Name = "Lầu 2" });
            modelBuilder.Entity<Room>().HasData(new Room { Id = 3, Name = "Lầu 3" });
        }
    }
}
