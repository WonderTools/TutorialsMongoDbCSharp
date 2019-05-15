using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Mongo2Go;
using MongoDbTutorials.MongoDbTutorials.MongoBasics;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using NUnit.Framework;
using MongoDbTutorials.MongoDbTutorials.MongoBasics.Model;

namespace MongoDbTutorials.MongoDbTutorials.MongoBasics
{
    public class MongoOperations_Day_1
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
        public void MongoCrud_01_InsertOne_Document()
        {
            var objectId = ObjectId.GenerateNewId().ToString();
            InsertOne(objectId);
            MongoOperationsVerifier.VerifyInsertOne(_runner.ConnectionString, objectId);
        }

        [Test]
        // Insert a list of 10 documents in database named "testdb", in a collection named "testcollection"
        //The document to be inserted should be of type Test Class
        public void MongoCrud_02_InsertMany_Document()
        {
            List<Test> documents = InsertMany();
            MongoOperationsVerifier.VerifyInsertMany(_runner.ConnectionString, documents);
        }

        [Test]
        public void MongoCrud_03_1_Find_All_Document()
        {
            //Insert 10 documents with name as Myname + i \\\
            var documents = InsertMany();

            var filterFindAll = Builders<Test>.Filter.Empty;
            var dbDocuments = mongoCollection.Find(filterFindAll).ToList();

            MongoOperationsVerifier.VerifyFindAll(dbDocuments);

        }

        [Test]
        public void MongoCrud_03_2_Find_DocumentBy_Name()
        {
            //Insert 10 documents with name as Myname + i \\\
            var documents = InsertMany();

            var filterFindOne = Builders<Test>.Filter.Eq(x => x.Name, "MyName0");
            var dbDocument = mongoCollection.Find(filterFindOne).FirstOrDefault();
            MongoOperationsVerifier.VerifyFindMyName0(dbDocument);

        }



        [TearDown]
        public void CleanUp()
        {
            _runner.Dispose();
        }

        private void InsertOne(string ObjectId)
        {
            mongoCollection.InsertOne(new Test()
            { Id = ObjectId, Name = "MyName", Age = 20 });

        }

        private List<Test> InsertMany()
        {
            var documents = Enumerable.Range(0, 10)
                .Select(i => new Test()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "MyName" + i,
                    Age = 10 + i * 10
                }).ToList();
            mongoCollection.InsertMany(documents);
            return documents;
        }

    }

        
    

}