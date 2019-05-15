using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDbTutorials.MongoDbTutorials.MongoBasics.Model;
using NUnit.Framework;

namespace MongoDbTutorials.MongoDbTutorials.MongoBasics
{
    public class MongoOperationsVerifier
    {
        private class PrivateTest
        {
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public static void VerifyInsertOne(string connectionString, string objectId)
        {
            
            var result = GetCollection(connectionString).FindAsync(FilterDefinition<PrivateTest>.Empty);
            var resultData = result.Result.ToList();
            Assert.AreEqual(1, resultData.Count, "No document found in the collection testdb.testcollection");
            var data = resultData.ElementAt(0);
            Assert.AreEqual("MyName", data.Name, "The property value is not equal to MyName");
            Assert.AreEqual(objectId, data.Id, "The property value is not equal to ", objectId);
        }

        public static void VerifyInsertMany(string connectionString, IEnumerable<Test> documents)
        {
            Assert.AreNotEqual(documents, null);
            var result = GetCollection(connectionString).FindAsync(FilterDefinition<PrivateTest>.Empty);
            var resultData = result.Result.ToList();
            Assert.AreEqual(documents.Count(), resultData.Count, "No document found in the collection testdb.testcollection");
            var data = resultData.Find(x => x.Id == documents.ElementAt(0).Id);
            Console.WriteLine(data);
            Assert.AreEqual("MyName0", data.Name, "The property value is not equal to MyName0");
            Assert.AreEqual(documents.ElementAt(0).Id, data.Id, "The property value is not equal to document at 0");
        }

        public static void VerifyFindAll(IEnumerable<Test> documents)
        {
            Assert.AreNotEqual(documents, null);
            Assert.AreEqual(documents.Count(),10);
        }

        public static void VerifyFindMyName0(Test document)
        {
            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Name, "MyName0");
        }

        public static void VerifyUpdateOne(string connectionString, IEnumerable<Test> documents)
        {
            Assert.AreNotEqual(documents, null);
            var result = GetCollection(connectionString).FindAsync(FilterDefinition<PrivateTest>.Empty);
            var resultData = result.Result.ToList();
            var data = resultData.Find(x => x.Id == documents.ElementAt(1).Id);
            Console.WriteLine(data);
            Assert.AreEqual("UpdatedName", data.Name, "The property value is not equal to MyName1");
        }

        public static void VerifyUpdateMany(string connectionString, IEnumerable<Test> documents)
        {
            Assert.AreNotEqual(documents, null);
            var result = GetCollection(connectionString).FindAsync(FilterDefinition<PrivateTest>.Empty);
            var resultData = result.Result.ToList();
            resultData.ForEach(x => { Assert.AreEqual("UpdatedName", x.Name); });
        }

        public static void VerifyDeleteOne(string connectionString, IEnumerable<Test> documents)
        {
            Assert.AreNotEqual(documents, null);
            var result = GetCollection(connectionString).FindAsync(FilterDefinition<PrivateTest>.Empty);
            var resultData = result.Result.ToList();
            Assert.AreEqual(documents.Count() - 1, resultData.Count, "No document found in the collection testdb.testcollection");
            resultData.ForEach(x => { Assert.AreNotEqual("MyName0", x.Name); });
        }

        public static void VerifyFindOneAndUpdate(string connectionString, Test document)
        {
            Assert.AreNotEqual(document, null);
            var result = GetCollection(connectionString).Find(Builders<PrivateTest>.Filter.Eq( x=> x.Id,document.Id)).FirstOrDefault();
            Assert.AreEqual(document.Name, result.Name);
        }

        private static IMongoCollection<PrivateTest> GetCollection(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var coll = client.GetDatabase("testdb").GetCollection<PrivateTest>("testcollection");
            return coll;
        }

        
    }
}