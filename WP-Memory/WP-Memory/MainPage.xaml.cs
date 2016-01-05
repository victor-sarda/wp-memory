using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WP_Memory.Resources;

namespace WP_Memory
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructeur
        public MainPage()
        {
            InitializeComponent();
            // Exemple de code pour la localisation d'ApplicationBar
            //BuildLocalizedApplicationBar();


            // Exemple de code pour la conception d'une ApplicationBar localisée
            //private void BuildLocalizedApplicationBar()
            //{
            //    // Définit l'ApplicationBar de la page sur une nouvelle instance d'ApplicationBar.
            //    ApplicationBar = new ApplicationBar();

            //    // Crée un bouton et définit la valeur du texte sur la chaîne localisée issue d'AppResources.
            //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            //    appBarButton.Text = AppResources.AppBarButtonText;
            //    ApplicationBar.Buttons.Add(appBarButton);

            //    // Crée un nouvel élément de menu avec la chaîne localisée d'AppResources.
            //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            //    ApplicationBar.MenuItems.Add(appBarMenuItem);
            //}
        }
    
        
    // Fonctions qui gèrent le click sur les boutons 
        private void JeuFacile(object sender, RoutedEventArgs e)
        {
            PhoneApplicationService.Current.State["nb"] = 6;
            NavigationService.Navigate(new Uri("/Jeu.xaml", UriKind.Relative));
        }

        private void JeuMoyen(object sender, RoutedEventArgs e)
        {
            PhoneApplicationService.Current.State["nb"] = 8;
            NavigationService.Navigate(new Uri("/Jeu.xaml", UriKind.Relative));
        }

        private void JeuDifficile(object sender, RoutedEventArgs e)
        {
            PhoneApplicationService.Current.State["nb"] = 10;
            NavigationService.Navigate(new Uri("/Jeu.xaml", UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Scores.xaml", UriKind.Relative));
        }
    }
}