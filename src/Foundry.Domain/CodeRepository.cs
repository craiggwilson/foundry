using System;

using Sikai.EventSourcing.Domain;

namespace Foundry.Domain
{
    public class CodeRepository : AggregateRootBase
    {
        private bool _deleted;
        private Uri _location;
        private string _name;
        private string _type;

        private CodeRepository(Guid id)
            : base(id)
        {
            WireUpEventHandlers();
        }

        public CodeRepository(string name, string type, Uri location)
            : this(Guid.NewGuid())
        {
            Raise(new CreatedEvent { SourceId = Id, Name = name, Type = type, Location = location });
        }

        public void Delete()
        {
            if (_deleted)
                throw new InvalidOperationException("CodeRepository is already deleted.");

            Raise(new DeletedEvent { SourceId = Id });
        }

        public void Move(Uri location)
        {
            if (_deleted)
                throw new InvalidOperationException("CodeRepository is deleted.");
            if (_location == location)
                throw new InvalidOperationException("New location is the same as the old location.");

            Raise(new MovedEvent { SourceId = Id, Location = location });
        }

        private void WireUpEventHandlers()
        {
            RegisterEventHandler<CreatedEvent>(OnCreated);
            RegisterEventHandler<DeletedEvent>(OnDeleted);
            RegisterEventHandler<MovedEvent>(OnMoved);
        }

        private void OnCreated(CreatedEvent @event)
        {
            _deleted = false;
            _name = @event.Name;
            _type = @event.Type;
            _location = @event.Location;
        }

        private void OnDeleted(DeletedEvent @event)
        {
            _deleted = true;
        }

        private void OnMoved(MovedEvent @event)
        {
            _location = @event.Location;
        }

        public class CreatedEvent : IDomainEvent
        {
            public Guid SourceId { get; set; }

            public string Name { get; set; }

            public string Type { get; set; }

            public Uri Location { get; set; }
        }

        public class DeletedEvent : IDomainEvent
        {
            public Guid SourceId { get; set; }
        }

        public class MovedEvent : IDomainEvent
        {
            public Guid SourceId { get; set; }

            public Uri Location { get; set; }
        }
    }
}
