using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel.Composition;
using System.Configuration;
using GitSharp;
using GitSharp.Commands;
using Foundry.Domain;

namespace Foundry.SourceControl.GitIntegration
{
    [SourceControlProvider("Git")]
    public class GitSourceControlProvider : ISourceControlProvider
    {
        public void CreateRepository(Project project)
        {
            var cmd = new InitCommand
            {
                GitDirectory = Path.Combine(GitSettings.RepositoriesPath, project.Name),
                Quiet = false,
                Bare = true
            };
            cmd.Execute();
        }

        public IEnumerable<Branch> GetBranches(Project project)
        {
            return Enumerable.Empty<Branch>();
        }

        public IEnumerable<Commit> GetCommits(Project project, string branchName, int page, int pageCount)
        {
            var repo = GetRepository(project);

            var branch = repo.Branches[branchName];

            var commits = branch.CurrentCommit.Ancestors
                .Skip((page - 1) * pageCount)
                .Take(pageCount)
                .Select(x => new Commit
                {
                    Username = x.Author.Name,
                    DateTime = x.CommitDate.DateTime,
                    Message = x.Message,
                    Version = x.Tree.Hash
                });
            return new[] { new Commit
            {
                Username = branch.CurrentCommit.Author.Name,
                DateTime = branch.CurrentCommit.CommitDate.DateTime,
                Message = branch.CurrentCommit.Message,
                Version = branch.CurrentCommit.Tree.Hash
            }}
            .Union(commits);
        }

        private static Repository GetRepository(Project project)
        {
            return new Repository(Path.Combine(GitSettings.RepositoriesPath, project.Name + ".git"));
        }
    }
}