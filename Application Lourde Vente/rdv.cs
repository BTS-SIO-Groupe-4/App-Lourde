using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Lourde_Vente
{
    public class rdv
    {
        #region Champs
        private int _numRdv;
        private Client _clientRdv;
        private Commerciale _comRdv;
        private string _dateRdv;
        #endregion

        #region Constructeurs
        public rdv(int n, Client cli, Commerciale com, string dt)
        {
            _numRdv = n;
            _clientRdv = cli;
            _comRdv = com;
            _dateRdv = dt;
        }
        #endregion

        public int NumRdv
        {
            get { return _numRdv; }
            set { _numRdv = value; }
        }

        public Client ClientRdv
        {
            get { return _clientRdv; }
            set { _clientRdv = value; }
        }

        public Commerciale CommercialeRdv
        {
            get { return _comRdv; }
            set { _comRdv = value; }
        }

        public string DateRdv
        {
            get { return _dateRdv; }
            set { _dateRdv = value; }
        }
    }


}
