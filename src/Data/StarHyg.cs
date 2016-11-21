using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyVisual.DrawingObjects;

namespace Simple_Scope.Data
{
    [Serializable]
    public class StarHyg: Star
    { 
        private int _hip;
        private int _hd;
        private int _hr;
        private string _gl = "";
        private string _bf = "";
        private string _proper = "";

        public StarHyg() : base() { }

        public int Hip
        {
            get { return _hip; }
            set { _hip = value; }
        }

        public int Hd
        {
            get { return _hd; }
            set { _hd = value; }
        }

        public int Hr
        {
            get { return _hr; }
            set { _hr = value; }
        }

        public string Gl
        {
            get { return _gl; }
            set { _gl = value; }
        }

        public string Bf
        {
            get { return _bf; }
            set { _bf = value; }
        }

        public string Proper
        {
            get { return _proper; }
            set
            {
                _proper = value;
            }
        }

        public override string Name {
            get {
                if (Proper != String.Empty) {
                    return Proper;
                } else if (Gl != String.Empty) {
                    return Gl;
                } else {
                    return "NoName";
                }
            }
        }
    }
}
