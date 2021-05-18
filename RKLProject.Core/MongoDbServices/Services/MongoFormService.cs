using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using RKLProject.Core.MongoDbServices.IServices;

namespace RKLProject.Core.MongoDbServices.Services
{
    public class MongoFormService<T> : IMongoFormService<T> where T : class
    {
        private IMongoCollection<T> db;
        public MongoFormService(IMongoClient client)
        {
            var database = client.GetDatabase("user_forms");
            db = database.GetCollection<T>("forms");
        }
        public async Task DeleteRecord(string table, Guid id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
           await db.DeleteOneAsync(filter);
        }

        public async Task InsertRecord(string table, T record)
        {
            await db.InsertOneAsync(record);
        }

        public async Task<T> LoadRecordById(string table, Guid id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);

            return await db.FindAsync(filter).Result.FirstAsync();
        }

        public async Task<List<T>> LoadRecords(string table)
        {
            return await db.FindAsync(new BsonDocument()).Result.ToListAsync();
        }

        [Obsolete]
        public async Task UpsetRecord(string table, Guid id, T record)
        {
            var result = await db.ReplaceOneAsync(
                new BsonDocument("_id", id),
                record,
                new UpdateOptions { IsUpsert = true });
        }
    }
}