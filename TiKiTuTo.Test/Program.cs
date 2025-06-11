using Xunit;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace TiKiTuTo.Test
{
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
                Program.Main(null);
            }
            catch (Exception ex)
            {
                // Catch the Environment.Exit call
                Assert.IsType<ThreadAbortException>(ex);
            }

            // Assert
            var consoleOutput = output.ToString();
            Assert.Contains("5. Exit", consoleOutput);
        }
    }
}