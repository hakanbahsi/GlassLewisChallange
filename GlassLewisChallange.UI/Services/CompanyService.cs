using GlassLewisChallange.Infrastructure.Models;
using GlassLewisChallange.UI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace GlassLewisChallange.UI.Services
{
    public class CompanyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public CompanyService(HttpClient httpClient, IOptions<ApiSettings> options)
        {
            _httpClient = httpClient;
            _baseUrl = options.Value.BaseUrl;
        }

        public async Task<bool> CreateCompanyAsync(CreateCompanyModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/company", content);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {response.StatusCode} - {error}");
            }
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CompanyListItemModel>> GetAllCompaniesAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/company");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {response.StatusCode} - {error}");
            }

            var json = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<CompanyListItemModel>>>(json);

            return apiResponse?.Data ?? new List<CompanyListItemModel>();
        }

        public async Task<CompanyDetailModel?> GetCompanyByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/company/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {response.StatusCode} - {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<CompanyDetailModel>>(json);

            return apiResponse?.Data;
        }

        public async Task<bool> UpdateCompanyAsync(CompanyDetailModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}/company", content);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {response.StatusCode} - {error}");
            }
            return response.IsSuccessStatusCode;
        }
    }
}
