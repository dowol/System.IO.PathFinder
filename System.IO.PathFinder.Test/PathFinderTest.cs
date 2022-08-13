namespace System.IO.PathFinderTest
{
    [TestClass]
    public class PathFinderTest
    {
        [TestMethod]
        public void EnumerateAllPaths()
        {
            Console.WriteLine($"User '{Environment.UserName}' PathFinder");
            foreach (var dir in PathFinder.User.Directories)
                Console.WriteLine(dir.FullName);

            Console.WriteLine("\nSystem PathFinder");
            foreach (var dir in PathFinder.System.Directories)
                Console.WriteLine(dir.FullName);
        }

        [DataTestMethod]
        [DataRow("arduino.exe")]
        [DataRow("devenv.exe")]
        [DataRow("dotnet.exe")]
        [DataRow("ffmpeg.exe")]
        [DataRow("git.exe")]
        [DataRow("node.exe")]
        [DataRow("npm.cmd")]
        [DataRow("notepad.exe")]
        [DataRow("pwsh.cmd")]
        [DataRow("python.exe")]
        public void Find(string itemName)
        {
            FileInfo[] result = PathFinder.All.Find(itemName).ToArray();
            string msg =
                result.LongLength > 0 ?
                $"PathFinder found the item '{itemName}' located on {result.LongLength} folder{(result.LongLength == 1 ? "" : "s")}:" :
                $"PathFinder cannot find the item '{itemName}' on entire folders in PATH.";
            Console.WriteLine(msg);
            foreach (FileInfo item in result) Console.WriteLine(item.FullName);
            Console.WriteLine();
        }

        [DataTestMethod]
        [DataRow("arduino")]
        [DataRow("devenv")]
        [DataRow("dotnet")]
        [DataRow("ffmpeg")]
        [DataRow("git")]
        [DataRow("node")]
        [DataRow("npm")]
        [DataRow("notepad")]
        [DataRow("pwsh")]
        [DataRow("python")]
        public void FindWithoutExtension(string itemName)
        {
            FileInfo[] result = PathFinder.All.Find(itemName).ToArray();
            string msg =
                result.LongLength > 0 ?
                $"PathFinder.User found the item '{itemName}' located on {result.LongLength} folder{(result.LongLength == 1 ? "" : "s")}:" :
                $"PathFinder.User cannot find the item '{itemName}' on entire folders in PATH.";
            Console.WriteLine(msg);
            foreach (FileInfo item in result) Console.WriteLine(item.FullName);
            Console.WriteLine();
        }

        // [DataTestMethod]
        public void FindInUserPathFinder(string itemName)
        {
            FileInfo[] result = PathFinder.User.Find(itemName).ToArray();
            string msg =
                result.LongLength > 0 ?
                $"PathFinder.User found the item '{itemName}' located on {result.LongLength} folder{(result.LongLength == 1 ? "" : "s")}:" :
                $"PathFinder.User cannot find the item '{itemName}' on entire folders in PATH.";
            Console.WriteLine(msg);
            foreach (FileInfo item in result) Console.WriteLine(item.FullName);
            Console.WriteLine();
        }

        // [DataTestMethod]
        public void FindInSystemPathFinder(string itemName)
        {
            FileInfo[] result = PathFinder.System.Find(itemName).ToArray();
            string msg =
                result.LongLength > 0 ?
                $"PathFinder.System found the item '{itemName}' located on {result.LongLength} folder{(result.LongLength == 1 ? "" : "s")}:" :
                $"PathFinder.System cannot find the item '{itemName}' on entire folders in PATH.";
            Console.WriteLine(msg);
            foreach (FileInfo item in result) Console.WriteLine(item.FullName);
            Console.WriteLine();
        }
    }
}