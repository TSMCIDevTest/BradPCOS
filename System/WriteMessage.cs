using System;

namespace BradPCOS.System
{
    public static class WriteMessage
    {
        public static void WriteError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[Error] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(error);
        }

        public static void WriteWarn(string warn)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[Warning] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(warn);
        }

        public static void WriteInfo(string info)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[Info] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(info);
        }

        public static void WriteOK(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[OK] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public static void WriteLogo()
        {
            string[] logoLines = {
                "                    @                           @                     ",
                "                   @@                           @@                    ",
                "                  @@@                           @@@                   ",
                "         @        @@@@                         @@@@        @          ",
                "          @@      @@@@@@                     @@@@@       @@           ",
                "           @@@      @@@@@                   @@@@@      @@@            ",
                "            @@@@     @@@@@@               @@@@@@     @@@@             ",
                "            @@@@      @@@@@@             @@@@@@      @@@@             ",
                "             @@@@@    @@@@@@@@@@@@@@@@@@@@@@@@@    @@@@@              ",
                "         @   @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@   @          ",
                "         @@@   @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@   @@@          ",
                "          @@@@@  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ @@@@@@           ",
                "           @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@            ",
                "              @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@               ",
                "                 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                  ",
                "                  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                   ",
                "                  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                   ",
                "                   @@@@@ @@@@@@@@@@@@@@@@@@@ @@@@@                    ",
                "                    @@@@  @@@@@@@@@@@@@@@@@  @@@@                     ",
                "                     @@@@@@@@@@@@@@@@@@@@@@@@@@@                      ",
                "                      @@@@@@@@@@@@@@@@@@@@@@@@@                       ",
                "                        @@@@@@@@@@@@@@@@@@@@@                         ",
                "                           @@@@@@@@@@@@@@@                            "
            };

            foreach (var line in logoLines)
            {
                Console.WriteLine(CenterText(line));
            }
        }

        public static string CenterText(string text)
        {
            int consoleWidth = 90;
            int padding = (consoleWidth - text.Length) / 2;
            return text.PadLeft(padding + text.Length).PadRight(consoleWidth);
        }
    }
}