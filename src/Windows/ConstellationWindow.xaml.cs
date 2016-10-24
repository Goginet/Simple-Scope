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
    /// Interaction logic for ConstellationWindow.xaml
    /// </summary>
    public partial class ConstellationWindow : Window, SpaceObjectEditWindow
    {
        public SaveExit Save { get; set; }

        public ConstellationWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Save();
            Close();
        }

        private void AddStarButton_Click(object sender, RoutedEventArgs e)
        {
            Constellation infoObject = this.Resources["infoObject"] as Constellation;
            if (infoObject != null)
            {
                Star newChild = infoObject.Universe.GetObjectByName(AddStarTextBox.Text.ToString()) as Star;
                if (newChild != null)
                {
                    infoObject.Stars.Add(newChild);
                }
            }
            AddStarTextBox.Clear();
        }
    }
}
