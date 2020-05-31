using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace TracConnectApi
{
    public class MongoCRUD 
    {
        //private readonly IConfiguration _configuration;
        public IMongoDatabase db;

        //public MongoCRUD(string database, string connectString)
        //{
        //    var client = new MongoClient(connectString);
        //    db = client.GetDatabase(database);
        //}

        public MongoCRUD(IConfiguration connectString)
        {
            var client = new MongoClient(connectString.GetConnectionString("TRACCONNECT_MongoDB"));
            db = client.GetDatabase("TRACCONNECT");
        }

        public void InsertRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        public List<T> LoadRecords<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }

        public T LoadRecordById<T>(string table, Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var fillter = Builders<T>.Filter.Eq("Id", id);
            return collection.Find(fillter).FirstOrDefault();
            //return collection.Find(fillter).First();  // if id not found will crash
            //return default(T);
        }

        public T LoadRecordByFeild<T>(string table, string field, string key)
        {
            var collection = db.GetCollection<T>(table);
            var fillter = Builders<T>.Filter.Eq(field, key);
            return collection.Find(fillter).FirstOrDefault();
            //return collection.Find(fillter).First();  // if id not found will crash
            //return default(T);
        }

        public List<T> LoadRecordsByFeild<T>(string table, string field, string key)
        {
            var collection = db.GetCollection<T>(table);
            var fillter = Builders<T>.Filter.Eq(field, key);
            return collection.Find(new BsonDocument()).ToList();
        }

        //[Obsolete]
        public T UpsertRecord<T>(string table, Guid id, T record)
        {   // Upsert is like insert if record dose not exist
            var collection = db.GetCollection<T>(table);

            var result = collection.ReplaceOne(
                new BsonDocument("_id", id),
                record,
                new ReplaceOptions { IsUpsert = true });
            //Console.WriteLine(result);
            if (result.UpsertedId != null || result.MatchedCount > 0)
                return record;  // found match/updated or Inserted (when id not found)

            return default(T);  // mean return null
        }

        public Boolean DeleteRecord<T>(string table, Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("_id", id);
            var result = collection.DeleteOne(filter);
            return (result.DeletedCount > 0) ? true : false;
        }
    }

}
