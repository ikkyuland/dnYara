﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using dnYara;

namespace YaraInteractive
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new YaraContext())
            {
                Console.WriteLine("# Welcome to Yara Interactive Console...");
                while (true)
                {
                    Console.Write("> ");

                    string command = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(command))
                        continue;

                    bool isManagedCmd = CmdHandler.ExecuteCmd(command);

                    if (!isManagedCmd)
                        Console.WriteLine(":Err: Unknown command...");
                }
            }
        }

    }
}
