using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toy_Robot.Types
{
        public enum Direction { NORTH, SOUTH, EAST, WEST }


        public record RobotState(Position Position, Direction Direction);

 
}
