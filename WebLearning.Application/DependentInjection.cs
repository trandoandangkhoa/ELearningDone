using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebLearning.Application.Assets.Services;
using WebLearning.Application.BookingCalender.Services;
using WebLearning.Application.ELearning.Services;
using WebLearning.Application.Email;
using WebLearning.Application.Mapping.AccountMappinng;
using WebLearning.Application.Mapping.AnswerMapping;
using WebLearning.Application.Mapping.AssetMapping;
using WebLearning.Application.Mapping.AvatarMapping;
using WebLearning.Application.Mapping.BookingCalender;
using WebLearning.Application.Mapping.CorrectAnswerMappingProfile;
using WebLearning.Application.Mapping.CourseMapping;
using WebLearning.Application.Mapping.CourseRoleMapping;
using WebLearning.Application.Mapping.HistorySubmitMapping;
using WebLearning.Application.Mapping.LessionMapping;
using WebLearning.Application.Mapping.LoginMapping;
using WebLearning.Application.Mapping.NotificationMapping;
using WebLearning.Application.Mapping.OptionMapping;
using WebLearning.Application.Mapping.QuestionMapping;
using WebLearning.Application.Mapping.QuizMapping;
using WebLearning.Application.Mapping.ReportScoreMapping;
using WebLearning.Application.Mapping.RoleMapping;
using WebLearning.Application.Validation;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.Course.CourseAdminView;
using WebLearning.Contract.Dtos.Lession;
using WebLearning.Contract.Dtos.Lession.LessionAdminView;
using WebLearning.Contract.Dtos.Question.QuestionCourseAdminView;
using WebLearning.Contract.Dtos.Question.QuestionLessionAdminView;
using WebLearning.Contract.Dtos.Question.QuestionMonthlyAdminView;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application
{
    public static class DependentInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<WebLearningContext>(options => options.UseSqlServer(configuration.GetConnectionString("WebLearningConnection")
                                                      , o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));


            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IAnswerCourseService, AnswerCourseService>();

            services.AddScoped<IAnswerLessionService, AnswerLessionService>();

            services.AddScoped<IAnswerMonthlyService, AnswerMonthlyService>();

            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IAssetMovedService, AssetMovedService>();

            services.AddScoped<ICourseService, CourseService>();

            services.AddScoped<ICourseRoleService, CourseRoleService>();

            services.AddScoped<IOptionLessionService, OptionLessionService>();

            services.AddScoped<IOptionCourseService, OptionCourseService>();

            services.AddScoped<IOptionMonthlyService, OptionMonthlyService>();

            services.AddScoped<IAppointmentService, AppointmentService>();

            services.AddScoped<ICorrectAnswerService, CorrectAnswerService>();

            services.AddScoped<ICorrectAnswerCourseService, CorrectAnswerCourseService>();

            services.AddScoped<ICorrectAnswerMonthlyService, CorrectAnswerMonthlyService>();


            services.AddScoped<IHistorySubmitCourseService, HistorySubmitCourseService>();

            services.AddScoped<IHistorySubmitLessionService, HistorySubmitLessionService>();

            services.AddScoped<IHistorySubmitMonthlyService, HistorySubmitMonthlyService>();

            services.AddScoped<ILessionService, LessionService>();

            services.AddScoped<ILoginService, LoginService>();

            services.AddScoped<IQuestionCourseService, QuestionCourseService>();

            services.AddScoped<IQuestionLessionService, QuestionLessionService>();

            services.AddScoped<IQuestionMonthlyService, QuestionMonthlyService>();

            services.AddScoped<IQuizCourseService, QuizCourseService>();

            services.AddScoped<IQuizLessionService, QuizLessionService>();

            services.AddScoped<IQuizMonthlyService, QuizMonthlyService>();

            services.AddScoped<IReportScoreCourseService, ReportScoreCourseService>();

            services.AddScoped<IReportScoreLessionService, ReportScoreLessionService>();

            services.AddScoped<IReportScoreMonthlyService, ReportScoreMonthlyService>();

            services.AddScoped<IStorageService, StorageService>();

            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IRoomService, RoomService>();

            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IImportExcelService, ImportExcelService>();

            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IChartService, ChartService>();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();


            //Validation
            services.AddScoped<IValidator<CreateCourseDto>, CreateCourseValidation>();
            services.AddScoped<IValidator<UpdateCourseAdminView>, UpdateCourseValidation>();
            services.AddScoped<IValidator<CreateLessionAdminView>, CreateLessionValidation>();
            services.AddScoped<IValidator<UpdateLessionDto>, UpdateLessionValidation>();


            services.AddScoped<IValidator<CreateQuizLessionDto>, CreateQuizLessionValidation>();
            services.AddScoped<IValidator<CreateQuizCourseDto>, CreateQuizCourseValidation>();
            services.AddScoped<IValidator<CreateQuizMonthlyDto>, CreateQuizMonthlyValidation>();

            services.AddScoped<IValidator<UpdateQuizLessionDto>, UpdateQuizLessionValidation>();
            services.AddScoped<IValidator<UpdateQuizCourseDto>, UpdateQuizCourseValidation>();
            services.AddScoped<IValidator<UpdateQuizMonthlyDto>, UpdateQuizMonthlyValidation>();

            services.AddScoped<IValidator<CreateAllConcerningQuestionLessionDto>, CreateQuestionLessionValidation>();
            services.AddScoped<IValidator<CreateAllConcerningQuestionCourseDto>, CreateQuestionCourseValidation>();
            services.AddScoped<IValidator<CreateAllConcerningQuestionMonthlyDto>, CreateQuestionMonthlyValidation>();

            services.AddScoped<IValidator<UpdateAllConcerningQuestionLesstionDto>, UpdateQuestionLessionValidation>();
            services.AddScoped<IValidator<UpdateAllConcerningQuestionCourseDto>, UpdateQuestionCourseValidation>();
            services.AddScoped<IValidator<UpdateAllConcerningQuestionMonthlyDto>, UpdateQuestionMonthlyValidation>();

            services.AddScoped<IEmailSender, EmailSender>();

            services.AddAutoMapper(typeof(AccountMappingProfile).Assembly);

            services.AddAutoMapper(typeof(AnswerCourseMappingProfile).Assembly);

            services.AddAutoMapper(typeof(AnswerLessionMappingProfile).Assembly);

            services.AddAutoMapper(typeof(AnswerMonthlyMappingProfile).Assembly);

            services.AddAutoMapper(typeof(AvatarMappingProfile).Assembly);

            services.AddAutoMapper(typeof(AssetsMappingProfile).Assembly);
            services.AddAutoMapper(typeof(AssetsCategoryMappingProfile).Assembly);
            services.AddAutoMapper(typeof(AssetsDepartmentMappingProfile).Assembly);
            services.AddAutoMapper(typeof(AssetsStatusMappingProfile).Assembly);
            services.AddAutoMapper(typeof(AssetsMovedMappingProfile).Assembly);
            services.AddAutoMapper(typeof(AssetsMovedStatusMappingProfile).Assembly);

            services.AddAutoMapper(typeof(AppointmentMappingProfile).Assembly);

            services.AddAutoMapper(typeof(RoomMappingProfile).Assembly);


            services.AddAutoMapper(typeof(CorrectAnswerLessionMappingProfile).Assembly);

            services.AddAutoMapper(typeof(CorrectAnswerCourseMappingProfile).Assembly);

            services.AddAutoMapper(typeof(CorrectAnswerMonthlyMappingProfile).Assembly);



            services.AddAutoMapper(typeof(OptionLessionMappingProfile).Assembly);

            services.AddAutoMapper(typeof(OptionCourseMappingProfile).Assembly);

            services.AddAutoMapper(typeof(OptionMonthlyMappingProfile).Assembly);



            services.AddAutoMapper(typeof(CourseMappingProfile).Assembly);

            services.AddAutoMapper(typeof(CourseRoleMappingProfile).Assembly);


            services.AddAutoMapper(typeof(HistorySubmitCourseMappingProfile).Assembly);

            services.AddAutoMapper(typeof(HistorySubmitLessionMappingProfile).Assembly);

            services.AddAutoMapper(typeof(HistorySubmitMonthlyMappingProfile).Assembly);
            services.AddAutoMapper(typeof(HistoryAddSlotMappingProfile).Assembly);


            services.AddAutoMapper(typeof(LessionMappingProfile).Assembly);

            services.AddAutoMapper(typeof(LoginMappingPofile).Assembly);

            services.AddAutoMapper(typeof(QuestionLessionMappingProfile).Assembly);

            services.AddAutoMapper(typeof(QuestionCourseMappingProfile).Assembly);

            services.AddAutoMapper(typeof(QuestionMonthlyMappingProfile).Assembly);


            services.AddAutoMapper(typeof(QuizLessionMappingProfile).Assembly);

            services.AddAutoMapper(typeof(QuizCourseMappingProfile).Assembly);

            services.AddAutoMapper(typeof(QuizMonthlyMappingProfile).Assembly);


            services.AddAutoMapper(typeof(ReportScoreCourseMappingProfile).Assembly);

            services.AddAutoMapper(typeof(ReportScoreLessionMappingProfile).Assembly);

            services.AddAutoMapper(typeof(ReportScoreMonthlyMappingProfile).Assembly);


            services.AddAutoMapper(typeof(RoleMappingProfile).Assembly);

            services.AddAutoMapper(typeof(NotificationMappingProfile).Assembly);


            //Config AppSetting
            services.Configure<AppSetting>(configuration.GetSection("AppSettings"));

            var secretKey = configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        //tự cấp token
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        //ký vào token
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                        ClockSkew = TimeSpan.Zero,

                        ValidateLifetime = false,
                    };
                });

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            var emailConfig = configuration
            .GetSection("EmailConfiguration")
            .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            return services;
        }
    }
}