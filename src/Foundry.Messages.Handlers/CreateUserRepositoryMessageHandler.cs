using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Messaging.Infrastructure;
using Foundry.SourceControl;
using Sikai.EventSourcing.Domain;
using Foundry.Domain;
using Sikai.EventSourcing.Infrastructure;

namespace Foundry.Messages.Handlers
{
    public class CreateUserRepositoryMessageHandler : IMessageHandler<CreateUserRepositoryMessage>
    {
        private readonly IUnitOfWork _domain;
        private readonly IEnumerable<Lazy<ISourceControlProvider, ISourceControlProviderMetadata>> _sourceControlProviders;

        public CreateUserRepositoryMessageHandler(IUnitOfWork domain, IEnumerable<Lazy<ISourceControlProvider, ISourceControlProviderMetadata>> sourceControlProviders)
        {
            _domain = domain;
            _sourceControlProviders = sourceControlProviders;
        }

        public void Handle(CreateUserRepositoryMessage message)
        {
            var provider = _sourceControlProviders.Single(x => x.Metadata.Name == message.SourceControlProvider);

            provider.Value.CreateRepository(message.RepositoryName);

            var domainRepository = new Repository(_domain);

            var repo = new CodeRepository(message.SourceControlProvider, message.RepositoryName);
            domainRepository.Add(repo);

            var user = domainRepository.GetById<User>(message.UserId);
            user.AddRepository(repo.Id);

            _domain.Commit();
        }
    }
}