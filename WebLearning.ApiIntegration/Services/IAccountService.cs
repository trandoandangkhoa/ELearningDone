using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.Account;
using WebLearning.Contract.Dtos.Avatar;

namespace WebLearning.ApiIntegration.Services
{

    public interface IAccountService
    {
        public Task<PagedViewModel<AccountDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest);
        public Task<AccountDto> GetAccountById(Guid id);
        public Task<UserAllInformationDto> GetAccountByEmail(string accountName);

        public Task<bool> InsertAccount(CreateAccountDto createAccountDto);
        public Task<bool> DeleteAccount(Guid id);
        public Task<bool> UpdateAccount(UpdateAccountDto updateAccountDto, Guid Id);
        public Task<IEnumerable<AccountDto>> GetAccount();

        Task<string> GetAvatrImagePath(string accountName);

        Task<AccountDto> GetFullName(string accountName);
        Task<bool> AddImage(Guid accountId, CreateAvatarDto createAvatarDto);

        Task<bool> UpdateImage(Guid accountId, UpdateAvatarDto updateAvatarDto);
    }
    public class AccountService : IAccountService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountService(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<PagedViewModel<AccountDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {


            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/accountpaging?PageIndex=" +
    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<PagedViewModel<AccountDto>>(body);

            return users;
            //var a = getListPagingRequest.Keyword;

            //if (!string.IsNullOrEmpty(a))
            //{
            //    _httpContextAccessor.HttpContext.Session.SetString("Keyword", a);
            //}
            //var ksession = _httpContextAccessor.HttpContext.Session.GetString("Keyword");

            //if (string.IsNullOrEmpty(ksession))
            //{
            //    if (string.IsNullOrEmpty(a))
            //    {

            //        var response = await client.GetAsync($"/accountpaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var users = JsonConvert.DeserializeObject<PagedViewModel<AccountDto>>(body);

            //        return users;
            //    }
            //    else if (!string.IsNullOrEmpty(a))
            //    {

            //        var response = await client.GetAsync($"/accountpaging?PageIndex=" +
            //            $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={getListPagingRequest.Keyword}");

            //        var body = await response.Content.ReadAsStringAsync();

            //        var users = JsonConvert.DeserializeObject<PagedViewModel<AccountDto>>(body);

            //        return users;
            //    }
            //}
            //else
            //{
            //    var response = await client.GetAsync($"/accountpaging?PageIndex=" +
            //    $"{getListPagingRequest.PageIndex}&PageSize={getListPagingRequest.PageSize}&Keyword={ksession}");

            //    var body = await response.Content.ReadAsStringAsync();

            //    var users = JsonConvert.DeserializeObject<PagedViewModel<AccountDto>>(body);

            //    return users;
            //}

            //return default;

        }
        public async Task<bool> InsertAccount(CreateAccountDto createAccountDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            createAccountDto.PasswordHased = Password.HashedPassword(createAccountDto.Password);

            createAccountDto.DateCreated = DateTime.UtcNow;

            createAccountDto.LastLogin = DateTime.UtcNow;

            var json = JsonConvert.SerializeObject(createAccountDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/Accounts/", httpContent);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteAccount(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.DeleteAsync($"/api/Accounts/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<AccountDto> GetAccountById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Accounts/{id}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<AccountDto>(body);

            return user;
        }
        public async Task<bool> UpdateAccount(UpdateAccountDto updateAccountDto, Guid id)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            updateAccountDto.PasswordHased = Password.HashedPassword(updateAccountDto.Password);

            updateAccountDto.DateCreated = DateTime.UtcNow;

            updateAccountDto.LastLogin = DateTime.UtcNow;

            var json = JsonConvert.SerializeObject(updateAccountDto);

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/Accounts/{id}", httpContent);

            return response.IsSuccessStatusCode;

        }

        public async Task<IEnumerable<AccountDto>> GetAccount()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);



            var response = await client.GetAsync("api/Accounts");

            var body = await response.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<IEnumerable<AccountDto>>(body);

            return users;
        }

        public async Task<UserAllInformationDto> GetAccountByEmail(string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Accounts/AccountDetail/{accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<UserAllInformationDto>(body);

            return user;
        }

        public async Task<bool> AddImage(Guid accountId, CreateAvatarDto createAvatarDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            createAvatarDto.Id = Guid.NewGuid();

            createAvatarDto.DateCreated = DateTime.Now;

            createAvatarDto.AccountId = accountId;

            var requestContent = new MultipartFormDataContent();

            if (createAvatarDto.Image != null)
            {
                string extension = Path.GetExtension(createAvatarDto.Image.FileName);

                string image = Utilities.SEOUrl(createAvatarDto.Name) + extension;

                createAvatarDto.ImagePath = await Utilities.UploadAvatar(createAvatarDto.Image, "avatarImage", image);

                createAvatarDto.FileSize = createAvatarDto.Image.Length;


                byte[] data;
                using (var br = new BinaryReader(createAvatarDto.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)createAvatarDto.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "Image", createAvatarDto.Image.FileName);

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createAvatarDto.ImagePath.ToString()) ? "" : createAvatarDto.ImagePath.ToString()), "imagePath");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(createAvatarDto.FileSize.ToString()) ? "" : createAvatarDto.FileSize.ToString()), "fileSize");

            }
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createAvatarDto.AccountId.ToString()) ? "" : createAvatarDto.AccountId.ToString()), "accountId");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createAvatarDto.Name) ? "" : createAvatarDto.Name.ToString()), "caption");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(createAvatarDto.DateCreated.ToString()) ? "" : createAvatarDto.DateCreated.ToString()), "dateCreated");


            var response = await client.PostAsync($"/api/Accounts/{accountId}/Avatar", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateImage(Guid accountId, UpdateAvatarDto updateAvatarDto)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            updateAvatarDto.DateCreated = DateTime.Now;

            var requestContent = new MultipartFormDataContent();

            if (updateAvatarDto.Image != null)
            {
                //string extension = Path.GetExtension(updateAvatarDto.Image.FileName);

                //string image = Utilities.SEOUrl(updateAvatarDto.Name) + extension;

                //updateAvatarDto.ImagePath = await Utilities.UploadFile(updateAvatarDto.Image, "avatarImage", image);

                updateAvatarDto.FileSize = updateAvatarDto.Image.Length;


                byte[] data;
                using (var br = new BinaryReader(updateAvatarDto.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)updateAvatarDto.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);

                requestContent.Add(bytes, "Image", updateAvatarDto.Image.FileName);

                //requestContent.Add(new StringContent(string.IsNullOrEmpty(updateAvatarDto.ImagePath.ToString()) ? "" : updateAvatarDto.ImagePath.ToString()), "imagePath");

                requestContent.Add(new StringContent(string.IsNullOrEmpty(updateAvatarDto.FileSize.ToString()) ? "" : updateAvatarDto.FileSize.ToString()), "fileSize");


            }

            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateAvatarDto.Name) ? "" : updateAvatarDto.Name.ToString()), "caption");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(updateAvatarDto.DateCreated.ToString()) ? "" : updateAvatarDto.DateCreated.ToString()), "dateCreated");


            var response = await client.PutAsync($"/api/Accounts/{accountId}/Avatar", requestContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<string> GetAvatrImagePath(string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Accounts/AvatarImagePath?accountName={accountName}");

            var path = await response.Content.ReadAsStringAsync();

            return path;
        }
        public async Task<AccountDto> GetFullName(string accountName)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var response = await client.GetAsync($"/api/Accounts/GetFullName/{accountName}");

            var body = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<AccountDto>(body);

            return user;
        }
    }
}
