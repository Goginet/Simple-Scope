using Simple_Scope.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Simple_Scope.Windows
{
    public delegate void SaveExit();
    interface SpaceObjectEditWindow
    {
        SaveExit Save { get; set; }
    }
}