using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Permissions;
using static System.Console;
using static System.ConsoleColor;
using System.Threading;


namespace System_Watcher
{
    class Program
    {

        public static bool active = false;

        public static string Blacklist = "";
        static void Main(string[] args)
        {
            Title = "Tool by I LOVE PIZZA#0001";


            Write("Directory to Scan: ");

            string direc = ReadLine();

            if(!Directory.Exists(direc))
            {
                Console.WriteLine(direc + " does not exists");
                Console.Read();
                return;
            }


            WriteLine("Blacklist a Folder? [y][n]");
            ConsoleKey key = Console.ReadKey().Key;
            if (key == ConsoleKey.Y)
            {
                WriteLine("Type in the Directory: ");
                Blacklist = ReadLine();
            }
            WriteLine("Filter for temp dirs etc...? [y][n]");
            ConsoleKey key1 = Console.ReadKey().Key;
            if (key1 == ConsoleKey.Y)
            {
                WriteLine("Setting up...");
                StartMonitorWithFilter(direc);
            }
            else
            {
                WriteLine("Setting up...");
                StartMonitorWithoutFilter(direc);
            }



            
                
            



            Thread t = new Thread(OutputLoop);
            t.Start();

            Clear();
            WriteLine("Active");
            WriteLine("Press Q to exit");

            active = true;
            while (Console.ReadKey().Key != ConsoleKey.Q)
            { }
        }

        public static void StartMonitorWithoutFilter(string path)
        {
            try
            {
                var dir = Directory.GetDirectories(path);
                foreach (var i in dir)
                {
                    if (i != Blacklist)
                    {
                        //WriteLine("Watching " + i);
                        Monitor(i);
                        StartMonitorWithoutFilter(i);
                    }
                }
            }
            catch { }
        }
        public static void StartMonitorWithFilter(string path)
        {
            string Username = Environment.UserName;
            try
            {
                var dir = Directory.GetDirectories(path);
                foreach (var i in dir) 
                {
                    if (i != Blacklist && i != @"C:\Users\All Users\NVIDIA Corporation\NvTelemetry" && i != @"C:\Users\" + Username + @"\AppData\Local\Google\Chrome" && i != @"C:\Windows\Prefetch" && i != @"C:\Users\" + Username + @"\AppData\Local\Temp" && i != @"C:\ProgramData\NVIDIA Corporation" && i != @"C:\Users\" + Username + @"\AppData\Local\FortniteGame" && i != @"C:\Users\" + Username + @"\AppData\Roaming\Discord")
                    {
                        //WriteLine("Watching " + i);
                        Monitor(i);
                        StartMonitorWithFilter(i);
                        
                    }
                }
            }
            catch { }
        }


        public static void Monitor(string path)
        {
            // instantiate the object
            var fileSystemWatcher = new FileSystemWatcher();

            // Associate event handlers with the events
            fileSystemWatcher.Created += FileSystemWatcher_Created;
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
            fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;
            

            // tell the watcher where to look
            fileSystemWatcher.Path = path;

            try
            {
                // You must add this line - this allows events to fire.
                fileSystemWatcher.EnableRaisingEvents = true;
            }
            catch { }

           
        }


        public static void OutputLoop()
        {
            while (true)
            {
                File.WriteAllLines("Output.txt", Output);
                System.Threading.Thread.Sleep(1000);
            }
        }



        static List<string> Output = new List<string>();


        private static void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            if (active && e.FullPath != Directory.GetCurrentDirectory() + @"\Output.txt")
            {
                ForegroundColor = Yellow;
                WriteLine($"renamed - {e.OldFullPath} to {e.FullPath}");
                Output.Add($"renamed - {e.OldFullPath} to {e.FullPath}");
            }
        }

        private static void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (active && e.FullPath != Directory.GetCurrentDirectory() + @"\Output.txt")
            {
                ForegroundColor = Red;
                WriteLine($"deleted - {e.FullPath}");
                Output.Add($"deleted - {e.FullPath}");
            }
        }

        private static void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (active && e.FullPath != Directory.GetCurrentDirectory() + @"\Output.txt")
            {
                ForegroundColor = Green;
                WriteLine($"changed - {e.FullPath}");
                Output.Add($"changed - {e.FullPath}");
            }
        }

        private static void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (active && e.FullPath != Directory.GetCurrentDirectory() + @"\Output.txt")
            {
                ForegroundColor = Blue;
                WriteLine($"created - {e.FullPath}");
                Output.Add($"created - {e.FullPath}");
            }
        }
    }
}
