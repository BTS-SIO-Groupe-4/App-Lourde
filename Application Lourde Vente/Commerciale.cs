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
    public class Commerciale
    {
        #region Champs
        private int _numCom;
        private string _nomCom;
        private string _prenomCom;
        private int _telCom;
        private string _mailCom;
        private string _mdpCom;
        private int _postemp;



        #endregion

        #region Constructeur
        public Commerciale(int id, string nm, string pre, int tel, string mail, string mdp, int postemp)
        {
            _numCom = id;
            _nomCom = nm;
            _prenomCom = pre;
            _telCom = tel;
            _mailCom = mail;
            _mdpCom = mdp;
            _postemp = postemp;
        }
        #endregion


        #region Accesseurs/Mutateurs
        public int NumCom
        {
            get { return _numCom; }
            set { _numCom = value; }
        }

        public string NomCom
        {
            get { return _nomCom; }
            set { _nomCom = value; }
        }

        public string PrenomCom
        {
            get { return _prenomCom; }
            set { _prenomCom = value; }
        }

        public int TelCom
        {
            get { return _telCom; }
            set { _telCom = value; }
        }
        public string MailCom
        {
            get { return _mailCom; }
            set { _mailCom = value; }
        }



        public string MdpCom
        {
            get { return _mdpCom; }
            set { _mdpCom = value; }
        }

        public int PostEmp
        {
            get { return _postemp; }
            set { _postemp = value; }
        }


        #endregion


        #region Methodes
        override public string ToString()
        {
            // Méthode ToString() surchargée qui écrase la méthode ToString() de base
            return _nomCom + " " + _prenomCom;
        }
        #endregion

    }
}
