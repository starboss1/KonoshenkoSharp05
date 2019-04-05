using System;
using System.Management;

namespace KMA.ProgrammingInCSharp2019.KonoshenkoLab05.Tools
{
    internal static class MemoryCalc
    {

        public static double TotalRam = Initialize();

        internal static double Initialize()
        {
            ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();
            double fres = 0;
            foreach (ManagementObject result in results)
            {
                double res = Convert.ToDouble(result["TotalVisibleMemorySize"]);
                fres = Math.Round((res / 1024), 2);
            }

            return fres;
        }
    }
}
