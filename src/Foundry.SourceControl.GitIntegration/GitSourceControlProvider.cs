using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel.Composition;
using System.Configuration;
using GitSharp;
using GitSharp.Commands;

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

        public IEnumerable<Commit> GetCommits(string repositoryName, string path, int page, int pageCount)
        {
            var repo = new Repository(Path.Combine(GitSettings.RepositoriesPath, repositoryName + ".git"));

            var branch = repo.Branches[path];

            var commits = branch.CurrentCommit.Ancestors
                .Skip((page - 1) * pageCount)
                .Take(pageCount)
                .Select(x => new Commit
                {
                    Username = x.Author.Name,
                    DateTime = x.CommitDate.DateTime,
                    Comment = x.Message,
                    Version = x.Tree.Hash
                });
            return new[] { new Commit
            {
                Username = branch.CurrentCommit.Author.Name,
                DateTime = branch.CurrentCommit.CommitDate.DateTime,
                Comment = branch.CurrentCommit.Message,
                Version = branch.CurrentCommit.Tree.Hash
            }}
            .Union(commits);
        }
    }
}