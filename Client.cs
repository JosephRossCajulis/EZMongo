using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EZMongo.NET
{
    public class Client : IDisposable
    {
        private MongoClient _client;
        private IMongoDatabase _database;

        public Client(string constring, string db)
        {
            _client = new MongoClient(constring);
            _database = _client.GetDatabase(db);
        }

        public void Create(string collection, object item, bool removeT = true)
        {
            var coll = _database.GetCollection<BsonDocument>(collection);
            var returnDocument = new BsonDocument(item.ToBsonDocument());
            if (removeT)
            {
                returnDocument.Remove("_t");
            }

            coll.InsertOne(returnDocument);
        }

        public List<object> Read(string collectionName)
        {
            var coll = _database.GetCollection<BsonDocument>(collectionName);
            var returnDocument = coll.Find(new BsonDocument()).ToList();
            var dotNetObj = returnDocument.ConvertAll(BsonTypeMapper.MapToDotNetValue);
            return dotNetObj;
        }

        public List<object> Read(string collectionName, Dictionary<object,object> paramss)
        {
            if (paramss.Count == 1)
            {
                var filter = Builders<BsonDocument>.Filter.Eq(paramss.Keys.First().ToString(), paramss.Values.First().ToString());
                var coll = _database.GetCollection<BsonDocument>(collectionName);
                var returnDocument = coll.Find(filter).ToList();
                var dotNetObj = returnDocument.ConvertAll(BsonTypeMapper.MapToDotNetValue);
                return dotNetObj;
            }
            else
            {
                var filter = Builders<BsonDocument>.Filter.Eq(paramss.Keys.First().ToString(), paramss.Values.First().ToString());
                for (int i = 1; i < paramss.Count; i++)
                {
                    var key = paramss.Keys.ElementAt(i).ToString();
                    var value = paramss.Values.ElementAt(i).ToString();

                    if ((value.ToString().ToLowerInvariant() == "true") || (value.ToString().ToLowerInvariant() == "false"))
                    { filter = filter & (Builders<BsonDocument>.Filter.Eq(key, Convert.ToBoolean(value))); }
                    else
                    { filter = filter & (Builders<BsonDocument>.Filter.Eq(key, value)); }


                }
                var coll = _database.GetCollection<BsonDocument>(collectionName);
                var returnDocument = coll.Find(filter).ToList();
                var dotNetObj = returnDocument.ConvertAll(BsonTypeMapper.MapToDotNetValue);
                return dotNetObj;
            }
        }

        public void Update(string collection, object item, Dictionary<object, object> paramss, bool removeT = true)
        {
            if (paramss.Count == 1)
            {
                var filter = Builders<BsonDocument>.Filter.Eq(paramss.Keys.First().ToString(), paramss.Values.First().ToString());
                var updatedDoc = item.ToBsonDocument();
                if (removeT)
                {
                    updatedDoc.Remove("_t");
                }

                updatedDoc.Remove("_id");
                var coll = _database.GetCollection<BsonDocument>(collection);
                var returnDocument = coll.ReplaceOne(filter, updatedDoc);
            }
            else
            {
                var filter = Builders<BsonDocument>.Filter.Eq(paramss.Keys.First().ToString(), paramss.Values.First().ToString());
                for (int i = 1; i < paramss.Count; i++)
                {
                    var key = paramss.Keys.ElementAt(i).ToString();
                    var value = paramss.Values.ElementAt(i).ToString();

                    if ((value.ToString().ToLowerInvariant() == "true") || (value.ToString().ToLowerInvariant() == "false"))
                    { filter = filter & (Builders<BsonDocument>.Filter.Eq(key, Convert.ToBoolean(value))); }
                    else
                    { filter = filter & (Builders<BsonDocument>.Filter.Eq(key, value)); }


                }
                var updatedDoc = item.ToBsonDocument();
                if (removeT)
                {
                    updatedDoc.Remove("_t");
                }
                updatedDoc.Remove("_id");
                var coll = _database.GetCollection<BsonDocument>(collection);
                var returnDocument = coll.UpdateOne(filter, updatedDoc);
            }
        }

        public void Delete(string collection, string key, string value)
        {
            var coll = _database.GetCollection<BsonDocument>(collection);
            var filter = Builders<BsonDocument>.Filter.Eq(key, value);
            var res = coll.DeleteOne(filter);
        }

        public int Count(string collection)
        {
            var coll = _database.GetCollection<BsonDocument>(collection);
            var returnDocument = coll.Find(new BsonDocument()).ToList();
            var count = returnDocument.Count;
            return count;
        }


        public void Dispose()
        {
            _database = null;
            _client = null;
        }

    }
}
