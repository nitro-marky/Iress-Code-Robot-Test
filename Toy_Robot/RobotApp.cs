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
                    return;
                }

                Console.WriteLine($"Reading commands from: {filename}");
                var lines = File.ReadAllLines(filename);

                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var command = line.Trim().Split(' ')[0].ToUpper();

                        if (IsValidRobotCommand(command))
                        {
                            Console.WriteLine($"{line}");
                        }

                        _processor.ProcessCommand(line);
                    }
                }

                Console.WriteLine("Finished executing commands from file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }

        private bool IsValidRobotCommand(string command)
        {
            return command == "PLACE" || command == "MOVE" ||
                   command == "LEFT" || command == "RIGHT" || command == "REPORT";
        }
    }
}

