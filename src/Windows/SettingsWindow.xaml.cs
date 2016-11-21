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

namespace Simple_Scope.Windows {
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    /// 
    public delegate void Load(SettingsWindow.Parameters parameters);
    public delegate void Save(SettingsWindow.Parameters parameters);

    public partial class SettingsWindow: Window {
        public struct Parameters {
            public string Path { get; set; }
            public bool Compression { get; set; }
            public DataFile.FileType Type { get; set; }
        }

        public Load Load { get; set; }
        public Save Save { get; set; }

        public SettingsWindow() {
            InitializeComponent();
        }

        private Parameters SetParameters() {
            Parameters parameters = new Parameters();
            parameters.Path = this.path.Text.ToString();
            parameters.Compression = (bool)this.compression.IsChecked;
            if (this.text.IsChecked == true) {
                parameters.Type = DataFile.FileType.Text;
            } else if (this.binary.IsChecked == true) {
                parameters.Type = DataFile.FileType.Binary;
            } else if (this.binaryModify.IsChecked == true) {
                parameters.Type = DataFile.FileType.BinaryModify;
            }
                return parameters;
        }

        private void loadButton_Click(object sender, RoutedEventArgs e) {
            Load(SetParameters());
        }

        private void saveButton_Click(object sender, RoutedEventArgs e) {
            Save(SetParameters());
        }
    }
}
