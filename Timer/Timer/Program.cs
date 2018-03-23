using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Timer
{
    class Program
    {
        static DateTime startTime;
        static DateTime endTime;
        static TimeSpan allTime;
        static TimeSpan sumTime = TimeSpan.Zero;
        static TimeSpan other = TimeSpan.Zero;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Приложение ожидает открытия Visual Studio...\n\n");
            Console.ResetColor();

            Update();

            Console.ReadLine();
        }

        static void RewriteTimeInFile(TimeSpan timeSpan)
        {
            TimeSpan time = timeSpan;

            string oldTime = TimeSpan.Zero.ToString();
            string fileName = "AllTimeInVisualStudio.txt";

            if (File.Exists(fileName) == true)
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    oldTime = reader.ReadLine();                    
                }
            }

            time += TimeSpan.Parse(oldTime); ;            
     
            using (StreamWriter writer = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                writer.Write(time);
            }
        }

        static void Update()
        {            
            bool wasOpen = false;
            bool isOpen = false;

            while (true)
            {
                Thread.Sleep(1000);

                var openVS = System.Diagnostics.Process.GetProcessesByName("devenv");

                try
                {
                    string name = openVS[0].ProcessName;

                    if (name == "devenv" && (wasOpen == false && isOpen == false))
                    {
                        other = TimeSpan.Zero;
                        wasOpen = true;
                        isOpen = true;
                        startTime = DateTime.Now;
                        Console.WriteLine("(First start)Visual studio was started in: " + startTime.ToShortTimeString());                        
                    }
                    else if (name == "devenv" && (wasOpen == true && isOpen == false))
                    {
                        other = TimeSpan.Zero;
                        startTime = DateTime.Now;
                        Console.WriteLine("Visual studio was started in: " + DateTime.Now.ToShortTimeString());
                        isOpen = true;
                    }
                }
                catch (Exception)
                {
                    if (wasOpen == true && isOpen == true)
                    {
                        isOpen = false;
                        endTime = DateTime.Now;
                        allTime = endTime - startTime;
                        sumTime += allTime;
                        Console.WriteLine("Visual studio was closed in: " + endTime.ToShortTimeString());
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("-> Общее время работы: " + sumTime);
                        Console.ResetColor();
                        other = allTime;
                        RewriteTimeInFile(other);
                    }                  
                }
            }
        }
    }
}
