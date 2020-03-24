using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameHighScoreAPI
{
    public class DataAccess : IDataAccess
    {
        private HighscoreContext context;
        //private List<HighscoreEntry> highscoreEntries = new List<HighscoreEntry>();

        public DataAccess(HighscoreContext _context)
        {
            context = _context;
        }
        public async Task<List<HighscoreEntry>> AddHighscore(HighscoreEntry he)
        {
            if (context.Highscores.Where(h => h.Credentials == he.Credentials).FirstOrDefault() == null)
            {
                context.Highscores.Add(he);
                await context.SaveChangesAsync();
                if (context.Highscores.ToList().Count > 10)
                {
                    context.Highscores.Remove(context.Highscores.OrderBy(s => s.Score).FirstOrDefault());
                }
                
            } else
            {
                if (context.Highscores.Where(i => i.Credentials == he.Credentials).FirstOrDefault().Score < he.Score)
                {
                    context.Highscores.Where(i => i.Credentials == he.Credentials).FirstOrDefault().Score = he.Score;
                }    
            }
            await context.SaveChangesAsync();
            return context.Highscores.OrderByDescending(s => s.Score).ToList();
        }

        public void Dispose()
        {
            if(this.context != null)
            {
                this.context.Dispose();
            }
            
        }

        public List<HighscoreEntry> GetHighscores()
        {
            return context.Highscores.OrderByDescending(s => s.Score).ToList(); ;
        }
    }
}
