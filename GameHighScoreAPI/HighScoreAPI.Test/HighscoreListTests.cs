using GameHighScoreAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HighScoreAPI.Test
{


    public class HighscoreListTests
    {
        DbContextOptions<HighscoreContext> options;
        HighscoreContext hc;
        DataAccess da;

        public HighscoreListTests()
        {
            options = new DbContextOptionsBuilder<HighscoreContext>()
.UseSqlServer("Server=localhost;Database=HighscoreEntries;Trusted_Connection=True")
.Options;
           hc  = new HighscoreContext(options);
            this.da = new DataAccess(this.hc);
        }
        [Fact]
        public void CheckEmptyListCount()
        {

            Assert.Empty(da.GetHighscores());
        }

        [Fact]
        public void CheckAdd()
        {
            HighscoreEntry he = new HighscoreEntry();
            he.Credentials = "Username1";
            he.Score = 10;
            List<HighscoreEntry> entries = da.AddHighscore(he).Result;
            Assert.Single(entries);
            Assert.Equal(he, entries.FirstOrDefault());
            Assert.Equal(0, entries.IndexOf(he));
            hc.Highscores.RemoveRange(hc.Highscores.ToList());
            hc.SaveChanges();

        }

        [Fact]

        public void CheckAddSamePlayerTwiceHigherHighscore()
        {
            HighscoreEntry he = new HighscoreEntry();
            he.Credentials = "Username1";
            he.Score = 10;
            da.AddHighscore(he).Wait();
            HighscoreEntry he2 = new HighscoreEntry();
            he2.Credentials = "Username1";
            he2.Score = 20;
            List<HighscoreEntry> entries = da.AddHighscore(he2).Result;

            Assert.Single(entries);
            Assert.Equal(he, entries.FirstOrDefault());
            Assert.Equal(0, entries.IndexOf(he));
            Assert.Equal(20, entries.FirstOrDefault().Score);
            hc.Highscores.RemoveRange(hc.Highscores.ToList());
            hc.SaveChanges();
        }

        [Fact]

        public void CheckAddSamePlayerTwiceLowerHighscore()
        {
            HighscoreEntry he = new HighscoreEntry();
            he.Credentials = "Username1";
            he.Score = 10;
            da.AddHighscore(he).Wait();
            HighscoreEntry he2 = new HighscoreEntry();
            he2.Credentials = "Username1";
            he2.Score = 5;
            List<HighscoreEntry> entries = da.AddHighscore(he2).Result;

            Assert.Single(entries);
            Assert.Equal(he, entries.FirstOrDefault());
            Assert.Equal(0, entries.IndexOf(he));
            Assert.Equal(10, entries.FirstOrDefault().Score);
            hc.Highscores.RemoveRange(hc.Highscores.ToList());
            hc.SaveChanges();
        }

        [Fact]

        public void CheckMoreThan10HighScores()
        {
            HighscoreEntry he = new HighscoreEntry();
            for (int i = 1; i <= 10; i++)
            {

                he.Credentials = "Username"+i;
                he.Score = i;
                da.AddHighscore(he).Wait();
                he = new HighscoreEntry();
            }
            he = new HighscoreEntry();
            he.Credentials = "Username11";
            he.Score = 11;
            List<HighscoreEntry> entries = da.AddHighscore(he).Result;


            Assert.Equal(10,entries.Count);
            Assert.Equal(he, entries.FirstOrDefault());
            Assert.Equal(0, entries.IndexOf(he));
            Assert.Equal(11, entries.FirstOrDefault().Score);
            Assert.Equal(2, entries.LastOrDefault().Score);
            hc.Highscores.RemoveRange(hc.Highscores.ToList());
            hc.SaveChanges();
        }
        [Fact]
        public void checkOrderedList()
        {
            
            HighscoreEntry he = new HighscoreEntry();
            he.Credentials = "Username11";
            he.Score = 11;
            da.AddHighscore(he).Wait();
            he = new HighscoreEntry();
            he.Credentials = "Username1";
            he.Score = 1;
            da.AddHighscore(he).Wait();
            he = new HighscoreEntry();
            he.Credentials = "Username5";
            he.Score = 5;
            List<HighscoreEntry> entries = da.AddHighscore(he).Result;

            Assert.Equal(11, entries.FirstOrDefault().Score);
            Assert.Equal(5, entries.ToArray()[1].Score);
            Assert.Equal(1, entries.ToArray()[2].Score);

            entries = da.GetHighscores();
            Assert.Equal(11, entries.FirstOrDefault().Score);
            Assert.Equal(5, entries.ToArray()[1].Score);
            Assert.Equal(1, entries.ToArray()[2].Score);
            hc.Highscores.RemoveRange(hc.Highscores.ToList());
            hc.SaveChanges();
        }

        
    }
}
