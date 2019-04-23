using System;
using System.Collections.Generic;
using System.Linq;
using Mongo2Go;
using MongoDbTutorials.MongoDbTutorials.MongoBasics;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using NUnit.Framework;

namespace MongoDbTutorials.MongoDbTutorials.MongoBasics
{
    public class MongoOperations
    {
        private MongoDbRunner _runner;
        private IMongoCollection<Test> mongoCollection;
        

        [SetUp]
        public void Setup()
        {
            _runner = MongoDbRunner.Start();
            mongoCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<Test>("testcollection");
        }

        [Test]
        // Insert a document in database named "testdb", in a collection named "testcollection"
        //The document to be inserted should have a property named "Name" and it's value should be "MyName"
        public void MongoCrud_InsertOne()
        {
            var objectId = ObjectId.GenerateNewId().ToString();
            InsertOne(objectId);
            MongoOperationsVerifier.VerifyInsertOne(_runner.ConnectionString, objectId);
        }

        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "testcollection"
        //The document to be inserted should have a property named "Name"
        public void MongoCrud_InsertMany()
        {
            var documents = InsertMany();
            MongoOperationsVerifier.VerifyInsertMany(_runner.ConnectionString, documents);
        }

        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "testcollection"
        // update the first Document to UpdatedName
        // although multiple documents match only first document will be updated

        public void MongoCrud_UpdateOne()
        {
            var documents = UpdateOne();
            MongoOperationsVerifier.VerifyUpdateOne(_runner.ConnectionString, documents);
        }

        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "testcollection"
        // update all the Documents matching filter condition

        public void MongoCrud_UpdateMany()
        {
            var documents = UpdateMany();
            MongoOperationsVerifier.VerifyUpdateMany(_runner.ConnectionString, documents);
        }

        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "testcollection"
        // delete the document with name "MyName0"

        public void MongoCrud_DeleteOne()
        {
            var documents = DeleteOne();
            MongoOperationsVerifier.VerifyDeleteOne(_runner.ConnectionString, documents);
        }

        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "testcollection"
        // find a document and do the specified operation
        // support for three type of operation FindOneAndUpdate, FindOneAndReplace, FindOneAndDelete
        // Returns the document before or after operation

        public void MongoCrud_FindAndDoOperation()
        {
            var document = FindOneAndUpdate();
            MongoOperationsVerifier.FindOneAndUpdate(_runner.ConnectionString, document);
        }

        [TearDown]
        public void CleanUp()
        {
            _runner.Dispose();
        }

        private void InsertOne(string ObjectId)
        {
            if (null == ObjectId)
            {
                mongoCollection.InsertOne(new Test() { Name = "MyName" });
            }
            else
            {
                mongoCollection.InsertOne(new Test() { Id = ObjectId, Name = "MyName" });
            }
            
        }

        private IEnumerable<Test> InsertMany()
        {
            var documents = Enumerable.Range(0, 10).Select(i => new Test(){ Id = ObjectId.GenerateNewId().ToString(), Name = "MyName"+i }).ToList();
            mongoCollection.InsertMany(documents);
            return documents;
        }

        private IEnumerable<Test> UpdateOne()
        {
            var documents = InsertMany();
            var filter = new BsonDocument();
            var update = new BsonDocument("$set", new BsonDocument("Name","UpdatedName" ));
            mongoCollection.UpdateOne(filter, update);
            return documents;
        }

        private IEnumerable<Test> UpdateMany()
        {
            var documents = InsertMany();
            var filter = new BsonDocument();
            var update = new BsonDocument("$set", new BsonDocument("Name", "UpdatedName"));
            mongoCollection.UpdateMany(filter, update);
            return documents;
        }

        private IEnumerable<Test> DeleteOne()
        {
            var documents = InsertMany();
            var filter = new BsonDocument(new BsonDocument("Name", "MyName0"));
            mongoCollection.DeleteOne(filter);
            return documents;
        }

        private Test FindOneAndUpdate()
        {
            InsertMany();
            var filter = new BsonDocument(new BsonDocument("Name", "MyName0"));
            var update = new BsonDocument("$set", new BsonDocument("Name", "UpdatedName"));
            var options = new FindOneAndUpdateOptions<Test>
            {
                ReturnDocument = ReturnDocument.After
            };
            return mongoCollection.FindOneAndUpdate(filter, update, options);
        }

    }
   
    public class Test
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}