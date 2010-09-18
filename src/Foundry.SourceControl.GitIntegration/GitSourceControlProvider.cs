using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel.Composition;
using System.Configuration;
using GitSharp.Commands;
using GitSharp.Core;

namespace Foundry.SourceControl.GitIntegration
{
    [SourceControlProvider("Git")]
    public class GitSourceControlProvider : ISourceControlProvider
    {
        public void CreateRepository(string name)
        {
            var cmd = new InitCommand
            {
                GitDirectory = Path.Combine(GitSettings.RepositoriesPath, name),
                Quiet = false,
                Bare = true
            };
            cmd.Execute();
        }

        public IEnumerable<Commit> GetCommits(string name, int page, int pageCount)
        {
            var repo = Repository.Open(Path.Combine(GitSettings.RepositoriesPath, name + ".git"));

            var reader = new ReflogReader(repo, "master");

            var entries = reader.getReverseEntries(page * pageCount);

            return entries.Select(e =>
                new Commit
                {
                    Username = e.getWho().Name,
                    DateTime = DateTime.FromFileTimeUtc(e.getWho().When),
                    Message = e.getComment(),
                    Version = e.getNewId().ToString()
                });
        }
    }
}