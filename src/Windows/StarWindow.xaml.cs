using Simple_Scope.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// 
    public partial class StarWindow : Window, SpaceObjectEditWindow {
        private Universe _universe;
        private Star _infoObject;

        public SaveExit Save { get; set; }

        public StarWindow(Star infoObject)
        {
            _universe = infoObject.Universe;
            _infoObject = infoObject;
            Resources.Add("infoObject", _infoObject);
            Resources.Add("universe", _universe);
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
            string objName = AddPlanetTextBox.Text.ToString();
            Planet newChild = _universe.GetObjectByName(objName) as Planet;
            if (!_infoObject.Children.Contains(newChild)) {
                _infoObject.Children.Add(newChild);
            }
            AddPlanetTextBox.Clear();
        }
    }
}
