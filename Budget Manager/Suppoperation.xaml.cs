using System;
using System.Collections.Generic;
using System.Windows;
using System.Data.SQLite;

namespace Budget_Manager
{
    /// <summary>
    /// Logique d'interaction pour Suppoperation.xaml
    /// </summary>
    public partial class Suppoperation : Window
    {
        public Suppoperation()
        {
            InitializeComponent();
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
            nomcmt.ItemsSource = comptes;
            m_dbConnection.Close();
        }

        private void valid(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Etes vous sûr de vouloir supprimer cette opération ?",
  "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    SQLiteConnection m_dbConnection;
                    m_dbConnection = new SQLiteConnection("Data Source=BudgetManager.sqlite;Version=3;");
                    m_dbConnection.Open();
                    string nom = nomcmt.Text;
                    string lib = libelle.Text;
                    DateTime da = DateTime.Parse(date.Text);
                    decimal mont = decimal.Parse(montant.Text);
                    string sql0 = "select * from '" + nom + "' where label=@label and date=@date and montant=@montant";
                    SQLiteCommand command0 = new SQLiteCommand(sql0, m_dbConnection);
                    command0.Parameters.AddWithValue("@label", lib);
                    command0.Parameters.AddWithValue("@date", da);
                    command0.Parameters.AddWithValue("@montant", mont);
                    SQLiteDataReader reader0 = command0.ExecuteReader();
                    string ok=null;
                    while (reader0.Read())
                    {
                        ok = (Convert.ToString(reader0["label"]));
                    }
                    if (ok != null)
                    {
                        string sql = "delete from '" + nom + "' where label=@label and date=@date and montant=@montant";
                        SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                        command.Parameters.AddWithValue("@label", lib);
                        command.Parameters.AddWithValue("@date", da);
                        command.Parameters.AddWithValue("@montant", mont);
                        command.ExecuteNonQuery();
                        string sql2 = "select * from comptes where name='" + nom + "'";
                        SQLiteCommand command2 = new SQLiteCommand(sql2, m_dbConnection);
                        SQLiteDataReader reader = command2.ExecuteReader();
                        decimal solde = 0;
                        while (reader.Read())
                        {
                            solde = (Convert.ToDecimal(reader["solde"]));
                        }
                        decimal nvsolde = solde - mont;
                        string sql3 = "update comptes set solde=@nvsolde where name ='" + nom + "'";
                        SQLiteCommand command3 = new SQLiteCommand(sql3, m_dbConnection);
                        command3.Parameters.AddWithValue("@nvsolde", nvsolde);
                        command3.ExecuteNonQuery();
                        Close();
                    }
                    else { MessageBox.Show("Pas d'opération trouvée !"); }
                }
                catch
                {
                    MessageBox.Show("Erreur !");
                    Close();
                }
            }
            else { Close(); }
        }

        private void libel(object sender, RoutedEventArgs e)
        {
            string encours = nomcmt.Text;
            if (encours != "")
            {
                SQLiteConnection m_dbConnection;
                m_dbConnection = new SQLiteConnection("Data Source=BudgetManager.sqlite;Version=3;");
                m_dbConnection.Open();
                string sql = "select * from '" + encours + "' order by date desc";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                List<string> label = new List<string>();
                while (reader.Read())
                {
                    label.Add(Convert.ToString(reader["label"]));
                }
                libelle.ItemsSource = label;
            }
        }

        private void remplir(object sender, RoutedEventArgs e)
        {
            string encours = nomcmt.Text;
            string label = libelle.Text;
            if (label != "" && encours != "")
            {
                SQLiteConnection m_dbConnection;
                m_dbConnection = new SQLiteConnection("Data Source=BudgetManager.sqlite;Version=3;");
                m_dbConnection.Open();
                string sql = "select * from '" + encours + "' where label=@label";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.Parameters.AddWithValue("@label", label);
                SQLiteDataReader reader = command.ExecuteReader();
                string dates = "";
                decimal mont = 0;
                while (reader.Read())
                {
                    dates=Convert.ToString(reader["date"]);
                    mont = Convert.ToDecimal(reader["montant"]);
                }
                dates = dates.Replace(" 00:00:00", "");
                date.Text = dates;
                montant.Text = Convert.ToString(mont).Replace(".", ",");
            }
        }

    }
}
