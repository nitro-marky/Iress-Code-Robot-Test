using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toy_Robot.Types;

namespace Toy_Robot.Interfaces
{
    // Interface Segregation
    public interface IPlaceable
    {
        bool Place(int x, int y, Direction direction);
    }

    public interface IMovable
    {
        bool Move();
    }

    public interface IRotatable
    {
        void TurnLeft();
        void TurnRight();
    }

    public interface IReportable
    {
        string Report();
    }

    // Combined robot interface
    public interface IRobot : IPlaceable, IMovable, IRotatable, IReportable
    {
        bool IsPlaced { get; }
    }

    public interface ITable
    {
        bool IsValidPosition(int x, int y);
    }

    public interface ICommander
    {
        void ProcessCommand(string command);
    }

    public interface IApplication
    {
        void Run(string[] args);
    }


}
