using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebLearning.ApiIntegration.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddSessionStateTempDataProvider();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/dang-nhap.html";
        options.AccessDeniedPath = "/User/Forbidden/";
        options.ExpireTimeSpan = TimeSpan.FromDays(2);

    });
builder.Services.AddSession(options =>
{

    options.IdleTimeout = TimeSpan.FromDays(2);
});
builder.Services.AddResponseCaching();

builder.Services.AddScoped<ICourseService, CourseService>();

builder.Services.AddScoped<ICourseRoleService, CourseRoleService>();

builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<ILoginService, LoginServie>();

builder.Services.AddScoped<ILessionService, LessionService>();

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IAssetService, AssetService>();

builder.Services.AddScoped<IAssetSupplierService, AssetSupplierService>();

builder.Services.AddScoped<IAssetRepairedService, AssetRepairedService>();


builder.Services.AddScoped<IAssetService, AssetService>();

builder.Services.AddScoped<IImportExcelService, ImportExcelService>();

builder.Services.AddScoped<IAssetCategoryService, AssetCategoryService>();


builder.Services.AddScoped<IAssetDepartmentService, AssetDepartmentService>();

builder.Services.AddScoped<IAssetStatusService, AssetStatusService>();

builder.Services.AddScoped<IAssetMovedService, AssetMoveService>();


builder.Services.AddScoped<IQuizLessionService, QuizLessionService>();

builder.Services.AddScoped<IQuizCourseService, QuizCourseService>();

builder.Services.AddScoped<IQuizMonthlyService, QuizMonthlyService>();

builder.Services.AddScoped<IAnswerLessionService, AnswerLessionService>();

builder.Services.AddScoped<IAnswerCourseService, AnswerCourseService>();

builder.Services.AddScoped<IAnswerMonthlyService, AnswerMonthlyService>();

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

builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddScoped<IRoomService, RoomService>();

builder.Services.AddScoped<IIChartService, IChartService>();

builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
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
app.UseResponseCaching();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
