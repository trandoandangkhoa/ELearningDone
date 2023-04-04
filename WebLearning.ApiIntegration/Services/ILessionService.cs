using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Lession;
using WebLearning.Contract.Dtos.Lession.LessionAdminView;
using WebLearning.Contract.Dtos.LessionFileDocument;
using WebLearning.Contract.Dtos.VideoLession;

namespace WebLearning.ApiIntegration.Services
{
    public interface ILessionService
    {
        Task<PagedViewModel<LessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);

        Task<IEnumerable<LessionDto>> GetAllLession();

        Task<LessionDto> GetLessionById(Guid id);
        Task<LessionDto> GetNameLession(Guid id);
        Task<LessionDto> UserGetLessionById(Guid id, string accountName);


        Task<bool> InsertLession(CreateLessionAdminView createLessionAdminView);

        Task<bool> UpdateLession(UpdateLessionDto updateLessionDto, Guid id);

        Task<bool> DeleteLession(Guid id);

        Task<bool> AddImage(Guid lessionId, CreateLessionVideoDto createLessionVideoDto);

        Task<bool> AddDocument(Guid lessionId, CreateLessionFileDocumentDto createLessionFileDocumentDto);

        Task<bool> RemoveImage(Guid imageId);
        Task<bool> RemoveDocument(Guid imageId);

        Task<bool> UpdateImage(Guid imageId, UpdateLessionVideoDto updateLessionVideoDto);

        Task<LessionVideoDto> GetLessionVideoById(Guid id);


    }
    public class LessionService : ILessionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LessionService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<LessionDto> UserGetLessionById(Guid id, string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Lessions/UserGetLessionById/{id}?accountName={accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<LessionDto>(body);

            return user;
        }

        public async Task<bool> DeleteLession(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/Lessions/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<LessionDto>> GetAllLession()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Lessions/");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<IEnumerable<LessionDto>>(body);

            return user;
        }

        public async Task<LessionDto> GetLessionById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Lessions/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<LessionDto>(body);

            return user;
        }

        public async Task<LessionVideoDto> GetLessionVideoById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Lessions/VideoLessions/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<LessionVideoDto>(body);

            return user;
        }


        public async Task<PagedViewModel<LessionDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var a = getListPagingRequest.Keyword;

            var response = await client.GetAsync($"/api/Lessions/Lessionpaging?PageIndex=" +
        $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<PagedViewModel<LessionDto>>(body);

            return users;

            //if (a != null)
            //{
            //    _httpContextAccessor.HttpContext.Session.SetString("Keyword", a);
            //}
            //var ksession = _httpContextAccessor.HttpContext.Session.GetString("Keyword");

            //if (string.IsNullOrEmpty(ksession))
            //{
            //    if (a == null)
            //    {

            //        var response = await client.GetAsync($"/api/Lessions/Lessionpaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var users = JsonConvert.DeserializeObject<PagedViewModel<LessionDto>>(body);

            //        return users;
            //    }
            //    else if (a != null)
            //    {

            //        var response = await client.GetAsync($"/api/Lessions/Lessionpaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var users = JsonConvert.DeserializeObject<PagedViewModel<LessionDto>>(body);

            //        return users;
            //    }
            //}
            //else
            //{
            //    var response = await client.GetAsync($"/api/Lessions/Lessionpaging?PageIndex=" +
            //    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={ksession}");

            //    var body = await response.Content.ReadAsStringAsync();

            //    var users = JsonConvert.DeserializeObject<PagedViewModel<LessionDto>>(body);

            //    return users;
            //}

            //return default;
        }

        public async Task<bool> InsertLession(CreateLessionAdminView createLessionAdminView)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            createLessionAdminView.CreateLessionDto.Id = Guid.NewGuid();

            createLessionAdminView.CreateLessionDto.Alias = Utilities.SEOUrl(createLessionAdminView.CreateLessionDto.Name);

            createLessionAdminView.CreateLessionDto.DescNotify = "Bạn có một chương mới";

            var requestContent = new MultipartFormDataContent();

            if (createLessionAdminView.CreateLessionVideoDto.ImageFile != null)
            {
                createLessionAdminView.CreateLessionVideoDto.LessionId = createLessionAdminView.CreateLessionDto.Id;

                string extension = Path.GetExtension(createLessionAdminView.CreateLessionVideoDto.ImageFile.FileName);

                createLessionAdminView.CreateLessionVideoDto.DescNotify = "Bạn có một bài học mới";

                //string image = Utilities.SEOUrl(createLessionAdminView.CreateLessionDto.Name) + extension;

                //createLessionAdminView.CreateLessionVideoDto.ImagePath = await Utilities.UploadFile(createLessionAdminView.CreateLessionVideoDto.ImageFile, "imageLession", image);

                createLessionAdminView.CreateLessionVideoDto.Alias = Utilities.SEOUrl(createLessionAdminView.CreateLessionVideoDto.Caption);


                byte[] data;
                using (var br = new BinaryReader(createLessionAdminView.CreateLessionVideoDto.ImageFile.OpenReadStream()))
                {
                    data = br.ReadBytes((int)createLessionAdminView.CreateLessionVideoDto.ImageFile.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "createLessionAdminView.CreateLessionVideoDto.ImageFile", createLessionAdminView.CreateLessionVideoDto.ImageFile.FileName);

                //requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.ImagePath.ToString()) ? "" : createLessionAdminView.CreateLessionVideoDto.ImagePath.ToString()), "createLessionAdminView.CreateLessionVideoDto.imagePath");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.FileSize.ToString()) ? "" : createLessionAdminView.CreateLessionVideoDto.FileSize.ToString()), "createLessionAdminView.CreateLessionVideoDto.fileSize");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.Notify.ToString()) ? "" : createLessionAdminView.CreateLessionVideoDto.Notify.ToString()), "createLessionAdminView.CreateLessionVideoDto.notify");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.DescNotify.ToString()) ? "" : createLessionAdminView.CreateLessionVideoDto.DescNotify.ToString()), "createLessionAdminView.CreateLessionVideoDto.descNotify");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.LessionId.ToString()) ? "" : createLessionAdminView.CreateLessionVideoDto.LessionId.ToString()), "createLessionAdminView.CreateLessionVideoDto.lessionId");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.Caption) ? "" : createLessionAdminView.CreateLessionVideoDto.Caption.ToString()), "createLessionAdminView.CreateLessionVideoDto.caption");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.DateCreated.ToString()) ? "" : createLessionAdminView.CreateLessionVideoDto.DateCreated.ToString()), "createLessionAdminView.CreateLessionVideoDto.dateCreated");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.SortOrder.ToString()) ? "" : createLessionAdminView.CreateLessionVideoDto.SortOrder.ToString()), "createLessionAdminView.CreateLessionVideoDto.sortOrder");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.Alias.ToString()) ? "" : createLessionAdminView.CreateLessionVideoDto.Alias.ToString()), "createLessionAdminView.CreateLessionVideoDto.alias");
            }
            if (createLessionAdminView.CreateLessionVideoDto.LinkVideo != null)
            {
                createLessionAdminView.CreateLessionVideoDto.LessionId = createLessionAdminView.CreateLessionDto.Id;

                createLessionAdminView.CreateLessionVideoDto.FileSize = 0;

                createLessionAdminView.CreateLessionVideoDto.DescNotify = "Bạn có một bài học mới";

                createLessionAdminView.CreateLessionVideoDto.Alias = Utilities.SEOUrl(createLessionAdminView.CreateLessionVideoDto.Caption);


                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.FileSize.ToString()) ? "" : createLessionAdminView.CreateLessionVideoDto.FileSize.ToString()), "createLessionAdminView.CreateLessionVideoDto.fileSize");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.Notify.ToString()) ? "" : createLessionAdminView.CreateLessionVideoDto.Notify.ToString()), "createLessionAdminView.CreateLessionVideoDto.notify");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.DescNotify.ToString()) ? "" : createLessionAdminView.CreateLessionVideoDto.DescNotify.ToString()), "createLessionAdminView.CreateLessionVideoDto.descNotify");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.LessionId.ToString()) ? "" : createLessionAdminView.CreateLessionVideoDto.LessionId.ToString()), "createLessionAdminView.CreateLessionVideoDto.lessionId");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.Caption) ? "" : createLessionAdminView.CreateLessionVideoDto.Caption.ToString()), "createLessionAdminView.CreateLessionVideoDto.caption");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.DateCreated.ToString()) ? "" : createLessionAdminView.CreateLessionVideoDto.DateCreated.ToString()), "createLessionAdminView.CreateLessionVideoDto.dateCreated");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.SortOrder.ToString()) ? "" : createLessionAdminView.CreateLessionVideoDto.SortOrder.ToString()), "createLessionAdminView.CreateLessionVideoDto.sortOrder");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.Alias.ToString()) ? "" : createLessionAdminView.CreateLessionVideoDto.Alias.ToString()), "createLessionAdminView.CreateLessionVideoDto.alias");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionVideoDto.LinkVideo.ToString()) ? "" : createLessionAdminView.CreateLessionVideoDto.LinkVideo.ToString()), "createLessionAdminView.CreateLessionVideoDto.linkVideo");

            }
            if (createLessionAdminView.CreateLessionFileDocumentDto.FileDocument != null)
            {
                createLessionAdminView.CreateLessionFileDocumentDto.LessionId = createLessionAdminView.CreateLessionDto.Id;

                byte[] data;
                using (var br = new BinaryReader(createLessionAdminView.CreateLessionFileDocumentDto.FileDocument.OpenReadStream()))
                {
                    data = br.ReadBytes((int)createLessionAdminView.CreateLessionFileDocumentDto.FileDocument.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "createLessionAdminView.CreateLessionFileDocumentDto.FileDocument", createLessionAdminView.CreateLessionFileDocumentDto.FileDocument.FileName);


                //requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionFileDocumentDto.ImagePath.ToString()) ? "" : createLessionAdminView.CreateLessionFileDocumentDto.ImagePath.ToString()), "createLessionAdminView.CreateLessionFileDocumentDto.imagePath");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionFileDocumentDto.FileSize.ToString()) ? "" : createLessionAdminView.CreateLessionFileDocumentDto.FileSize.ToString()), "createLessionAdminView.CreateLessionFileDocumentDto.fileSize");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionFileDocumentDto.LessionId.ToString()) ? "" : createLessionAdminView.CreateLessionFileDocumentDto.LessionId.ToString()), "createLessionAdminView.CreateLessionFileDocumentDto.lessionId");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionFileDocumentDto.Caption) ? "" : createLessionAdminView.CreateLessionFileDocumentDto.Caption.ToString()), "createLessionAdminView.CreateLessionFileDocumentDto.caption");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionFileDocumentDto.DateCreated.ToString()) ? "" : createLessionAdminView.CreateLessionFileDocumentDto.DateCreated.ToString()), "createLessionAdminView.CreateLessionFileDocumentDto.dateCreated");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionFileDocumentDto.SortOrder.ToString()) ? "" : createLessionAdminView.CreateLessionFileDocumentDto.SortOrder.ToString()), "createLessionAdminView.CreateLessionFileDocumentDto.sortOrder");
            }

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionDto.Name) ? "" : createLessionAdminView.CreateLessionDto.Name.ToString()), "createLessionAdminView.CreateLessionDto.name");


            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionDto.ShortDesc) ? "" : createLessionAdminView.CreateLessionDto.ShortDesc.ToString()), "createLessionAdminView.CreateLessionDto.shortDesc");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionDto.Active.ToString()) ? "" : createLessionAdminView.CreateLessionDto.Active.ToString()), "createLessionAdminView.CreateLessionDto.active");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionDto.CourseId.ToString()) ? "" : createLessionAdminView.CreateLessionDto.CourseId.ToString()), "createLessionAdminView.CreateLessionDto.courseId");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionDto.DateCreated.ToString()) ? "" : createLessionAdminView.CreateLessionDto.DateCreated.ToString()), "createLessionAdminView.CreateLessionDto.dateCreated");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionDto.Alias) ? "" : createLessionAdminView.CreateLessionDto.Alias.ToString()), "createLessionAdminView.CreateLessionDto.alias");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionDto.Author.ToString()) ? "" : createLessionAdminView.CreateLessionDto.Author.ToString()), "createLessionAdminView.CreateLessionDto.author");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionDto.Notify.ToString()) ? "" : createLessionAdminView.CreateLessionDto.Notify.ToString()), "createLessionAdminView.CreateLessionDto.notify");

            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionAdminView.CreateLessionDto.DescNotify.ToString()) ? "" : createLessionAdminView.CreateLessionDto.DescNotify.ToString()), "createLessionAdminView.CreateLessionDto.descNotify");

            var json = JsonConvert.SerializeObject(createLessionAdminView);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/Lessions/", requestContent);

            return response.IsSuccessStatusCode;
        }


        public async Task<bool> UpdateImage(Guid imageId, UpdateLessionVideoDto updateLessionVideoDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            updateLessionVideoDto.DateCreated = DateTime.Now;

            updateLessionVideoDto.DescNotify = "Bạn có một bài học mới";

            var requestContent = new MultipartFormDataContent();

            if (updateLessionVideoDto.ImageFile != null)
            {
                updateLessionVideoDto.DescNotify = "Bạn có một bài học mới";
                //string extension = Path.GetExtension(updateLessionVideoDto.ImageFile.FileName);

                //string image = Utilities.SEOUrl(updateLessionVideoDto.Caption) + updateLessionVideoDto.SortOrder + extension;

                //updateLessionVideoDto.ImagePath = await Utilities.UploadFile(updateLessionVideoDto.ImageFile, "imageLession", image.ToLower());

                updateLessionVideoDto.FileSize = updateLessionVideoDto.ImageFile.Length;

                updateLessionVideoDto.Alias = Utilities.SEOUrl(updateLessionVideoDto.Caption);

                byte[] data;
                using (var br = new BinaryReader(updateLessionVideoDto.ImageFile.OpenReadStream()))
                {
                    data = br.ReadBytes((int)updateLessionVideoDto.ImageFile.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "ImageFile", updateLessionVideoDto.ImageFile.FileName);

                //requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionVideoDto.ImagePath.ToString()) ? "" : updateLessionVideoDto.ImagePath.ToString()), "imagePath");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionVideoDto.FileSize.ToString()) ? "" : updateLessionVideoDto.FileSize.ToString()), "fileSize");


            }
            else
            {
                updateLessionVideoDto.DescNotify = "Bạn có một bài học mới";

                updateLessionVideoDto.FileSize = 0;

                updateLessionVideoDto.Alias = Utilities.SEOUrl(updateLessionVideoDto.Caption);

                requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionVideoDto.FileSize.ToString()) ? "" : updateLessionVideoDto.FileSize.ToString()), "fileSize");
                requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionVideoDto.LinkVideo.ToString()) ? "" : updateLessionVideoDto.LinkVideo.ToString()), "linkVideo");

            }
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionVideoDto.Notify.ToString()) ? "" : updateLessionVideoDto.Notify.ToString()), "notify");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionVideoDto.DescNotify.ToString()) ? "" : updateLessionVideoDto.DescNotify.ToString()), "descNotify");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionVideoDto.Caption) ? "" : updateLessionVideoDto.Caption.ToString()), "caption");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionVideoDto.DateCreated.ToString()) ? "" : updateLessionVideoDto.DateCreated.ToString()), "dateCreated");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionVideoDto.SortOrder.ToString()) ? "" : updateLessionVideoDto.SortOrder.ToString()), "sortOrder");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionVideoDto.Alias.ToString()) ? "" : updateLessionVideoDto.Alias.ToString()), "alias");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionVideoDto.Notify.ToString()) ? "" : updateLessionVideoDto.Notify.ToString()), "notify");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionVideoDto.DescNotify.ToString()) ? "" : updateLessionVideoDto.DescNotify.ToString()), "descNotify");


            var response = await client.PutAsync($"/api/Lessions/VideoLessions/{imageId}", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateLession(UpdateLessionDto updateLessionDto, Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            updateLessionDto.DateCreated = DateTime.Now;

            updateLessionDto.DescNotify = "Bạn có một chương mới";

            updateLessionDto.Alias = Utilities.SEOUrl(updateLessionDto.Name);

            var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionDto.Name) ? "" : updateLessionDto.Name.ToString()), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionDto.ShortDesc) ? "" : updateLessionDto.ShortDesc.ToString()), "shortDesc");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionDto.Active.ToString()) ? "" : updateLessionDto.Active.ToString()), "active");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionDto.DateCreated.ToString()) ? "" : updateLessionDto.DateCreated.ToString()), "dateCreated");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionDto.Alias) ? "" : updateLessionDto.Alias.ToString()), "alias");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionDto.Author.ToString()) ? "" : updateLessionDto.Author.ToString()), "author");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionDto.Notify.ToString()) ? "" : updateLessionDto.Notify.ToString()), "notify");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateLessionDto.DescNotify.ToString()) ? "" : updateLessionDto.DescNotify.ToString()), "descNotify");
            var json = JsonConvert.SerializeObject(updateLessionDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Lessions/{id}", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveImage(Guid imageId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/Lessions/VideoLessions/{imageId}");

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> RemoveDocument(Guid imageId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/Lessions/DocumentLessions/{imageId}");

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> AddImage(Guid lessionId, CreateLessionVideoDto createLessionVideoDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            createLessionVideoDto.Id = Guid.NewGuid();

            createLessionVideoDto.DateCreated = DateTime.Now;

            createLessionVideoDto.LessionId = lessionId;


            var requestContent = new MultipartFormDataContent();

            if (createLessionVideoDto.ImageFile != null)
            {
                createLessionVideoDto.DescNotify = "Bạn có một bài học mới";
                //string extension = Path.GetExtension(createLessionVideoDto.ImageFile.FileName);

                //string image = Utilities.SEOUrl(createLessionVideoDto.Caption) + extension;

                //createLessionVideoDto.ImagePath = await Utilities.UploadFile(createLessionVideoDto.ImageFile, "imageLession", image.ToLower());

                createLessionVideoDto.FileSize = createLessionVideoDto.ImageFile.Length;

                createLessionVideoDto.Alias = Utilities.SEOUrl(createLessionVideoDto.Caption);

                byte[] data;
                using (var br = new BinaryReader(createLessionVideoDto.ImageFile.OpenReadStream()))
                {
                    data = br.ReadBytes((int)createLessionVideoDto.ImageFile.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "ImageFile", createLessionVideoDto.ImageFile.FileName);

                //requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionVideoDto.ImagePath.ToString()) ? "" : createLessionVideoDto.ImagePath.ToString()), "imagePath");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionVideoDto.FileSize.ToString()) ? "" : createLessionVideoDto.FileSize.ToString()), "fileSize");

            }
            else
            {
                createLessionVideoDto.DescNotify = "Bạn có một bài học mới";

                createLessionVideoDto.FileSize = 0;

                createLessionVideoDto.Alias = Utilities.SEOUrl(createLessionVideoDto.Caption);

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionVideoDto.FileSize.ToString()) ? "" : createLessionVideoDto.FileSize.ToString()), "fileSize");
                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionVideoDto.LinkVideo.ToString()) ? "" : createLessionVideoDto.LinkVideo.ToString()), "linkVideo");

            }
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionVideoDto.LessionId.ToString()) ? "" : createLessionVideoDto.LessionId.ToString()), "lessionId");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionVideoDto.Caption) ? "" : createLessionVideoDto.Caption.ToString()), "caption");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionVideoDto.DateCreated.ToString()) ? "" : createLessionVideoDto.DateCreated.ToString()), "dateCreated");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionVideoDto.SortOrder.ToString()) ? "" : createLessionVideoDto.SortOrder.ToString()), "sortOrder");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionVideoDto.Alias.ToString()) ? "" : createLessionVideoDto.Alias.ToString()), "alias");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionVideoDto.Notify.ToString()) ? "" : createLessionVideoDto.Notify.ToString()), "notify");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionVideoDto.DescNotify.ToString()) ? "" : createLessionVideoDto.DescNotify.ToString()), "descNotify");


            var response = await client.PostAsync($"/api/Lessions/VideoLessions/{lessionId}", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddDocument(Guid lessionId, CreateLessionFileDocumentDto createLessionFileDocumentDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            createLessionFileDocumentDto.Id = Guid.NewGuid();

            createLessionFileDocumentDto.DateCreated = DateTime.Now;




            var requestContent = new MultipartFormDataContent();

            if (createLessionFileDocumentDto.FileDocument != null)
            {
                //string extension = Path.GetExtension(createLessionFileDocumentDto.FileDocument.FileName);

                //string image = Utilities.SEOUrl(createLessionFileDocumentDto.Caption) + extension;

                //createLessionFileDocumentDto.ImagePath = await Utilities.UploadFileDocument(createLessionFileDocumentDto.FileDocument, "document", image.ToLower());

                createLessionFileDocumentDto.FileSize = createLessionFileDocumentDto.FileDocument.Length;

                byte[] data;
                using (var br = new BinaryReader(createLessionFileDocumentDto.FileDocument.OpenReadStream()))
                {
                    data = br.ReadBytes((int)createLessionFileDocumentDto.FileDocument.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "FileDocument", createLessionFileDocumentDto.FileDocument.FileName);

                //requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionFileDocumentDto.ImagePath.ToString()) ? "" : createLessionFileDocumentDto.ImagePath.ToString()), "imagePath");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionFileDocumentDto.FileSize.ToString()) ? "" : createLessionFileDocumentDto.FileSize.ToString()), "fileSize");

            }
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionFileDocumentDto.LessionId.ToString()) ? "" : createLessionFileDocumentDto.LessionId.ToString()), "lessionId");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionFileDocumentDto.Caption) ? "" : createLessionFileDocumentDto.Caption.ToString()), "caption");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionFileDocumentDto.DateCreated.ToString()) ? "" : createLessionFileDocumentDto.DateCreated.ToString()), "dateCreated");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createLessionFileDocumentDto.SortOrder.ToString()) ? "" : createLessionFileDocumentDto.SortOrder.ToString()), "sortOrder");


            var response = await client.PostAsync($"/api/Lessions/FileDoucment/{lessionId}", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<LessionDto> GetNameLession(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Lessions/GetName?id={id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<LessionDto>(body);

            return user;
        }
    }
}
