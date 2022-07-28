global using System.Diagnostics;
global using System.Net.NetworkInformation;

namespace Network;

static class Network
{
    public static void GetAdapters()
    {
        foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                || netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            {
                _adapters.Add(netInterface.Name);
            }
        }
    }

    public static bool CheckAdapters()
    {
        if (!_adapters.Any()) return false;
        return true;
    }

    public static void EnableAdapters()
    {
        if (!CheckAdapters()) GetAdapters();
        
        foreach (var items in _adapters) SwitchAdapter(items, true);
    }

    public static void DisableAdapters()
    {
        if (!CheckAdapters()) GetAdapters();

        foreach (var items in _adapters) SwitchAdapter(items, false);
    }

    public static void SwitchAdapter(string adapter, bool state)
    {
        ProcessStartInfo psi;

        if (state) psi = new ProcessStartInfo("netsh", "interface set interface \"" + adapter + "\" enable");
        else psi = new ProcessStartInfo("netsh", "interface set interface \"" + adapter + "\" disable");

        Process process = new Process();
        process.StartInfo = psi;

        process.StartInfo.UseShellExecute = false;  // New window
        process.StartInfo.CreateNoWindow = true;    // Hidden window

        process.Start();
    }

    private static List<string> _adapters = new List<string>();
}

class Program
{
    static void Main()
    {
        Network.DisableAdapters();
        Thread.Sleep(5000);
        Network.EnableAdapters();
    }
}
