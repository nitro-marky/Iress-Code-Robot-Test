using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toy_Robot.Interfaces;

namespace Toy_Robot
{
    public class Table : ITable
    {
        // Table coordinate system: (0,0) is south-west corner, (4,4) is north-east corner
        // X increases eastward, Y increases northward
        private readonly int _width;   // X-axis dimension
        private readonly int _height;  // Y-axis dimension

        public Table(int width = 5, int height = 5)
        {
            _width = width;
            _height = height;
        }

        public bool IsValidPosition(int x, int y)
        {
            if (x < 0 || x >= _width) return false;
            if (y < 0 || y >= _height) return false;
            return true;
        }
    }
}
