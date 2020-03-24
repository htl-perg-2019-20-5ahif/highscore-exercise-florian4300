using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHighScoreAPI
{
    public interface IDataAccess : IDisposable
    {
        public List<HighscoreEntry> GetHighscores();
        public Task<List<HighscoreEntry>> AddHighscore(HighscoreEntry he);
    }
}
