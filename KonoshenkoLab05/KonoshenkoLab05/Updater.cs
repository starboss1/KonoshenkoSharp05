using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using KMA.ProgrammingInCSharp2019.KonoshenkoLab05.Tools;

namespace KMA.ProgrammingInCSharp2019.KonoshenkoLab05
{
    internal static class Updater
    {
        private static readonly Thread UpdateProcessesThread;
        private static readonly Thread UpdateModulesAndThreadsThread;

        internal static Dictionary<int, OwnProcess> Processes { get; set; }

        static Updater()
        {
            Processes = new Dictionary<int, OwnProcess>();
            UpdateModulesAndThreadsThread = new Thread(UpdateModulesAndThreads);
            UpdateProcessesThread = new Thread(UpdateProcesses);
            UpdateProcessesThread.Start();
            UpdateModulesAndThreadsThread.Start();
        }

        internal static void Close()
        {
            UpdateProcessesThread.Join(4000);
            UpdateModulesAndThreadsThread.Join(2000);
        }

        private static async void UpdateProcesses()
        {
            while (true)
            {
                await Task.Run(() =>
                {
                    lock (Processes)
                    {
                        List<Process> processes = Process.GetProcesses().ToList();
                        IEnumerable<int> keys = Processes.Keys.ToList()
                            .Where(id => processes.All(proc => proc.Id != id));
                        foreach (int key in keys)
                        {
                            Processes.Remove(key);
                        }

                        foreach (Process proc in processes)
                        {
                            if (!Processes.ContainsKey(proc.Id))
                            {
                                try
                                {
                                    Processes[proc.Id] = new OwnProcess(proc);
                                }
                                catch (InvalidOperationException e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                catch (ManagementException e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                catch (NullReferenceException e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                    }
                });
                Thread.Sleep(5000);
            }
        }

        private static async void UpdateModulesAndThreads()
        {
            while (true)
            {
                await Task.Run(() =>
                {
                    lock (Processes)
                    {
                        foreach (int id in Processes.Keys.ToList())
                        {
                            Process pr;
                            try
                            {
                                pr = Process.GetProcessById(id);
                            }
                            catch (ArgumentException)
                            {
                                Processes.Remove(id);
                                continue;
                            }

                            Processes[id].CpuTaken = Convert.ToInt32(Processes[id].CpuCounter.NextValue() / Environment.ProcessorCount);
                            Processes[id].RamTaken = Math.Round(Processes[id].RamCounter.NextValue() / (1024 * 1024),2);
                            Processes[id].RamTakenPercent = Math.Round((Processes[id].RamTaken / MemoryCalc.TotalRam) * 100, 2);
                            Processes[id].ThreadsNumber = pr.Threads.Count;
                        }
                    }
                });
                Thread.Sleep(2000);
            }
        }
    }
}
