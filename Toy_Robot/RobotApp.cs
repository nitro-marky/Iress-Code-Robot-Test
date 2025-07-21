using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toy_Robot.Interfaces;

namespace Toy_Robot
{
    public class RobotApp
    {
        private static RobotApp instance = null;
        private ICommander _processor;

        // Singleton
        private RobotApp(ICommander processor)
        {
            _processor = processor;
        }

        public static RobotApp getRobotApp(ICommander processor)
        {
            if (instance == null) { 
                instance = new RobotApp(processor);
            }
            return instance;
        }


        public void Run(string[] args)
        {
            // Check if file argument provided
            if (args.Length > 0)
            {
                RunFromFile(args[0]);
            }
            else
            {
                // Check for default commands.txt file in same folder
                string defaultFile = "robot-test.txt";
                if (File.Exists(defaultFile))
                {
                    Console.WriteLine($"Found {defaultFile} in current directory. Using it as default test.");
                    RunFromFile(defaultFile);
                }
            }
        }

        private void RunFromFile(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine($"Error: File '{filename}' not found.");
                    Console.WriteLine("Switching to interactive mode...");
                    return;
                }

                Console.WriteLine($"Reading commands from: {filename}");
                var lines = File.ReadAllLines(filename);

                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var trimmedLine = line.Trim();

                        // Check if it's a section header (starts with letter followed by ')')
                        if (IsSectionHeader(trimmedLine))
                        {
                            Console.WriteLine(); // Add blank line before section
                            Console.WriteLine(trimmedLine); // Display section header as-is
                            continue;
                        }

                        // Check if it's a comment (starts with #)
                        if (trimmedLine.StartsWith("#"))
                        {
                            continue; 
                        }

                        var command = trimmedLine.Split(' ')[0].ToUpper();

                        // Only show "Executing:" for valid robot commands (excluding REPORT)
                        if (IsValidRobotCommand(command) && command != "REPORT")
                        {
                            Console.WriteLine($"Executing: {trimmedLine}");
                        }

                        _processor.ProcessCommand(trimmedLine);
                    }
                }

                Console.WriteLine("Finished executing commands from file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }

        private bool IsSectionHeader(string line)
        {
            // Check if line matches pattern like "a)----------------"
            return line.Length >= 2 &&
                   char.IsLetter(line[0]) &&
                   line[1] == ')';
        }

        private bool IsValidRobotCommand(string command)
        {
            return command == "PLACE" || command == "MOVE" ||
                   command == "LEFT" || command == "RIGHT" || command == "REPORT";
        }
    }
}

