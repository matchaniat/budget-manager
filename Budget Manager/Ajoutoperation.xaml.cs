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
using System.Data.SQLite;

namespace Budget_Manager
{
    /// <summary>
    /// Logique d'interaction pour Ajoutoperation.xaml
    /// </summary>
    public partial class Ajoutoperation : Window
    {
        public Ajoutoperation()
        {
            InitializeComponent();
            var main = new Budget_Manager.MainWindow();
            RecupData();
        }

        private void annul(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void RecupData()
        {
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=BudgetManager.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "select * from comptes";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<string> comptes = new List<string>();
            while (reader.Read())
            {
                comptes.Add(Convert.ToString(reader["name"]));
            }
            compte.ItemsSource = comptes;
            string sql2 = "select * from cat_dep";
            SQLiteCommand command2 = new SQLiteCommand(sql2, m_dbConnection);
            SQLiteDataReader reader2 = command2.ExecuteReader();
            List<string> categories = new List<string>();
            categories.Add("Dépenses");
            while (reader2.Read())
            {
                categories.Add("- "+Convert.ToString(reader2["name"]));
            }
            string sql3 = "select * from cat_rev";
            SQLiteCommand command3 = new SQLiteCommand(sql3, m_dbConnection);
            SQLiteDataReader reader3 = command3.ExecuteReader();
            categories.Add("Revenus");
            while (reader3.Read())
            {
                categories.Add("- "+Convert.ToString(reader3["name"]));
            }
            cat.ItemsSource = categories;
            m_dbConnection.Close();
        }

        private void valider(object sender, RoutedEventArgs e)
        {
            string cmptsel = compte.Text;
            string lib = libelle.Text;
            DateTime da = DateTime.Parse(date.Text);
            string categorie = cat.Text;
            string ty = type.Text;
            decimal mont = decimal.Parse(montant.Text);
            if (ty=="Débit")
            {
                mont=-mont;
            }
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=BudgetManager.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "insert into '"+cmptsel+"' (label,date,categorie,montant) values (@label,@date,@cat,@montant)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.Parameters.AddWithValue("@label", lib);
            command.Parameters.AddWithValue("@date", da);
            command.Parameters.AddWithValue("@cat", categorie);
            command.Parameters.AddWithValue("@montant", mont);
            command.ExecuteNonQuery();
            calculsolde();
            Close();
        }
        private void calculsolde()
        {
            string cmptsel = compte.Text;
            decimal mont = decimal.Parse(montant.Text);
            string ty = type.Text;
            if (ty == "Débit")
            {
                mont = -mont;
            }
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=BudgetManager.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "select * from comptes where name='" + cmptsel+"'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            decimal solde=0;
            while (reader.Read())
            {
                solde = (Convert.ToDecimal(reader["solde"]));
            }
            decimal nvsolde = solde+mont;
            string sql2 = "update comptes set solde=@nvsolde where name ='" + cmptsel+"'";
            SQLiteCommand command2 = new SQLiteCommand(sql2, m_dbConnection);
            command2.Parameters.AddWithValue("@nvsolde", nvsolde);
            command2.ExecuteNonQuery();
        }
    }
}
