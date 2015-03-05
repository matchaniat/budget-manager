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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
namespace Budget_Manager
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            CreationBase();
            InitializeComponent();
            RecupData();
            
        }
        public void CreationBase()
        {
            if(!(System.IO.File.Exists(@"BudgetManager.sqlite")))
            {
                SQLiteConnection.CreateFile("BudgetManager.sqlite");
                SQLiteConnection m_dbConnection;
                m_dbConnection = new SQLiteConnection("Data Source=BudgetManager.sqlite;Version=3;");
                m_dbConnection.Open();
                string sql = "CREATE TABLE comptes (name VARCHAR(20),numero VARCHAR(20),banque VARCHAR(20), solde decimal)";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
                string sql2 = "CREATE TABLE cat_dep (name VARCHAR(20))";
                SQLiteCommand command2 = new SQLiteCommand(sql2, m_dbConnection);
                command2.ExecuteNonQuery();
                string sql3 = "CREATE TABLE cat_rev (name VARCHAR(20))";
                SQLiteCommand command3 = new SQLiteCommand(sql3, m_dbConnection);
                command3.ExecuteNonQuery();
            }
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
            List<string> numeros = new List<string>();
            List<string> banques = new List<string>();
            List<decimal> soldes = new List<decimal>();
            while (reader.Read())
            {
                comptes.Add(Convert.ToString(reader["name"]));
                numeros.Add(Convert.ToString(reader["numero"]));
                banques.Add(Convert.ToString(reader["banque"]));
                soldes.Add(Convert.ToDecimal(reader["solde"]));
            }
            Comptes.ItemsSource= comptes;
            List<Comptes> Listecomptes = new List<Comptes>();

            for (int i = 0; i < comptes.Count; i++)
            {
                Listecomptes.Add(new Comptes() { Compte = comptes[i], Numero = numeros[i], Banque = banques[i], Solde = soldes[i] });
            }
            ListeComptes.ItemsSource = Listecomptes;
        }
        private void AjoutCompte(object sender, RoutedEventArgs e)
        {
            var ajoutcmt = new Budget_Manager.Ajoutcompte();
            ajoutcmt.Show();
        }

        private void activated(object sender, EventArgs e)
        {
            RecupData();
            rafraitable();
        }
        private void Apropos(object sender, RoutedEventArgs e)
        {
            var apropos = new Budget_Manager.Apropos();
            apropos.Show();
        }

        private void Quit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void SuppCompte(object sender, RoutedEventArgs e)
        {
            var suppcmt = new Budget_Manager.Window1();
            suppcmt.Show();           
        }

        private void menu(object sender, SelectionChangedEventArgs e)
        {
            string encours = (string)Comptes.SelectedItem;
            titre.Content = encours;
            ListeComptes.Visibility=Visibility.Collapsed;
            Operation.Visibility = Visibility.Visible;
            solde.Visibility = Visibility.Visible;
            mont.Visibility = Visibility.Visible;
            btnaccueil.Opacity = 1;
            btnaccueil.IsEnabled = true;
            btnplus.Opacity = 1;
            btnplus.IsEnabled = true;
            btnmoins.Opacity = 1;
            btnmoins.IsEnabled = true;
            rafraitable();
        }
        private void rafraitable()
        {
            if ((string)Comptes.SelectedItem !=null)
            {
                string encours = (string)Comptes.SelectedItem;
                SQLiteConnection m_dbConnection;
                m_dbConnection = new SQLiteConnection("Data Source=BudgetManager.sqlite;Version=3;");
                m_dbConnection.Open();
                string sql = "select * from '" + encours + "' order by date desc";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                List<string> label = new List<string>();
                List<string> dates = new List<string>();
                List<string> cat = new List<string>();
                List<decimal> montant = new List<decimal>();
                while (reader.Read())
                {
                    label.Add(Convert.ToString(reader["label"]));
                    dates.Add(Convert.ToString(reader["date"]));
                    cat.Add(Convert.ToString(reader["categorie"]));
                    montant.Add(Convert.ToDecimal(reader["montant"]));
                }
                List<Comptes> Operations = new List<Comptes>();
                for (int i = 0; i < label.Count; i++)
                {
                    dates[i] = dates[i].Replace(" 00:00:00", "");
                    Operations.Add(new Comptes() { Label = label[i], Date = dates[i], Categorie = cat[i], Montant = montant[i] });
                }
                Operation.ItemsSource = Operations;
                string sql2 = "select * from comptes where name='"+encours+"'";
                SQLiteCommand command2 = new SQLiteCommand(sql2, m_dbConnection);
                SQLiteDataReader reader2 = command2.ExecuteReader();
                decimal solde=0;
                while (reader2.Read())
                {
                    solde = Convert.ToDecimal(reader2["solde"]);
                }
                mont.Content = Convert.ToString(solde)+"€";
            }
        }

        private void accueil(object sender, RoutedEventArgs e)
        {
            Comptes.SelectedItem = null;
            titre.Content = "Comptes";
            ListeComptes.Visibility = Visibility.Visible;
            Operation.Visibility = Visibility.Collapsed;
            solde.Visibility = Visibility.Collapsed;
            mont.Visibility = Visibility.Collapsed;
            Comptes.SelectedItem = null;
            btnaccueil.Opacity = 0.5;
            btnaccueil.IsEnabled = false;
            btnplus.Opacity = 0.5;
            btnplus.IsEnabled = false;
            btnmoins.Opacity = 0.5;
            btnmoins.IsEnabled = false;
            RecupData();
        }
        private void ajoutope(object sender, RoutedEventArgs e)
        {
            var ajoutope = new Budget_Manager.Ajoutoperation();
            ajoutope.Show();
        }

        private void gerecat(object sender, RoutedEventArgs e)
        {
            var gerecat = new Budget_Manager.GererCat();
            gerecat.Show();
        }

        private void supprope(object sender, RoutedEventArgs e)
        {
            var suppope = new Budget_Manager.Suppoperation();
            suppope.Show();
        }
    }
    public class Comptes
    {
        public string Compte { get; set; }

        public string Numero { get; set; }

        public string Banque { get; set; }

        public decimal Solde { get; set; }

        public string Label { get; set; }

        public string Date { get; set; }

        public string Categorie { get; set; }

        public decimal Montant { get; set; }
    }
}
