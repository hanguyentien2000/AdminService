using EventBusRabbitMqueue.Abstractions;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventBusRabbitMqueue.Logging
{
    public class MongoErrorLogger : IErrorLogger
    {
        private readonly IMongoCollection<BsonDocument> _collection;

        public MongoErrorLogger(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDb:ConnectionString"]);
            var db = client.GetDatabase(config["MongoDb:Database"]);
            _collection = db.GetCollection<BsonDocument>("message_errors");
        }

        public async Task LogErrorAsync(string queueName, byte[] rawMessage, Exception ex)
        {
            var doc = new BsonDocument
        {
            { "Queue", queueName },
            { "Message", Encoding.UTF8.GetString(rawMessage) },
            { "Exception", ex.ToString() },
            { "Timestamp", DateTime.UtcNow }
        };
            await _collection.InsertOneAsync(doc);
        }
    }
}
