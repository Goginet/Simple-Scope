using SkyVisual.DrawingObjects;
using SkyVisual.ViewPorts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SkyVisual
{
    public delegate void Init();

    public interface ISkyDrawingObject
    {
        SkyDrawingObject SkyDrawingObject { get; }
    }

    public sealed class VariableSkyView : UserControl
    {
        private bool _visible = false;
        private SkyViewPort port;
        public ObservableCollection<SkyDrawingObject> Objects { get; set; }

        private Init ChengePort;

        public VariableSkyView()
        {
            Objects = new ObservableCollection<SkyDrawingObject>();
            ChengePort += SetContent;
            Objects.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
            {
                SetContent();
            };
        }

        private void SetContent()
        {
            if(port != null && Visible)
            {
                Content = port.GetPort(Objects.ToArray<SkyDrawingObject>());
            } else {
                Content = null;
            }
        }

        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                SetContent();
            }
        }

        public SkyViewPort Port
        {
            get { return port; }
            set
            {
                port = value;
                ChengePort();
            }
        }

        public IEnumerable ItemsSource
        {
            get { return null; }
            set
            {
                Visible = false;
                Objects.Clear();
                if(value is INotifyCollectionChanged)
                {
                    (value as INotifyCollectionChanged).CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
                    {
                        if (e.Action == NotifyCollectionChangedAction.Add)
                        {
                            foreach(object obj in e.NewItems)
                            {
                                if (obj is ISkyDrawingObject)
                                {
                                    ISkyDrawingObject convertedObject = (obj as ISkyDrawingObject);
                                    Objects.Add(convertedObject.SkyDrawingObject);
                                }
                            }
                        }
                        if (e.Action == NotifyCollectionChangedAction.Remove)
                        {
                            foreach (object obj in e.OldItems)
                            {
                                if (obj is ISkyDrawingObject)
                                {
                                    ISkyDrawingObject convertedObject = (obj as ISkyDrawingObject);
                                    Objects.Remove(convertedObject.SkyDrawingObject);
                                }
                            }
                        }
                    };
                }

                foreach(object obj in value)
                {
                    if(obj is ISkyDrawingObject) {
                        ISkyDrawingObject convertedObject = (obj as ISkyDrawingObject);
                        Objects.Add(convertedObject.SkyDrawingObject);
                    }
                }
                Visible = true;
            }
        }
    }
}
