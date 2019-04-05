using System.Diagnostics;

namespace KMA.ProgrammingInCSharp2019.KonoshenkoLab05.Tools.Managers
{
    internal static class NavigationManager
    {
        private static InfoWindow _infoWindow;

        internal static void Navi(Process process = null)
        {
            if (process != null)
            {
                _infoWindow = new InfoWindow(process);
                _infoWindow.Show();
            }
        }

        internal static void Close()
        {
            _infoWindow?.Close();
        }
    }
}
