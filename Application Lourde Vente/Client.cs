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
    public class Client
    {
        #region Champs
        private int _numClient;
        private string _nomClient;
        private string _prenomClient;
        private string _cpClient;
        private string _villeClient;
        private string _mailClient;
        private int _numTelClient;
        private string _numrueClient;
        private int _prospect;



        #endregion

        #region Constructeur
        public Client(int n, string nm, string p, int t, string m, string v, string nr, string cp, int pro)
        {
            _numClient = n;
            _nomClient = nm;
            _prenomClient = p;
            _numTelClient = t;
            _mailClient = m;
            _villeClient = v;
            _numrueClient = nr;
            _cpClient = cp;
            _prospect = pro;
        }
        #endregion


        #region Accesseurs/Mutateurs
        public int NumClient
        {
            get { return _numClient; }
            set { _numClient = value; }
        }

        public string NomClient
        {
            get { return _nomClient; }
            set { _nomClient = value; }
        }

        public string PrenomClient
        {
            get { return _prenomClient; }
            set { _prenomClient = value; }
        }
        public int NumTelClient
        {
            get { return _numTelClient; }
            set { _numTelClient = value; }
        }

        public string MailClient
        {
            get { return _mailClient; }
            set { _mailClient = value; }
        }

        public string VilleClient
        {
            get { return _villeClient; }
            set { _villeClient = value; }
        }

        public string NumRueClient
        {
            get { return _numrueClient; }
            set { _numrueClient = value; }
        }

        public string CPClient
        {
            get { return _cpClient; }
            set { _cpClient = value; }
        }

        public int Prospect
        {
            get { return _prospect; }
            set { _prospect = value; }
        }
        #endregion


        #region Methodes
        override public string ToString()
        {
            // Méthode ToString() surchargée qui écrase la méthode ToString() de base
            return _nomClient + " " + _prenomClient;
        }
        #endregion

    }
}
