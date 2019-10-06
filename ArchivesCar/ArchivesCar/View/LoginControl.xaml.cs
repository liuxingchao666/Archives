using ArchivesCar.BLL;
using ArchivesCar.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ArchivesCar.View
{
    /// <summary>
    /// LoginControl.xaml 的交互逻辑
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public LoginControl(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = null;
            DataContext = new LoginViewModel(mainWindow);
        }

        private void PassWord_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Regex re = new Regex("[^0-9.-]+");
            //e.Handled = re.IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            wirelessRfid wirelessRfid = new wirelessRfid();
            if (wirelessRfid.conn())
            {
                int i = 0;
            }
        }
    }
}
