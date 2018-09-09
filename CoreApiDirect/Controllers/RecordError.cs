using System;

namespace CoreApiDirect.Controllers
{
    internal class RecordError
    {
        public RecordErrorType ErrorType { get; set; }
        public Type EntityType { get; set; }
        public object EntityId { get; set; }
        public Type ParentEntityType { get; set; }
        public object ParentEntityId { get; set; }
    }
}
