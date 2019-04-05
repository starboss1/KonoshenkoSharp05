using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KMA.ProgrammingInCSharp2019.KonoshenkoLab05.Tools
{
    internal abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
