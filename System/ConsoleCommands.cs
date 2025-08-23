using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sys = Cosmos.System;
using Cosmos.HAL;


namespace BradPCOS.System
{
    public static class ConsoleCommands
    {
        public static void RunCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                WriteMessage.WriteError("Please enter a valid command!");
                Help();
                return;
            }

            string[] words = command.Split(' ');
            string mainCommand = words[0].ToLower();

            switch (mainCommand)
            {
                case "info":
                    ShowSystemInfo();
                    break;
                case "help":
                    Help();
                    break;
                case "calendar":
                    ShowCurrentTime();
                    break;
                case "format":
                    FormatDisk();
                    break;
                case "space":
                    ShowFreeSpace();
                    break;
                case "dir":
                    ListDirectoryContents();
                    break;
                case "echo":
                    CreateFileWithContent(words);
                    break;
                case "cat":
                    DisplayFileContent(words);
                    break;
                case "del":
                    DeleteFile(words);
                    break;
                case "mkdir":
                    CreateDirectory(words);
                    break;
                case "cd":
                    ChangeDirectory(words);
                    break;
                default:
                    WriteMessage.WriteError($"Unknown command: {mainCommand}");
                    break;
            }
        }

        public static void Help()
        {
            Console.WriteLine("© 2025 CrazyBrad77 Studios Corporation. All Rights Reserved.");
            Console.WriteLine(@"
  help                 Show this help.
  echo <text>          Print text.
  calendar             Show current date/time.
  info                 About BradOS.
  dir [path]           List files in a directory.
  mkdir <dir>          Create a directory.
  cd <drive:>          Change drive (e.g. 'cd 0:').
  copy <src> <dest>    Copy a file (supports 0:\ style paths).
  cat <file>           Display file contents.
  del <file>           Delete defined files (e.g. del test.txt).
  format <drive>       Formats any drive (e.g. format 0:).
  space                Display drive space.

");
        }

        public static void ShowSystemInfo()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            WriteMessage.WriteLogo();
            Console.WriteLine();
            Console.Write(WriteMessage.CenterText("BradOS"));
            Console.Write(WriteMessage.CenterText(Kernel.Version));
            Console.Write(WriteMessage.CenterText("Created by CrazyBrad77"));
            Console.Write(WriteMessage.CenterText("https://youtube.com/@CrazyBrad77"));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void FormatDisk()
        {
            if (Kernel.FileSystem.Disks[0].Partitions.Count > 0)
            {
                Kernel.FileSystem.Disks[0].DeletePartition(0);
            }

            Kernel.FileSystem.Disks[0].Clear();
            long diskSizeMB = Kernel.FileSystem.Disks[0].Size / (1024 * 1024);
            Kernel.FileSystem.Disks[0].CreatePartition((int)diskSizeMB);
            Kernel.FileSystem.Disks[0].FormatPartition(0, "FAT32", true);

            WriteMessage.WriteOK("Success!");
            WriteMessage.WriteWarn("System will reboot in 3 seconds!");
            Sys.Power.Reboot();
        }

        public static void ShowFreeSpace()
        {
            long freeSpace = Kernel.FileSystem.GetAvailableFreeSpace(Kernel.CurrentPath);
            Console.WriteLine($"Free space: {freeSpace / 1024}KB");
        }

        public static void ShowCurrentTime()
        {
            try
            {
                string date = $"{RTC.DayOfTheMonth}/{RTC.Month}/{RTC.Year}";
                string time = $"{RTC.Hour}:{RTC.Minute}:{RTC.Second}";
                Console.WriteLine("© 2025 CrazyBrad77 Studios Corporation. All Rights Reserved.");
                Console.WriteLine("Time: " + time);
                Console.WriteLine("Date: " + date);
            }
            catch 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: RTC is not available on this platform. Error Code 1001");
            }
        }



        public static void ListDirectoryContents()
        {
            try
            {
                var directories = Directory.GetDirectories(Kernel.CurrentPath);
                var files = Directory.GetFiles(Kernel.CurrentPath);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Directories ({directories.Length})");
                Console.ForegroundColor = ConsoleColor.Gray;
                foreach (var directory in directories)
                {
                    Console.WriteLine(Path.GetFileName(directory));
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Files ({files.Length})");
                Console.ForegroundColor = ConsoleColor.Gray;
                foreach (var file in files)
                {
                    Console.WriteLine(Path.GetFileName(file));
                }
            }
            catch (Exception ex)
            {
                WriteMessage.WriteError($"Error listing directory: {ex.Message}");
            }
        }

        public static void CreateFileWithContent(string[] words)
        {
            if (words.Length < 2)
            {
                WriteMessage.WriteError("Invalid Syntax! Usage: echo <text> > <filename>");
                return;
            }

            string fullCommand = string.Join(" ", words, 1, words.Length - 1);
            int separatorIndex = fullCommand.LastIndexOf('>');

            if (separatorIndex == -1)
            {
                WriteMessage.WriteError("Invalid Syntax! Missing '>' separator");
                return;
            }

            string content = fullCommand.Substring(0, separatorIndex).Trim();
            string filePath = fullCommand.Substring(separatorIndex + 1).Trim();

            if (!Path.IsPathRooted(filePath))
            {
                filePath = Path.Combine(Kernel.CurrentPath, filePath);
            }

            try
            {
                File.WriteAllText(filePath, content);
                WriteMessage.WriteOK($"File created: {filePath}");
            }
            catch (Exception ex)
            {
                WriteMessage.WriteError($"Error creating file: {ex.Message}");
            }
        }

        public static void DisplayFileContent(string[] words)
        {
            if (words.Length < 2)
            {
                WriteMessage.WriteError("Invalid Syntax! Usage: cat <filename>");
                return;
            }

            string filePath = words[1];
            if (!Path.IsPathRooted(filePath))
            {
                filePath = Path.Combine(Kernel.CurrentPath, filePath);
            }

            try
            {
                if (File.Exists(filePath))
                {
                    string content = File.ReadAllText(filePath);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(content);
                }
                else
                {
                    WriteMessage.WriteError($"File not found: {filePath}");
                }
            }
            catch (Exception ex)
            {
                WriteMessage.WriteError($"Error reading file: {ex.Message}");
            }
        }

        public static void DeleteFile(string[] words)
        {
            if (words.Length < 2)
            {
                WriteMessage.WriteError("Invalid Syntax! Usage: del <filename>");
                return;
            }

            string filePath = words[1];
            if (!Path.IsPathRooted(filePath))
            {
                filePath = Path.Combine(Kernel.CurrentPath, filePath);
            }

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    WriteMessage.WriteOK($"Deleted: {filePath}");
                }
                else
                {
                    WriteMessage.WriteError($"File not found: {filePath}");
                }
            }
            catch (Exception ex)
            {
                WriteMessage.WriteError($"Error deleting file: {ex.Message}");
            }
        }

        public static void CreateDirectory(string[] words)
        {
            if (words.Length < 2)
            {
                WriteMessage.WriteError("Invalid Syntax! Usage: mkdir <dirname>");
                return;
            }

            string dirPath = words[1];
            if (!Path.IsPathRooted(dirPath))
            {
                dirPath = Path.Combine(Kernel.CurrentPath, dirPath);
            }

            try
            {
                Directory.CreateDirectory(dirPath);
                WriteMessage.WriteOK($"Directory created: {dirPath}");
            }
            catch (Exception ex)
            {
                WriteMessage.WriteError($"Error creating directory: {ex.Message}");
            }
        }

        public static void ChangeDirectory(string[] words)
        {
            if (words.Length < 2)
            {
                Kernel.CurrentPath = @"0:\";
                return;
            }

            string targetPath = words[1];

            if (targetPath == "..")
            {
                if (Kernel.CurrentPath != @"0:\")
                {
                    Kernel.CurrentPath = Path.GetDirectoryName(Kernel.CurrentPath.TrimEnd('\\')) + "\\";
                }
                return;
            }

            if (!Path.IsPathRooted(targetPath))
            {
                targetPath = Path.Combine(Kernel.CurrentPath, targetPath);
            }

            if (!targetPath.EndsWith("\\"))
            {
                targetPath += "\\";
            }

            try
            {
                if (Directory.Exists(targetPath))
                {
                    Kernel.CurrentPath = targetPath;
                }
                else
                {
                    WriteMessage.WriteError($"Directory not found: {targetPath}");
                }
            }
            catch (Exception ex)
            {
                WriteMessage.WriteError($"Error changing directory: {ex.Message}");
            }
        }
    }
}