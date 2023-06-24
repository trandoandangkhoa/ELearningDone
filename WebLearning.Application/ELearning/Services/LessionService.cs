using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.Lession;
using WebLearning.Contract.Dtos.Lession.LessionAdminView;
using WebLearning.Contract.Dtos.LessionFileDocument;
using WebLearning.Contract.Dtos.Notification;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Contract.Dtos.ReportScore;
using WebLearning.Contract.Dtos.VideoLession;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.ELearning.Services
{
    public interface ILessionService
    {
        Task<IEnumerable<LessionDto>> GetLessionDtos();

        Task<LessionDto> GetName(Guid id);

        Task<LessionDto> CheckExist(Guid id, string name);
        Task<PagedViewModel<LessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<LessionDto> UserGetLessionById(Guid id, string accountName);

        Task<LessionDto> GetLessionById(Guid id);

        Task CreateLessionAdminView(CreateLessionAdminView createLessionAdminView);

        Task UpdateLessionDto(Guid id, UpdateLessionDto updateLessionDto);

        Task AddImage(Guid lessionId, CreateLessionVideoDto createLessionVideoDto);

        Task AddDoucment(Guid lessionId, CreateLessionFileDocumentDto createLessionFileDocumentDto);

        Task UpdateImage(Guid imageId, UpdateLessionVideoDto updateLessionVideoDto);

        Task<LessionVideoDto> GetImageById(Guid imageId);

        Task DeleteLessionDto(Guid id);

        Task RemoveImage(Guid imageId);

        Task RemoveDocument(Guid imageId);

        Task<LessionDto> GetCode(string Code);
    }
    public class LessionService : ILessionService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;

        public LessionService(WebLearningContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;

        }

        public async Task DeleteLessionDto(Guid id)
        {
            using var transaction = _context.Database.BeginTransaction();

            var lession = await _context.Lessions.FindAsync(id);

            var imageLession = _context.LessionVideoImages.Where(x => x.LessionId.Equals(id));

            foreach (var item in imageLession)
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "imageLession", item.ImagePath);

                var fileExsit = File.Exists(imagePath);

                if (fileExsit == true)
                {
                    File.Delete(imagePath);

                }
            }

            _context.Lessions.Remove(lession);
            await _context.SaveChangesAsync();

            var reportScore = _context.ReportUsersScore.Where(x => x.LessionId.Equals(id));

            _context.ReportUsersScore.RemoveRange(reportScore);
            await _context.SaveChangesAsync();

            transaction.Commit();
        }

        public async Task<LessionDto> UserGetLessionById(Guid id, string accountName)
        {
            var lession = await _context.Lessions.Include(x => x.Courses).Include(x => x.LessionVideoImages).Include(x => x.Quizzes)

                                                 .Include(x => x.OtherFileUploads)

                                                .OrderByDescending(x => x.DateCreated).AsNoTracking()

                                                .FirstOrDefaultAsync(x => x.Id.Equals(id));



            var lessionQuery = _mapper.Map<LessionDto>(lession);



            foreach (var item in lessionQuery.QuizlessionDtos)
            {
                var reportScoreLession = _context.ReportUsersScore.Where(x => x.QuizLessionId.Equals(item.ID) && x.LessionId.Equals(item.LessionId)).AsNoTracking().AsQueryable();

                var reportLessionDto = _mapper.Map<List<ReportScoreLessionDto>>(reportScoreLession);

                if (reportLessionDto.Count > 0)
                {
                    item.ReportScoreLessionDtos = reportLessionDto;
                }

            }


            return lessionQuery;
        }

        public async Task<LessionDto> GetLessionById(Guid id)
        {
            var lession = await _context.Lessions.Include(x => x.Courses).Include(x => x.LessionVideoImages).OrderByDescending(x => x.DateCreated)

                                                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            var imageLession = _context.LessionVideoImages.Where(x => x.LessionId.Equals(lession.Id)).AsNoTracking().AsQueryable();


            var quizLession = _context.QuizLessions.Where(x => x.LessionId.Equals(lession.Id) && x.Active == true).AsNoTracking().AsQueryable();

            var quizCourse = _context.QuizCourses.Where(x => x.CourseId.Equals(lession.CourseId) && x.Active == true).AsNoTracking().AsQueryable();

            var reportScoreLession = _context.ReportUsersScore.Where(x => x.LessionId.Equals(lession.Id)).AsNoTracking().AsQueryable();

            var reportScoreCourse = _context.ReportUserScoreFinals.Where(x => x.CourseId.Equals(lession.CourseId)).AsNoTracking().AsQueryable();

            var fileDocument = _context.OtherFileUploads.Where(x => x.LessionId.Equals(lession.Id)).AsNoTracking().AsQueryable();

            var imageLessionDto = _mapper.Map<List<LessionVideoDto>>(imageLession);

            var quizLessionDto = _mapper.Map<List<QuizlessionDto>>(quizLession);

            var quizCourseDto = _mapper.Map<List<QuizCourseDto>>(quizCourse);

            var reportScoreLessionDto = _mapper.Map<List<ReportScoreLessionDto>>(reportScoreLession);

            var reportScoreCourseDto = _mapper.Map<List<ReportScoreCourseDto>>(reportScoreCourse);

            var fileDocumentDto = _mapper.Map<List<LessionFileDocumentDto>>(fileDocument);


            LessionDto lessionDto = new()
            {
                Id = lession.Id,

                Name = lession.Name,

                ShortDesc = lession.ShortDesc,

                Active = lession.Active,

                CourseId = lession.CourseId,

                DateCreated = lession.DateCreated,

                Alias = lession.Alias,

                Author = lession.Author,

                Code = lession.Code,
                CourseDto = new CourseDto()
                {
                    Id = lession.Courses.Id,

                    Name = lession.Courses.Name,

                    DateCreated = lession.Courses.DateCreated,

                    Alias = lession.Courses.Alias,

                    CreatedBy = lession.Courses.CreatedBy,
                    //RoleId = lession.Courses.RoleId,

                },
                LessionFileDocumentDtos = new List<LessionFileDocumentDto>(fileDocumentDto),
                LessionVideoDtos = new List<LessionVideoDto>(imageLessionDto),
                QuizCourseDtos = new List<QuizCourseDto>(quizCourseDto),
                QuizlessionDtos = new List<QuizlessionDto>(quizLessionDto),
                ReportScoreLessionDtos = new List<ReportScoreLessionDto>(reportScoreLessionDto),
                ReportScoreCourseDtos = new List<ReportScoreCourseDto>(reportScoreCourseDto),

            };

            return lessionDto;
        }

        public async Task<IEnumerable<LessionDto>> GetLessionDtos()
        {
            var lession = await _context.Lessions.Include(x => x.LessionVideoImages).OrderByDescending(x => x.DateCreated).AsNoTracking().ToListAsync();
            var lessionDtos = _mapper.Map<List<LessionDto>>(lession);

            return lessionDtos;
        }

        public async Task<PagedViewModel<LessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:Lession"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var pageCount = Math.Ceiling(_context.Lessions.Count() / (double)pageResult);

            var query = _context.Lessions.Include(x => x.Courses).Include(x => x.LessionVideoImages).OrderByDescending(x => x.DateCreated).AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.Name.Contains(getListPagingRequest.Keyword) || x.Code.Contains(getListPagingRequest.Keyword));

                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }

            var imageCourse = _context.LessionVideoImages.ToList();

            var image = _mapper.Map<List<LessionVideoDto>>(imageCourse);

            var totalRow = await query.CountAsync();

            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new LessionDto()
                                    {
                                        Id = x.Id,

                                        Name = x.Name,

                                        ShortDesc = x.ShortDesc,

                                        Active = x.Active,

                                        CourseId = x.CourseId,

                                        DateCreated = x.DateCreated,

                                        Alias = x.Alias,

                                        //Author = x.Courses.Role.RoleName,

                                        CourseDto = new CourseDto()
                                        {
                                            Id = x.Courses.Id,

                                            Name = x.Courses.Name,

                                            DateCreated = x.Courses.DateCreated,


                                        },
                                        Code = x.Code,
                                        LessionVideoDtos = new List<LessionVideoDto>(image)

                                    })
                                    .OrderByDescending(x => x.DateCreated).ToListAsync();
            var roleResponse = new PagedViewModel<LessionDto>
            {
                Items = data,

                PageIndex = getListPagingRequest.PageIndex,

                PageSize = getListPagingRequest.PageSize,

                TotalRecord = (int)pageCount,
            };

            return roleResponse;
        }

        public async Task<LessionVideoDto> GetImageById(Guid imageId)
        {
            var image = await _context.LessionVideoImages.FindAsync(imageId);
            if (image == null)
                throw new Exception($"Cannot find an image with id {imageId}");

            var viewModel = new LessionVideoDto()
            {
                Caption = image.Caption,
                DateCreated = image.DateCreated,
                FileSize = image.FileSize,
                Id = image.Id,
                ImagePath = image.ImagePath,
                LessionId = image.LessionId,
                SortOrder = image.SortOrder,
                Code = image.Code,
                LinkVideo = image.LinkVideo,
                Alias = image.Alias,
            };
            return viewModel;
        }

        public async Task RemoveImage(Guid imageId)
        {
            var lessionVideo = await _context.LessionVideoImages.FindAsync(imageId);
            if (lessionVideo == null)
                throw new Exception($"Cannot find an image with id {imageId}");

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "imageLession", lessionVideo.ImagePath);

            var fileExsit = File.Exists(imagePath);

            if (fileExsit == true)
            {
                File.Delete(imagePath);

            }
            _context.LessionVideoImages.Remove(lessionVideo);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveDocument(Guid imageId)
        {
            var lessionDocument = await _context.OtherFileUploads.FindAsync(imageId);
            if (lessionDocument == null)
                throw new Exception($"Cannot find an image with id {imageId}");

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "document", lessionDocument.ImagePath);

            var fileExsit = File.Exists(imagePath);

            if (fileExsit == true)
            {
                File.Delete(imagePath);

            }
            _context.OtherFileUploads.Remove(lessionDocument);
            await _context.SaveChangesAsync();
        }
        public async Task CreateLessionAdminView([FromForm] CreateLessionAdminView createLessionAdminView)
        {
            var transaction = _context.Database.BeginTransaction();

            var code = _configuration.GetValue<string>("Code:Lession");


            var key = code + Utilities.GenerateStringDateTime();
            createLessionAdminView.CreateLessionDto.Code = key;

            createLessionAdminView.CreateLessionDto.Id = Guid.NewGuid();

            createLessionAdminView.CreateLessionDto.DateCreated = DateTime.Now;

            createLessionAdminView.CreateLessionDto.Alias = Utilities.SEOUrl(createLessionAdminView.CreateLessionDto.Name);

            Lession lession = _mapper.Map<Lession>(createLessionAdminView.CreateLessionDto);

            if (_context.Lessions.Any(x => x.Name.Equals(createLessionAdminView.CreateLessionDto.Name) && x.CourseId.Equals(createLessionAdminView.CreateLessionDto.CourseId)) == false)
            {
                _context.Add(lession);

                await _context.SaveChangesAsync();
            }


            if (createLessionAdminView.CreateLessionVideoDto != null)
            {
                //byte[] data ;
                //using (var br = new BinaryReader(createLessionDto.Image.OpenReadStream()))
                //{
                //    using var memoryStream = new MemoryStream();

                //    br.BaseStream.CopyTo(memoryStream);

                //    data = memoryStream.ToArray();
                //}



                var count = _context.LessionVideoImages.Where(x => x.LessionId.Equals(createLessionAdminView.CreateLessionDto.Id)).AsQueryable().Count();

                var codeV = _configuration.GetValue<string>("Code:VideoLession");

                var keyV = codeV + Utilities.GenerateStringDateTime();

                createLessionAdminView.CreateLessionVideoDto.Code = keyV;

                if (createLessionAdminView.CreateLessionVideoDto.ImageFile != null)
                {

                    string extension = Path.GetExtension(createLessionAdminView.CreateLessionVideoDto.ImageFile.FileName);

                    string image = Utilities.SEOUrl(createLessionAdminView.CreateLessionDto.Name) + extension;

                    createLessionAdminView.CreateLessionVideoDto.Alias = Utilities.SEOUrl(createLessionAdminView.CreateLessionVideoDto.Caption);

                    var productImage = new LessionVideoImage()
                    {
                        Id = Guid.NewGuid(),

                        Caption = createLessionAdminView.CreateLessionVideoDto.Caption,

                        DateCreated = DateTime.Now,

                        SortOrder = count,

                        LessionId = createLessionAdminView.CreateLessionDto.Id,

                        Alias = createLessionAdminView.CreateLessionVideoDto.Alias,

                        DescNotify = createLessionAdminView.CreateLessionVideoDto.DescNotify,

                        Notify = createLessionAdminView.CreateLessionVideoDto.Notify,

                        ImagePath = await Utilities.UploadFile(createLessionAdminView.CreateLessionVideoDto.ImageFile, "imageLession", image),

                        FileSize = createLessionAdminView.CreateLessionVideoDto.ImageFile.Length,

                        Code = createLessionAdminView.CreateLessionVideoDto.Code
                    };

                    _context.LessionVideoImages.Add(productImage);

                    await _context.SaveChangesAsync();
                }
                else
                {


                    createLessionAdminView.CreateLessionVideoDto.ImagePath = "upload-link";

                    var productImage = new LessionVideoImage()
                    {
                        Id = Guid.NewGuid(),

                        Caption = createLessionAdminView.CreateLessionVideoDto.Caption,

                        DateCreated = DateTime.Now,

                        SortOrder = count,

                        LessionId = createLessionAdminView.CreateLessionDto.Id,

                        Alias = createLessionAdminView.CreateLessionVideoDto.Alias,

                        DescNotify = createLessionAdminView.CreateLessionVideoDto.DescNotify,

                        Notify = createLessionAdminView.CreateLessionVideoDto.Notify,

                        ImagePath = createLessionAdminView.CreateLessionVideoDto.ImagePath,

                        FileSize = createLessionAdminView.CreateLessionVideoDto.FileSize,

                        LinkVideo = createLessionAdminView.CreateLessionVideoDto.LinkVideo,
                        Code = createLessionAdminView.CreateLessionVideoDto.Code,

                    };
                    if (_context.LessionVideoImages.Any(x => x.LinkVideo.Equals(createLessionAdminView.CreateLessionVideoDto.LinkVideo) && x.LessionId.Equals(createLessionAdminView.CreateLessionVideoDto.LessionId)) == false)
                    {
                        _context.LessionVideoImages.Add(productImage);

                        await _context.SaveChangesAsync();
                    }

                }




            }
            if (createLessionAdminView.CreateLessionFileDocumentDto != null)
            {
                string extension = Path.GetExtension(createLessionAdminView.CreateLessionFileDocumentDto.FileDocument.FileName);

                string document = Utilities.SEOUrl(createLessionAdminView.CreateLessionFileDocumentDto.Caption) + extension;

                var count = _context.OtherFileUploads.Where(x => x.LessionId.Equals(createLessionAdminView.CreateLessionDto.Id)).AsQueryable().Count();

                if (count != 0)
                {
                    count++;
                }
                var productImage = new OtherFileUpload()
                {
                    Id = Guid.NewGuid(),

                    Caption = createLessionAdminView.CreateLessionFileDocumentDto.Caption,

                    DateCreated = DateTime.Now,

                    SortOrder = count,

                    LessionId = createLessionAdminView.CreateLessionDto.Id,

                    ImagePath = await Utilities.UploadFileDocument(createLessionAdminView.CreateLessionFileDocumentDto.FileDocument, "document", document),

                    FileSize = createLessionAdminView.CreateLessionFileDocumentDto.FileDocument.Length


                };

                _context.OtherFileUploads.Add(productImage);

                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
        }

        //public async Task UpdateLessionAdminView(Guid id, UpdateLessionAdminView updateLessionAdminView)
        //{
        //    var lession = await _context.Lessions.FirstOrDefaultAsync(x => x.Id.Equals(id));


        //    updateLessionAdminView.UpdateLessionDto.DateCreated = DateTime.Now;

        //    updateLessionAdminView.UpdateLessionDto.Alias = Utilities.SEOUrl(updateLessionAdminView.UpdateLessionDto.Name);

        //    updateLessionAdminView.UpdateLessionDto.Notify = true;

        //    Lession lessionUpdate = (_mapper.Map(updateLessionAdminView.UpdateLessionDto, lession));

        //    _context.Lessions.Update(lessionUpdate);

        //    await _context.SaveChangesAsync();

        //    if(updateLessionAdminView.CreateLessionVideoDto.ImageFile != null) 
        //    {
        //        string extension = Path.GetExtension(updateLessionAdminView.CreateLessionVideoDto.ImageFile.FileName);

        //        string image = Utilities.SEOUrl(updateLessionAdminView.UpdateLessionDto.Name) + extension;

        //        updateLessionAdminView.CreateLessionVideoDto.Alias = Utilities.SEOUrl(updateLessionAdminView.CreateLessionVideoDto.Caption);

        //        var count = _context.LessionVideoImages.Where(x => x.LessionId.Equals(id)).AsQueryable().Count();

        //        var productImage = new LessionVideoImage()
        //        {
        //            Id = Guid.NewGuid(),

        //            Caption = updateLessionAdminView.CreateLessionVideoDto.Caption,

        //            DateCreated = DateTime.Now,

        //            SortOrder = count,

        //            IsDefault = updateLessionAdminView.CreateLessionVideoDto.IsDefault,

        //            LessionId = id,

        //            Alias = updateLessionAdminView.CreateLessionVideoDto.Alias,

        //            DescNotify = updateLessionAdminView.CreateLessionVideoDto.DescNotify,

        //            Notify = updateLessionAdminView.CreateLessionVideoDto.Notify,

        //            ImagePath = await Utilities.UploadFile(updateLessionAdminView.CreateLessionVideoDto.ImageFile, "imageLession", image),

        //            FileSize = updateLessionAdminView.CreateLessionVideoDto.ImageFile.Length

        //        };
        //        _context.LessionVideoImages.Add(productImage);

        //        await _context.SaveChangesAsync();
        //    }

        //    if(updateLessionAdminView.CreateLessionFileDocumentDto.FileDocument != null)
        //    {
        //        string extension = Path.GetExtension(updateLessionAdminView.CreateLessionFileDocumentDto.FileDocument.FileName);

        //        string document = Utilities.SEOUrl(updateLessionAdminView.CreateLessionFileDocumentDto.Caption) + extension;

        //        var count = _context.OtherFileUploads.Where(x => x.LessionId.Equals(id)).AsQueryable().Count();

        //        if (count != 0)
        //        {
        //            count++;
        //        }
        //        var productImage = new OtherFileUpload()
        //        {
        //            Id = Guid.NewGuid(),

        //            Caption = updateLessionAdminView.CreateLessionFileDocumentDto.Caption,

        //            DateCreated = DateTime.Now,

        //            SortOrder = count,

        //            IsDefault = updateLessionAdminView.CreateLessionFileDocumentDto.IsDefault,

        //            LessionId = id,

        //            ImagePath = await Utilities.UploadFileDocument(updateLessionAdminView.CreateLessionFileDocumentDto.FileDocument, "document", document),

        //            FileSize = updateLessionAdminView.CreateLessionFileDocumentDto.FileDocument.Length
        //        };

        //        _context.OtherFileUploads.Add(productImage);

        //        await _context.SaveChangesAsync();
        //    }
        //}

        public async Task AddImage(Guid lessionId, [FromForm] CreateLessionVideoDto createLessionVideoDto)
        {
            var count = _context.LessionVideoImages.Where(x => x.LessionId.Equals(lessionId) && createLessionVideoDto.LessionId.Equals(lessionId)).AsQueryable().Count();

            var codeVideo = _configuration.GetValue<string>("Code:VideoLession");

            var keyv = codeVideo + Utilities.GenerateStringDateTime();

            createLessionVideoDto.Code = keyv;

            var productImage = new LessionVideoImage()
            {
                Id = Guid.NewGuid(),
                Caption = createLessionVideoDto.Caption,

                DateCreated = DateTime.Now,

                SortOrder = count,

                LessionId = lessionId,

                Alias = createLessionVideoDto.Alias,

                Code = createLessionVideoDto.Code,
            };

            if (createLessionVideoDto.ImageFile != null)
            {
                string extension = Path.GetExtension(createLessionVideoDto.ImageFile.FileName);

                string image = Utilities.SEOUrl(createLessionVideoDto.Caption) + extension;

                createLessionVideoDto.Alias = Utilities.SEOUrl(createLessionVideoDto.Caption);

                productImage.ImagePath = await Utilities.UploadFile(createLessionVideoDto.ImageFile, "imageLession", image);

                productImage.FileSize = createLessionVideoDto.ImageFile.Length;

                productImage.DescNotify = createLessionVideoDto.DescNotify;
                productImage.Notify = createLessionVideoDto.Notify;
            }
            else
            {

                productImage.LinkVideo = createLessionVideoDto.LinkVideo;

                productImage.FileSize = 0;

                productImage.ImagePath = "upload-link";

                productImage.DescNotify = createLessionVideoDto.DescNotify;

                productImage.Notify = createLessionVideoDto.Notify;
            }
            if (_context.LessionVideoImages.Any(x => x.LessionId.Equals(createLessionVideoDto.LessionId) && x.Caption.Equals(createLessionVideoDto.Caption)) == false)
            {
                _context.LessionVideoImages.Add(productImage);

                await _context.SaveChangesAsync();
            }



        }
        public async Task UpdateImage(Guid imageId, [FromForm] UpdateLessionVideoDto updateLessionVideoDto)
        {
            using var transaction = _context.Database.BeginTransaction();



            updateLessionVideoDto.Alias = Utilities.SEOUrl(updateLessionVideoDto.Caption);

            var lessionVideo = await _context.LessionVideoImages.FindAsync(imageId);

            if (lessionVideo == null)
                throw new Exception();

            if (updateLessionVideoDto.ImageFile != null)
            {
                if (lessionVideo.LinkVideo != null)
                {
                    updateLessionVideoDto.LinkVideo = null;
                    lessionVideo.LinkVideo = updateLessionVideoDto.LinkVideo;
                }
                string extension = Path.GetExtension(updateLessionVideoDto.ImageFile.FileName);

                string image = Utilities.SEOUrl(updateLessionVideoDto.Caption) + extension;

                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "imageLession", image);

                var fileExsit = File.Exists(imagePath);

                if (fileExsit == true)
                {
                    File.Delete(imagePath);

                }
                lessionVideo.ImagePath = await Utilities.UploadFile(updateLessionVideoDto.ImageFile, "imageLession", image);

                lessionVideo.FileSize = updateLessionVideoDto.ImageFile.Length;
            }
            else
            {
                lessionVideo.LinkVideo = updateLessionVideoDto.LinkVideo;

                lessionVideo.FileSize = 0;

                lessionVideo.ImagePath = "upload-link";

                lessionVideo.DescNotify = lessionVideo.DescNotify;

                lessionVideo.Notify = lessionVideo.Notify;
            }
            updateLessionVideoDto.DateCreated = DateTime.Now;

            lessionVideo.DateCreated = updateLessionVideoDto.DateCreated;

            lessionVideo.SortOrder = updateLessionVideoDto.SortOrder;

            lessionVideo.Caption = updateLessionVideoDto.Caption;

            lessionVideo.Alias = updateLessionVideoDto.Alias;

            lessionVideo.Notify = updateLessionVideoDto.Notify;

            lessionVideo.DescNotify = updateLessionVideoDto.DescNotify;



            _context.LessionVideoImages.Update(lessionVideo);
            await _context.SaveChangesAsync();

            var notificationResponeDb = _context.NotificationResponses.Where(x => x.TargetNotificationId.Equals(imageId));

            foreach (var item in notificationResponeDb)
            {
                if (item.Notify == false)
                {
                    UpdateNotificationResponseDto updateNotificationResponseDto = new();

                    updateNotificationResponseDto.Notify = true;

                    updateNotificationResponseDto.DateCreated = DateTime.Now;


                    NotificationResponse notificationResponseDto = _mapper.Map(updateNotificationResponseDto, item);

                    _context.NotificationResponses.Update(notificationResponseDto);

                    await _context.SaveChangesAsync();
                }
            }

            await transaction.CommitAsync();
        }

        public async Task AddDoucment(Guid lessionId, [FromForm] CreateLessionFileDocumentDto createLessionFileDocumentDto)
        {
            string extension = Path.GetExtension(createLessionFileDocumentDto.FileDocument.FileName);

            string document = Utilities.SEOUrl(createLessionFileDocumentDto.Caption) + extension;

            var count = _context.OtherFileUploads.Where(x => x.LessionId.Equals(lessionId) && createLessionFileDocumentDto.LessionId.Equals(lessionId)).AsQueryable().Count();

            var productImage = new OtherFileUpload()
            {
                Id = Guid.NewGuid(),

                Caption = createLessionFileDocumentDto.Caption,

                DateCreated = DateTime.Now,

                SortOrder = count,

                LessionId = lessionId,
            };

            if (createLessionFileDocumentDto.FileDocument != null)
            {
                productImage.ImagePath = await Utilities.UploadFileDocument(createLessionFileDocumentDto.FileDocument, "document", document);

                productImage.FileSize = createLessionFileDocumentDto.FileDocument.Length;
            }
            _context.OtherFileUploads.Add(productImage);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateLessionDto(Guid id, [FromForm] UpdateLessionDto updateLessionDto)
        {
            using var transaction = _context.Database.BeginTransaction();
            var lession = await _context.Lessions.FirstOrDefaultAsync(x => x.Id.Equals(id));

            updateLessionDto.DateCreated = DateTime.Now;

            updateLessionDto.Alias = Utilities.SEOUrl(updateLessionDto.Name);

            Lession lessionUpdate = _mapper.Map(updateLessionDto, lession);

            _context.Lessions.Update(lessionUpdate);

            await _context.SaveChangesAsync();

            var notificationResponeDb = _context.NotificationResponses.Where(x => x.TargetNotificationId.Equals(id));

            foreach (var item in notificationResponeDb)
            {
                if (item.Notify == false)
                {
                    UpdateNotificationResponseDto updateNotificationResponseDto = new();

                    updateNotificationResponseDto.Notify = true;

                    updateNotificationResponseDto.DateCreated = DateTime.Now;


                    NotificationResponse notificationResponseDto = _mapper.Map(updateNotificationResponseDto, item);

                    _context.NotificationResponses.Update(notificationResponseDto);

                    await _context.SaveChangesAsync();
                }
            }

            await transaction.CommitAsync();
        }

        public async Task<LessionDto> GetName(Guid id)
        {
            var name = await _context.Lessions.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

            var lessionDto = _mapper.Map<LessionDto>(name);

            return lessionDto;
        }

        public async Task<LessionDto> GetCode(string Code)
        {
            var account = await _context.Lessions.FirstOrDefaultAsync(x => x.Code.Equals(Code));

            return _mapper.Map<LessionDto>(account);
        }

        public async Task<LessionDto> CheckExist(Guid id, string name)
        {
            var account = await _context.Lessions.FirstOrDefaultAsync(x => x.CourseId.Equals(id) && x.Name.Equals(name));

            return _mapper.Map<LessionDto>(account);
        }
    }
}
