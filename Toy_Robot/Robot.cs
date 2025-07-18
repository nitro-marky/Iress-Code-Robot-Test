using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toy_Robot.Interfaces;
using Toy_Robot.Types;

namespace Toy_Robot
{
    public class Robot : IRobot
    {
        private readonly ITable _table;
        private Position? _position; //Nullable here to remove warning
        private Direction _direction;
        private bool _isPlaced;

        public Robot(ITable table)
        {
            _table = table;
            _isPlaced = false;
        }

        public bool IsPlaced
        {
            get { return _isPlaced; }
        }

        public bool Place(int x, int y, Direction direction)
        {
            if (!_table.IsValidPosition(x, y))
                return false;

            _position = new Position(x, y);
            _direction = direction;
            _isPlaced = true;
            return true;
        }

        public bool Move()
        {
            if (IsPlacedAndPositioned() == false) return false;

            int newX = _position.X;
            int newY = _position.Y;

            switch (_direction)
            {
                case Direction.NORTH:
                    newY++;
                    break;
                case Direction.SOUTH:
                    newY--;
                    break;
                case Direction.EAST:
                    newX++;
                    break;
                case Direction.WEST:
                    newX--;
                    break;
            }

            if (!_table.IsValidPosition(newX, newY))
                return false;

            _position.X = newX;
            _position.Y = newY;
            return true;
        }

        public void TurnLeft()
        {
            if (IsPlacedAndPositioned() == false) return;

            switch (_direction)
            {
                case Direction.NORTH:
                    _direction = Direction.WEST;
                    break;
                case Direction.WEST:
                    _direction = Direction.SOUTH;
                    break;
                case Direction.SOUTH:
                    _direction = Direction.EAST;
                    break;
                case Direction.EAST:
                    _direction = Direction.NORTH;
                    break;
            }
        }

        public void TurnRight()
        {
            if (IsPlacedAndPositioned() == false) return;

            switch (_direction)
            {
                case Direction.NORTH:
                    _direction = Direction.EAST;
                    break;
                case Direction.EAST:
                    _direction = Direction.SOUTH;
                    break;
                case Direction.SOUTH:
                    _direction = Direction.WEST;
                    break;
                case Direction.WEST:
                    _direction = Direction.NORTH;
                    break;
            }
        }

        public string Report()
        {
            if (!_isPlaced || _position == null)
                return "Robot not placed";

            return $"Output: {_position.X},{_position.Y},{_direction}";
        }

        private bool IsPlacedAndPositioned()
        {
            if (!_isPlaced || _position == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
