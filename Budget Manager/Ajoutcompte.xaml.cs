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
    /// Logique d'interaction pour Ajoutcompte.xaml
    /// </summary>
    public partial class Ajoutcompte : Window
    {
        public Ajoutcompte()
        {
            InitializeComponent();
        }

        private void AjoutValid(object sender, RoutedEventArgs e)
        {
            inserertable();
            creertable();
        }

        private void AjoutAnnul(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void inserertable()
        {
            char[] soldestring = soldecmt.Text.ToCharArray();
            for (int i = 0; i < soldecmt.Text.Length; i++)
            {
                if (soldestring[i] == '.')
                {
                    MessageBox.Show("Veuillez remplacer le point par une virgule !");
                }
            }
            try
            {
                SQLiteConnection m_dbConnection;
                m_dbConnection = new SQLiteConnection("Data Source=BudgetManager.sqlite;Version=3;");
                m_dbConnection.Open();
                string nom = nomcmt.Text;
                string numero = numcmt.Text;
                string banque = bank.Text;
                decimal solde = decimal.Parse(soldecmt.Text);
                string sql = "insert into comptes (name,numero,banque,solde) values (@nomcmt,@numero,@banque,@solde)";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.Parameters.AddWithValue("@nomcmt", nom);
                command.Parameters.AddWithValue("@numero", numero);
                command.Parameters.AddWithValue("@banque", banque);
                command.Parameters.AddWithValue("@solde", solde);
                command.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Erreur !");
            }
        }
        private void creertable()
        {
            try
            {
                SQLiteConnection m_dbConnection;
                m_dbConnection = new SQLiteConnection("Data Source=BudgetManager.sqlite;Version=3;");
                m_dbConnection.Open();
                string nom = nomcmt.Text;
                string sql = "create table '" + nom + "'(label varchar(20),date date, categorie varchar(20), montant decimal)";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Erreur !");
            }
            finally
            {
                Close();
            }
        }
    }
}
