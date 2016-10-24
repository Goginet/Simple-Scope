using Simple_Scope.Data;
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

namespace Simple_Scope.Windows
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class StarWindow : Window, SpaceObjectEditWindow
    {
        public SaveExit Save { get; set; }

        public StarWindow()
        {
            InitializeComponent();
        }

        private void SaveExit_Click(object sender, RoutedEventArgs e)
        {
            Save();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddPlanetButton_Click(object sender, RoutedEventArgs e)
        {
            Star infoObject = this.Resources["infoObject"] as Star;
            if (infoObject != null)
            {
                Planet newChild = infoObject.Universe.GetObjectByName(AddPlanetTextBox.Text.ToString()) as Planet;
                if (newChild != null)
                {
                    infoObject.Planets.Add(newChild);
                }
            }
            AddPlanetTextBox.Clear();
        }
    }
}
