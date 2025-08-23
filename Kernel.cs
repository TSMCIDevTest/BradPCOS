using BradPCOS.System;
using Cosmos.System.FileSystem;
using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using System.IO;
using Cosmos.HAL;

namespace BradPCOS
{
    public class Kernel : Sys.Kernel
    {
        public const string Version = "1.0.0";
        public static string CurrentPath = @"0:\";
        public static CosmosVFS FileSystem;

        protected override void BeforeRun()
        {
            Console.SetWindowSize(90, 30);
            Console.OutputEncoding = Cosmos.System.ExtendedASCII.CosmosEncodingProvider.Instance.GetEncoding(437);

            InitializeFileSystem();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Booting BradOS {Version}");
            ConsoleCommands.ShowCurrentTime();
            Console.WriteLine("BradOS");
            Console.WriteLine(Version);
            Console.WriteLine("Created by CrazyBrad77");
            Console.WriteLine("https://youtube.com/@CrazyBrad77");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void InitializeFileSystem()
        {
            FileSystem = new CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(FileSystem);
        }


        protected override void Run()
        {
            try
            {
                Console.Write($"{CurrentPath}>");
                var command = Console.ReadLine();
                ConsoleCommands.RunCommand(command);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}