using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthenticationJwt.Core.Base
{
    public abstract class Entity<T> : EntityBase<T> where T : Entity<T>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public override string ToString()
        {
            return GetType().Name + "[Id = " + Id + "]";
        }
    }
}
