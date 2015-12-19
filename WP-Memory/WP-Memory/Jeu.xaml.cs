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

namespace WP_Memory
{
    public partial class Jeu : PhoneApplicationPage
    {
        public Jeu()
        {
            InitializeComponent();
            // Initialisation du listing des images
            List<Image> images = new List<Image> { Image1, Image2, Image3, Image4, Image5, Image6, Image7, Image8, Image9, Image10, Image11, Image12 };
            if (PhoneApplicationService.Current.State.ContainsKey("nb"))
            {
                int nb = (int) PhoneApplicationService.Current.State["nb"];
                // Modification du header
                switch (nb)
                {
                    case 4:
                        A.Text = "Facile";
                        break;
                    case 6:
                        A.Text = "Moyen";
                        break;
                    case 8:
                        A.Text = "Difficile";
                        break;
                }
                // On affiche le nombre de cartes correspondant au niveau de difficulté
                for (int i = 0; i < nb; i++)
                {
                    images[i].Source = new BitmapImage(new Uri("/Assets/dos12.png", UriKind.Relative));
                } 
            }
        }

        // Fonction qui retourne la carte
        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Image image_tap = (Image) sender;
            image_tap.Source = new BitmapImage(new Uri("/Assets/AlignmentGrid.png", UriKind.Relative));
        }
    }
}