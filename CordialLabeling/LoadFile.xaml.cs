using CordialLabeling.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for LoadFile.xaml
    /// </summary>
    public partial class LoadFile : Window
    {
        public Graph Graph { get; set; }

        public LoadFile()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void LoadMatrix_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                Graph = Graph.ReadFromMatrixFile(openFileDialog.FileName);
            }
            Close();
        }

        private void LoadMinizinc_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MiniZinc Files (*.dzn)|*.dzn|Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                Graph = Graph.ReadFromMiniZincFile(openFileDialog.FileName);
            }
            Close();
        }

    }
}
