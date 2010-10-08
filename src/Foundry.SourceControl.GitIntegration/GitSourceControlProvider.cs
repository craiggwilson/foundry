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

        public ICommit GetCommit(Project project, string id)
        {
            var repo = GetRepository(project);

            var commit = repo.Get<Commit>(id);
            if (commit == null)
                return null;

            return new GitCommit
            {
                Username = commit.Committer.Name,
                DateTime = commit.CommitDate.DateTime,
                Message = commit.Message,
                Id = commit.Hash,
                ParentIds = commit.HasParents ? commit.Parents.Select(p => p.Hash) : Enumerable.Empty<string>(),
                Changes = GetChanges(commit)
            };
        }

        public IEnumerable<IHistoricalItem> GetHistory(Project project, string id)
        {
            var repo = GetRepository(project);

            var branch = repo.Branches[id];
            yield return new GitHistoricalItem
            {
                Username = branch.CurrentCommit.Committer.Name,
                DateTime = branch.CurrentCommit.CommitDate.DateTime,
                Message = branch.CurrentCommit.Message,
                Id = branch.CurrentCommit.Hash,
                ParentIds = branch.CurrentCommit.HasParents ? branch.CurrentCommit.Parents.Select(p => p.Hash) : Enumerable.Empty<string>()
            };

            foreach (var ancestor in branch.CurrentCommit.Ancestors)
            {
                yield return new GitHistoricalItem
                {
                    Username = ancestor.Committer.Name,
                    DateTime = ancestor.CommitDate.DateTime,
                    Message = ancestor.Message,
                    Id = ancestor.Hash,
                    ParentIds = ancestor.HasParents ? ancestor.Parents.Select(p => p.ShortHash) : Enumerable.Empty<string>()
                };
            }
        }

        public ISourceObject GetSourceObject(Project project, string treeId, string path)
        {
            var repo = GetRepository(project);

            AbstractTreeNode node = null;
            if (repo.Branches.ContainsKey(treeId))
                node = repo.Branches[treeId].CurrentCommit.Tree;
            else
                node = repo.Get<Tree>(treeId);

            if(!string.IsNullOrEmpty(path))
                node = GetNode(node, path.Split('/'));

            return CreateGitSourceObject(treeId, node);
        }

        private static AbstractTreeNode GetNode(AbstractTreeNode parentNode, IEnumerable<string> parts)
        {
            if (parts.Any())
            {
                if (parentNode.IsTree)
                {
                    var name = parts.First();
                    var treeNode = (Tree)parentNode;
                    parentNode = treeNode.Trees.SingleOrDefault(x => x.Name == name);
                    if(parentNode == null)
                        parentNode = treeNode.Leaves.SingleOrDefault(x => x.Name == name);
                    if(parentNode == null)
                        throw new InvalidOperationException("Unable to determine path.");

                    return GetNode(parentNode, parts.Skip(1));
                }
                else
                {
                    throw new InvalidOperationException("Unable to determine path.");
                }
            }

            return parentNode;
        }

        private static GitSourceObject CreateGitSourceObject(string tree, AbstractTreeNode node)
        {
            if (node.IsTree)
            {
                var treeNode = (Tree)node;
                return new GitSourceDirectory
                {
                    Name = treeNode.Name,
                    IsDirectory = true,
                    TreeId = tree,
                    Path = node.Path,
                    DateTime = treeNode.GetLastCommit().CommitDate.DateTime,
                    Message = treeNode.GetLastCommit().Message,
                    Children = treeNode.Trees.Select(x => CreateGitSourceObject(tree, x))
                        .Union(treeNode.Leaves.Select(x => CreateGitSourceObject(tree, x)))
                };
            }
            else
            {
                return new GitSourceFile
                {
                    Name = node.Name,
                    IsDirectory = false,
                    TreeId = tree,
                    Path = node.Path,
                    DateTime = node.GetLastCommit().CommitDate.DateTime,
                    Message = node.GetLastCommit().Message,
                    Content = ((Leaf)node).RawData
                };
            }
        }

        private static IEnumerable<IChange> GetChanges(Commit commit)
        {
            foreach (var change in commit.Changes.Where(c => c.ChangedObject.IsBlob))
            {
                Blob blob = null;
                Blob oldBlob = null;
                switch (change.ChangeType)
                {
                    case GitSharp.ChangeType.Added:
                        blob = (Blob)change.ComparedObject;
                        break;
                    case GitSharp.ChangeType.Deleted:
                        oldBlob = (Blob)change.ComparedObject;
                        break;
                    case GitSharp.ChangeType.Modified:
                        blob = (Blob)change.ReferenceObject;
                        oldBlob = (Blob)change.ComparedObject;
                        break;
                }
                var gitChange = new GitChange { Type = (ChangeType)(int)change.ChangeType };
                if (blob != null)
                {
                    gitChange.File = new GitSourceFile
                    {
                        Name = change.Name,
                        Content = blob.RawData,
                        IsDirectory = false,
                        DateTime = commit.CommitDate.DateTime,
                        Message = commit.Message,
                        TreeId = commit.Tree.Hash,
                        Path = change.Path
                    };
                }

                if (oldBlob != null)
                {
                    gitChange.OldFile = new GitSourceFile
                    {
                        Name = change.Name,
                        Content = oldBlob.RawData,
                        IsDirectory = false,
                        DateTime = change.ReferenceCommit.CommitDate.DateTime,
                        Message = change.ReferenceCommit.Message,
                        TreeId = change.ReferenceCommit.Tree.Hash,
                        Path = change.Path
                    };
                }

                yield return gitChange;
            }
        }

        private static Repository GetRepository(Project project)
        {
            return new Repository(Path.Combine(GitSettings.RepositoriesPath, project.Name + ".git"));
        }
    }
}