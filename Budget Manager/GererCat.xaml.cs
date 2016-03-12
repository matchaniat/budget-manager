using System;
using System.Collections.Generic;
using System.Windows;
using System.Data.SQLite;

namespace Budget_Manager
{
    /// <summary>
    /// Logique d'interaction pour GererCat.xaml
    /// </summary>
    public partial class GererCat : Window
    {
        public GererCat()
        {
            InitializeComponent();
            RecupData();
        }
        private void RecupData()
        {
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=BudgetManager.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "select * from cat_dep";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<string> depenses = new List<string>();
            while (reader.Read())
            {
                depenses.Add(Convert.ToString(reader["name"]));
            }
            listedepenses.ItemsSource = depenses;
            string sql2 = "select * from cat_rev";
            SQLiteCommand command2 = new SQLiteCommand(sql2, m_dbConnection);
            SQLiteDataReader reader2 = command2.ExecuteReader();
            List<string> revenus = new List<string>();
            while (reader2.Read())
            {
                revenus.Add(Convert.ToString(reader2["name"]));
            }
            listerevenus.ItemsSource = revenus;
            m_dbConnection.Close();
        }
        private void fermer(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void supdep(object sender, RoutedEventArgs e)
        {
            string sel = (string)listedepenses.SelectedItem;
            if (sel!=null)
            {
                if (MessageBox.Show("Etes vous sûr de vouloir supprimer cette catégorie ?",
  "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    SQLiteConnection m_dbConnection;
                    m_dbConnection = new SQLiteConnection("Data Source=BudgetManager.sqlite;Version=3;");
                    m_dbConnection.Open();
                    string sql = "delete from cat_dep where name=@sel";
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                    command.Parameters.AddWithValue("@sel", sel);
                    command.ExecuteNonQuery();
                    RecupData();
                }
            }
        }

        private void suprev(object sender, RoutedEventArgs e)
        {
            string sel = (string)listerevenus.SelectedItem;
            if (sel != null)
            {
                if (MessageBox.Show("Etes vous sûr de vouloir supprimer cette catégorie ?",
  "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    SQLiteConnection m_dbConnection;
                    m_dbConnection = new SQLiteConnection("Data Source=BudgetManager.sqlite;Version=3;");
                    m_dbConnection.Open();
                    string sql = "delete from cat_rev where name=@sel";
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                    command.Parameters.AddWithValue("@sel", sel);
                    command.ExecuteNonQuery();
                    RecupData();
                }
            }
        }

        private void ajoutdep(object sender, RoutedEventArgs e)
        {
            string nomdep = textdep.Text;
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=BudgetManager.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "insert into cat_dep (name) values (@nomdep)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.Parameters.AddWithValue("@nomdep", nomdep);
            command.ExecuteNonQuery();
            RecupData();
            textdep.Text = "";
        }

        private void ajourev(object sender, RoutedEventArgs e)
        {
            string nomrev = textrev.Text;
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=BudgetManager.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "insert into cat_rev (name) values (@nomrev)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.Parameters.AddWithValue("@nomrev", nomrev);
            command.ExecuteNonQuery();
            RecupData();
            textrev.Text = "";
        }

    }
}
