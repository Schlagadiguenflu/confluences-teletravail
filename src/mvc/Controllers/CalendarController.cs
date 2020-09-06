using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using Newtonsoft.Json;
using Swan;

namespace mvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private IConfiguration _configuration { get; }

        public CalendarController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: api/Calendar
        [HttpGet]
        public async Task<string> GetCalendar(DateTime start, DateTime end, string userId)
        {
            // Préparation de l'appel à l'API
            //string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");

            //string content = await client.GetStringAsync(_configuration["URLAPI"] + $"api/homework/sessionhomework?sessionid={sessionId}&homeworktypeid={homeworkTypeId}");
            string url = _configuration["URLAPI"] + "api/calendar?start=" + start.ToString("yyyy-MM-dd") + "&end=" + end.ToString("yyyy-MM-dd") + "&userId=" + userId;
            string content = await client.GetStringAsync(url);

            return content;
        }
    }
}