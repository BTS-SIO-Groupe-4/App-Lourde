using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Lourde_Vente
{
    public class facture
    {
        #region Champs

        private int _numFacture;
        private Client _numClient;
        private string _datefac;

        #endregion

        #region Constructeur
        public facture(int numF, Client c, string datefac)
        {
            _numFacture = numF;
            _numClient = c;
            _datefac = datefac;
            
        }
        #endregion


        #region Accesseurs/Mutateurs
        public int NumFacture
        {
            get { return _numFacture; }
            set { _numFacture = value; }
        }

        public Client ClientFacture
        {
            get { return _numClient; }
            set { _numClient = value; }
        }

        public string DateFac
        {
            get { return _datefac; }
            set { _datefac = value; }
        }
        #endregion
    }
}

