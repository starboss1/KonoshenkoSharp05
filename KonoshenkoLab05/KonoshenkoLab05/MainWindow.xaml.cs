using System.ComponentModel;
using System.Windows;

namespace KMA.ProgrammingInCSharp2019.KonoshenkoLab05
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    internal partial class MainWindow : Window
    {
        private ProcessesListView _processesListView;

        public MainWindow()
        {
            InitializeComponent();
            ShowProcessesListView();
        }

        private void ShowProcessesListView()
        {
            MainGrid.Children.Clear();
            if (_processesListView == null)
                _processesListView = new ProcessesListView();
            MainGrid.Children.Add(_processesListView);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _processesListView?.Close();
            Updater.Close();
            base.OnClosing(e);
        }
    }
}
