using Simple_Scope.Data;
using SkyVisual.DrawingObjects;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simple_Scope.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public enum ObjectsPanelType {
            Constellations,
            Stars,
            Planets,
        }

        public const string UniverseKey = "universe";
        public static int ListLayoutWidth = 300;
        SphericalCamera cameraControl;
        ObjectsPanelType currenObjectsPaneltType;

        public MainWindow()
        {
            InitializeComponent();

            sky.Visible = true;

            BindCamera();
        }

        public void BindCamera()
        {
            cameraControl = new SphericalCamera(cameraSpeed: 10, scale: 1, relativeTo: this);
            sky.MouseDown += cameraControl.Unfix;
            sky.MouseUp += cameraControl.Fix;
            sky.MouseMove += cameraControl.Move;
            sky.MouseMove += UpdateCamera;
            cameraControl.DirectionInGrad = new Point(0, 90);
            camera.LookDirection = cameraControl.Direction;
        }

        public void UpdateCamera(object sender, MouseEventArgs e)
        {
            camera.LookDirection = cameraControl.Direction;
        }

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////

        private void ViewStarsList_Click(object sender, RoutedEventArgs e) {
            LoadPanel(FilterByType<Star>, ObjectsPanelType.Stars);
        }

        private void ViewConstellationList_Click(object sender, RoutedEventArgs e) {
            LoadPanel(FilterByType<Constellation>, ObjectsPanelType.Constellations);
        }

        private void ViewPlanetsList_Click(object sender, RoutedEventArgs e) {
            LoadPanel(FilterByType<Planet>, ObjectsPanelType.Planets);
        }

        private void ListPanelCloseButton_Click(object sender, RoutedEventArgs e) {
            CloseListPanel();
        }

        private void DeleteSelectedItem(object sender, RoutedEventArgs e) {
            (this.Resources[UniverseKey] as Universe).Objects.Remove(ListView.SelectedItem as SpaceObject);
        }

        private void ShowSelectedItemInMap(object sender, RoutedEventArgs e) {
            
        }

        private void ShowSelectedItemInfo(object sender, RoutedEventArgs e) {
            SpaceObject oldObj = ListView.SelectedItem as SpaceObject;
            SpaceObject newObj = oldObj.GetCopy();
            Universe universe = this.Resources["universe"] as Universe;

            OpenInfoWindow(newObj, delegate() {
                universe.Objects.Remove(oldObj);
                universe.Objects.Add(newObj);
            });
        }

        private void ListPanelAddButton_Click(object sender, RoutedEventArgs e)
        {
            Universe universe = this.Resources["universe"] as Universe;
            SpaceObject newObj = null;

            switch (currenObjectsPaneltType)
            {
                case ObjectsPanelType.Constellations:
                    newObj = new Constellation();
                    break;
                case ObjectsPanelType.Planets:
                    newObj = new Planet();
                    break;
                case ObjectsPanelType.Stars:
                    newObj = new Star();
                    break;
            }

            OpenInfoWindow(newObj, delegate ()
            {
                universe.Objects.Add(newObj);
            });
        }

        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////

        private void LoadPanel(Predicate<object> filter, ObjectsPanelType panelType) {
            ListPanelLabel.Content = panelType;
            currenObjectsPaneltType = panelType;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(this.ListView.ItemsSource);
            view.Filter = filter;
            OpenPanel();
        }

        private void CloseListPanel() {
            ListLayout.Width = 0;
        }

        private void OpenPanel() {
            ListLayout.Width = ListLayoutWidth;
        }

        public static bool FilterByType<T>(object item) {
            if (item is T) {
                return true;
            }
            return false;
        }

        private void OpenInfoWindow(SpaceObject obj, SaveExit save)
        {
            Window infoWindow = null;
            if (obj is Star)
            {
                infoWindow = new StarWindow();
            }
            else if (obj is Planet)
            {
                infoWindow = new PlanetWindow();
            }
            else if (obj is Constellation)
            {
                infoWindow = new ConstellationWindow();
            }
            infoWindow.Resources.Add("infoObject", obj);
            infoWindow.Owner = this;
            (infoWindow as SpaceObjectEditWindow).Save = save;
            infoWindow.Show();
        }


    }
}
