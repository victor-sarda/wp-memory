using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP_Memory
{
    class Carte
    {

        int valeur;
        bool estRetournee;
        bool estGelee;

        string image;
        int imgId;


        public int Valeur
        {
            get { return valeur; }
            set { valeur = value; }
        }

        public bool EstRetournee
        {
            get { return estRetournee; }
            set { estRetournee = value; }
        }

        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        public int ImgId
        {
            get { return imgId; }
            set { imgId = value; }
        }
        public bool EstGelee
        {
            get { return estGelee; }
            set { estGelee = value; }
        }
        public Carte(int val, bool retourne, string img) {

            this.valeur = val;
            this.estRetournee = retourne;
            this.image = img;
            this.estGelee = false;
        }
    }

}
