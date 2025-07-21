using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Toy_Robot;
using Toy_Robot.Types;

namespace ToyRobot.Tests
{
    [TestClass]
    public class TableTests
    {
        [TestMethod]
        public void DefaultSize_Creates5x5Table()
        {
            var table = new Table();

            Assert.IsTrue(table.IsValidPosition(0, 0));
            Assert.IsTrue(table.IsValidPosition(4, 4));
            Assert.IsFalse(table.IsValidPosition(5, 5));
        }

        [TestMethod]
        public void IsValidPosition_ValidPositions_ReturnsTrue()
        {
            var table = new Table(5, 5);

            Assert.IsTrue(table.IsValidPosition(0, 0));    // Bottom-left corner
            Assert.IsTrue(table.IsValidPosition(4, 4));    // Top-right corner
            Assert.IsTrue(table.IsValidPosition(2, 3));    // Middle position
            Assert.IsTrue(table.IsValidPosition(0, 4));    // Top-left corner
            Assert.IsTrue(table.IsValidPosition(4, 0));    // Bottom-right corner
        }

        [TestMethod]
        public void IsValidPosition_InvalidPositions_ReturnsFalse()
        {
            var table = new Table(5, 5);

            Assert.IsFalse(table.IsValidPosition(-1, 0));  // Negative X
            Assert.IsFalse(table.IsValidPosition(0, -1));  // Negative Y
            Assert.IsFalse(table.IsValidPosition(5, 0));   // X out of bounds
            Assert.IsFalse(table.IsValidPosition(0, 5));   // Y out of bounds
            Assert.IsFalse(table.IsValidPosition(5, 5));   // Both out of bounds
            Assert.IsFalse(table.IsValidPosition(-1, -1)); // Both negative
        }

    }

    [TestClass]
    public class PositionTests
    {
        [TestMethod]
        public void Constructor_SetsXAndYCorrectly()
        {
            var position = new Position(3, 7);

            Assert.AreEqual(3, position.X);
            Assert.AreEqual(7, position.Y);
        }

        [TestMethod]
        public void Properties_CanBeModified()
        {
            var position = new Position(1, 2);

            // Act
            position.X = 5;
            position.Y = 8;

            // Assert
            Assert.AreEqual(5, position.X);
            Assert.AreEqual(8, position.Y);
        }
    }

    [TestClass]
    public class RobotTests
    {
        private Table _table;
        private Robot _robot;

        [TestInitialize]
        public void Setup()
        {
            _table = new Table(5, 5);
            _robot = new Robot(_table);
        }

        [TestMethod]
        public void Constructor_RobotNotPlaced()
        {
            Assert.IsFalse(_robot.IsPlaced);
            Assert.AreEqual("Robot not placed", _robot.Report());
        }

        [TestMethod]
        public void Place_ValidPosition_Success()
        {
            var result = _robot.Place(2, 3, Direction.NORTH);

            Assert.IsTrue(result);
            Assert.IsTrue(_robot.IsPlaced);
            Assert.AreEqual("Output: 2,3,NORTH", _robot.Report());
        }

        [TestMethod]
        public void Place_InvalidPosition_Fails()
        {
            var result = _robot.Place(-1, 2, Direction.NORTH);

            Assert.IsFalse(result);
            Assert.IsFalse(_robot.IsPlaced);
            Assert.AreEqual("Robot not placed", _robot.Report());
        }

        [TestMethod]
        public void Place_CanReplaceExistingPosition()
        {
            _robot.Place(1, 1, Direction.NORTH);

            var result = _robot.Place(3, 4, Direction.SOUTH);

            Assert.IsTrue(result);
            Assert.AreEqual("Output: 3,4,SOUTH", _robot.Report());
        }

        [TestMethod]
        public void Move_NotPlaced_ReturnsFalse()
        {
            var result = _robot.Move();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Move_ValidMove_Success()
        {
            _robot.Place(2, 2, Direction.NORTH);

            var result = _robot.Move();

            Assert.IsTrue(result);
            Assert.AreEqual("Output: 2,3,NORTH", _robot.Report());
        }

        [TestMethod]
        public void Move_WouldFallOff_Ignored()
        {
            _robot.Place(0, 0, Direction.SOUTH);

            var result = _robot.Move();

            Assert.IsFalse(result);
            Assert.AreEqual("Output: 0,0,SOUTH", _robot.Report()); // Position unchanged
        }

        [TestMethod]
        public void Move_AllDirections_WorksCorrectly()
        {
            // Test NORTH
            _robot.Place(2, 2, Direction.NORTH);
            _robot.Move();
            Assert.AreEqual("Output: 2,3,NORTH", _robot.Report());

            // Test SOUTH
            _robot.Place(2, 2, Direction.SOUTH);
            _robot.Move();
            Assert.AreEqual("Output: 2,1,SOUTH", _robot.Report());

            // Test EAST
            _robot.Place(2, 2, Direction.EAST);
            _robot.Move();
            Assert.AreEqual("Output: 3,2,EAST", _robot.Report());

            // Test WEST
            _robot.Place(2, 2, Direction.WEST);
            _robot.Move();
            Assert.AreEqual("Output: 1,2,WEST", _robot.Report());
        }

        [TestMethod]
        public void TurnLeft_NotPlaced_DoesNothing()
        {
            _robot.TurnLeft();

            Assert.IsFalse(_robot.IsPlaced);
        }

        [TestMethod]
        public void TurnLeft_AllDirections_WorksCorrectly()
        {
            _robot.Place(0, 0, Direction.NORTH);
            _robot.TurnLeft();
            Assert.AreEqual("Output: 0,0,WEST", _robot.Report());

            _robot.TurnLeft();
            Assert.AreEqual("Output: 0,0,SOUTH", _robot.Report());

            _robot.TurnLeft();
            Assert.AreEqual("Output: 0,0,EAST", _robot.Report());

            _robot.TurnLeft();
            Assert.AreEqual("Output: 0,0,NORTH", _robot.Report()); // Full circle
        }

        [TestMethod]
        public void TurnRight_NotPlaced_DoesNothing()
        {
            _robot.TurnRight();

            Assert.IsFalse(_robot.IsPlaced);
        }

        [TestMethod]
        public void TurnRight_AllDirections_WorksCorrectly()
        {
            _robot.Place(0, 0, Direction.NORTH);
            _robot.TurnRight();
            Assert.AreEqual("Output: 0,0,EAST", _robot.Report());

            _robot.TurnRight();
            Assert.AreEqual("Output: 0,0,SOUTH", _robot.Report());

            _robot.TurnRight();
            Assert.AreEqual("Output: 0,0,WEST", _robot.Report());

            _robot.TurnRight();
            Assert.AreEqual("Output: 0,0,NORTH", _robot.Report()); // Full circle
        }
    }

    [TestClass]
    public class CommandProcessorTests
    {
        private Table _table;
        private Robot _robot;
        private Commander _processor;

        [TestInitialize]
        public void Setup()
        {
            _table = new Table(5, 5);
            _robot = new Robot(_table);
            _processor = new Commander(_robot);
        }

        [TestMethod]
        public void ProcessCommand_PlaceCommand_WorksCorrectly()
        {
            _processor.ProcessCommand("PLACE 1,2,NORTH");

            Assert.IsTrue(_robot.IsPlaced);
            Assert.AreEqual("Output: 1,2,NORTH", _robot.Report());
        }

        [TestMethod]
        public void ProcessCommand_InvalidPlaceCommand_Ignored()
        {
            _processor.ProcessCommand("PLACE invalid");
            _processor.ProcessCommand("PLACE 1,2");
            _processor.ProcessCommand("PLACE a,b,NORTH");

            Assert.IsFalse(_robot.IsPlaced);
        }

        [TestMethod]
        public void ProcessCommand_MoveCommand_WorksCorrectly()
        {
            _robot.Place(1, 1, Direction.NORTH);

            _processor.ProcessCommand("MOVE");

            Assert.AreEqual("Output: 1,2,NORTH", _robot.Report());
        }

        [TestMethod]
        public void ProcessCommand_LeftCommand_WorksCorrectly()
        {
            _robot.Place(1, 1, Direction.NORTH);

            _processor.ProcessCommand("LEFT");

            Assert.AreEqual("Output: 1,1,WEST", _robot.Report());
        }

        [TestMethod]
        public void ProcessCommand_RightCommand_WorksCorrectly()
        {
            _robot.Place(1, 1, Direction.NORTH);

            _processor.ProcessCommand("RIGHT");

            Assert.AreEqual("Output: 1,1,EAST", _robot.Report());
        }

        [TestMethod]
        public void ProcessCommand_ReportCommand_DoesNotCrash()
        {
            _robot.Place(3, 4, Direction.SOUTH);

            //Verify it doesn't throw an exception
            _processor.ProcessCommand("REPORT");

            // Verify robot state is still correct
            Assert.AreEqual("Output: 3,4,SOUTH", _robot.Report());
        }

        [TestMethod]
        public void ProcessCommand_InvalidCommand_Ignored()
        {
            _robot.Place(1, 1, Direction.NORTH);

            _processor.ProcessCommand("INVALID");
            _processor.ProcessCommand("");
            _processor.ProcessCommand("   ");

            Assert.AreEqual("Output: 1,1,NORTH", _robot.Report()); // Unchanged
        }

        [TestMethod]
        public void ProcessCommand_CaseInsensitive_WorksCorrectly()
        {
            _processor.ProcessCommand("place 1,1,north");
            _processor.ProcessCommand("move");
            _processor.ProcessCommand("left");
            _processor.ProcessCommand("right");

            Assert.AreEqual("Output: 1,2,NORTH", _robot.Report());
        }
    }

    [TestClass]
    public class IntegrationTests
    {
        private Table _table;
        private Robot _robot;
        private Commander _processor;

        [TestInitialize]
        public void Setup()
        {
            _table = new Table(5, 5);
            _robot = new Robot(_table);
            _processor = new Commander(_robot);
        }

        [TestMethod]
        public void TestCase1_PlaceMoveReport()
        {
            // Example from requirements:
            // PLACE 0,0,NORTH
            // MOVE
            // REPORT
            // Expected Output: 0,1,NORTH

            _processor.ProcessCommand("PLACE 0,0,NORTH");
            _processor.ProcessCommand("MOVE");
            // Instead of testing console Output, test robot state directly
            Assert.AreEqual("Output: 0,1,NORTH", _robot.Report());
        }

        [TestMethod]
        public void TestCase2_PlaceLeftReport()
        {
            // Example from requirements:
            // PLACE 0,0,NORTH
            // LEFT
            // REPORT
            // Expected Output: 0,0,WEST

            _processor.ProcessCommand("PLACE 0,0,NORTH");
            _processor.ProcessCommand("LEFT");
            Assert.AreEqual("Output: 0,0,WEST", _robot.Report());
        }

        [TestMethod]
        public void TestCase3_ComplexMovement()
        {
            // Example from requirements:
            // PLACE 1,2,EAST
            // MOVE
            // MOVE
            // LEFT
            // MOVE
            // REPORT
            // Expected Output: 3,3,NORTH

            _processor.ProcessCommand("PLACE 1,2,EAST");
            _processor.ProcessCommand("MOVE");
            _processor.ProcessCommand("MOVE");
            _processor.ProcessCommand("LEFT");
            _processor.ProcessCommand("MOVE");
            _processor.ProcessCommand("REPORT");
            Assert.AreEqual("Output: 3,3,NORTH", _robot.Report());
        }

        [TestMethod]
        public void TestCase4_CommandsBeforePlaceIgnored()
        {
            // Commands before PLACE should be ignored
            _processor.ProcessCommand("MOVE");
            _processor.ProcessCommand("LEFT");
            _processor.ProcessCommand("RIGHT");

            // Robot should still not be placed
            Assert.IsFalse(_robot.IsPlaced);
            Assert.AreEqual("Robot not placed", _robot.Report());

            _processor.ProcessCommand("PLACE 2,2,SOUTH");
            Assert.AreEqual("Output: 2,2,SOUTH", _robot.Report());
        }

        [TestMethod]
        public void TestCase5_PreventFalling()
        {
            // Test that robot doesn't fall off table
            _processor.ProcessCommand("PLACE 0,0,SOUTH");
            _processor.ProcessCommand("MOVE"); // Should be ignored
            Assert.AreEqual("Output: 0,0,SOUTH", _robot.Report());

            _processor.ProcessCommand("PLACE 4,4,NORTH");
            _processor.ProcessCommand("MOVE"); // Should be ignored
            Assert.AreEqual("Output: 4,4,NORTH", _robot.Report());
        }

        [TestMethod]
        public void TestCase6_MultipleCommands()
        {
            // Test a sequence of valid commands
            _processor.ProcessCommand("PLACE 1,1,NORTH");
            Assert.AreEqual("Output: 1,1,NORTH", _robot.Report());

            _processor.ProcessCommand("MOVE");
            Assert.AreEqual("Output: 1,2,NORTH", _robot.Report());

            _processor.ProcessCommand("RIGHT");
            Assert.AreEqual("Output: 1,2,EAST", _robot.Report());

            _processor.ProcessCommand("MOVE");
            Assert.AreEqual("Output: 2,2,EAST", _robot.Report());

            _processor.ProcessCommand("LEFT");
            _processor.ProcessCommand("LEFT");
            Assert.AreEqual("Output: 2,2,WEST", _robot.Report());
        }

        [TestMethod]
        public void TestCase7_BoundaryTesting()
        {
            // Test all four corners and edges

            // Bottom-left corner
            _processor.ProcessCommand("PLACE 0,0,WEST");
            _processor.ProcessCommand("MOVE"); // Should be ignored
            Assert.AreEqual("Output: 0,0,WEST", _robot.Report());

            // Top-right corner
            _processor.ProcessCommand("PLACE 4,4,EAST");
            _processor.ProcessCommand("MOVE"); // Should be ignored
            Assert.AreEqual("Output: 4,4,EAST", _robot.Report());

            // Valid edge movement
            _processor.ProcessCommand("PLACE 0,0,NORTH");
            _processor.ProcessCommand("MOVE");
            Assert.AreEqual("Output: 0,1,NORTH", _robot.Report());
        }
    }
}
