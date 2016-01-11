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
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Xml;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;

namespace WP_Memory
{
    public partial class Jeu : PhoneApplicationPage
    {
        // Initialisation des variables 
        bool bloque;
        bool stop ;
        String Mode;
        List<Carte> cartes;
        List<Image> images;
        int nb = (int)PhoneApplicationService.Current.State["nb"];
        int score;

        public Jeu()
        {
            Jouer();
        }

        private void Jouer()
        {
            InitializeComponent();
            bloque = true;
            stop = false;
            score = 0;
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
            for (int i=0; i<images.Count; i++){
                images[i].Opacity = 1.0;
            }


            if (PhoneApplicationService.Current.State.ContainsKey("nb"))
            {
                // Modification du header
                switch (nb)
                {
                    case 4:
                        Mode = "Débutant";
                        A.Text = Mode;
                        cartes = new List<Carte> { carte1, carte2, carte3, carte4 };
                        break;
                    case 6:
                        Mode = "Facile";
                        A.Text = Mode;
                        cartes = new List<Carte> { carte1, carte2, carte3, carte4, carte5, carte6 };
                        break;
                    case 8:
                        Mode = "Moyen";
                        A.Text = Mode;
                        cartes = new List<Carte> { carte1, carte2, carte3, carte4, carte5, carte6, carte7, carte8 };
                        break;
                    case 10:
                        Mode = "Difficile";
                        A.Text = Mode;
                        cartes = new List<Carte> { carte1, carte2, carte3, carte4, carte5, carte6, carte7, carte8, carte9, carte10 };
                        break;
                    default:
                        cartes = new List<Carte> { carte1, carte2, carte3 };
                        break;

                }
                // Variable de textes 
                textScore.Text = "Nombre de mouvements : 0";
                textScore.TextAlignment = TextAlignment.Center;
                A.TextAlignment = TextAlignment.Center;

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

                // Si deux cartes retournées
                Paire();
            }
        }

        // Fonctions qui gèrent l'attente
        public async Task Attendre(int tache)
        {
            Task<int> delai = Delai(tache);
            int result = await delai;
        }

        public async Task<int> Delai(int tache) 
        {
            await Task.Delay(1000);
            if (tache == 1)
            {
                bloque = true;
            }
            else
            {
                BtnRejouer.IsEnabled = true;
            }
            return 1;
        }

        // Fonction qui vérifie si les deux cartes retournées sont les meme 
        private async void Paire()
        {
            // Initialisation des parametres
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
                // Initialisation des variables et incrementation du score
                score++;
                textScore.Text = "Nombre de mouvements : " + score;
                textScore.TextAlignment = TextAlignment.Center;
                bloque = false;

                // Si même cartes
                if (cartesRetournee[0].Valeur == cartesRetournee[1].Valeur)
                {
                    // On les bloques
                    cartesRetournee[0].EstGelee = true;
                    cartesRetournee[1].EstGelee = true;
                    bloque = true;

                    // On anime leur opacité : 
                    DoubleAnimation myDoubleAnimation1 = new DoubleAnimation();
                    DoubleAnimation myDoubleAnimation2 = new DoubleAnimation();

                    // Ajout de la durée
                    Duration temps = new Duration(TimeSpan.FromSeconds(1));
                    myDoubleAnimation1.Duration = temps;
                    myDoubleAnimation2.Duration = temps;

                    // Réglage des valeurs d'opacité
                    myDoubleAnimation1.From = 1.0;
                    myDoubleAnimation1.To = 0; 
                    myDoubleAnimation2.From = 1.0;
                    myDoubleAnimation2.To = 0;

                    // Définition de l'animation
                    Storyboard sb = new Storyboard();
                    sb.Duration = temps;
                    sb.Children.Add(myDoubleAnimation1);
                    sb.Children.Add(myDoubleAnimation2);
                    Storyboard.SetTarget(myDoubleAnimation1, images[cartesRetournee[0].ImgId]);
                    Storyboard.SetTarget(myDoubleAnimation2, images[cartesRetournee[1].ImgId]);
                    Storyboard.SetTargetProperty(myDoubleAnimation1, new PropertyPath("Opacity"));
                    Storyboard.SetTargetProperty(myDoubleAnimation2, new PropertyPath("Opacity"));

                    // Lancement de l'animation
                    sb.Begin();

                    // Si plus de paires restantes a trouver :
                    if (nbTrouve == (nb - 2))
                    {
                        // Popup de victoire
                        TextPopup.Text = "Gagné ! \n Nombre de mouvements : " + score.ToString() +"\n Entrez votre pseudo ci-dessous :";
                        BtnAnnuler.Visibility = System.Windows.Visibility.Collapsed; 
                        BtnRejouer.Visibility = System.Windows.Visibility.Visible;
                        BtnRejouer.IsEnabled = false;
                        await Attendre(2);
                        BtnStop.IsEnabled = false;
                        TextPopup.TextAlignment = TextAlignment.Center;
                        if (!StandardPopup.IsOpen) { StandardPopup.IsOpen = true; }
                    }
                }  else
                {
                    // On  : 
                    DoubleAnimation myDoubleAnimation1 = new DoubleAnimation();
                    DoubleAnimation myDoubleAnimation2 = new DoubleAnimation();

                    // Ajout de la durée
                    Duration temps = new Duration(TimeSpan.FromSeconds(1));
                    myDoubleAnimation1.Duration = temps;
                    myDoubleAnimation2.Duration = temps;                  

                    // Réglage des valeurs d'opacité
                    myDoubleAnimation1.To = 360 ;
                    myDoubleAnimation2.To = 360;

                    // Définition de l'animation
                    Storyboard sb = new Storyboard();
                    sb.Duration = temps;
                    sb.Children.Add(myDoubleAnimation1);
                    sb.Children.Add(myDoubleAnimation2);

                    RotateTransform rt = new RotateTransform();
                    RotateTransform rt2 = new RotateTransform();

                    Storyboard.SetTarget(myDoubleAnimation1, rt);
                    Storyboard.SetTarget(myDoubleAnimation2, rt2);
                    Storyboard.SetTargetProperty(myDoubleAnimation1, new PropertyPath("Angle"));
                    Storyboard.SetTargetProperty(myDoubleAnimation2, new PropertyPath("Angle"));

                    images[cartesRetournee[0].ImgId].RenderTransform = rt;
                    images[cartesRetournee[0].ImgId].RenderTransformOrigin = new Point(0.5, 0.5);
                    images[cartesRetournee[1].ImgId].RenderTransform = rt;
                    images[cartesRetournee[1].ImgId].RenderTransformOrigin = new Point(0.5, 0.5);
                    // Lancement de l'animation
                    sb.Begin();

                    // On retourne les cartes face caché apres une seconde
                    await Attendre(1);
                    cartesRetournee[0].EstRetournee = false;
                    cartesRetournee[1].EstRetournee = false;
                    images[cartesRetournee[0].ImgId].Source = new BitmapImage(new Uri("/Assets/back.png", UriKind.Relative));
                    images[cartesRetournee[1].ImgId].Source = new BitmapImage(new Uri("/Assets/back.png", UriKind.Relative));
                }
            }
        }
        // Fonction rejouer
          private void RestartPartie(object sender, RoutedEventArgs e)
        {
            Jouer();
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }

        // Fonction annuler 
        private void FermerPopup(object sender, RoutedEventArgs e)
        {
            stop = false; 
            if (StandardPopup.IsOpen) { StandardPopup.IsOpen = false; }
        }

        // Fonction pour arreter le jeu
        private void ArreterJeu(object sender, RoutedEventArgs e)
        {
            // Popup de victoire
            stop = true;
            TextPopup.Text = "Etes vous sur de vouloir quitter ? \n Nombre de mouvements : " + score.ToString() +"\n La partie ne sera pas sauvegardée";
            txtPseudo.Visibility = System.Windows.Visibility.Collapsed;
            BtnRejouer.Visibility = System.Windows.Visibility.Collapsed;
            BtnAnnuler.Visibility = System.Windows.Visibility.Visible;
            TextPopup.TextAlignment = TextAlignment.Center;
            if (!StandardPopup.IsOpen) { StandardPopup.IsOpen = true; }
        }

        // Fonction de fin de partie
        private void FinPartie(object sender, RoutedEventArgs e)
        {
            if (stop == false)
            {
                SauvegarderScore(txtPseudo.Text);
            }
            NavigationService.GoBack();
        }

        // Fonction qui sauvegarde le score
        private void SauvegarderScore(String pseudo)
        {
            // Write to the Isolated Storage
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                List<Users> list = new List<Users>();
                // Mode création ou edition selon l'existance du fichier
                if (myIsolatedStorage.FileExists(Mode + ".xml"))
                {
                   using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile(Mode + ".xml", FileMode.Open))
                   {
                       XmlSerializer serializer = new XmlSerializer(typeof(List<Users>));
                       list = (List<Users>)serializer.Deserialize(stream);
                   }
                } 

                // Ecriture du score
                using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile(Mode + ".xml", FileMode.Create))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Users>));
                        if (pseudo.Length == 0) { 
                            pseudo = "Undefined";
                        }
                        Users user = new Users(pseudo, score.ToString());
                        list.Add(user);
                        using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                        {
                            serializer.Serialize(xmlWriter, list);
                        }
                    }
            }
        }
    }
}