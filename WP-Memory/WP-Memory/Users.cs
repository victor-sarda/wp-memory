using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP_Memory
{
    public class Users : IComparable<Users>
    {
        string pseudo;
        string score;

        public string Pseudo
        {
            get { return pseudo; }
            set { pseudo = value; }
        }

        public string Score
        {
            get { return score; }
            set { score = value; }
        }
        public Users() {
            this.pseudo = "";
            this.score = "0";
        }
        public Users(string pseudo, string score)
        {
            this.score = score;
            this.pseudo = pseudo;
        }

        // tri du score
        public int CompareTo(Users compareScore)
        {
            // A null value means that this object is greater.
            if (compareScore == null)
                return 1;

            else
                return this.Score.CompareTo(compareScore.Score);
        }
    }  
}
