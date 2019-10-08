using ArchivesCar.View;
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

namespace ArchivesCar
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
      
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginControl loginControl = new LoginControl(this);
            this.grid.Children.Add(loginControl);
           
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (PublicData.ServerConfig.connState)
                PublicData.ServerConfig.wirelessRfid.close();
            System.Environment.Exit(0);
        }
    }
}
