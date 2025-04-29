
using System;
using System.IO;
using Xunit;
using TiKiTuTo;


public class ProgramTests
{
    [Fact]
    public void TestExitOption()
    {
        // Arrange
        var input = new StringReader("5");
        var output = new StringWriter();

        Console.SetIn(input);
        Console.SetOut(output);

        // Act
        try
        {
            Program.Main();
        }
        catch (Exception ex)
        {
            // Catch the Environment.Exit call
            Assert.IsType<System.Threading.ThreadAbortException>(ex);
        }

        // Assert
        var consoleOutput = output.ToString();
        Assert.Contains("5. Exit", consoleOutput);
    }
}