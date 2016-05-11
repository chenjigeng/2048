using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048.Models
{
    class player
    {
        public int Id;
        public string Username;
        public string Password;
        public string Account;
        public long HighestScore;
        public string Birthday;
        public long CurrentScore;
        public player(string username, string password, string account, string birthday = null, long highestScore = 0)
        {
            this.Username = username;
            this.Password = password;
            this.Account = account;
            this.Birthday = birthday;
            this.HighestScore = highestScore;
        }
        public void update()
        {
            var conn = App.conn;
            try {
                using (var play = App.conn.Prepare("UPDATE Players SET HighestScore = ? WHERE Username = ?"))
                {
                    play.Bind(1, this.HighestScore);
                    play.Bind(2, this.Username);
                    play.Step();
                }
            }
            catch(Exception exe)
            {
                
            }
        }
    }
}
