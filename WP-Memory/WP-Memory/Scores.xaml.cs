using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Xml;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;

namespace WP_Memory
{
    public partial class Scores : PhoneApplicationPage
    {
        public Scores()
        {
            InitializeComponent();
        }

        private void AfficheScore(object sender, RoutedEventArgs e)
        {
            try
            {
                // Mode de jeu selectionné
                String mode="";
                if (CBDebutant.IsChecked == true)
                {
                    mode = "Débutant";
                }
                else if (CBFacile.IsChecked == true)
                {
                    mode = "Facile";
                }
                else if (CBMoyen.IsChecked == true)
                {
                    mode = "Moyen";
                }
                else if (CBDifficile.IsChecked == true)
                {
                    mode = "Difficile";
                }

                if (mode.Length > 1)
                {
                    using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        List<Users> list = new List<Users>();
                        using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile(mode + ".xml", FileMode.Open))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(List<Users>));
                            list = (List<Users>)serializer.Deserialize(stream);
                        }
                       list.Sort();
                        listScores.DataContext = list;
                    }
                }
            }
            catch (InvalidOperationException exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void Retour(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }


    }
}