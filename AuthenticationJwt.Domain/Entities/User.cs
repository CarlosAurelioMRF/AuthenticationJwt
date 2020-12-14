using AuthenticationJwt.Core.Base;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AuthenticationJwt.Domain.Entities
{
    public class User : Entity<User>
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? UpdatedAt { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? DeletedAt { get; set; }
    }
}
