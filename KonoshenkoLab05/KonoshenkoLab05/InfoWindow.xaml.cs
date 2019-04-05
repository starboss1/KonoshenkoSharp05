using System.Diagnostics;
using System.Windows;
using KMA.ProgrammingInCSharp2019.KonoshenkoLab05.ViewModels;

namespace KMA.ProgrammingInCSharp2019.KonoshenkoLab05
{
    /// <summary>
    /// Логика взаимодействия для InfoWindow.xaml
    /// </summary>
    internal partial class InfoWindow : Window
    {

        internal InfoWindow(Process process)
        {
            InitializeComponent();
            Title = $"{process.ProcessName} Info";
            DataContext = new InfoViewModel(process.Modules, process.Threads);
        }

    }
}
