using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameHighScoreAPI.Controllers
{
    [ApiController]
    [Route("highscoreentries")]
    public class HighscoreController : ControllerBase
    {
        static readonly HttpClient client = new HttpClient();
        private readonly IDataAccess dal;

        private readonly ILogger<HighscoreController> _logger;

        public HighscoreController(IDataAccess _dal, ILogger<HighscoreController> logger)
        {
            dal = _dal;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<HighscoreEntry> GetHighscores()
        {
            return dal.GetHighscores();
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<HighscoreEntry>>> AddHighscore([FromBody] HighscoreEntryWithCaptcha he)
        {
            if (!await UserResponse(he))
            {
                return BadRequest();
            }
            HighscoreEntry h = new HighscoreEntry();
            h.Credentials = he.Credentials;
            h.Score = he.Score;
            return await dal.AddHighscore(h);
        }

        public async Task<bool> UserResponse(HighscoreEntryWithCaptcha he)
        {
            CaptchaData cd = new CaptchaData();
            cd.response = he.UserToken;
            cd.secret = "6LfDoOMUAAAAAIAZ4rPoMgC1a_6u1DpdR3-NcLuY";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("secret", cd.secret),
                new KeyValuePair<string, string>("response", cd.response)
            });

            var responseString = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
            var responseBody = await responseString.Content.ReadAsStringAsync();
            var resp = JsonSerializer.Deserialize<GoogleAPIResponse>(responseBody);
            return resp.Success;
        }
    }
}
