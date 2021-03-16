using ApiClientMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ApiClientMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger) => _logger = logger;
        public IActionResult Privacy() => View();

        public IActionResult Index()
        {
            return View();
        }

        // using NewtonsoftJson:  too many lines of code
        private static async Task<Product> SteamNewtonsoftJson(string uri, HttpClient httpClient)
        {
            using var httpResponse = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            
            // throws error if status code is not 200
            httpResponse.EnsureSuccessStatusCode();
            if(httpResponse.Content is object && 
                httpResponse.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = await httpResponse.Content.ReadAsStreamAsync();
                using var streamReader = new StreamReader(contentStream);
                using var jsonReader = new JsonTextReader(streamReader);

                JsonSerializer serializer = new JsonSerializer();
                try
                {
                    return serializer.Deserialize<Product>(jsonReader);
                }
                catch (Exception ex)
                {
                    // "Invalid JSON."
                    throw;
                }
            }
            else
            {
                // "HTTP Response was invalid and cannot be deserialised."
            }
            return null;
        }

        private static async Task<Product> StreamSystemTextJson(string uri, HttpClient httpClient)
        {
            using var httpResponse = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            httpResponse.EnsureSuccessStatusCode();

            if(httpResponse.Content is object && httpResponse.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = await httpResponse.Content.ReadAsStreamAsync();
                try
                {
                    return await System.Text.Json.JsonSerializer.DeserializeAsync<Product>(contentStream);
                }
                catch (JsonException jex)
                {
                    // invalid Json
                    throw;
                }
            }
            else
            {
                // "HTTP Response was invalid and cannot be deserialised."
            }
            return null;
        }

        private static async Task<Product> StreamSystemNetHttpJson(string uri, HttpClient httpClient)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.TryAddWithoutValidation("header-name", "header-value");

            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<Product>();
                }
                catch(NotSupportedException ex) { /* When content type is not valid*/}
                catch (JsonException ex) {/* Invalid JSON */ }
            }
            else { }
            return null;
        }

        private static async Task PostJsonHttpClient(string uri, HttpClient httpClient)
        {
            var postProduct = new Product() { Id=1, Name="John" };
            var postResponse = await httpClient.PostAsJsonAsync(uri, postProduct);

            postResponse.EnsureSuccessStatusCode();
        }

        private static async Task PostJsonContent(string uri, HttpClient httpClient)
        {
            var postProduct = new Product() { Id = 1, Name = "John" };
            var postRequest = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = JsonContent.Create(postProduct)
            };
            var postResponse = await httpClient.SendAsync(postRequest);
            postResponse.EnsureSuccessStatusCode();
        }

        internal class Product { public int Id { get; set; } public string Name { get; set; } }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
