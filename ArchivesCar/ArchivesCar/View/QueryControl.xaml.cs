﻿using ArchivesCar.ViewModel;
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

namespace ArchivesCar.View
{
    /// <summary>
    /// QueryControl.xaml 的交互逻辑
    /// </summary>
    public partial class QueryControl : UserControl
    {
        public QueryControl(MainWindow mainWindow)
        {
            InitializeComponent();
           // DataContext = null;
            DataContext = new QueryViewModel(mainWindow,this);
            PublicData.ServerConfig.EpcS = new List<string>();
            PublicData.ServerConfig.UEpcS = new List<string>(); 
        }
    }
}
