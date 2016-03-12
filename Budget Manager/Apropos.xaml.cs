using System.Windows;
using System.Windows.Input;

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
            try { System.Diagnostics.Process.Start("https://github.com/matchaniat/budget-manager"); }
            catch { };
        }
    }
}
