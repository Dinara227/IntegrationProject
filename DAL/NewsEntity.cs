using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DAL
{
    internal class NewsEntity
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        public string ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        [BsonDateTimeOptions]
        public DateTime DateOfPublication { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        [BsonDateTimeOptions]
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
