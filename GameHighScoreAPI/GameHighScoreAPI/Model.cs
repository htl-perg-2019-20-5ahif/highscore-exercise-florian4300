using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameHighScoreAPI
{
    public class HighscoreEntry
    {
        public int HighscoreEntryId { get; set; }
        public int Score { get; set; }
        public string Credentials { get; set; }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                HighscoreEntry he = (HighscoreEntry)obj;
                return (Score == he.Score) && (Credentials == he.Credentials);
            }
        }
    }
    public class HighscoreEntryWithCaptcha
    {
        public int HighscoreEntryId { get; set; }
        public int Score { get; set; }
        public string Credentials { get; set; }

        [JsonPropertyName("captcha")]
        public String UserToken { get; set; }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                HighscoreEntry he = (HighscoreEntry)obj;
                return (Score == he.Score) && (Credentials == he.Credentials);
            }
        }
    }
    public class CaptchaData
    {
        [JsonPropertyName("secret")]
        public String secret { get; set; }

        [JsonPropertyName("response")]
        public String response { get; set; }
    }

    public class GoogleAPIResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("challenge_ts")]
        public string Challenge_ts { get; set; }

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }
    }
}
