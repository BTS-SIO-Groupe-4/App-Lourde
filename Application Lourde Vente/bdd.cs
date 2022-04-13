using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Application_Lourde_Vente
{
    public class bdd
    {
        //public static List<int> IdRdv = new List<int>();
        private static MySqlConnection connection;
        private static string server;
        private static string database;
        private static string uid;
        private static string password;
        public static string idCo;
        public static string PostCo;


        #region Initialize
        //Initialisation des valeurs
        public static void Initialize()
        {
            server = "localhost";
            database = "infotools";
            uid = "root";
            password = "root";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            //verifier le connectionString

            connection = new MySqlConnection(connectionString);
        }
        #endregion

        #region OpenConnection
        //open connection to database
        public static bool OpenConnection()
        {
            if (connection is null)
            {
                Initialize();
            }
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                Console.WriteLine("Erreur connexion BDD");
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }
        #endregion

        #region CloseConnection
        //Close connection
        public static bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        #endregion

        #region VerifConnexion
        public static bool VerifConnexion(string mail, string mdp)
        {
            //Requête Insertion Magazine
            string query = "Select * from commerciaux where MailEmp = '" + mail + "'  and MdpEmp = '" + mdp + "'";
            Console.WriteLine(query);

            //open connection
            if (bdd.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    idCo = dataSet.Tables[0].Rows[0]["IdEmp"].ToString();
                    PostCo = dataSet.Tables[0].Rows[0]["PostEmp"].ToString();
                    string username = dataSet.Tables[0].Rows[0]["PrenomEmp"].ToString() + " " + dataSet.Tables[0].Rows[0]["NomEmp"].ToString();
                                    //close connection
                    bdd.CloseConnection();   
                    return true;

                }
                else
                {
                    //close connection
                    bdd.CloseConnection();
                    return false;
                }          
            }
            else
                return false;
        }
        #endregion

        #region Client

        public static void InsertClient(string nomC, string prenomC, string numTelC, string mailC, string villeC, string nrC, string CPC, int Pros)
        {
            //Requête Insertion Pigiste
            string query = "INSERT INTO client (NomCli, PrenomCli, NumCli, MailCli, VilleCli, NumRueCli, CPCli, prospect) VALUES('" + nomC + "','" + prenomC + "','" + numTelC + "','" + mailC + "','" + villeC + "','" + nrC + "','" + CPC + "','" + Pros + "')";
            Console.WriteLine(query);

            //open connection
            if (bdd.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                bdd.CloseConnection();
            }
        }

        public static void DeleteClient(int numC)
        {
            //Delete Pigiste
            string query = "DELETE FROM client WHERE IdCli=" + numC;

            if (bdd.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                bdd.CloseConnection();
            }
        }

        public static void UpdateClient(int numC, string nomC, string prenomC, string numTelC, string mailC, string villeC, string nrC, string CPC, int Pros)
        {
            string query = "UPDATE client SET NomCli='" + nomC + "', PrenomCli='" + prenomC + "', NumCli='" + numTelC + "', MailCli = '" + mailC + "', VilleCli = '" + villeC + "', NumRueCli = '" + nrC + "', CPCli = '" + CPC + "', prospect = '" + Pros + "' WHERE IdCli=" + numC;
            Console.WriteLine(query);
            //Open connection
            if (bdd.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                bdd.CloseConnection();
            }
        }

        public static List<Client> SelectClient()
        {
            //Select statement
            string query = "SELECT * FROM client";

            //Create a list to store the result
            List<Client> dbClient = new List<Client>();

            //Ouverture connection
            if (bdd.OpenConnection() == true)
            {
                //Creation Command MySQL
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Création d'un DataReader et execution de la commande
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Lecture des données et stockage dans la collection
                while (dataReader.Read())
                {
                    Client leClient = new Client(Convert.ToInt16(dataReader["IdCli"]), Convert.ToString(dataReader["NomCli"]), Convert.ToString(dataReader["PrenomCli"]), Convert.ToInt32(dataReader["NumCli"]), Convert.ToString(dataReader["MailCli"]), Convert.ToString(dataReader["VilleCli"]), Convert.ToString(dataReader["NumRueCli"]), Convert.ToString(dataReader["CPCli"]), Convert.ToInt32(dataReader["prospect"]));
                    dbClient.Add(leClient);
                }

                //fermeture du Data Reader
                dataReader.Close();

                //fermeture Connection
                bdd.CloseConnection();

                //retour de la collection pour être affichée

            }

            return dbClient;
        }

        #endregion

        #region Rdv

        public static void InsertRdv(int leClient, int leCommerciale, string DateRdv)
        {
            //Requête Insertion Pigiste
            string query = "INSERT INTO rdv (IdCli, IdCom, DateRdv) VALUES('" + leClient + "','" + leCommerciale + "','" + DateRdv + "')";
            Console.WriteLine(query);

            //open connection
            if (bdd.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                bdd.CloseConnection();
            }
        }

        public static void DeleteRdv(int numRdv)
        {
            //Delete Pigiste
            string query = "DELETE FROM rdv WHERE IdRdv=" + numRdv;

            if (bdd.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                bdd.CloseConnection();
            }
        }

        public static void UpdateRdv(int numRdv, int leClient, int leCommerciale, string DateRdv)
        {
            string query = "UPDATE rdv SET idCli='" + leClient + "', idCom='" + leCommerciale + "', DateRdv='" + DateRdv + "' WHERE IdRdv=" + numRdv;
            Console.WriteLine(query);
            //Open connection
            if (bdd.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                bdd.CloseConnection();
            }
        }

        public static List<rdv> SelectRdv(List<Client> cli, List<Commerciale> emp)
        {
            string query = "";
            Console.WriteLine("vous etes bien connecter a la base de donnee");
            //Select statement
            if (PostCo == "0")
            {
                query = "SELECT * FROM rdv Where IdCom = " + idCo;
            }
            else
            {
                if(PostCo == "1")
                {
                    query = "SELECT * FROM rdv";
                }
            }
            

            //Create a list to store the result
            List<rdv> dbRdv = new List<rdv>();

            //Ouverture connection
            if (bdd.OpenConnection() == true)
            {
                //Creation Command MySQL
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Création d'un DataReader et execution de la commande
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Lecture des données et stockage dans la collection
                while (dataReader.Read())
                {
                    var numcli = Convert.ToInt32(dataReader["IdCli"]);
                    var numcli1 = cli.Find(x => x.NumClient == numcli);
                    var numemp = Convert.ToInt32(dataReader["IdCom"]);
                    var numemp1 = emp.Find(y => y.NumCom == numemp);
                    
                    rdv leRdv = new rdv(Convert.ToInt32(dataReader["IdRdv"]), numcli1, numemp1, Convert.ToString(dataReader["DateRdv"]));
                    dbRdv.Add(leRdv);
                    //IdRdv.Add(_idRdv);
                }

                //fermeture du Data Reader
                dataReader.Close();

                //fermeture Connection
                bdd.CloseConnection();

                //retour de la collection pour être affichée
            }
            return dbRdv;
        }

        #endregion

        #region Prix/produit

        public static void InsertProduit(string typeP,double prixP, string descripP, string nomP)
        {
            //Requête Insertion Pigiste
            string query = "INSERT INTO produit (TypeProd, PrixProd, NomProd, DescripProd) VALUES('" + typeP + "','" + prixP + "','" + nomP + "','" + descripP + "')";
            Console.WriteLine(query);

            //open connection
            if (bdd.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                bdd.CloseConnection();
            }
        }

        public static void DeleteProduit(int numP)
        {
            //Delete Pigiste
            string query = "DELETE FROM produit WHERE IdProd=" + numP;

            if (bdd.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                bdd.CloseConnection();
            }
        }

        public static void UpdateProduit(int numP, string typeP,double prixP, string descripP, string nomP)
        {
            string query = "UPDATE produit SET TypeProd='" + typeP + "', PrixProd='" + prixP + "', DescripProd='" + descripP + "', NomProd = '" + nomP + "' WHERE IdProd=" + numP;
            Console.WriteLine(query);
            //Open connection
            if (bdd.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                bdd.CloseConnection();
            }
        }

        public static List<produit> SelectProduit()
        {
            //Select statement
            string query = "SELECT * FROM produit";

            //Create a list to store the result
            List<produit> dbProduit = new List<produit>();

            //Ouverture connection
            if (bdd.OpenConnection() == true)
            {
                //Creation Command MySQL
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Création d'un DataReader et execution de la commande
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Lecture des données et stockage dans la collection
                while (dataReader.Read())
                {
                    produit leProduit = new produit(Convert.ToInt16(dataReader["IdProd"]), Convert.ToString(dataReader["TypeProd"]), Convert.ToString(dataReader["NomProd"]), Convert.ToInt16(dataReader["PrixProd"]), Convert.ToString(dataReader["DescripProd"]));
                    dbProduit.Add(leProduit);
                }

                //fermeture du Data Reader
                dataReader.Close();

                //fermeture Connection
                bdd.CloseConnection();

                //retour de la collection pour être affichée

            }

            return dbProduit;
        }

        #endregion

        #region Select
        public static List<Client> SelectClient(int id)
        {
            //Select statement
            string query = "SELECT * FROM client Where IdCli = '" + id + "'";

            //Create a list to store the result
            List<Client> dbClient = new List<Client>();

            //Ouverture connection
            if (bdd.OpenConnection() == true)
            {
                //Creation Command MySQL
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Création d'un DataReader et execution de la commande
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Lecture des données et stockage dans la collection
                while (dataReader.Read())
                {
                    Client leClient = new Client(Convert.ToInt16(dataReader["IdCli"]), Convert.ToString(dataReader["NomCli"]), Convert.ToString(dataReader["PrenomCli"]), Convert.ToInt32(dataReader["NumCli"]), Convert.ToString(dataReader["MailCli"]), Convert.ToString(dataReader["VilleCli"]), Convert.ToString(dataReader["NumRueCli"]), Convert.ToString(dataReader["CPCli"]), Convert.ToInt32(dataReader["prospect"]));
                    dbClient.Add(leClient);
                }

                //fermeture du Data Reader
                dataReader.Close();

                //fermeture Connection
                bdd.CloseConnection();

                //retour de la collection pour être affichée

            }

            return dbClient;
        }


        public static List<Commerciale> SelectCommerciale(int id)
        {
            //Select statement
            string query = "SELECT * FROM commerciaux Where IdCom = '" + id + "'";

            //Create a list to store the result
            List<Commerciale> dbCom = new List<Commerciale>();

            //Ouverture connection
            if (bdd.OpenConnection() == true)
            {
                //Creation Command MySQL
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Création d'un DataReader et execution de la commande
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Lecture des données et stockage dans la collection
                while (dataReader.Read())
                {
                    Commerciale leCom = new Commerciale(Convert.ToInt16(dataReader["IdCom"]), Convert.ToString(dataReader["NomCom"]), Convert.ToString(dataReader["PrenomCom"]), Convert.ToInt32(dataReader["TelCom"]), Convert.ToString(dataReader["MailCom"]), Convert.ToString(dataReader["MdpCom"]), Convert.ToInt16(dataReader["PostEmp"]));
                    dbCom.Add(leCom);
                }

                //fermeture du Data Reader
                dataReader.Close();

                //fermeture Connection
                bdd.CloseConnection();

                //retour de la collection pour être affichée

            }

            return dbCom;
        }

        public static List<Commerciale> SelectCommerciale()
        {
            //Select statement
            string query = "SELECT * FROM commerciaux";

            //Create a list to store the result
            List<Commerciale> dbCom = new List<Commerciale>();

            //Ouverture connection
            if (bdd.OpenConnection() == true)
            {
                //Creation Command MySQL
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Création d'un DataReader et execution de la commande
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Lecture des données et stockage dans la collection
                while (dataReader.Read())
                {
                    Commerciale leCom = new Commerciale(Convert.ToInt16(dataReader["IdEmp"]), Convert.ToString(dataReader["NomEmp"]), Convert.ToString(dataReader["PrenomEmp"]), Convert.ToInt32(dataReader["TelEmp"]), Convert.ToString(dataReader["MailEmp"]), Convert.ToString(dataReader["MdpEmp"]), Convert.ToInt16(dataReader["PostEmp"]));
                    dbCom.Add(leCom);
                }

                //fermeture du Data Reader
                dataReader.Close();

                //fermeture Connection
                bdd.CloseConnection();

                //retour de la collection pour être affichée

            }

            return dbCom;
        }
        #endregion

        #region Facture
        public static List<facture> SelectFct(List<Client> clif)
        {
            Console.WriteLine("vous etes bien connecter a la base de donnee");
            //Select statement
            string query = "SELECT * FROM facture";

            //Create a list to store the result
            List<facture> dbFac = new List<facture>();

            //Ouverture connection
            if (bdd.OpenConnection() == true)
            {
                //Creation Command MySQL
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Création d'un DataReader et execution de la commande
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Lecture des données et stockage dans la collection
                while (dataReader.Read())
                {
                    var numclif = Convert.ToInt32(dataReader["IdCli"]);
                    var numcli1f = clif.Find(xf=> xf.NumClient == numclif);
                    
                    facture laFac = new facture(Convert.ToInt32(dataReader["IdFac"]), numcli1f, Convert.ToString(dataReader["DateFac"]));
                    dbFac.Add(laFac);
                }

                //fermeture du Data Reader
                dataReader.Close();

                //fermeture Connection
                bdd.CloseConnection();

                //retour de la collection pour être affichée
            }
            return dbFac;
        }
        public static void DeleteFac(int numF)
        {
            //Delete Pigiste
            string query = "DELETE FROM facture WHERE IdFac=" + numF;

            if (bdd.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                bdd.CloseConnection();
            }
        }

        public static void InsertFacture(int idcli, string datefac)
        {
            //Requête Insertion Pigiste
            Console.WriteLine(datefac);
            string query = "INSERT INTO facture (IdCli, DateFac) VALUES('" + idcli + "','" + datefac + "')";
            Console.WriteLine(query);

            //open connection
            if (bdd.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                bdd.CloseConnection();
            }
        }

        public static void UpdateFacture(int numF, int cliF, string DateF)
        {
            string query = "UPDATE facture SET IdCli='" + cliF + "', DateFac='" + DateF + "' WHERE IdFac=" + numF;
            Console.WriteLine(query);
            //Open connection
            if (bdd.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                bdd.CloseConnection();
            }
        }

        #endregion

        #region Filtre

        public static List<Client> SelectClientLike(string namee)
        {
            //Select statement
            string query = "SELECT * FROM client Where NomCli like '" + namee + "%'";

            //Create a list to store the result
            List<Client> dbClientTri = new List<Client>();

            //Ouverture connection
            if (bdd.OpenConnection() == true)
            {
                //Creation Command MySQL
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Création d'un DataReader et execution de la commande
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Lecture des données et stockage dans la collection
                while (dataReader.Read())
                {

                    Client leClientTri = new Client(Convert.ToInt32(dataReader["IdCli"]), Convert.ToString(dataReader["NomCli"]), Convert.ToString(dataReader["PrenomCli"]), Convert.ToInt32(dataReader["NumCli"]), Convert.ToString(dataReader["MailCli"]), Convert.ToString(dataReader["VilleCli"]), Convert.ToString(dataReader["NumRueCli"]), Convert.ToString(dataReader["CPCli"]), Convert.ToInt32(dataReader["prospect"]));
                    dbClientTri.Add(leClientTri);
                }

                //fermeture du Data Reader
                dataReader.Close();

                //fermeture Connection
                bdd.CloseConnection();

                //retour de la collection pour être affichée

            }

            return dbClientTri;
        }

        public static List<produit> SelectProduitLike(string namee)
        {
            //Select statement
            string query = "SELECT * FROM produit Where NomProd like '" + namee + "%'";

            //Create a list to store the result
            List<produit> dbProduitTri = new List<produit>();

            //Ouverture connection
            if (bdd.OpenConnection() == true)
            {
                //Creation Command MySQL
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Création d'un DataReader et execution de la commande
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Lecture des données et stockage dans la collection
                while (dataReader.Read())
                {
                    produit leProduitTri = new produit(Convert.ToInt32(dataReader["IdProd"]), Convert.ToString(dataReader["TypeProd"]), Convert.ToString(dataReader["NomProd"]), Convert.ToDouble(dataReader["PrixProd"]), Convert.ToString(dataReader["DescripProd"]));
                    dbProduitTri.Add(leProduitTri);
                }

                //fermeture du Data Reader
                dataReader.Close();

                //fermeture Connection
                bdd.CloseConnection();

                //retour de la collection pour être affichée

            }

            return dbProduitTri;
        }

        public static List<rdv> SelectRdvLike(string namee, List<Client> cli, List<Commerciale> emp)
        {
            //Select statement
            string query = "SELECT * from rdv JOIN client on rdv.IdCli = client.IdCli where client.NomCli LIKE '" + namee + "%'";

            //Create a list to store the result
            List<rdv> dbRdvTri = new List<rdv>();

            //Ouverture connection
            if (bdd.OpenConnection() == true)
            {
                //Creation Command MySQL
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Création d'un DataReader et execution de la commande
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Lecture des données et stockage dans la collection
                while (dataReader.Read())
                {
                    var numcli = Convert.ToInt32(dataReader["IdCli"]);
                    var numcli1 = cli.Find(x => x.NumClient == numcli);
                    var numemp = Convert.ToInt32(dataReader["IdCom"]);
                    var numemp1 = emp.Find(y => y.NumCom == numemp);

                    rdv leRdvTri = new rdv(Convert.ToInt32(dataReader["IdRdv"]), numcli1, numemp1, Convert.ToString(dataReader["DateRdv"]));
                    dbRdvTri.Add(leRdvTri);
                }

                //fermeture du Data Reader
                dataReader.Close();

                //fermeture Connection
                bdd.CloseConnection();

                //retour de la collection pour être affichée

            }

            return dbRdvTri;
        }

        #endregion

    }
}
