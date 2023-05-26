using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using System.Reflection;
using WebLearning.Application;
using WebLearning.Application.Validation;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

var urlAdmin = builder.Configuration["UrlAdmin"];
var urlUser = builder.Configuration["UrlUser"];
var urlBooking = builder.Configuration["UrlBooking"];

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
        builder =>
        {
            builder.WithOrigins(urlAdmin, urlUser, urlBooking)
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});
//Add services to the container.
//Fix  A possible object cycle was detected which is not supported
builder.Services.AddControllers()
    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddValidatorsFromAssemblyContaining<CreateCourseValidation>();

builder.Services.AddResponseCaching();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Van Xuan System", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
    {
        new OpenApiSecurityScheme
        {
        Reference = new OpenApiReference
            {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,
        },
        new List<string>()
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

//builder.Services.AddAutoMapper(typeof(AccountMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(AnswerCourseMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(AnswerLessionMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(AnswerMonthlyMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(AvatarMappingProfile).Assembly);


//builder.Services.AddAutoMapper(typeof(CorrectAnswerLessionMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(CorrectAnswerCourseMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(CorrectAnswerMonthlyMappingProfile).Assembly);



//builder.Services.AddAutoMapper(typeof(OptionLessionMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(OptionCourseMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(OptionMonthlyMappingProfile).Assembly);



//builder.Services.AddAutoMapper(typeof(CourseMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(CourseRoleMappingProfile).Assembly);


//builder.Services.AddAutoMapper(typeof(HistorySubmitCourseMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(HistorySubmitLessionMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(HistorySubmitMonthlyMappingProfile).Assembly);


//builder.Services.AddAutoMapper(typeof(LessionMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(LoginMappingPofile).Assembly);

//builder.Services.AddAutoMapper(typeof(QuestionLessionMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(QuestionCourseMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(QuestionMonthlyMappingProfile).Assembly);


//builder.Services.AddAutoMapper(typeof(QuizLessionMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(QuizCourseMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(QuizMonthlyMappingProfile).Assembly);


//builder.Services.AddAutoMapper(typeof(ReportScoreCourseMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(ReportScoreLessionMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(ReportScoreMonthlyMappingProfile).Assembly);


//builder.Services.AddAutoMapper(typeof(RoleMappingProfile).Assembly);

//builder.Services.AddAutoMapper(typeof(NotificationMappingProfile).Assembly);


//Config DI in application 
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddDirectoryBrowser();
builder.Services.Configure<ForwardedHeadersOptions>(opt =>
{
    opt.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Van Xuan System");
    });
}
app.UseForwardedHeaders();
app.UseHttpsRedirection();
app.UseDeveloperExceptionPage();

app.UseStaticFiles();
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), "avatarImage")),
    RequestPath = "/avatarImage",
    EnableDirectoryBrowsing = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), "imageCourse")),
    RequestPath = "/imageCourse",
    EnableDirectoryBrowsing = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), "imageLession")),
    RequestPath = "/imageLession",
    EnableDirectoryBrowsing = true
});
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), "document")),
    RequestPath = "/document",
    EnableDirectoryBrowsing = true
});
app.UseRouting();

//app.UseCors("MyAllowedOrigins");
app.UseCors("AllowOrigin");

app.UseResponseCaching();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.Run();
