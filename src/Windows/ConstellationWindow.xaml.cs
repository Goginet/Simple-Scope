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

namespace Simple_Scope.Windows {
    /// <summary>
    /// Interaction logic for ConstellationWindow.xaml
    /// </summary>
    public partial class ConstellationWindow : Window, SpaceObjectEditWindow {
        private Universe _universe;
        private Constellation _infoObject;

        public SaveExit Save { get; set; }

        public ConstellationWindow(Constellation infoObject) {
            _infoObject = infoObject;
            _universe = infoObject.Universe;
            Resources.Add("infoObject", _infoObject);
            Resources.Add("universe", _universe);
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            Save();
            Close();
        }

        private void AddStarButton_Click(object sender, RoutedEventArgs e) {
            string objName = AddStarTextBox.Text.ToString();
            Star newChild = _universe.GetObjectByName(objName) as Star;
            if (!_infoObject.Children.Contains(newChild)) {
                _infoObject.Children.Add(newChild);
            }
            AddStarTextBox.Clear();
        }
    }
}
