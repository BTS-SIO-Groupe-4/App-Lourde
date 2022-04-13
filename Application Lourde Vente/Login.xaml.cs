using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Text;  
using System.Windows;  
using System.Windows.Controls;  
using System.Windows.Data;  
using System.Windows.Documents;  
using System.Windows.Input;  
using System.Windows.Media;  
using System.Windows.Media.Imaging;  
using System.Windows.Navigation;  
using System.Windows.Shapes;  
using System.Data;  
using System.Data.SqlClient;  
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Application_Lourde_Vente
{
    /// <summary>
    /// Logique d'interaction pour Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            bdd.Initialize();
        }

        private void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnConnexion_Click(sender, e);
                e.Handled = true;
            }
        }
        private void BtnConnexion_Click(object sender, RoutedEventArgs e)
        {
            string Test = TxtMdp.Password;
                if(bdd.VerifConnexion(TxtPseudo.Text, TxtMdp.Password) == true)
                {
                Main main = new Main();
                main.Show();
                Close();

                if (bdd.PostCo == "0")
                {
                    main.BtnAjCli.Visibility = Visibility.Hidden;
                    main.BtnAjFac.Visibility = Visibility.Hidden;
                    main.BtnAjProd.Visibility = Visibility.Hidden;
                    main.BtnAjRdv.Visibility = Visibility.Hidden;                    

                    main.BtnMdfCli.Visibility = Visibility.Hidden;
                    main.BtnMdfFac.Visibility = Visibility.Hidden;
                    main.BtnMdfProd.Visibility = Visibility.Hidden;
                    //main.BtnMdfRdv.Visibility = Visibility.Hidden;

                    main.BtnSupCli.Visibility = Visibility.Hidden;
                    main.BtnSupFac.Visibility = Visibility.Hidden;
                    main.BtnSupProd.Visibility = Visibility.Hidden;
                    //main.BtnSupRdv.Visibility = Visibility.Hidden;

                    main.CbxListeCom.Visibility = Visibility.Hidden;
                    main.LblNomCom.Visibility = Visibility.Hidden;

                    main.TxtNomCli.IsEnabled = false;
                    main.TxtPrenomCli.IsEnabled = false;
                    main.TxtVilleCli.IsEnabled = false;
                    main.TxtRueCli.IsEnabled = false;
                    main.TxtCPCli.IsEnabled = false;
                    main.TxtMailCli.IsEnabled = false;
                    main.TxtPros.IsEnabled = false;
                    main.TxtNumCli.IsEnabled = false;

                    main.TxtTypeProd.IsEnabled = false;
                    main.TxtPrixProd.IsEnabled = false;
                    main.TxtDescProd.IsEnabled = false;
                    main.TxtNomProd.IsEnabled = false;

                    main.TxtDateFac.IsEnabled = false;
                    main.CbxCliFac.IsEnabled = false;

                    main.CbxListeCli.IsEnabled = false;
                }

                }
                else
                {
                    errormessage.Text = "Désolé! Veuillez entrez un identifiant/mot de passe valide.";
                }
            //}
        }
    }
}
