using System;
using Toy_Robot;
using Toy_Robot.Interfaces;
using Toy_Robot.Types;

public class Program
{
    public static void Main(string[] args)
    {
        ITable table = new Table();  

        IRobot robot = new Robot(table);
        ICommander processor = new Commander(robot);
        var app = RobotApp.getRobotApp(processor);

        app.Run(args);
    }
}
