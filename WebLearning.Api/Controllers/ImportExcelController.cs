using Microsoft.AspNetCore.Mvc;
using NuGet.ContentModel;
using OfficeOpenXml;
using System;
using System.Globalization;
using WebLearning.Application.Assets.Services;
using WebLearning.Application.ELearning.Services;
using WebLearning.Application.Helper;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.Account;
using WebLearning.Contract.Dtos.Assets;
using WebLearning.Contract.Dtos.Assets.Department;
using WebLearning.Contract.Dtos.CourseRole;
using WebLearning.Contract.Dtos.Lession.LessionAdminView;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Contract.Dtos.Question.QuestionCourseAdminView;
using WebLearning.Contract.Dtos.Question.QuestionLessionAdminView;
using WebLearning.Contract.Dtos.Question.QuestionMonthlyAdminView;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Contract.Dtos.Role;
using WebLearning.Contract.Dtos.VideoLession;
using WebLearning.Domain.Entites;

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportExcelController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IAccountService _accountService;
        private readonly IQuestionLessionService _questionLessionService;
        private readonly ICourseService _courseService;
        private readonly ILessionService _lessionService;
        private readonly ICourseRoleService _courseRoleService;
        private readonly IQuizLessionService _quizLessionService;
        private readonly IQuizCourseService _quizCourseService;
        private readonly IQuizMonthlyService _quizMonthlyService;
        private readonly IQuestionCourseService _questionCourseService;
        private readonly IQuestionMonthlyService _questionMonthlyService;
        private readonly IAssetService _assetService;
        private readonly ICategoryService _categoryService;
        private readonly IDepartmentService _departmentService;
        private readonly IStatusService _statusService;
        public ImportExcelController(IQuestionLessionService questionLessionService, IRoleService roleService, IAccountService accountService, ICourseRoleService courseRoleService
, ICourseService courseService, ILessionService lessionService, IQuizLessionService quizLessionService, IQuizCourseService quizCourseService, IQuizMonthlyService quizMonthlyService, IQuestionCourseService questionCourseService, IQuestionMonthlyService questionMonthlyService
            ,IAssetService assetService, IDepartmentService departmentService, IStatusService statusService,ICategoryService categoryService)
        
        {
            _roleService = roleService;
            _accountService = accountService;
            _courseRoleService = courseRoleService;
            _courseService = courseService;
            _lessionService = lessionService;
            _quizLessionService = quizLessionService;
            _quizCourseService = quizCourseService;
            _quizMonthlyService = quizMonthlyService;
            _questionLessionService = questionLessionService;
            _questionCourseService = questionCourseService;
            _questionMonthlyService = questionMonthlyService;
            _assetService = assetService;
            _departmentService = departmentService;
            _statusService = statusService;
            _categoryService = categoryService;
        }



        [HttpPost("import")]
        [Consumes("multipart/form-data")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.TeacherRole)]

        public async Task<string> Import([FromForm] ImportResponse importResponse, bool role, bool account, bool courseRole, bool lession, bool videoLession
                    , bool quizLession, bool quizCourse, bool quizMonthly, bool questionLession, bool questionCourse, bool questionMonthly, CancellationToken cancellationToken)
        {
            if (importResponse.File == null || importResponse.File.Length <= 0)
            {
                return importResponse.Msg = "Không tìm thấy File";
            }

            if (!Path.GetExtension(importResponse.File.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return importResponse.Msg = "File không đúng định dạng .xlsx";
            }
            if (role == true)
            {

                var createRoleDtos = new List<CreateRoleDto>();
                using (var stream = new MemoryStream())
                {
                    await importResponse.File.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        var key = Utilities.GenerateStringDateTime();
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;
                        var count = 0;
                        for (int row = 2; row <= rowCount; row++)
                        {

                            var name = await _roleService.GetName(worksheet.Cells[row, 1].Value.ToString().Trim());
                            if (name != null)
                            {
                                count++;
                            }
                        }

                        if (count > 0) return importResponse.Msg = $"Bạn có {count} quyền đã có dữ liệu. Vui lòng kiểm tra lại! ";

                        for (int row = 2; row <= rowCount; row++)
                        {

                            createRoleDtos.Add(new CreateRoleDto
                            {

                                //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                Id = Guid.NewGuid(),
                                RoleName = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                Description = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                //Active = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 4].Value.ToString().Trim())),
                            });
                        }
                    }
                }
                foreach (var item in createRoleDtos)
                {
                    await _roleService.InsertRole(item);
                }
            }
            if (account == true)
            {
                var createAccountDtos = new List<CreateAccountDto>();

                using (var stream = new MemoryStream())
                {
                    await importResponse.File.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        var rowCount = worksheet.Dimension.Rows;
                        var count = 0;
                        for (int row = 2; row <= rowCount; row++)
                        {

                            var name = await _accountService.GetNameUser(worksheet.Cells[row, 4].Value.ToString().Trim());
                            if (name != null)
                            {
                                count++;
                            }
                        }
                        if (count > 0) return importResponse.Msg = $"Bạn có {count} tài khoản bị trùng email. Vui lòng kiểm tra lại! ";
                        for (int row = 2; row <= rowCount; row++)
                        {
                            createAccountDtos.Add(new CreateAccountDto
                            {

                                //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                Id = Guid.NewGuid(),
                                FullName = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                Phone = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                Address = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                Email = worksheet.Cells[row, 4].Value.ToString().Trim(),
                                Password = worksheet.Cells[row, 5].Value.ToString().Trim(),
                                //Active = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 5].Value.ToString().Trim())),
                                Active = int.Parse(worksheet.Cells[row, 6].Value.ToString().Trim()),
                                CodeRole = worksheet.Cells[row, 7].Value.ToString().Trim(),

                            });

                        }
                    }
                }
                foreach (var item in createAccountDtos)
                {
                    var roleId = await _roleService.GetCode(item.CodeRole);
                    if (roleId == null) return importResponse.Msg = $"Không tìm thấy mã quyền {item.CodeRole}";
                    item.RoleId = roleId.Id;
                    await _accountService.InsertUserInfo(item);
                }
            }
            if (courseRole == true)
            {
                var createCourseRoleDtos = new List<CreateCourseRoleDto>();

                using (var stream = new MemoryStream())
                {
                    await importResponse.File.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[2];
                        var rowCount = worksheet.Dimension.Rows;
                        var count = 0;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var courseId = await _courseService.GetCode(worksheet.Cells[row, 1].Value.ToString().Trim());
                            var roleId = await _roleService.GetCode(worksheet.Cells[row, 2].Value.ToString().Trim());
                            var name = await _courseRoleService.AdminGetCourse(courseId.Id, roleId.Id);
                            if (name != null)
                            {
                                count++;
                            }
                        }
                        if (count > 0) return importResponse.Msg = $"Bạn có {count} quyền truy cập đã được cấp phép. Vui lòng kiểm tra lại! ";
                        for (int row = 2; row <= rowCount; row++)
                        {
                            createCourseRoleDtos.Add(new CreateCourseRoleDto
                            {

                                //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                Id = Guid.NewGuid(),
                                CodeCourse = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                CodeRole = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                //Active = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 3].Value.ToString().Trim())),
                                //CompletedTime = int.Parse(worksheet.Cells[row, 4].Value.ToString().Trim()),
                                //Notify = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 5].Value.ToString().Trim())),

                            });

                        }
                    }
                }
                foreach (var item in createCourseRoleDtos)
                {
                    var courseId = await _courseService.GetCode(item.CodeCourse);
                    if (courseId == null) return importResponse.Msg = $"Không tìm thấy mã khóa học {item.CodeCourse}";


                    var roleId = await _roleService.GetCode(item.CodeRole);
                    if (roleId == null) importResponse.Msg = $"Không tìm thấy mã quyền {item.CodeRole}";

                    item.RoleId = roleId.Id;
                    item.CourseId = courseId.Id;
                    await _courseRoleService.InsertCourseRole(item);
                }
            }
            if (lession == true)
            {
                var createLessionAdminViews = new List<CreateLessionAdminView>();

                using (var stream = new MemoryStream())
                {
                    await importResponse.File.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[3];
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            createLessionAdminViews.Add(new CreateLessionAdminView
                            {

                                //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                CreateLessionDto = new Contract.Dtos.Lession.CreateLessionDto()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                    ShortDesc = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                    Active = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 4].Value.ToString().Trim())),
                                    Notify = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 5].Value.ToString().Trim())),
                                    CodeCourse = worksheet.Cells[row, 6].Value.ToString().Trim()
                                },
                            });

                        }
                    }
                }
                foreach (var item in createLessionAdminViews)
                {

                    var roleId = await _courseService.GetCode(item.CreateLessionDto.CodeCourse);
                    if (roleId == null) return importResponse.Msg = $"Không tìm thấy mã khóa học {item.CreateLessionDto.CodeCourse}";
                    item.CreateLessionDto.CourseId = roleId.Id;

                    if (await _lessionService.CheckExist(item.CreateLessionDto.CourseId, item.CreateLessionDto.Name) != null)
                    {
                        return importResponse.Msg = $"Đã tồn tại chương {item.CreateLessionDto.Name} trong khóa học {roleId.Name}";
                    }
                    else
                    {
                        item.CreateLessionDto.DescNotify = "Bạn có một chương mới";
                        item.CreateLessionDto.Author = User.Identity.Name;
                        await _lessionService.CreateLessionAdminView(item);
                    }

                }
            }
            if (videoLession == true)
            {
                var createLessionVideoDtos = new List<CreateLessionVideoDto>();

                using (var stream = new MemoryStream())
                {
                    await importResponse.File.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[10];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            createLessionVideoDtos.Add(new CreateLessionVideoDto
                            {

                                Id = Guid.NewGuid(),
                                CodeLession = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                Caption = worksheet.Cells[row, 2].Value.ToString(),
                                Notify = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 3].Value.ToString().Trim())),
                                LinkVideo = worksheet.Cells[row, 4].Value.ToString().Trim(),
                                //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                            });

                        }
                    }
                }
                foreach (var item in createLessionVideoDtos)
                {

                    var roleId = await _lessionService.GetCode(item.CodeLession);
                    if (roleId == null) return importResponse.Msg = $"Không tìm thấy mã chương {item.CodeLession}";
                    item.LessionId = roleId.Id;
                    item.Alias = Utilities.SEOUrl(item.Caption);
                    item.DescNotify = "Bạn có một bài học mới";
                    await _lessionService.AddImage(item.LessionId, item);
                }
            }
            if (quizLession == true)
            {
                var createQuizLessionDtos = new List<CreateQuizLessionDto>();

                using (var stream = new MemoryStream())
                {
                    await importResponse.File.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[4];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            createQuizLessionDtos.Add(new CreateQuizLessionDto
                            {

                                //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                ID = Guid.NewGuid(),
                                Name = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                Description = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                Active = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 4].Value.ToString().Trim())),
                                TimeToDo = int.Parse(worksheet.Cells[row, 5].Value.ToString().Trim()),
                                ScorePass = int.Parse(worksheet.Cells[row, 6].Value.ToString().Trim()),
                                Notify = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 7].Value.ToString().Trim())),
                                CodeLession = worksheet.Cells[row, 2].Value.ToString().Trim()
                            });

                        }
                    }
                }
                foreach (var item in createQuizLessionDtos)
                {

                    var roleId = await _lessionService.GetCode(item.CodeLession);
                    if (roleId == null) return importResponse.Msg = $"Không tìm thấy mã chương {item.CodeLession}";
                    item.LessionId = roleId.Id;

                    if (await _quizLessionService.CheckExist(item.LessionId, item.Name) != null)
                    {
                        return importResponse.Msg = $"Đã tồn tại bài kiểm tra {item.Name} trong chương {roleId.Name}";
                    }
                    item.DescNotify = "Bạn có một bài kiểm tra mới";
                    await _quizLessionService.InsertQuizLessionDto(item);
                }
            }
            if (quizCourse == true)
            {
                var createQuizCourseDtos = new List<CreateQuizCourseDto>();

                using (var stream = new MemoryStream())
                {
                    await importResponse.File.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[5];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            createQuizCourseDtos.Add(new CreateQuizCourseDto
                            {

                                //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                ID = Guid.NewGuid(),
                                Name = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                Description = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                Active = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 4].Value.ToString().Trim())),
                                TimeToDo = int.Parse(worksheet.Cells[row, 5].Value.ToString().Trim()),
                                ScorePass = int.Parse(worksheet.Cells[row, 6].Value.ToString().Trim()),
                                Notify = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 7].Value.ToString().Trim())),

                                CodeCOurse = worksheet.Cells[row, 2].Value.ToString().Trim()
                            });

                        }
                    }
                }
                foreach (var item in createQuizCourseDtos)
                {

                    var roleId = await _courseService.GetCode(item.CodeCOurse);
                    if (roleId == null) return importResponse.Msg = $"Không tìm thấy mã khóa học {item.CodeCOurse}";
                    item.CourseId = roleId.Id;
                    if (await _quizCourseService.CheckExist(item.CourseId, item.Name) != null)
                    {
                        return importResponse.Msg = $"Đã tồn tại bài kiểm tra {item.Name} trong khóa học {roleId.Name}";
                    }
                    item.DescNotify = "Bạn có một bài kiểm tra mới";
                    await _quizCourseService.InsertQuizDto(item);
                }
            }
            if (quizMonthly == true)
            {
                var createQuizMonthlyDtos = new List<CreateQuizMonthlyDto>();

                using (var stream = new MemoryStream())
                {
                    await importResponse.File.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[6];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            createQuizMonthlyDtos.Add(new CreateQuizMonthlyDto
                            {

                                //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                ID = Guid.NewGuid(),
                                Name = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                Description = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                Active = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 4].Value.ToString().Trim())),
                                TimeToDo = int.Parse(worksheet.Cells[row, 5].Value.ToString().Trim()),
                                ScorePass = int.Parse(worksheet.Cells[row, 6].Value.ToString().Trim()),
                                Notify = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 7].Value.ToString().Trim())),

                                CodeRole = worksheet.Cells[row, 2].Value.ToString().Trim()
                            });

                        }
                    }
                }
                foreach (var item in createQuizMonthlyDtos)
                {

                    var roleId = await _roleService.GetCode(item.CodeRole);
                    if (roleId == null) return importResponse.Msg = $"Không tìm thấy mã quyền {item.CodeRole}";
                    item.RoleId = roleId.Id;

                    if (await _quizCourseService.CheckExist(item.RoleId, item.Name) != null)
                    {
                        return importResponse.Msg = $"Đã tồn tại bài kiểm tra {item.Name}";
                    }
                    item.DescNotify = "Bạn có một bài kiểm tra mới";
                    await _quizMonthlyService.InsertQuizDto(item);
                }
            }
            if (questionLession == true)
            {
                var createAllConcerningQuestionLessionDtos = new List<CreateAllConcerningQuestionLessionDto>();
                using (var stream = new MemoryStream())
                {
                    await importResponse.File.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[7];

                        var rowCount = worksheet.Dimension.Rows;
                        int count = 0;

                        string[] optionsSplit = null;

                        string[] correctArray = null;

                        //check equal
                        for (int row = 2; row <= rowCount; row++)
                        {
                            optionsSplit = worksheet.Cells[row, 5].Value.ToString().Trim().Split(new Char[] { '\n' });

                            correctArray = worksheet.Cells[row, 6].Value.ToString().Trim().Split(",");

                            if (optionsSplit.Length != correctArray.Length)
                            {
                                count++;
                            }
                        }
                        if (count > 0) return importResponse.Msg = $"Bạn có {count} dòng sai định dạng vị trí đúng và lựa chọn";
                        //insert
                        for (int row = 2; row <= rowCount; row++)
                        {

                            optionsSplit = worksheet.Cells[row, 5].Value.ToString().Trim().Split(new Char[] { '\n' });

                            correctArray = worksheet.Cells[row, 6].Value.ToString().Trim().Split(",");

                            bool[] correct = new bool[correctArray.Length];


                            for (int i = 0; i < correctArray.Length; i++)
                            {
                                var cr = Convert.ToBoolean(Convert.ToInt32(correctArray[i]));
                                correct[i] = cr;
                            };

                            createAllConcerningQuestionLessionDtos.Add(new CreateAllConcerningQuestionLessionDto
                            {

                                //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                CreateQuestionLessionDto = new CreateQuestionLessionDto()
                                {
                                    Id = Guid.NewGuid(),
                                    CodeQuiz = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                    Name = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                    Active = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 4].Value.ToString().Trim())),
                                    Point = int.Parse(worksheet.Cells[row, 3].Value.ToString().Trim()),
                                },
                                OptionLessions = optionsSplit,
                                CorrectAnswers = correct,
                            });

                        }
                    }
                }
                foreach (var item in createAllConcerningQuestionLessionDtos)
                {
                    var roleId = await _quizLessionService.GetCode(item.CreateQuestionLessionDto.CodeQuiz);
                    if (roleId == null) return importResponse.Msg = $"Không tìm thấy mã bài kiểm tra theo chương {item.CreateQuestionLessionDto.CodeQuiz}";
                    item.CreateQuestionLessionDto.QuizLessionId = roleId.ID;
                    //var questionName = await _questionLessionService.CheckExist(item.CreateQuestionLessionDto.QuizLessionId, item.CreateQuestionLessionDto.Name);

                    //if (questionName != null) continue; importResponse.Msg = $"Đã tồn tại câu hỏi {item.CreateQuestionLessionDto.Name}";

                    await _questionLessionService.InsertConcerningQuestionLessionDto(item);
                }

            }
            if (questionCourse == true)
            {
                var createAllConcerningQuestionCourseDtos = new List<CreateAllConcerningQuestionCourseDto>();

                using (var stream = new MemoryStream())
                {
                    await importResponse.File.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[8];
                        var rowCount = worksheet.Dimension.Rows;
                        int count = 0;

                        string[] optionsSplit = null;

                        string[] correctArray = null;

                        //check equal
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var s = worksheet.Cells[row, 5].Value.ToString().Trim();
                            optionsSplit = worksheet.Cells[row, 5].Value.ToString().Trim().Split(new Char[] { '\n' });

                            correctArray = worksheet.Cells[row, 6].Value.ToString().Trim().Split(",");

                            if (optionsSplit.Length != correctArray.Length)
                            {
                                count++;
                            }
                        }
                        if (count > 0) return importResponse.Msg = $"Bạn có {count} dòng sai định dạng vị trí đúng và lựa chọn";

                        for (int row = 2; row <= rowCount; row++)
                        {
                            optionsSplit = worksheet.Cells[row, 5].Value.ToString().Trim().Split(new Char[] { '\n' });

                            correctArray = worksheet.Cells[row, 6].Value.ToString().Trim().Split(",");

                            bool[] correct = new bool[correctArray.Length];


                            for (int i = 0; i < correctArray.Length; i++)
                            {
                                var cr = Convert.ToBoolean(Convert.ToInt32(correctArray[i]));
                                correct[i] = cr;
                            };
                            var check = worksheet.Cells[row, 1].Value.ToString().Trim();
                            if (check != null)
                            {
                                createAllConcerningQuestionCourseDtos.Add(new CreateAllConcerningQuestionCourseDto
                                {

                                    //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),

                                    CreateQuestionCourseDto = new CreateQuestionCourseDto()
                                    {
                                        Id = Guid.NewGuid(),
                                        CodeQuiz = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                        Name = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                        Active = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 4].Value.ToString().Trim())),
                                        Point = int.Parse(worksheet.Cells[row, 3].Value.ToString().Trim()),
                                    },
                                    OptionCourses = optionsSplit,
                                    CorrectAnswers = correct,

                                });
                            }


                        }
                    }
                }
                foreach (var item in createAllConcerningQuestionCourseDtos)
                {

                    var roleId = await _quizCourseService.GetCode(item.CreateQuestionCourseDto.CodeQuiz);
                    if (roleId == null) return importResponse.Msg = $"Không tìm thấy mã bài kiểm tra cuối khóa {item.CreateQuestionCourseDto.CodeQuiz}";
                    item.CreateQuestionCourseDto.QuizCourseId = roleId.ID;
                    //var questionName = await _questionCourseService.CheckExist(item.CreateQuestionCourseDto.QuizCourseId, item.CreateQuestionCourseDto.Name);
                    //if (questionName != null) return importResponse.Msg = $"Đã tồn tại câu hỏi {item.CreateQuestionCourseDto.Name}";
                    await _questionCourseService.InsertConcerningQuestionCourseDto(item);
                }
            }
            if (questionMonthly == true)
            {
                var createAllConcerningQuestionMonthlyDtos = new List<CreateAllConcerningQuestionMonthlyDto>();

                using (var stream = new MemoryStream())
                {
                    await importResponse.File.CopyToAsync(stream, cancellationToken);

                    using (var package = new ExcelPackage(stream))
                    {
                        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[9];
                        var rowCount = worksheet.Dimension.Rows;
                        int count = 0;

                        string[] optionsSplit = null;

                        string[] correctArray = null;

                        //check equal
                        for (int row = 2; row <= rowCount; row++)
                        {
                            optionsSplit = worksheet.Cells[row, 5].Value.ToString().Trim().Split(new Char[] { '\n' });

                            correctArray = worksheet.Cells[row, 6].Value.ToString().Trim().Split(",");

                            if (optionsSplit.Length != correctArray.Length)
                            {
                                count++;
                            }
                        }
                        if (count > 0) return importResponse.Msg = $"Bạn có {count} dòng sai định dạng vị trí đúng và lựa chọn";
                        for (int row = 2; row <= rowCount; row++)
                        {
                            optionsSplit = worksheet.Cells[row, 5].Value.ToString().Trim().Split(new Char[] { '\n' });

                            correctArray = worksheet.Cells[row, 6].Value.ToString().Trim().Split(",");

                            bool[] correct = new bool[correctArray.Length];


                            for (int i = 0; i < correctArray.Length; i++)
                            {
                                var cr = Convert.ToBoolean(Convert.ToInt32(correctArray[i]));
                                correct[i] = cr;
                            };
                            createAllConcerningQuestionMonthlyDtos.Add(new CreateAllConcerningQuestionMonthlyDto
                            {

                                //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                CreateQuestionMonthlyDto = new CreateQuestionMonthlyDto()
                                {
                                    Id = Guid.NewGuid(),
                                    CodeQuiz = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                    Name = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                    Active = Convert.ToBoolean(Convert.ToInt32(worksheet.Cells[row, 4].Value.ToString().Trim())),
                                    Point = int.Parse(worksheet.Cells[row, 3].Value.ToString().Trim()),
                                },
                                OptionMonthlys = optionsSplit,
                                CorrectAnswers = correct,
                            });

                        }
                    }
                }
                foreach (var item in createAllConcerningQuestionMonthlyDtos)
                {
                    var roleId = await _quizMonthlyService.GetCode(item.CreateQuestionMonthlyDto.CodeQuiz);
                    if (roleId == null) return importResponse.Msg = $"Không tìm thấy mã bài kiểm tra định kì {item.CreateQuestionMonthlyDto.CodeQuiz}";
                    item.CreateQuestionMonthlyDto.QuizMonthlyId = roleId.ID;
                    //var questionName = await _questionCourseService.CheckExist(item.CreateQuestionMonthlyDto.QuizMonthlyId, item.CreateQuestionMonthlyDto.Name);
                    //if (questionName != null) return importResponse.Msg = $"Đã tồn tại câu hỏi {item.CreateQuestionMonthlyDto.Name}";
                    await _questionMonthlyService.InsertConcerningQuestionMonthlyDto(item);
                }
            }

            return importResponse.Msg = "Import Success";
        }


        [HttpPost("import/assets")]
        [Consumes("multipart/form-data")]

        public async Task<string> ImportAsset([FromForm] ImportResponse importResponse, CancellationToken cancellationToken)
        {
            if (importResponse.File == null || importResponse.File.Length <= 0)
            {
                return importResponse.Msg = "Không tìm thấy File";
            }

            if (!Path.GetExtension(importResponse.File.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return importResponse.Msg = "File không đúng định dạng .xlsx";
            }
            var createAssetsDtos = new List<CreateAssetsDto>();

            using (var stream = new MemoryStream())
            {
                await importResponse.File.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    var a = package.Workbook.Worksheets.Count();
                    for(int i = 0; i< a; i++)
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[i];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            string customer = "";
                            string spec = "";
                            string note = "";
                            string orNum = "";
                            string seri = "";
                            string price = "";
                            string supplier = "";
                            int expireDay = 0;
                            if (worksheet.Cells[row, 3].Value != null) orNum = worksheet.Cells[row, 3].Value.ToString().Trim();
                            if (worksheet.Cells[row, 4].Value != null) seri = worksheet.Cells[row, 4].Value.ToString().Trim();
                            if (worksheet.Cells[row, 5].Value != null) price = worksheet.Cells[row, 5].Value.ToString().Trim();
                            if (worksheet.Cells[row, 6].Value != null) supplier = worksheet.Cells[row, 6].Value.ToString().Trim();

                            if (worksheet.Cells[row, 11].Value != null) customer = worksheet.Cells[row, 11].Value.ToString().Trim();
                            if (worksheet.Cells[row, 14].Value != null) spec = worksheet.Cells[row, 14].Value.ToString().Trim();
                            if (worksheet.Cells[row, 15].Value != null) note = worksheet.Cells[row, 15].Value.ToString().Trim();
                            if (worksheet.Cells[row, 19].Value != null) expireDay= int.Parse(worksheet.Cells[row, 19].Value.ToString().Trim());

                            var catId = await _categoryService.GetCode(worksheet.Cells[row, 7].Value.ToString().Trim());
                            var depId = await _departmentService.GetCode(worksheet.Cells[row, 10].Value.ToString().Trim());


                            if (catId == null ) return importResponse.Msg = $"Dòng {row} trong sheet {worksheet.Name} có mã loại không tồn tại";

                            if (worksheet.Cells[row, 16].Value == null || worksheet.Cells[row, 18].Value == null)
                            {
                                if(worksheet.Cells[row, 16].Value == null && worksheet.Cells[row,18].Value != null)
                                {
                                    createAssetsDtos.Add(new CreateAssetsDto
                                    {

                                        //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                        AssetId = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                        AssetName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                        OrderNumber = orNum,
                                        SeriNumber = seri,
                                        Price = price,
                                        Supplier = supplier,
                                        AssetsCategoryId = catId.Id,
                                        AssetSubCategory = worksheet.Cells[row, 8].Value.ToString().Trim(),
                                        Quantity = int.Parse(worksheet.Cells[row, 9].Value.ToString().Trim()),
                                        AssetsDepartmentId = depId.Id,
                                        Customer = customer,
                                        Manager = worksheet.Cells[row, 12].Value.ToString().Trim(),
                                        AssetsStatusId = int.Parse(worksheet.Cells[row, 13].Value.ToString().Trim()),
                                        Spec = spec,
                                        Note = note,
                                        Active = true,
                                        DateCreated = DateTime.Now,
                                        DateChecked = DateTime.ParseExact(worksheet.Cells[row, 17].Value.ToString().Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        DateBuyed = DateTime.ParseExact(worksheet.Cells[row, 18].Value.ToString().Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        ExpireDay = expireDay,
                                    });
                                }
                                else if(worksheet.Cells[row, 16].Value != null && worksheet.Cells[row, 18].Value == null)
                                {
                                    createAssetsDtos.Add(new CreateAssetsDto
                                    {

                                        //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                        AssetId = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                        AssetName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                        OrderNumber = orNum,
                                        SeriNumber = seri,
                                        Price = price,
                                        Supplier = supplier,
                                        AssetsCategoryId = catId.Id,
                                        AssetSubCategory = worksheet.Cells[row, 8].Value.ToString().Trim(),
                                        Quantity = int.Parse(worksheet.Cells[row, 9].Value.ToString().Trim()),
                                        AssetsDepartmentId = depId.Id,
                                        Customer = customer,
                                        Manager = worksheet.Cells[row, 12].Value.ToString().Trim(),
                                        AssetsStatusId = int.Parse(worksheet.Cells[row, 13].Value.ToString().Trim()),
                                        Spec = spec,
                                        Note = note,
                                        Active = true,
                                        DateCreated = DateTime.Now,
                                        DateUsed = DateTime.ParseExact(worksheet.Cells[row, 16].Value.ToString().Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        DateChecked = DateTime.ParseExact(worksheet.Cells[row, 17].Value.ToString().Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        ExpireDay = expireDay,
                                    });
                                }
                                else if (worksheet.Cells[row, 16].Value == null && worksheet.Cells[row, 18].Value == null)
                                {
                                    createAssetsDtos.Add(new CreateAssetsDto
                                    {

                                        //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                        AssetId = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                        AssetName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                        OrderNumber = orNum,
                                        SeriNumber = seri,
                                        Price = price,
                                        Supplier = supplier,
                                        AssetsCategoryId = catId.Id,
                                        AssetSubCategory = worksheet.Cells[row, 8].Value.ToString().Trim(),
                                        Quantity = int.Parse(worksheet.Cells[row, 9].Value.ToString().Trim()),
                                        AssetsDepartmentId = depId.Id,
                                        Customer = customer,
                                        Manager = worksheet.Cells[row, 12].Value.ToString().Trim(),
                                        AssetsStatusId = int.Parse(worksheet.Cells[row, 13].Value.ToString().Trim()),
                                        Spec = spec,
                                        Note = note,
                                        Active = true,
                                        DateCreated = DateTime.Now,
                                        DateChecked = DateTime.ParseExact(worksheet.Cells[row, 17].Value.ToString().Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        ExpireDay = expireDay,
                                    });
                                }
                            }
                            else
                            {
                                createAssetsDtos.Add(new CreateAssetsDto
                                {

                                    //Id = Guid.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                    AssetId = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                    AssetName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                    OrderNumber = orNum,
                                    SeriNumber = seri,
                                    Price = price,
                                    Supplier = supplier,
                                    AssetsCategoryId = catId.Id,
                                    AssetSubCategory = worksheet.Cells[row, 8].Value.ToString().Trim(),
                                    Quantity = int.Parse(worksheet.Cells[row, 9].Value.ToString().Trim()),
                                    AssetsDepartmentId = depId.Id,
                                    Customer = customer,
                                    Manager = worksheet.Cells[row, 12].Value.ToString().Trim(),
                                    AssetsStatusId = int.Parse(worksheet.Cells[row, 13].Value.ToString().Trim()),
                                    Spec = spec,
                                    Note = note,
                                    Active = true,
                                    DateCreated = DateTime.Now,
                                    DateUsed = DateTime.ParseExact(worksheet.Cells[row, 16].Value.ToString().Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    DateChecked = DateTime.ParseExact(worksheet.Cells[row, 17].Value.ToString().Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    DateBuyed = DateTime.ParseExact(worksheet.Cells[row, 18].Value.ToString().Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    ExpireDay = expireDay,
                                });

                            }

                        }

                    }
                    foreach (var item in createAssetsDtos)
                    {
                        await _assetService.InsertAsset(item);
                    }

                }
            }
            


            return importResponse.Msg = "Import Success";
        }
    }
}
