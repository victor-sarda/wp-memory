using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;

namespace WP_Memory
{
    public partial class Jeu : PhoneApplicationPage
    {
        // Initialisation des variables 
        bool bloque = true;
        List<Carte> cartes;
        List<Image> images;
        int nb = (int)PhoneApplicationService.Current.State["nb"];
        int score =0;

        public Jeu()
        {
            InitializeComponent();
            
            // Initialisation des cartes
            Carte carte1 = new Carte(1, false, "/Assets/etoile.png");
            Carte carte2 = new Carte(1, false, "/Assets/etoile.png");
            Carte carte3 = new Carte(2, false, "/Assets/pingouin.png");
            Carte carte4 = new Carte(2, false, "/Assets/pingouin.png");
            Carte carte5 = new Carte(3, false, "/Assets/bonhomme.png");
            Carte carte6 = new Carte(3, false, "/Assets/bonhomme.png");
            Carte carte7 = new Carte(4, false, "/Assets/pere-noel.png");
            Carte carte8 = new Carte(4, false, "/Assets/pere-noel.png");
            Carte carte9 = new Carte(5, false, "/Assets/renne.png");
            Carte carte10 = new Carte(5, false, "/Assets/renne.png");
            
            // Initialisation du listing des images
            images = new List<Image> { Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10};

            if (PhoneApplicationService.Current.State.ContainsKey("nb"))
            {
                // Modification du header
                switch (nb)
                {
                    case 6:
                        A.Text = "Facile";
                        cartes = new List<Carte> { carte1, carte2, carte3, carte4, carte5, carte6 };
                        break;
                    case 8:
                        A.Text = "Moyen";
                        cartes = new List<Carte> { carte1, carte2, carte3, carte4, carte5, carte6, carte7, carte8 };
                        break;
                    case 10:
                        A.Text = "Difficile";
                        cartes = new List<Carte> { carte1, carte2, carte3, carte4, carte5, carte6, carte7, carte8, carte9, carte10 };
                        break;
                    default:
                        cartes = new List<Carte> { carte1, carte2, carte3 };
                        break;
                }

                // On trie de facon aléatoire les cartes
                Random rnd = new Random();
                cartes = cartes.OrderBy(x => rnd.Next()).ToList();

                // On affiche le nombre de cartes correspondant au niveau de difficulté
                for (int i = 0; i < nb; i++)
                {
                    cartes[i].ImgId = i;
                    images[i].Source = new BitmapImage(new Uri("/Assets/back.png", UriKind.Relative));                       
                }
            }
        }

        // Fonction qui retourne la carte
        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (bloque == true)
            {
                Image image_tap = (Image)sender;

                // Récupération de l'id de la carte
                int val = (int.Parse(image_tap.Name.ToString().Substring(5)) - 1);

                // Retourne la carte
                if (cartes[val].EstRetournee == false)
                {
                    image_tap.Source = new BitmapImage(new Uri(cartes[val].Image.ToString(), UriKind.Relative));
                    image_tap.OnApplyTemplate();
                    cartes[val].EstRetournee = true;
                }

                Paire();
            }
        }

        // Fonctions qui gèrent l'attente
        public async Task Attendre()
        {
            Task<int> delai = Delai();
            int result = await delai;
        }

        public async Task<int> Delai() 
        {
            await Task.Delay(1000);
            bloque = true;
            return 1;
        }

        private async void Paire()
        {
            List<Carte> cartesRetournee = new List<Carte>();
            int nbTrouve = 0;
            // Recherche si une carte est retournee 
            for (int i = 0; i < cartes.Count; i++)
            {
                if (cartes[i].EstRetournee == true && cartes[i].EstGelee == false)
                {
                    cartesRetournee.Add(cartes[i]);
                }
                if (cartes[i].EstGelee)
                {
                    nbTrouve++;
                }
            }

            // Si il y a deja une carte retournée
            if (cartesRetournee.Count == 2)
            {
                score++;
                textScore.Text = "Essais : " + score;
                bloque = false;
                if (cartesRetournee[0].Valeur == cartesRetournee[1].Valeur)
                {
                    cartesRetournee[0].EstGelee = true;
                    cartesRetournee[1].EstGelee = true;
                    bloque = true;

                    if (nbTrouve == (nb - 2)){
                        System.Diagnostics.Debug.WriteLine("T'as gg mec !");
                        
                    }
                }
                else
                {
                    // On retourne les cartes face caché apres une seconde
                    await Attendre();
                    cartesRetournee[0].EstRetournee = false;
                    cartesRetournee[1].EstRetournee = false;
                    images[cartesRetournee[0].ImgId].Source = new BitmapImage(new Uri("/Assets/back.png", UriKind.Relative));
                    images[cartesRetournee[1].ImgId].Source = new BitmapImage(new Uri("/Assets/back.png", UriKind.Relative));
                }
            }
        }
        public static List<T> Randomize<T>(List<T> list)
        {
            List<T> randomizedList = new List<T>();
            Random rnd = new Random();
            while (list.Count > 0)
            {
                int index = rnd.Next(0, list.Count); //pick a random item from the master list
                randomizedList.Add(list[index]); //place it at the end of the randomized list
                list.RemoveAt(index);
            }
            return randomizedList;
        }
    }
}