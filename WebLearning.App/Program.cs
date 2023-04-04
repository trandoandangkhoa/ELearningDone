using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Validation;
using WebLearning.Contract.Dtos;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDirectoryBrowser();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));

var secretKey = builder.Configuration["AppSettings:SecretKey"];


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/dang-nhap.html";
        options.AccessDeniedPath = "/User/Forbidden/";
        options.ExpireTimeSpan = TimeSpan.FromDays(2);

    });

builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromHours(1));

builder.Services.AddScoped<ILoginService, LoginServie>();

builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<ICourseService, CourseService>();

builder.Services.AddScoped<ICourseRoleService, CourseRoleService>();

builder.Services.AddScoped<ICorrectAnswerLessionService, CorrectAnswerLessionService>();

builder.Services.AddScoped<ICorrectAnswerCourseService, CorrectAnswerCourseService>();

builder.Services.AddScoped<ICorrectAnswerMonthlyService, CorrectAnswerMonthlyService>();

builder.Services.AddScoped<IOptionLessionService, OptionLessionService>();

builder.Services.AddScoped<IOptionCourseService, OptionCourseService>();

builder.Services.AddScoped<IOptionMonthlyervice, OptionMonthlyervice>();


builder.Services.AddScoped<ILessionService, LessionService>();

builder.Services.AddScoped<IQuizLessionService, QuizLessionService>();

builder.Services.AddScoped<IQuizMonthlyService, QuizMonthlyService>();

builder.Services.AddScoped<IQuizCourseService, QuizCourseService>();

builder.Services.AddScoped<IQuestionLessionService, QuestionLessionService>();

builder.Services.AddScoped<IQuestionCourseService, QuestionCourseService>();

builder.Services.AddScoped<IQuestionMonthlyService, QuestionMonthlyService>();

builder.Services.AddScoped<IHistorySubmitScoreLession, HistorySubmitScoreLession>();

builder.Services.AddScoped<IHistorySubmitScoreCourse, HistorySubmitScoreCourse>();

builder.Services.AddScoped<IHistorySubmitScoreMonthly, HistorySubmitScoreMonthly>();

builder.Services.AddScoped<IReportScoreLessionService, ReportScoreLessionService>();

builder.Services.AddScoped<IReportScoreCourseService, ReportScoreCourseService>();

builder.Services.AddScoped<IReportScoreMonthlyService, ReportScoreMonthlyService>();

builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IImportExcelService, ImportExcelService>();


builder.Services.AddValidatorsFromAssemblyContaining<CreateCourseValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCourseValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateLessionValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateLessionValidation>();


builder.Services.AddValidatorsFromAssemblyContaining<CreateQuestionLessionValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateQuestionCourseValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateQuestionMonthlyValidation>();

builder.Services.AddValidatorsFromAssemblyContaining<UpdateQuestionLessionValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateQuestionCourseValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateQuestionMonthlyValidation>();


builder.Services.AddValidatorsFromAssemblyContaining<CreateQuizLessionValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateQuizCourseValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateQuizMonthlyValidation>();

builder.Services.AddValidatorsFromAssemblyContaining<UpdateQuizLessionValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateQuizCourseValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateQuizMonthlyValidation>();



builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseNotyf();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "AdminIndex",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
