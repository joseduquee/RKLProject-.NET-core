using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RKLProject.Core.MongoDbServices.IServices
{
    public interface IMongoFormService<T> where T : class
    {
        Task InsertRecord(string table, T record);

        Task<List<T>> LoadRecords(string table);

        Task<T> LoadRecordById(string table, Guid id);

        Task UpsetRecord(string table, Guid id, T record);

        Task DeleteRecord(string table, Guid id);
    }
}