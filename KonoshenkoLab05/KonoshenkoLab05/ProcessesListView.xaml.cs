using System.Windows.Controls;
using KMA.ProgrammingInCSharp2019.KonoshenkoLab05.ViewModels;


namespace KMA.ProgrammingInCSharp2019.KonoshenkoLab05
{
    /// <summary>
    /// Логика взаимодействия для ProcessesListView.xaml
    /// </summary>
    internal partial class ProcessesListView : UserControl
    {
        internal ProcessesListView()
        {
            InitializeComponent();
            DataContext = new ProcessesListViewModel();
        }

        internal void Close()
        {
            ((ProcessesListViewModel)DataContext).Close();
        }
    }
}
