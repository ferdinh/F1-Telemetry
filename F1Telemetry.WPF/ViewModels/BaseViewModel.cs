using System.ComponentModel;

namespace F1Telemetry.WPF.ViewModels
{
    /// <summary>
    /// Base class for View Model.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}