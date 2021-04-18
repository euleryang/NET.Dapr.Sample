using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Example usage
            Logger logger = LogManager.GetLogger("UnitTest1");
            logger.Debug("trace log message");

            System.Console.WriteLine("Hello UnitTest");

        }
    }
}
