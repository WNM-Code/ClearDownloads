using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;

namespace CLRDownloads
{
    class Remover
    {
        string location = "";
        string store = "";
        string storeLoc = "";
        bool moved;
        bool remArchive;
        //false means archiving
        bool CorR;
        private static Dictionary<string, int> dayEnabled = new Dictionary<string, int>
        {
            {"Monday"   , 0 },
            {"Tuesday"  , 0 },
            {"Wednesday", 0 },
            {"Thursday" , 0 },
            {"Friday"   , 0 },
            {"Saturday" , 0 },
            {"Sunday"   , 0 }
        };

        public static Dictionary<int, string> months = new Dictionary<int, string>
        {
            {1, "January" },
            {2, "Febuary" },
            {3, "March" },
            {4, "April" },
            {5, "May" },
            {6, "June" },
            {7, "July" },
            {8, "August" },
            {9, "September" },
            {10, "October" },
            {11, "November" },
            {12, "December" },

        };
        private DateTime repeatTime;

        private bool running;

        public Remover()
        {
            location = "null";
            store = "$ Archived " + string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
            storeLoc = location + "\\" + store;
            moved = false;
            running = false;
            CorR = false;
            remArchive = false;
            repeatTime = new DateTime(1997,4,1,0,0,0);
        }

        public async void run(ListBox l, Button r)
        {
            
            DateTime now = DateTime.Now;
            DateTime before = DateTime.Now;
            await Task.Run(() =>
            {
                while (true)
                {
                    now = DateTime.Now;
                    if (running)
                    {
                        if (dayEnabled[now.DayOfWeek.ToString()] == 1 &&
                            now.Hour == repeatTime.Hour &&
                            now.Minute == repeatTime.Minute)
                        {
                            callRemove(now, l);
                            before = now;
                        }
                    }
                    Thread.Sleep(60000);
                }
            });
            
        }

        public async void runNow(ListBox l, Button r)
        {

            DateTime now = DateTime.Now;
            await Task.Run(() =>
            {

                callRemove(now, l);

                r.Dispatcher.Invoke(() => { r.Content = "Run"; });
                setRunning(false);
                        
            });

        }

        public void callRemove(DateTime now, ListBox l)
        {
            setStore();
            setLoc();
            Console.WriteLine(storeLoc);
            if (location != "null")
            {
                if (Directory.GetParent(storeLoc).Exists)
                {
                    Directory.CreateDirectory(storeLoc);
                    remove(location, now, l);
                    return;
                }
                else
                {
                    l.Dispatcher.Invoke(() => { l.Items.Add(Directory.GetParent(storeLoc) + " does not exist anymore.\nSelect a new location."); });
                }

            }
            else
            {
                l.Dispatcher.Invoke(() => { l.Items.Add("You must correctly set the Clear Location"); });
            }
        }

        public void remove(string path, DateTime now, ListBox l)
        {
            string[] folders;
            if (location != "null") {
                folders = Directory.GetDirectories(path);
            }
            else
            {
                l.Dispatcher.Invoke(() => { l.Items.Add("You must correctly set the Clear Location"); });
                return;
            }

            string filename;
            string fileLoc;
            foreach (string file in Directory.GetFiles(path))
            {
                filename = file.Remove(0, path.Length);
                fileLoc = storeLoc + "\\" + filename;
                Directory.Move(file, fileLoc);
                moved = true;
            }
            foreach (string folder in folders)
            {
                if (!folder.Contains(location + "\\$ Archived ") || remArchive)
                {
                    if (folder != storeLoc)
                    {
                        filename = folder.Remove(0, location.Length);
                        fileLoc = storeLoc + filename;
                        Directory.Move(folder, fileLoc);
                        moved = true;
                    }
                }
            }
            if (!moved)
            {
                Directory.Delete(storeLoc);
                l.Dispatcher.Invoke(() => { l.Items.Add(getRemovedString(false, false, now)); });
            }
            else
            {
                if (CorR)
                {
                    var shf = new SHFILEOPSTRUCT();
                    shf.wFunc = FO_DELETE;
                    shf.fFlags = FOF_ALLOWUNDO;
                    shf.pFrom = storeLoc;
                    SHFileOperation(ref shf);
                    l.Dispatcher.Invoke(() => { l.Items.Add(getRemovedString(true, true, now)); });
                }
                else
                {
                    l.Dispatcher.Invoke(() => { l.Items.Add(getRemovedString(true, false, now)); });
                    moved = false;
                }
            }
        }

        private void setStore()
        {
            store = "$ Archived " + string.Format("{0:MM-dd-yyyy HH.mm}", DateTime.Now);
        }

        private void setLoc()
        {
            storeLoc = location + "\\" + store;
        }

        public static void setDayEnabled(string day, int val)
        {
            dayEnabled[day] = val;
            Console.WriteLine(dayEnabled[day]);
        }

        public Dictionary<string, int> getDayEnabled()
        {
            return dayEnabled;
        }

        public void setRunning(bool r)
        {
            this.running = r;
        }

        public bool getRunning()
        {
            return this.running;
        }

        public void setRepeatTime(string s)
        {
            int hour = Int32.Parse(s.Substring(0, 2));
            int min = Int32.Parse(s.Substring(3, 2));
            Console.WriteLine("Hour: " + hour);
            Console.WriteLine("Min: " + min);
            repeatTime = new DateTime(1997, 4, 1, hour, min, 0);
        }

        private string getRemovedString(bool removed, bool deleted, DateTime now)
        {
            if (removed)
            {
                if (deleted)
                {
                    return "Recycled: " + now.DayOfWeek + ", " + months[now.Month] + " " + now.Day + " @ " + now.Hour + ":" + now.Minute;

                }
                else
                {
                    return "Archived: " + now.DayOfWeek + ", " + months[now.Month] + " " + now.Day + " @ " + now.Hour + ":" + now.Minute;

                }
            }
            else
            {
                return "Nothing to be archived or recycled: " + now.DayOfWeek + ", " + months[now.Month] + " " + now.Day + " @ " + now.Hour + ":" + now.Minute;
            }

        }

        public bool getCorR()
        {
            return CorR;
        }

        public void setCorR(bool b)
        {
            CorR = b;
        }

        public void setLocation(string s)
        {
            location = @"" + s;
        }

        public bool getRemArchive()
        {
            return remArchive;
        }

        public void setRemArchive(bool b)
        {
            remArchive = b;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        public struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.U4)]
            public int wFunc;
            public string pFrom;
            public string pTo;
            public short fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp);

        public const int FO_DELETE = 3;
        public const int FOF_ALLOWUNDO = 0x40;
        public const int FOF_NOCONFIRMATION = 0x10;
    }
}
