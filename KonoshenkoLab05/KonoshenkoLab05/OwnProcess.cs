using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using KMA.ProgrammingInCSharp2019.KonoshenkoLab05.Tools;

namespace KMA.ProgrammingInCSharp2019.KonoshenkoLab05
{
    internal class OwnProcess
    {

        internal PerformanceCounter RamCounter { get; }

        internal PerformanceCounter CpuCounter { get; }

        public string Name { get; }

        public int Id { get; }

        public bool IsActive { get; }

        public int CpuTaken { get; set; }

        public double RamTaken { get; set; }

        public double RamTakenPercent { get; set; }

        public int ThreadsNumber { get; set; }

        public string Username { get; }

        public string FilePath{ get; }

        public string RunOn { get; }

        internal OwnProcess(Process systemProcess)
        {
            RamCounter = new PerformanceCounter("Process", "Private Bytes", systemProcess.ProcessName);
            CpuCounter = new PerformanceCounter("Process", "% Processor Time", systemProcess.ProcessName);
            Name = systemProcess.ProcessName;
            Id = systemProcess.Id;
            IsActive = systemProcess.Responding;
            CpuTaken = Convert.ToInt32(CpuCounter.NextValue() / Environment.ProcessorCount);
            RamTaken = Math.Round(RamCounter.NextValue() / (1024 * 1024),2);
            RamTakenPercent = Math.Round((RamTaken / MemoryCalc.TotalRam)*100,2);
            ThreadsNumber = systemProcess.Threads.Count;
            Username = GetProcessOwner(systemProcess.Id);
            try
            {
                FilePath = systemProcess.MainModule.FileName;
            }
            catch (Win32Exception e)
            {
                FilePath = e.Message;
            }

            try
            {
                RunOn = systemProcess.StartTime.ToString();
            }
            catch (Win32Exception e)
            {
                RunOn = e.Message;
            }
        }

        private static string GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();
            foreach (ManagementObject obj in processList)
            {
                string[] argList = {string.Empty, string.Empty};
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                    return argList[0];
            }
            return "NO OWNER";
        }
    }


}
