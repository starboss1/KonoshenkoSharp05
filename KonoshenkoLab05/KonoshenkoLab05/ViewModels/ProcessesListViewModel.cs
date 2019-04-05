using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using KMA.ProgrammingInCSharp2019.KonoshenkoLab05.Tools;
using KMA.ProgrammingInCSharp2019.KonoshenkoLab05.Tools.Managers;

namespace KMA.ProgrammingInCSharp2019.KonoshenkoLab05.ViewModels
{
    internal class ProcessesListViewModel:BaseViewModel, ILoaderOwner
    {

        private Visibility _loaderVisibility = Visibility.Hidden;
        private bool _isControlEnabled = true;

        private ObservableCollection<OwnProcess> _processes;
        private readonly Thread _updateThread;
        private OwnProcess _selectedProcess;
        private RelayCommand _endTaskCommand;
        private RelayCommand _getInfoCommand;
        private RelayCommand _openFileLocationCommand;

        public bool IsItemSelected => SelectedProcess != null;

        public Visibility LoaderVisibility
        {
            get { return _loaderVisibility; }
            set
            {
                _loaderVisibility = value;
                OnPropertyChanged();
            }
        }
        public bool IsControlEnabled
        {
            get { return _isControlEnabled; }
            set
            {
                _isControlEnabled = value;
                OnPropertyChanged();
            }
        }

        public OwnProcess SelectedProcess
        {
            get => _selectedProcess;
            set
            {
                _selectedProcess = value;
                OnPropertyChanged();
                OnPropertyChanged("IsItemSelected");
            }
        }

        public ObservableCollection<OwnProcess> Processes
        {
            get => _processes;
            private set
            {
                _processes = value;
                OnPropertyChanged();
            }
        }

        internal ProcessesListViewModel()
        {
            LoaderManager.Instance.Initialize(this);
            _updateThread = new Thread(UpdateUsers);
            Thread initializationThread = new Thread(InitializeProcesses);
            initializationThread.Start();
        }

        private async void InitializeProcesses()
        {
            LoaderManager.Instance.ShowLoader();
            await Task.Run(() =>
            {
                Processes = new ObservableCollection<OwnProcess>(Updater.Processes.Values);
            });
            _updateThread.Start();
            while (Updater.Processes.Count == 0)
                Thread.Sleep(7000);
            LoaderManager.Instance.HideLoader();
        }

        internal void Close()
        {
            _updateThread.Join(3000);
        }

        public RelayCommand EndTaskCommand => _endTaskCommand ?? (_endTaskCommand = new RelayCommand(EndTaskImplementation));
        public RelayCommand GetInfoCommand => _getInfoCommand ?? (_getInfoCommand = new RelayCommand(GetInfoImplementation));
        public RelayCommand OpenFileLocationCommand => _openFileLocationCommand ?? (_openFileLocationCommand = new RelayCommand(OpenLocationImplementation));

        private void EndTaskImplementation(object obj)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                Process process = Process.GetProcessById(SelectedProcess.Id);
                try
                {
                    process.Kill();
                }
                catch (Win32Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }

        private async void GetInfoImplementation(object obj)
        {
            try
            {
                await Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        Process process = Process.GetProcessById(SelectedProcess.Id);
                        NavigationManager.Close();
                        try
                        {
                            NavigationManager.Navi(process);
                        }
                        catch (Win32Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    });
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void OpenLocationImplementation(object o)
        {
            await Task.Run(() =>
            {
                Process process = Process.GetProcessById(SelectedProcess.Id);
                try
                {
                    string fullPath = process.MainModule.FileName;
                    Process.Start("explorer.exe", fullPath.Remove(fullPath.LastIndexOf('\\')));
                }
                catch (Win32Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }

        private async void UpdateUsers()
        {
            while (true)
            {
                await Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        try
                        {
                            lock (Processes)
                            {
                                List<OwnProcess> toRemove =
                                    new List<OwnProcess>(
                                        Processes.Where(proc => !Updater.Processes.ContainsKey(proc.Id)));
                                foreach (OwnProcess proc in toRemove)
                                {
                                    Processes.Remove(proc);
                                }

                                List<OwnProcess> toAdd =
                                    new List<OwnProcess>(
                                        Updater.Processes.Values.Where(proc => !Processes.Contains(proc)));
                                foreach (OwnProcess proc in toAdd)
                                {
                                    Processes.Add(proc);
                                }
                            }
                        }
                        catch (NullReferenceException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        catch (ArgumentNullException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        catch (InvalidOperationException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    });
                });
                Thread.Sleep(4000);
            }
        }
    }
}
