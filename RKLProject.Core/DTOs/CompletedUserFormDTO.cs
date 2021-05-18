using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace RKLProject.Core.DTOs
{
    public class CompletedUserFormDTO
    {
        [BsonId]
        public Guid Id { get; set; }
        public string FormName { get; set; }
        public List<CompletedUserFormDetails> Values{ get; set; }
    }
}
