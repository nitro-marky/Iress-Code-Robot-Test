using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toy_Robot.Interfaces;
using Toy_Robot.Types;

namespace Toy_Robot
{
    public class Commander : ICommander
    {
        private readonly IRobot _robot;

        public Commander(IRobot robot)
        {
            _robot = robot;
        }

        public void ProcessCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command)) return;

            // Split strings to find the action word
            var parts = command.Trim().Split(' ');
            var action = parts[0].ToUpper();

            switch (action)
            {
                case "PLACE":
                    if (parts.Length == 2)
                        ProcessPlaceCommand(parts[1]);
                    break;
                case "MOVE":
                    _robot.Move();
                    break;
                case "LEFT":
                    _robot.TurnLeft();
                    break;
                case "RIGHT":
                    _robot.TurnRight();
                    break;
                case "REPORT":
                    Console.WriteLine(_robot.Report());
                    break;
            }
        }

        //Find the X,Y and Direction as they are not split by spaces
        private void ProcessPlaceCommand(string parameters)
        {
            var parts = parameters.Split(',');
            if (parts.Length != 3) return;

            if (int.TryParse(parts[0], out int x) &&
                int.TryParse(parts[1], out int y) &&
                Enum.TryParse<Direction>(parts[2], true, out Direction direction))
            {
                _robot.Place(x, y, direction);
            }
        }
    }
}
