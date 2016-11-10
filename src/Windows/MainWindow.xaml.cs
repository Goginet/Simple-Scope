using Simple_Scope.Data;
using Simple_Scope.IO;
using SkyVisual.DrawingObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public const int ListLayoutWidth = 300;
        public const string UniverseKey = "universe";
        public const string CameraKey = "camera";

        private Universe _universe;
        private SphericalCamera _camera;
        private ObjectsPanelType currenObjectsPaneltType;

        public MainWindow()
        {
            InitializeComponent();

            _camera = Resources[CameraKey] as SphericalCamera;
            _universe = Resources[UniverseKey] as Universe;
        }

        //--------------------------------------------------------------------//
        //                               EVENTS                               //
        //--------------------------------------------------------------------//

        private void ViewStarsList_Click(object sender, RoutedEventArgs e) {
            LoadPanel(FilterStars, ObjectsPanelType.Stars);
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
            (this.Resources[UniverseKey] as Universe).Remove(ListView.SelectedItem as SpaceObject);
        }

        private void ShowSelectedItemInMap(object sender, RoutedEventArgs e) {
            Point3D point = (ListView.SelectedItem as SpaceObject).Position;
            Point3D centre = _universe.Position;
            _camera.Direction = point - centre;
        }

        private void ShowSelectedItemInfo(object sender, RoutedEventArgs e) {
            SpaceObject oldObj = ListView.SelectedItem as SpaceObject;
            SpaceObject newObj = oldObj.GetCopy();

            OpenInfoWindow(newObj, delegate() {
                _universe.Remove(oldObj);
                _universe.Add(newObj);
            });
        }

        private void ListPanelAddButton_Click(object sender, RoutedEventArgs e)
        {
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
            newObj.Universe = _universe;
            OpenInfoWindow(newObj, delegate ()
            {
                _universe.Add(newObj);
            });
        }

        private void Settings_Click(object sender, RoutedEventArgs e) {
            SettingsWindow window = new SettingsWindow();
            window.Load = delegate (SettingsWindow.Parameters parameters) {
                DataManager dataManager = new DataManager(new DataFile(
                    parameters.Path,
                    new HygCsvDataConverter(),
                    parameters.Type,
                    parameters.Compression
                    ));
                dataManager.LoadDataIn();
                _universe = dataManager.Data;
                this.Resources[UniverseKey] = _universe;
                sky.ItemsSource = _universe;
            };
            window.Save = delegate (SettingsWindow.Parameters parameters) {
                DataManager dataManager = new DataManager(new DataFile(
                    parameters.Path,
                    new HygCsvDataConverter(),
                    parameters.Type,
                    parameters.Compression
                    ));
                dataManager.Data = _universe;
                dataManager.LoadDataOut();
            };
            window.Owner = this;
            window.Show();
        }

        private void View3dLayout_MouseMove(object sender, MouseEventArgs e) {
            _camera.Move(e.GetPosition(this));
        }

        private void View3dLayout_MouseDown(object sender, MouseButtonEventArgs e) {
            _camera.Unfix(e.GetPosition(this));
        }

        private void View3dLayout_MouseUp(object sender, MouseButtonEventArgs e) {
            _camera.Fix();
        }

        //--------------------------------------------------------------------//
        //                               LOGIC                                //
        //--------------------------------------------------------------------//

        private void LoadPanel(Predicate<object> filter, ObjectsPanelType panelType) {
            ListPanelLabel.Content = panelType;
            currenObjectsPaneltType = panelType;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(this.ListView.ItemsSource);
            view.Filter = filter;
            view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            OpenPanel();
        }

        private void CloseListPanel() {
            ListLayout.Width = 0;
        }

        private void OpenPanel() {
            ListLayout.Width = ListLayoutWidth;
        }

        private void OpenInfoWindow(SpaceObject obj, SaveExit save)
        {
            Window infoWindow = null;
            if (obj is Star)
            {
                infoWindow = new StarWindow(obj as Star);
            }
            else if (obj is Planet)
            {
                infoWindow = new PlanetWindow(obj as Planet);
            }
            else if (obj is Constellation)
            {
                infoWindow = new ConstellationWindow(obj as Constellation);
            }
            infoWindow.Owner = this;
            (infoWindow as SpaceObjectEditWindow).Save = save;
            infoWindow.Show();
        }

        //--------------------------------------------------------------------//
        //                              FILTERS                               //
        //--------------------------------------------------------------------//

        public static bool FilterStars(object item) {
            if (item is Star) {
                if (item is StarHyg) {
                    if ((item as StarHyg).Proper != String.Empty) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return true;
                }
            }
            return false;
        }

        public static bool FilterByType<T>(object item) {
            if (item is T) {
                return true;
            }
            return false;
        }
    }
}
