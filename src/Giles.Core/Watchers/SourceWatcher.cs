using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Giles.Core.IO;

namespace Giles.Core.Watchers
{
    public class SourceWatcher : IDisposable
    {
        readonly IFileSystem fileSystem;
        readonly IBuildRunner buildRunner;
        readonly IFileWatcherFactory fileWatcherFactory;
        string solution;
        string path;
        public List<FileSystemWatcher> FileWatchers { get; set; }

        public SourceWatcher(IFileSystem fileSystem, IBuildRunner buildRunner, IFileWatcherFactory fileWatcherFactory)
        {
            this.FileWatchers = new List<FileSystemWatcher>();
            this.fileSystem = fileSystem;
            this.buildRunner = buildRunner;
            this.fileWatcherFactory = fileWatcherFactory;
        }

        public void Watch(string path, string filter)
        {
            this.path = path;
            var solutionFolder = fileSystem.GetDirectoryName(path);
            FileWatchers.Add(fileWatcherFactory.Build(solutionFolder, filter, ChangeAction, null, ErrorAction));
        }

        public void ErrorAction(object sender, ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void ChangeAction(object sender, FileSystemEventArgs e)
        {
            buildRunner.Run(path);
        }

        public void Dispose()
        {
            FileWatchers.ToList().ForEach(x => x.Dispose());
        }
    }

    public interface IBuildRunner
    {
        void Run(string solution);
    }
}