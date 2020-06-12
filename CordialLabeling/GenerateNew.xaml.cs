using CordialLabeling.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CordialLabeling
{
    /// <summary>
    /// Interaction logic for GenerateNew.xaml
    /// </summary>
    public partial class GenerateNew : Window
    {
        public bool IsConnected { get; set; }
        public int N { get; set; }
        public double P { get; set; }
        public int K { get; set; }

        public Graph Graph { get; set; }

        public GenerateNew()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            DataContext = this;
        }

        private void Gnp_Click(object sender, RoutedEventArgs e)
        {
            Graph = Graph.Generate(N, P, IsConnected);
            Close();
        }

        private void Gnk_Click(object sender, RoutedEventArgs e)
        {
            K = K > N * (N - 1) / 2 ? N * (N - 1) / 2 : K;
            Graph = Graph.Generate(N, K, IsConnected);
            Close();
        }
    }
}
