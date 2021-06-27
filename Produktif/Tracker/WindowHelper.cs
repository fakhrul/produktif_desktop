using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Automation;

namespace Produktif
{
    public class WindowHelper
    {
        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const uint EVENT_SYSTEM_FOREGROUND = 3;

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out Int32 lpdwProcessId);

        // used for an output LPCTSTR parameter on a method call
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct STRINGBUFFER
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szText;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, out STRINGBUFFER ClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, out STRINGBUFFER ClassName);

        static public string GetClassName(IntPtr hWnd)
        {
            STRINGBUFFER sLimitedLengthWindowTitle;
            GetClassName(hWnd, out sLimitedLengthWindowTitle, 256);
            return sLimitedLengthWindowTitle.szText;
        }

        public static Int32 GetWindowProcessID(IntPtr hwnd)
        {
            Int32 pid;
            GetWindowThreadProcessId(hwnd, out pid);
            return pid;
        }

        private WinEventDelegate dele = null;

        private Thread _monitoringThread;

        private string GetWindowTitle(IntPtr handle)
        {
            const int nChars = 256;
            StringBuilder title = new StringBuilder(nChars);
            if (GetWindowText(handle, title, nChars) > 0)
            {
                return title.ToString();
            }
            return null;
        }

        static public string GetWindowText(IntPtr hWnd)
        {
            int WM_GETTEXT = 0xD;
            STRINGBUFFER sLimitedLengthWindowTitle;
            SendMessage(hWnd, WM_GETTEXT, 256, out sLimitedLengthWindowTitle);
            //NativeWIN32.GetWindowText(hWnd, out sLimitedLengthWindowTitle, 256);
            return sLimitedLengthWindowTitle.szText;
        }

        public event EventHandler<ActiveWindowUpdateEventArgs> OnActiveWindowUpdate;

        public WindowHelper()
        {
            dele = new WinEventDelegate(WinEventProc);
            IntPtr m_hhook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);

            _monitoringThread = new Thread(MonitoringThread);
            _monitoringThread.IsBackground = true;
            _monitoringThread.Start();
        }

        private void MonitoringThread(object obj)
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(1000);
                    ProcesActiveWindow();
                }
                catch 
                {
                }
            }
        }

        public void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            ProcesActiveWindow();
        }

        private void ProcesActiveWindow()
        {
            var activeWindow = GetActiveWindow();
            if (OnActiveWindowUpdate != null)
                OnActiveWindowUpdate(this, new ActiveWindowUpdateEventArgs(activeWindow));
        }


        private string GetProcessNameByHandle(IntPtr handle, out Process process)
        {
            Int32 pid = GetWindowProcessID(handle);
            process = Process.GetProcessById(pid);
            return process.ProcessName;
        }
        public ActiveWindow GetActiveWindow()
        {
            ActiveWindow activeWindow = new ActiveWindow("idle", "","", "","");
            try
            {

                IntPtr handle = GetForegroundWindow();
                Process process = null;
                string processName = GetProcessNameByHandle(handle, out process);

                if (string.IsNullOrEmpty(processName))
                    return activeWindow;
                string title = GetWindowTitle(handle);
                if (string.IsNullOrEmpty(title))
                    return activeWindow;

                //string text = GetWindowText(handle);
                string url;
                string data;
                    GetBrowsedUrl(process, out url, out data);

                string className = GetClassName(handle);

                activeWindow = new ActiveWindow(processName, title, className, url, data);

                return activeWindow;
            }
            catch (Exception ex)
            {
            }
            return activeWindow;
        }

        private string GetDomainNameOfUrlString(string urlString)
        {
            try
            {
                string newUrl = Extensions.IsValidUrl(urlString);
                if (string.IsNullOrEmpty(newUrl))
                    return "";

                var uri = new Uri(newUrl);
                return uri.Host;
                //var host = uri.Host;
                //return host;// host.Substring(host.LastIndexOf('.', host.LastIndexOf('.') - 1) + 1);
            }
            catch (Exception ex)
            {
            }
            return "";
        }


        public bool GetBrowsedUrl(Process process, out string url, out string data)
        {
            url = "";
            data = "";
            try
            {
                if (process.ProcessName == "firefox")
                {
                    if (process == null)
                        return false;

                    if (process.MainWindowHandle == IntPtr.Zero)
                        return false;

                    AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
                    if (element == null)
                        return false;

                    AutomationElement doc = element.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document));
                    if (doc == null)
                        return false;
                    data = ((ValuePattern)doc.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
                    url = GetDomainNameOfUrlString(data);
                    return true;
                }
                else
                {
                    if (process == null)
                        throw new ArgumentNullException("process");

                    if (process.MainWindowHandle == IntPtr.Zero)
                        return false;

                    AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
                    if (element == null)
                        return false;

                    AutomationElement edit = element.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

                    if (edit == null)
                        return false;

                    data = ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
                    url = GetDomainNameOfUrlString(data);
                    return true;
                }

            }
            catch (Exception ex)
            {
            }

            return false;

        }
    }
}
