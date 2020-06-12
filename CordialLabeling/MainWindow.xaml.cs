using CordialLabeling.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace CordialLabeling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Graph Graph { get; set; }

        public MainWindow()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void GenerateNew_Click(object sender, RoutedEventArgs e)
        {
            var genNew = new GenerateNew();
            genNew.ShowDialog();
            if (genNew.Graph != null)
            {
                Graph = genNew.Graph;
                DisplayGraph();
            }
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            var loadFile = new LoadFile();
            loadFile.ShowDialog();
            if(loadFile.Graph != null)
            {
                Graph = loadFile.Graph;
                DisplayGraph();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void DisplayGraph()
        {
            OpenGraph.IsEnabled = false;
            try
            {
                Graph.ExportToMiniZincFile();
                Graph.ExecuteMiniZinc();
                Graph.UpdateVerticesLabel();
                Graph.ExportToGraphvizFileWithLabels();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Graph.ExportToGraphvizFile();
            }

            string fileName = $"{Graph.N}-{Graph.K}-{System.IO.Path.GetRandomFileName().Replace('.', 'x')}.png";

            Graph.ExecuteGraphviz(fileName);
            FileInfo fileInfo = new FileInfo($"Result\\{fileName}");
            Image.Source = new BitmapImage(new Uri(fileInfo.FullName));
            OpenGraph.IsEnabled = true;

        }

        private void OpenGraph_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", Environment.CurrentDirectory + "\\Result");
        }
    }
}
