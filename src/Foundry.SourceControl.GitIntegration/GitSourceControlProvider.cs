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
    [SourceControlProvider("Git", CommitsHaveParents = true)]
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

        public IEnumerable<IBranch> GetBranches(Project project)
        {
            var repo = GetRepository(project);

            return repo.Branches.Select(b => new GitBranch
            {
                Name = b.Key,
                IsCurrent = b.Value.IsCurrent
            });
        }

        public IEnumerable<ICommit> GetCommits(Project project, string branchName, int page, int pageCount)
        {
            var repo = GetRepository(project);

            var branch = repo.Branches[branchName];

            var commits = branch.CurrentCommit.Ancestors
                .Skip((page - 1) * pageCount)
                .Take(pageCount)
                .Select(x => new GitCommit
                {
                    Username = x.Author.Name,
                    DateTime = x.CommitDate.DateTime,
                    Message = x.Message,
                    Version = x.Tree.ShortHash,
                    ParentVersions = x.HasParents ? x.Parents.Select(p => p.ShortHash) : Enumerable.Empty<string>()
                });
            return new[] { new GitCommit
            {
                Username = branch.CurrentCommit.Author.Name,
                DateTime = branch.CurrentCommit.CommitDate.DateTime,
                Message = branch.CurrentCommit.Message,
                Version = branch.CurrentCommit.ShortHash,
                ParentVersions = branch.CurrentCommit.HasParents ? branch.CurrentCommit.Parents.Select(p => p.ShortHash) : Enumerable.Empty<string>()
            }}
            .Union(commits);
        }

        public ITree GetTree(Project project, string id, string path)
        {
            var repo = GetRepository(project);

            Tree tree;

            var branch = repo.Branches.ContainsKey(id) ? repo.Branches[id] : null;
            if (branch != null)
                tree = branch.CurrentCommit.Tree;
            else
                tree = repo.Get<Tree>(id);

            return GetTreeRecursive(null, tree);
        }

        public ILeaf GetLeaf(Project project, string id, string path)
        {
            throw new NotImplementedException();
        }

        private static GitTree GetTreeRecursive(GitTree parent, Tree tree)
        {
            var gt = new GitTree
            {
                Id = tree.Name,
                Parent = parent
            };

            gt.Leaves = tree.Leaves.Select(x => new GitLeaf { Id = x.Name, Parent = gt });
            gt.Trees = tree.Trees.Select(x => GetTreeRecursive(gt, x));

            return gt;                       
        }

        private static Repository GetRepository(Project project)
        {
            return new Repository(Path.Combine(GitSettings.RepositoriesPath, project.Name + ".git"));
        }
    }
}