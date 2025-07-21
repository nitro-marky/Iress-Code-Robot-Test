# Toy Robot - Mark Addis 

This guide explains how to test the Toy Robot simulator using the provided test files and interactive mode.
This has been written in C# .NET 8.0

### Requirements
## .Net SDK

**Required to run:** .NET 8.0 SDK installed - Choose the SDK (not just runtime or desktop)<br>

*Not 100% necessary as you can run `dotnet run` - Build Project (`dotnet build`)*

Test Files Included
The project includes two test files in the root directory:<br>

robot-test.txt - Basic functionality tests prodivded with the brief<br>
robot-test2.txt - Comprehensive in-depth testing

## Testing Methods - All these methods should be used in a command prompt opened in Toy_Robot Folder
### Method 1: Using Provided Test Files

Quick Test - Open a console window in the project folder (Toy_Robot) <br>

`dotnet run -- robot-test.txt`

Comprehensive Test (In-Depth)

`dotnet run -- robot-test2.txt`

This is the expected outcome from running a test file:<br>

![Expected output of a test file](https://github.com/nitro-marky/Robot-Test/blob/main/RobotTest2.png)

### Method 2: Custom Test Files
Create your own test file and run it:<br>
`dotnet run -- "C:\Code\Toy Robot\Toy_Robot\myTests.txt"`

## Running Automated Tests

Download the ToyRobotTest folder and place it in the same folder as the main project.<be>

This is what the outcome of the test file is, though the first time it is run, it may show some warnings but will still finish:<br>

![Expected output of automated tests, the first run may contain warnings, subsequent runs should look like this](https://github.com/nitro-marky/Robot-Test/blob/main/RobotTest.jpg)

Execute the automated test suite in the Toy_Robot folder (Important):<br>
`dotnet test`


### How the table is layed out
Y<br>
4  [ ][ ][ ][ ][ ]  ← NORTH edge<br>
3  [ ][ ][ ][ ][ ]<br>
2  [ ][ ][ ][ ][ ]<br>
1  [ ][ ][ ][ ][ ]<br>
0  [0][ ][ ][ ][4]  ← SOUTH edge (Y=0)<br>
   0  1  2  3  4 -  X<br>
   ↑---------             ↑<br>
 WEST---            EAST<br>
 edge----            edge<br>
