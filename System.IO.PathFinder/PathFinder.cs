using System.Collections.Generic;
using System.Linq;

namespace System.IO
{
    public class PathFinder
    {
        public static PathFinder User => new PathFinder(EnvironmentVariableTarget.User);
        public static PathFinder System => new PathFinder(EnvironmentVariableTarget.Process);
        public static PathFinder All => new PathFinder();

        private static bool IsWin32NT => Environment.OSVersion.Platform == PlatformID.Win32NT;


        public static char PathSeparatorChar => IsWin32NT ? ';' : ':';

        private static IEnumerable<string> PathExt =>
            from ext in Environment.GetEnvironmentVariable("PATHEXT").Split(';', StringSplitOptions.RemoveEmptyEntries) orderby ext select ext.ToLowerInvariant();


        public IEnumerable<DirectoryInfo> Directories => directories; 
        public IEnumerable<string> DirectoryStrings => from dir in Directories select dir.FullName;

        private readonly List<DirectoryInfo> directories = new List<DirectoryInfo>();

        private PathFinder()
        {
            directories = System.directories.Concat(User.directories).Distinct().ToList();
        }
        private PathFinder(EnvironmentVariableTarget target)
        {
            foreach(string path in Environment.GetEnvironmentVariable("PATH", target).Split(PathSeparatorChar, StringSplitOptions.RemoveEmptyEntries))
                directories.Add(new DirectoryInfo(path.Trim()));
            directories = directories.Distinct().ToList();
        }

        public bool HasItem(string itemName)
        {
            return Find(itemName).Any();
        }

        public virtual IEnumerable<FileInfo> Find(string itemName)
        {
            IEnumerable<string> items;
            if(!IsWin32NT || Path.HasExtension(itemName)) items = new List<string> { itemName };
            else items = (from ext in PathExt orderby ext select Path.ChangeExtension(itemName, ext)).DefaultIfEmpty(itemName);
            List<FileInfo> result = new List<FileInfo>();
            foreach (string item in items)
                result.AddRange(from path in DirectoryStrings 
                                where File.Exists(Path.Combine(path, item)) 
                                orderby path 
                                select new FileInfo(Path.Combine(path, item)));
            return result;
        }

        public FileInfo? FindOne(string itemName)
        {
            return (from item in Find(itemName) orderby item.FullName.LongCount() select item).FirstOrDefault();
        }

        public bool TryFind(string itemName, out IEnumerable<FileInfo> dest)
        {
            dest = Find(itemName);
            return dest?.Count() > 0;
        }

        public bool TryFindOne(string itemName, out FileInfo dest)
        {
#pragma warning disable CS8601
            dest = FindOne(itemName);
            return dest != null;
#pragma warning restore CS8601
        }
    }
}
