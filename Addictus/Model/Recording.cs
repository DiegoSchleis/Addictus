using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
using System.Timers;

namespace Addictus.Model
{
    public class Recording
    {
        User user;
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out int processId);

        Dictionary<string, double> records;

        bool record = true;
        
        public MainWindowView CallerMainWindow { get; set; }

        public Recording(User u, MainWindowView mw)
        {
            user = u;
            CallerMainWindow = mw;
            records = mw.Dict;
        }

        public void AddToBlacklist(string input)
        {
            CallerMainWindow.Blacklist.Add(input);
        }
        
        public void CreateThread()
        {
            Thread t1 = new Thread(new ThreadStart(RecordProcesses));
            t1.SetApartmentState(ApartmentState.STA);
            t1.Start();
        }

        public Dictionary<string, double> ReturnDict()
        {
            return records;
        }

        public void RecordProcesses()
        {
            AddToBlacklist("explorer");
            AddToBlacklist("Idle");
            AddToBlacklist("Addictus");
            Boolean stillsame = true;
            DateTime start = DateTime.Now;
            while (record)
            {
                if (!stillsame)
                {
                    start = DateTime.Now;
                }
                var win = GetForegroundWindow();
                int procid = 0;
                GetWindowThreadProcessId(win, out procid);
                Process proc = Process.GetProcessById(procid);
                string name = proc.ProcessName;
                var win2 = GetForegroundWindow();
                if (win != win2)
                {
                    DateTime end = DateTime.Now;
                    TimeSpan duration = end.Subtract(start);
                    double durationSeconds = duration.TotalSeconds;
                    durationSeconds = Math.Round(durationSeconds, 2);
                    if (!CallerMainWindow.Blacklist.Contains(name))
                    {
                        Console.WriteLine(name + ">: " + durationSeconds + "s");
                        double val = 0;
                        records.TryGetValue(name, out val);
                        if (!records.ContainsKey(name))
                        {
                            records.Add(name, durationSeconds);
                        }
                        else
                        {
                            records.Remove(name);
                            records.Add(name, val + durationSeconds);
                        }
                    }
                    stillsame = false;
                }
                else
                {
                    stillsame = true;
                }
            }
        }

        public void StopRecording()
        {
            record = false;
        }
    }
}
