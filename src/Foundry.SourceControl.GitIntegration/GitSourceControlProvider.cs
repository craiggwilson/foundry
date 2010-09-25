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

        public IEnumerable<ICommit> GetHistory(Project project, string path)
        {
            var repo = GetRepository(project);

            var branch = repo.Branches[path];
            yield return new GitCommit
            {
                Username = branch.CurrentCommit.Author.Name,
                DateTime = branch.CurrentCommit.CommitDate.DateTime,
                Message = branch.CurrentCommit.Message,
                Version = branch.CurrentCommit.ShortHash,
                ParentVersions = branch.CurrentCommit.HasParents ? branch.CurrentCommit.Parents.Select(p => p.ShortHash) : Enumerable.Empty<string>()
            };

            foreach (var ancestor in branch.CurrentCommit.Ancestors)
            {
                yield return new GitCommit
                {
                    Username = ancestor.Author.Name,
                    DateTime = ancestor.CommitDate.DateTime,
                    Message = ancestor.Message,
                    Version = ancestor.Tree.ShortHash,
                    ParentVersions = ancestor.HasParents ? ancestor.Parents.Select(p => p.ShortHash) : Enumerable.Empty<string>()
                };
            }
        }

        public ISourceObject GetSourceObject(Project project, string path)
        {
            var repo = GetRepository(project);

            var parts = path.Split('/');

            AbstractTreeNode node;
            if (!TryGetTreeNode(repo, parts[0], out node))
                return null;

            return GetSourceObject(repo, node, parts.Skip(1));
        }

        public static bool TryGetTreeNode(Repository repo, string id, out AbstractTreeNode node)
        {
            node = null;
            if (repo.Branches.ContainsKey(id))
                node = repo.Branches[id].CurrentCommit.Tree;

            return node != null;
        }

        private static ISourceObject GetSourceObject(Repository repo, AbstractTreeNode node, IEnumerable<string> path)
        {
            if (node.IsTree)
                return GetSourceObjectFromTree(repo, (Tree)node, path);

            return null;
        }

        private static ISourceObject GetSourceObjectFromTree(Repository repo, Tree tree, IEnumerable<string> path)
        {
            if (path.Any())
            {
                AbstractTreeNode node = tree.Trees.SingleOrDefault(x => x.Name == path.ElementAt(0));
                if (node != null)
                    return GetSourceObjectFromTree(repo, (Tree)node, path.Skip(1));

                node = tree.Leaves.SingleOrDefault(x => x.Name == path.ElementAt(0));
                if(node != null)
                    return CreateGitSourceObject(node);

                return null;
            }

            return new GitSourceTree
            {
                Name = tree.Name,
                IsTree = true,
                Path = tree.Path,
                LastModified = tree.GetLastCommit().CommitDate.DateTime,
                Message = tree.GetLastCommit().Message,
                Children = tree.Trees.Select(x => CreateGitSourceObject(x))
                    .Union(tree.Leaves.Select(x => CreateGitSourceObject(x)))
            };
        }

        private static GitSourceObject CreateGitSourceObject(AbstractTreeNode node)
        {
            return new GitSourceObject { Name = node.Name, Path = node.Path, IsTree = node.IsTree, LastModified = node.GetLastCommit().CommitDate.DateTime, Message = node.GetLastCommit().Message };
        }

        private static Repository GetRepository(Project project)
        {
            return new Repository(Path.Combine(GitSettings.RepositoriesPath, project.Name + ".git"));
        }
    }
}