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
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            RecupData();
        }
        public void RecupData()
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
            nomcmt.ItemsSource = comptes;
        }
        private void Annul(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Valid(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Etes vous sûr de vouloir supprimer ce compte ?",
  "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

                SQLiteConnection m_dbConnection;
                m_dbConnection = new SQLiteConnection("Data Source=BudgetManager.sqlite;Version=3;");
                m_dbConnection.Open();
                string nom = nomcmt.Text;
                string sql = "delete from comptes where name=@nomcmt";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.Parameters.AddWithValue("@nomcmt", nom);
                command.ExecuteNonQuery();
                supptable();
                Close();
            }
            else { Close(); }
        }
        private void supptable()
        {
                try
                {
                    SQLiteConnection m_dbConnection;
                    m_dbConnection = new SQLiteConnection("Data Source=BudgetManager.sqlite;Version=3;");
                    m_dbConnection.Open();
                    string nom = nomcmt.Text;
                    string sql = "drop table '" + nom + "'";
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                    Close();
                }
                catch
                {
                    MessageBox.Show("Erreur !");
                }
            
        }
    }
}
