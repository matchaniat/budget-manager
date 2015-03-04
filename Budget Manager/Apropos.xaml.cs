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

namespace Budget_Manager
{
    /// <summary>
    /// Logique d'interaction pour Apropos.xaml
    /// </summary>
    public partial class Apropos : Window
    {
        public Apropos()
        {
            InitializeComponent();
        }

        private void Site(object sender, MouseButtonEventArgs e)
        {
            try { System.Diagnostics.Process.Start("http://budget-manager.com/"); }
            catch { };
        }
    }
}
