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
            var documents = InsertMany();
            MongoOperationsVerifier.VerifyInsertMany(_runner.ConnectionString, documents);
        }

        [Test]
        public void MongoCrud_03_Find_Document()
        {
            //Insert 10 documents with name as Myname + i \\\
            var documents = InsertMany();

            //find All\\\
            /**********************************************/
            //var filterFindAll = new BsonDocument();
            var filterFindAll = Builders<Test>.Filter.Empty;
            var dbDocuments = mongoCollection.Find(filterFindAll).ToList();
            
            //using cursors
            using (var cursor = mongoCollection.Find(filterFindAll).ToCursor())
            {
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        Console.WriteLine("Documents", doc);
                    }
                }
            }

            
            //passing options
            var options = new FindOptions
            {
                MaxTime = TimeSpan.FromMilliseconds(20)
            };


            //Find One \\
            var filterFindOne = Builders<Test>.Filter.Eq( x => x.Name, "MyName0");
            var dbDocument = mongoCollection.Find(filterFindOne).FirstOrDefault();
            MongoOperationsVerifier.VerifyFindMyName0(dbDocument);
        }


        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "testcollection"
        // update the first Document to UpdatedName
        // although multiple documents match only first document will be updated

        public void MongoCrud_04_UpdateOne_SetDocumentWithName_MyName1_To_UpdatedName()
        {
            var documents = UpdateOne();
            MongoOperationsVerifier.VerifyUpdateOne(_runner.ConnectionString, documents);
        }

        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "testcollection"
        // update all the Documents matching filter condition

        public void MongoCrud_05_UpdateMany_Set_All_Document_Name_To_UpdatedName()
        {
            var documents = UpdateMany();
            MongoOperationsVerifier.VerifyUpdateMany(_runner.ConnectionString, documents);
        }

        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "testcollection"
        // delete the document with name "MyName0"

        public void MongoCrud_06_DeleteOne_DocumentWithName_MyName0()
        {
            var documents = DeleteOne();
            MongoOperationsVerifier.VerifyDeleteOne(_runner.ConnectionString, documents);
        }

        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "testcollection"
        // find a document and do the specified operation
        // support for three type of operation FindOneAndUpdate, FindOneAndReplace, FindOneAndDelete
        // Returns the document before or after operation

        public void MongoCrud_07_FindAndDoOperation_Update_MyName0_UpdatedName()
        {
            var document = FindOneAndUpdate();
            MongoOperationsVerifier.VerifyFindOneAndUpdate(_runner.ConnectionString, document);
        }

        [Test]
        // Insert a list of Documents
        // Find All Documents with Name starting with Myname
        // return only first 2 documents
        public void MongoCrud_08_Limit_The_Number_Of_Records()
        {
            InsertMany();
            var filter = Builders<Test>.Filter.Regex(x => x.Name, BsonRegularExpression.Create(new Regex("MyName.*")));
            var documents = mongoCollection.Find(filter).Limit(2).ToList();
            Assert.AreEqual(documents.Count, 2);
        }

        [Test]
        // Insert a list of Documents
        // Find All Documents with Name starting with Myname
        // skip the first 5 documents
        public void MongoCrud_09_Skip_First_Five_Documents()
        {
            InsertMany();
            var filter = Builders<Test>.Filter.Regex(x => x.Name, BsonRegularExpression.Create(new Regex("MyName.*")));
            var documents = mongoCollection.Find(filter).Limit(2).ToList();
            Assert.AreEqual(documents.Count, 2);
        }

        [Test]
        // Insert a list of Documents
        // find the first document order by name in desc order
        public void MongoCrud_10_Sort_Document_By_Name_Descending()
        {
            InsertMany();
            var filter = Builders<Test>.Filter.Empty;
            var document = mongoCollection.Find(filter).Sort(Builders<Test>.Sort.Descending(x => x.Name)).FirstOrDefault();
            Assert.AreEqual(document.Name, "MyName9");
            
        }

        [Test]
        //project only the Name in returned documents
        public void MongoCrud_11_Project_Document_Name()
        {
            InsertMany();
            var filter = Builders<Test>.Filter.Empty;
            var document = mongoCollection.Find(filter).Project(Builders<Test>.Projection.Include(x => x.Name).Exclude(x => x.Id ))
                .ToList().FirstOrDefault();
            Assert.AreEqual(document.ElementCount, 1);

        }

        [Test]
        //insert 10 objects of same name, group by name and project name and count of name
        public void MongoCrud_12_Group_Document_By_Name()
        {
            UpdateMany();
            var query = mongoCollection.AsQueryable()
                .GroupBy(p => p.Name)
                .Select(g => new { Name = g.Key, Count = g.Count()})
                ;
            var document = query.ToList().FirstOrDefault();
            Assert.AreNotEqual(document,null);
            Assert.AreEqual(document.Name, "UpdatedName");
            Assert.AreEqual(document.Count, 10);

        }

        [Test]
        //insert 10 objects of same name, group by name and project name and count of name and order by count descending
        public void MongoCrud_12_Group_Document_By_Various_Names()
        {
            UpdateMany();
            InsertMany();
            var query = mongoCollection.AsQueryable()
                    .GroupBy(p => p.Name)
                    .Select(g => new { Name = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count);
            var documents = query.ToList();
            var document = documents.FirstOrDefault();
            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Name, "UpdatedName");
            Assert.AreEqual(document.Count, 10);

        }
        [TearDown]
        public void CleanUp()
        {
            _runner.Dispose();
        }

        private void InsertOne(string ObjectId)
        {
            mongoCollection.InsertOne(new Test() { Id = ObjectId, Name = "MyName" , Age = 20 });
            
        }

        private IEnumerable<Test> InsertMany()
        {
            var documents = Enumerable.Range(0, 10).Select(i => new Test()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "MyName"+i,
                Age = 10 + i*10
            }).ToList();
            mongoCollection.InsertMany(documents);
            return documents;
        }

        private IEnumerable<Test> UpdateOne()
        {
            var documents = InsertMany();
            // var filter = new BsonDocument();
            // var update = new BsonDocument("$set", new BsonDocument("Name","UpdatedName" ));

            var filter = Builders<Test>.Filter.Eq(x => x.Name, "MyName1");
            var updateOperation = Builders<Test>.Update.Set(x => x.Name, "UpdatedName");
            mongoCollection.UpdateOne(filter, updateOperation);
            return documents;
        }

        private IEnumerable<Test> UpdateMany()
        {
            var documents = InsertMany();
            // var filter = new BsonDocument();
            // var update = new BsonDocument("$set", new BsonDocument("Name", "UpdatedName"));
            var filter = Builders<Test>.Filter.Empty;
            var updateOperation = Builders<Test>.Update.Set(x => x.Name, "UpdatedName");
            mongoCollection.UpdateMany(filter, updateOperation);
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
            //insert Documents
            InsertMany();
            // var filter = new BsonDocument(new BsonDocument("Name", "MyName0"));
            // var update = new BsonDocument("$set", new BsonDocument("Name", "UpdatedName"));

            var filterDefinitionBuilder = Builders<Test>.Filter;
            var filter = filterDefinitionBuilder.Eq(x => x.Name, "MyName0");
            var updateOperation = Builders<Test>.Update.Set(x => x.Name, "UpdatedName");
            var options = new FindOneAndUpdateOptions<Test>
            {
                ReturnDocument = ReturnDocument.After
            };
            return mongoCollection.FindOneAndUpdate(filter, updateOperation, options);

        }

    }
   
    public class Test
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }

        public int Age { get; set; }
    }
}