using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Messaging.Infrastructure;
using Foundry.Domain;

namespace Foundry.Messages.Handlers
{
    public class UserPermissionMessageHandler : IMessageHandler<UserProjectCreatedMessage>
    {
        private IBus _bus;
        private readonly IDomainRepository<UserPermission> _userPermissionRepository;

        public UserPermissionMessageHandler(IBus bus, IDomainRepository<UserPermission> userPermissionsRepository)
        {
            _bus = bus;
            _userPermissionRepository = userPermissionsRepository;
        }

        public void Handle(UserProjectCreatedMessage message)
        {
            var permission = GetOwnerPermissions(message.UserId, message.ProjectId, message.AccountName + "/" + message.RepositoryName);
            _userPermissionRepository.Add(permission);

            permission = GetEveryonePermissions(message.ProjectId, message.AccountName + "/" + message.RepositoryName, message.IsPrivate);
            _userPermissionRepository.Add(permission);
        }

        private static UserPermission GetOwnerPermissions(Guid userId, Guid repositoryId, string repositoryName)
        {
            return new UserPermission
            {
                UserId = userId,
                SubjectType = SubjectType.Project,
                SubjectId = repositoryId,
                Level = 50,
                Operation = Security.Operation.All,
                Allow = true
            };
        }

        private static UserPermission GetEveryonePermissions(Guid repositoryId, string repositoryName, bool isPrivate)
        {
            return new UserPermission
            {
                UserId = Guid.Empty,
                SubjectType = SubjectType.Project,
                SubjectId = repositoryId,
                Level = 1,
                Operation = Security.Operation.Read,
                Allow = !isPrivate
            };
        }
    }
}
