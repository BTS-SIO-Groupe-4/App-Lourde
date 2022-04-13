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
    public class Connexion
    {
        public static bool ToDo;
        Main main = new Main();
        public static void TestConnexion(string email, string password)
        {
            string query = "Select * from commerciaux where MailCom='" + email + "'  and MdpCom='" + password + "'";
            Console.WriteLine(query);

            //open connection
            if (bdd.OpenConnection() == true)
            {
                bdd.OpenConnection();
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query);
                cmd.CommandType = CommandType.Text;
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    string username = dataSet.Tables[0].Rows[0]["FirstName"].ToString() + " " + dataSet.Tables[0].Rows[0]["LastName"].ToString();
                    ToDo = true;
                }
                else
                {
                    ToDo = false;
                }

                //close connection
                bdd.CloseConnection();
            }
        }
    }
}
