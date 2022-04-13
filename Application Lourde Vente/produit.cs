using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Lourde_Vente
{
    public class produit
    {
        #region Champs
        private int _idProd;
        private string _typeprod;
        private string __nomProd;
        private double _prixProd;
        private string _descripProd;
        
        #endregion

        #region Constructeur
        public produit(int num, string typeprod,string nom, double prix, string descrip)
        {
            _idProd = num;
            _typeprod = typeprod;
            __nomProd = nom;
            _prixProd = prix;
            _descripProd = descrip;
            
        }
        #endregion


        #region Accesseurs/Mutateurs
        public int IdProd
        {
            get { return _idProd; }
            set { _idProd = value; }
        }

        public string TypeProd
        {
            get { return _typeprod; }
            set { _typeprod = value; }
        }

        public string NomProd
        {
            get { return __nomProd; }
            set { __nomProd = value; }
        }

        public double PrixProd
        {
            get { return _prixProd; }
            set { _prixProd = value; }
        }

        public string DescripProd
        {
            get { return _descripProd; }
            set { _descripProd = value; }
        }

        #endregion


        #region Methodes
        override public string ToString()
        {
            return __nomProd;



        }
        #endregion
    }
}
