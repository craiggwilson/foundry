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

        public ICommit GetCommit(Project project, string path)
        {
            var repo = GetRepository(project);

            AbstractObject obj;
            if (!TryGetObject(repo, path, out obj) || obj.IsCommit)
                return null;

            var commit = (Commit)obj;

            return new GitCommit
            {
                Username = commit.Committer.Name,
                DateTime = commit.CommitDate.DateTime,
                Message = commit.Message,
                Version = commit.ShortHash,
                ParentVersions = commit.HasParents ? commit.Parents.Select(p => p.ShortHash) : Enumerable.Empty<string>()
            };
        }

        public IEnumerable<ICommitInfo> GetHistory(Project project, string path)
        {
            var repo = GetRepository(project);

            var branch = repo.Branches[path];
            yield return new GitCommitInfo
            {
                Username = branch.CurrentCommit.Committer.Name,
                DateTime = branch.CurrentCommit.CommitDate.DateTime,
                Message = branch.CurrentCommit.Message,
                Version = branch.CurrentCommit.ShortHash,
                ParentVersions = branch.CurrentCommit.HasParents ? branch.CurrentCommit.Parents.Select(p => p.ShortHash) : Enumerable.Empty<string>()
            };

            foreach (var ancestor in branch.CurrentCommit.Ancestors)
            {
                yield return new GitCommitInfo
                {
                    Username = ancestor.Committer.Name,
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

            AbstractObject obj;
            if (!TryGetObject(repo, parts[0], out obj) || !obj.IsTree)
                return null;

            var node = GetNode((AbstractTreeNode)obj, parts.Skip(1));
            return CreateGitSourceObject(parts[0], node);
        }

        public static bool TryGetObject(Repository repo, string id, out AbstractObject obj)
        {
            obj = null;
            if (repo.Branches.ContainsKey(id))
                obj = repo.Branches[id].CurrentCommit.Tree;

            return obj != null;
        }

        private static AbstractTreeNode GetNode(AbstractTreeNode parentNode, IEnumerable<string> path)
        {
            if (path.Any())
            {
                if (parentNode.IsTree)
                {
                    var name = path.First();
                    var tree = (Tree)parentNode;
                    parentNode = tree.Trees.SingleOrDefault(x => x.Name == name);
                    if(parentNode == null)
                        parentNode = tree.Leaves.SingleOrDefault(x => x.Name == name);
                    if(parentNode == null)
                        throw new InvalidOperationException("Unable to determine path.");

                    return GetNode(parentNode, path.Skip(1));
                }
                else
                {
                    throw new InvalidOperationException("Unable to determine path.");
                }
            }

            return parentNode;
        }

        private static GitSourceObject CreateGitSourceObject(string prefix, AbstractTreeNode node)
        {
            if (node.IsTree)
            {
                var tree = (Tree)node;
                return new GitSourceTree
                {
                    Name = tree.Name,
                    IsTree = true,
                    Path = GetPath(prefix, tree.Path),
                    LastModified = tree.GetLastCommit().CommitDate.DateTime,
                    Message = tree.GetLastCommit().Message,
                    Children = tree.Trees.Select(x => CreateGitSourceObject(prefix, x))
                        .Union(tree.Leaves.Select(x => CreateGitSourceObject(prefix, x)))
                };
            }
            else
            {
                return new GitSourceFile
                {
                    Name = node.Name,
                    IsTree = false,
                    Path = GetPath(prefix, node.Path),
                    LastModified = node.GetLastCommit().CommitDate.DateTime,
                    Message = node.GetLastCommit().Message,
                    Content = ((Leaf)node).RawData
                };
            }
        }

        private static string GetPath(string branchName, string path)
        {
            return string.IsNullOrWhiteSpace(path) ? branchName : branchName + "/" + path;
        }

        private static Repository GetRepository(Project project)
        {
            return new Repository(Path.Combine(GitSettings.RepositoriesPath, project.Name + ".git"));
        }
    }
}