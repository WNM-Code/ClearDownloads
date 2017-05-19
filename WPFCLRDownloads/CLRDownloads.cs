using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Microsoft.VisualBasic;

namespace CLRDownloads
{
    class Remover
    {
        string location = "";
        string store = "";
        string storeLoc = "";
        bool moved;
        static int[] dayEnabled = {0,0,0,0,0,0,0};

        public Remover()
        {
            location = @"d:\Downloads";
            store = "$ Removed " + string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
            storeLoc = location + "\\" + store;
            moved = false;
        }

        public void run()
        {
            DateTime now = DateTime.Now;
            DateTime before = DateTime.Now;
            //while (true)
            //{
            //    now = DateTime.Now;
            //    if (now.Date > before.Date && now.Hour > 4)
            //    {
                    Directory.CreateDirectory(storeLoc);
                    remove(location, now);
                    before = now;
                    setStore();
                    setLoc();
                //}
                //Thread.Sleep(300000);
            //}
        }

        public void remove(string path, DateTime now)
        {
            string[] folders = Directory.GetDirectories(path);
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
                if (!folder.Contains(location + "\\$ Removed "))
                {
                    filename = folder.Remove(0, location.Length);
                    fileLoc = storeLoc + filename;
                    Directory.Move(folder, fileLoc);
                    moved = true;
                }
            }
            if (!moved)
            {
                Directory.Delete(storeLoc);
            }
            else
            {
                Console.WriteLine("Removed @: " + now);
            }
        }

        private void setStore()
        {
            store = "$ Removed " + string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
        }

        private void setLoc()
        {
            storeLoc = location + "\\" + store;
        }

        public static void setDayEnabled(int day, int val)
        {
            dayEnabled[day] = val;
        }
    }
}
