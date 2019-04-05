using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using KMA.ProgrammingInCSharp2019.KonoshenkoLab05.Tools;

namespace KMA.ProgrammingInCSharp2019.KonoshenkoLab05.ViewModels
{
    internal class InfoViewModel : BaseViewModel
    {
        private ObservableCollection<ProcessModule> _modules;
        private ObservableCollection<ProcessThread> _threads;

        public ObservableCollection<ProcessModule> Modules
        {
            get => _modules;
            private set
            {
                _modules = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ProcessThread> Threads
        {
            get => _threads;
            private set
            {
                _threads = value;
                OnPropertyChanged();
            }
        }

        internal InfoViewModel(ProcessModuleCollection modules, ProcessThreadCollection threads)
        {
            Modules = new ObservableCollection<ProcessModule>(modules.Cast<ProcessModule>());
            Threads = new ObservableCollection<ProcessThread>(threads.Cast<ProcessThread>());
        }
    }
}
