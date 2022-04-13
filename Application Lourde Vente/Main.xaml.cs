
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Application_Lourde_Vente
{
    /// <summary>
    /// Logique d'interaction pour Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        List<facture> cFac = new List<facture>();
        List<Client> cClient = new List<Client>();
        List<rdv> cRdv = new List<rdv>();
        List<Client> listecli = new List<Client>();
        List<Commerciale> listecom = new List<Commerciale>();
        List<produit> cProd = new List<produit>();
        List<Client> listecliTri { get; set; }
        List<produit> listeprodTri { get; set; }
        List<rdv> listerdvTri { get; set; }

        public Main()
        {
            
            InitializeComponent();
            cClient = bdd.SelectClient();
            DataGridClient.ItemsSource = cClient;
            listecli = bdd.SelectClient();
            CbxListeCli.ItemsSource = listecli;
            listecom = bdd.SelectCommerciale();
            CbxListeCom.ItemsSource = listecom;
            cRdv = bdd.SelectRdv(listecli,listecom);
            DataGrisRdv.ItemsSource = cRdv;
            cProd = bdd.SelectProduit();
            DataGridPrix.ItemsSource = cProd;
            listecli = bdd.SelectClient();
            CbxCliFac.ItemsSource = listecli;
            cFac = bdd.SelectFct(listecli);
            DataGridFacture.ItemsSource = cFac;
        }

        #region client
        private void DataGridClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // On récupère l'objet selectionné dans le datagrid et on le "cast" pour lui donner un type précis (Contrat, Magazine, Pigiste)
            Client clientSelected = (Client)DataGridClient.SelectedItem;
            try
            {
                // On met des valeurs dans les composants de l'interface via les valeurs de l'objet contratSelected
                TxtIdCli.Text = Convert.ToString(clientSelected.NumClient);
                TxtNomCli.Text = Convert.ToString(clientSelected.NomClient);
                TxtPrenomCli.Text = Convert.ToString(clientSelected.PrenomClient);
                TxtRueCli.Text = Convert.ToString(clientSelected.NumRueClient);
                TxtCPCli.Text = Convert.ToString(clientSelected.CPClient);
                TxtVilleCli.Text = Convert.ToString(clientSelected.VilleClient);
                TxtMailCli.Text = Convert.ToString(clientSelected.MailClient);
                TxtNumCli.Text = Convert.ToString(clientSelected.NumTelClient);
                TxtPros.Text = Convert.ToString(clientSelected.Prospect);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                bdd.CloseConnection();
            }
        }

        private void BtnSupCli_Click(object sender, RoutedEventArgs e)
        {
            {
                cClient.Remove((Client)DataGridClient.SelectedItem);
                var cli = (Client)DataGridClient.SelectedItem;
                try
                {
                    bdd.DeleteClient(cli.NumClient);
                    //On préselectionne par défaut le premier élément du Datagrid après la suppression
                    DataGridClient.SelectedIndex = 0;
                    DataGridClient.Items.Refresh();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    bdd.CloseConnection();
                }
            }
        }

        private void BtnAjCli_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bdd.InsertClient(TxtNomCli.Text, TxtPrenomCli.Text, TxtNumCli.Text, TxtMailCli.Text, TxtVilleCli.Text, TxtRueCli.Text, TxtCPCli.Text, Convert.ToInt32(TxtPros.Text));
                cClient.Clear();
                cClient = bdd.SelectClient();
                DataGridClient.ItemsSource = cClient;
                DataGridClient.Items.Refresh();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                bdd.CloseConnection();
            }
        }

        private void BtnMdfCli_Click(object sender, RoutedEventArgs e)
        {
            //On recherche à quel indice de la collection se trouve l'object selectionné dans le datagrid
            int indice = cClient.IndexOf((Client)DataGridClient.SelectedItem);

            // On change les propritétés de l'objet à l'indice trouvé. On ne change pas le numéro de magazine via l'interface, trop de risques d'erreurs en base de données
            cClient.ElementAt(indice).NomClient = TxtNomCli.Text;
            cClient.ElementAt(indice).PrenomClient = TxtPrenomCli.Text;
            cClient.ElementAt(indice).NumTelClient = Convert.ToInt32(TxtNumCli.Text);
            cClient.ElementAt(indice).MailClient = TxtMailCli.Text;
            cClient.ElementAt(indice).VilleClient = TxtVilleCli.Text;
            cClient.ElementAt(indice).NumRueClient = TxtRueCli.Text;
            cClient.ElementAt(indice).CPClient = TxtCPCli.Text;
            bdd.UpdateClient(Convert.ToInt32(TxtIdCli.Text), TxtNomCli.Text, TxtPrenomCli.Text, TxtNumCli.Text, TxtMailCli.Text, TxtVilleCli.Text, TxtRueCli.Text, TxtCPCli.Text, Convert.ToInt32(TxtPros.Text));


            DataGridClient.Items.Refresh();
        }

        #endregion

        #region Rdv
        private void DataGrisRdv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rdv rdvSelected = (rdv)DataGrisRdv.SelectedItem;
            if (rdvSelected != null)
            {
                CbxListeCli.SelectedItem = rdvSelected.ClientRdv;
                CbxListeCom.SelectedItem = rdvSelected.CommercialeRdv;

                TxtIdRdv.Text = Convert.ToString(rdvSelected.NumRdv);
                CbxListeCli.Text = Convert.ToString(rdvSelected.ClientRdv);
                CbxListeCom.Text = Convert.ToString(rdvSelected.CommercialeRdv);
                TxtDateRdv.Text = Convert.ToString(rdvSelected.DateRdv);
            }
        }

        private void BtnAjRdv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Client ModifClient = CbxListeCli.SelectedItem as Client;

                Commerciale ModifCom = CbxListeCom.SelectedItem as Commerciale;
                
                Client idclient = (Client)CbxListeCli.SelectedItem;
                int idclient2 = idclient.NumClient;

                Commerciale idcom = (Commerciale)CbxListeCom.SelectedItem;
                int idcom2 = idcom.NumCom;

                var parsedDate = DateTime.Parse(TxtDateRdv.Text);
                string date = parsedDate.ToString("yyyy-MM-dd HH:mm:ss");
                bdd.InsertRdv(idclient2, idcom2, date);
                DataGrisRdv.ItemsSource = cRdv;
                DataGrisRdv.Items.Refresh();
            }
            catch (Exception ex)                                                                                      
            {
                Console.WriteLine(ex);
                bdd.CloseConnection();
            }
        }

        private void BtnSupRdv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bdd.DeleteRdv(((rdv)DataGrisRdv.SelectedItem).NumRdv);
                cRdv.Remove((rdv)DataGrisRdv.SelectedItem);
                //On préselectionne par défaut le premier élément du Datagrid après la suppression
                DataGrisRdv.SelectedIndex = 0;
                DataGrisRdv.Items.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                bdd.CloseConnection();
            }

        }

        private void BtnMdfRdv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //On recherche à quel indice de la collection se trouve l'object selectionné dans le datagrid
                int indice = cRdv.IndexOf((rdv)DataGrisRdv.SelectedItem);
                if (indice >= 0)
                {
                    Client idclient = (Client)CbxListeCli.SelectedItem;
                    int idclient2 = idclient.NumClient;

                    Commerciale idcom = (Commerciale)CbxListeCom.SelectedItem;
                    int idcom2 = idcom.NumCom;
                    // On change les propriétés de l'objet à l'indice trouvé. On ne change pas le numéro de magazine via l'interface, trop de risques d'erreurs en base de données
                    cRdv.ElementAt(indice).ClientRdv = (Client)(CbxListeCli.SelectedItem);
                    cRdv.ElementAt(indice).CommercialeRdv = (Commerciale)(CbxListeCom.SelectedItem);
                    cRdv.ElementAt(indice).DateRdv = TxtDateRdv.Text;
                    //modif de la date pour qu'elle rentre dans la bdd
                    var parsedDate = DateTime.Parse(TxtDateRdv.Text);
                    string date = parsedDate.ToString("yyyy-MM-dd HH:mm:ss");
                    bdd.UpdateRdv(Convert.ToInt32(TxtIdRdv.Text), idclient2, idcom2, date);
                    cRdv = bdd.SelectRdv(listecli, listecom);
                    DataGrisRdv.ItemsSource = cRdv;
                    DataGrisRdv.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Veuillez sélectionner un rdv");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                bdd.CloseConnection();
            }
        }
        #endregion

        #region Prix/Produit

        private void DataGridPrix_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            produit produitSelected = (produit)DataGridPrix.SelectedItem;
            try
            {
                TxtIdProd.Text = Convert.ToString(produitSelected.IdProd);
                TxtNomProd.Text = Convert.ToString(produitSelected.NomProd);
                TxtPrixProd.Text = Convert.ToString(produitSelected.PrixProd);
                TxtDescProd.Text = Convert.ToString(produitSelected.DescripProd);
                TxtTypeProd.Text = Convert.ToString(produitSelected.TypeProd);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                bdd.CloseConnection();
            }
        }

        private void BtnAjProd_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                bdd.InsertProduit(TxtTypeProd.Text, Convert.ToDouble(TxtPrixProd.Text), TxtDescProd.Text, TxtNomProd.Text);
                cProd.Clear();
                cProd = bdd.SelectProduit();
                DataGridPrix.ItemsSource = cProd;
                DataGridPrix.Items.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                bdd.CloseConnection();
            }
        }

        private void BtnSupProd_Click(object sender, RoutedEventArgs e)
        {
            {
                cProd.Remove((produit)DataGridPrix.SelectedItem);
                var prod = (produit)DataGridPrix.SelectedItem;
                try
                {
                    bdd.DeleteProduit(prod.IdProd);
                    //On préselectionne par défaut le premier élément du Datagrid après la suppression
                    DataGridPrix.SelectedIndex = 0;
                    DataGridPrix.Items.Refresh();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    bdd.CloseConnection();
                }
            }
        }

        private void BtnMdfProd_Click(object sender, RoutedEventArgs e)
        {
            //On recherche à quel indice de la collection se trouve l'object selectionné dans le datagrid
            int indice = cProd.IndexOf((produit)DataGridPrix.SelectedItem);

            // On change les propritétés de l'objet à l'indice trouvé. On ne change pas le numéro de magazine via l'interface, trop de risques d'erreurs en base de données
            cProd.ElementAt(indice).TypeProd = TxtTypeProd.Text;
            cProd.ElementAt(indice).PrixProd = Convert.ToDouble(TxtPrixProd.Text);
            cProd.ElementAt(indice).DescripProd = TxtDescProd.Text;
            cProd.ElementAt(indice).NomProd = TxtNomProd.Text;
            bdd.UpdateProduit(Convert.ToInt32(TxtIdProd.Text), Convert.ToString(TxtTypeProd.Text), Convert.ToDouble(TxtPrixProd.Text), TxtDescProd.Text, TxtNomProd.Text);


            DataGridPrix.Items.Refresh();
        }

        //private void CbxFiltreRdv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    List<Client> listecli = new List<Client>();
        //    List<Commerciale> listecom = new List<Commerciale>();
        //    listecli = bdd.SelectClient();
        //    listecom = bdd.SelectCommerciale();
        //    //IdRdv = bdd.SearchRdv(Convert.ToInt32(CbxFiltreRdv.SelectedItem), listecli, listecom);
        //    DataGrisRdv.ItemsSource = cRdv;
        //}

        #endregion

        #region Facture
        private void DataGridFacture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            facture factureSelected = (facture)DataGridFacture.SelectedItem;
            if (factureSelected != null)
            {
                CbxCliFac.SelectedItem = factureSelected.ClientFacture;
                TxtIdFac.Text = Convert.ToString(factureSelected.NumFacture);
                CbxCliFac.Text = Convert.ToString(factureSelected.ClientFacture);
                TxtDateFac.Text = Convert.ToString(factureSelected.DateFac);
            }
 
        }

        private void BtnSupFac_Click(object sender, RoutedEventArgs e)
        {

            cFac.Remove((facture)DataGridFacture.SelectedItem);
            try
            {
                bdd.DeleteFac(((facture)DataGridFacture.SelectedItem).NumFacture);
                //On préselectionne par défaut le premier élément du Datagrid après la suppression
                DataGridFacture.SelectedIndex = 0;
                DataGridFacture.Items.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                bdd.CloseConnection();
            }
        }

        private void BtnAjFac_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //recup l'id du cli
                Client idclient = (Client)CbxCliFac.SelectedItem;
                int idclient2 = idclient.NumClient;
                //modif de la date pour qu'elle rentre dans la bdd
                var parsedDate = DateTime.Parse(TxtDateFac.Text);
                string date = parsedDate.ToString("yyyy-MM-dd");
                bdd.InsertFacture(idclient2, date);
                //Réini du datagrid
                cFac.Clear();
                listecli = bdd.SelectClient();
                cFac = bdd.SelectFct(listecli);
                DataGridFacture.ItemsSource = cFac;
                DataGridFacture.Items.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                bdd.CloseConnection();
            }
        }

        private void BtnMdfFac_Click(object sender, RoutedEventArgs e)
        {
            //On recherche à quel indice de la collection se trouve l'object selectionné dans le datagrid
            int indice = cFac.IndexOf((facture)DataGridFacture.SelectedItem);
            if (indice >= 0)
            {
                Client idclient = (Client)CbxCliFac.SelectedItem;
                int idclient2 = idclient.NumClient;

                // On change les propritétés de l'objet à l'indice trouvé. On ne change pas le numéro de magazine via l'interface, trop de risques d'erreurs en base de données
                cFac.ElementAt(indice).DateFac = TxtPrenomCli.Text;
                var parsedDate = DateTime.Parse(TxtDateFac.Text);
                string date = parsedDate.ToString("yyyy-MM-dd");
                bdd.UpdateFacture(Convert.ToInt32(TxtIdFac.Text), idclient2, date);
                cFac = bdd.SelectFct(listecli);
                DataGridFacture.ItemsSource = cFac;
                DataGridFacture.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une facture");
            }

        }
        #endregion

        #region Filtre

        private void TxtFiltreCli_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataGridClient.Items.Refresh();
            listecliTri = bdd.SelectClientLike(TxtFiltreCli.Text);
            DataGridClient.ItemsSource = listecliTri;
        }

        private void TxtFiltreProd_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataGridPrix.Items.Refresh();
            listeprodTri = bdd.SelectProduitLike(TxtFiltreProd.Text);
            DataGridPrix.ItemsSource = listeprodTri;
        }
        private void TxtFiltreRdv_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataGrisRdv.Items.Refresh();
            listerdvTri = bdd.SelectRdvLike(TxtFiltreRdv.Text, listecli, listecom);
            DataGrisRdv.ItemsSource = listerdvTri;
        }

        #endregion

        private void BtnDeco_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            Close();
        }
    }
}
