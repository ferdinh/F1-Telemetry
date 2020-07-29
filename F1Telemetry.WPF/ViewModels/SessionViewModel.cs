using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace F1Telemetry.WPF.ViewModels
{
    public class SessionViewModel : INotifyPropertyChanged
    {
        public string SessionType { get; set; }
        public ushort TrackLength { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
