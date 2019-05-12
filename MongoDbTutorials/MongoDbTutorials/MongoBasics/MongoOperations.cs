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
            MongoOperationsVerifier.VerifyInsertOne(_runner.ConnectionString, objectId);
        }

        [Test]
        // Insert a list of 10 documents in database named "testdb", in a collection named "testcollection"
        //The document to be inserted should be of type Test Class
        public void MongoCrud_02_InsertMany_Document()
        {
            List<Test> documents = null;
            MongoOperationsVerifier.VerifyInsertMany(_runner.ConnectionString, documents);
        }

        [Test]
        public void MongoCrud_03_Find_Document()
        {
            //Insert 10 documents with name as Myname + i \\\


            Test dbDocument = null;
            MongoOperationsVerifier.VerifyFindMyName0(dbDocument);


            //find All\\\
            /**********************************************/
            //var filterFindAll = new BsonDocument();

            List<Test> dbDocuments = null;

            MongoOperationsVerifier.VerifyFindAll(dbDocuments);

        }


        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "testcollection"
        // update the first Document to UpdatedName
        // although multiple documents match only first document will be updated

        public void MongoCrud_04_UpdateOne_SetDocumentWithName_MyName1_To_UpdatedName()
        {
            List<Test> documents = null;
            MongoOperationsVerifier.VerifyUpdateOne(_runner.ConnectionString, documents);
        }

        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "testcollection"
        // update all the Documents matching filter condition

        public void MongoCrud_05_UpdateMany_Set_All_Document_Name_To_UpdatedName()
        {
            List<Test> documents = null;
            MongoOperationsVerifier.VerifyUpdateMany(_runner.ConnectionString, documents);
        }

        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "testcollection"
        // delete the document with name "MyName0"

        public void MongoCrud_06_DeleteOne_DocumentWithName_MyName0()
        {
            List<Test> documents = null;
            MongoOperationsVerifier.VerifyDeleteOne(_runner.ConnectionString, documents);
        }

        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "testcollection"
        // find a document and do the specified operation
        // support for three type of operation FindOneAndUpdate, FindOneAndReplace, FindOneAndDelete
        // Returns the document before or after operation

        public void MongoCrud_07_FindAndDoOperation_Update_MyName0_UpdatedName()
        {
            Test document = null;
            MongoOperationsVerifier.VerifyFindOneAndUpdate(_runner.ConnectionString, document);
        }

        [Test]
        // Insert a list of Documents
        // Find All Documents with Name starting with Myname
        // return only first 2 documents
        public void MongoCrud_08_Limit_The_Number_Of_Records()
        {
            List<Test> documents = null;
            Assert.AreNotEqual(documents, null);
            Assert.AreEqual(documents.Count, 2);
        }

        [Test]
        // Insert a list of Documents
        // Find All Documents with Name starting with Myname
        // skip the first 5 documents
        public void MongoCrud_09_Skip_First_Five_Documents()
        {
            List<Test> documents = null;
            Assert.AreNotEqual(documents, null);
            Assert.AreEqual(documents.Count, 2);
        }

        [Test]
        // Insert a list of Documents
        // find the first document order by name in desc order
        public void MongoCrud_10_Sort_Document_By_Name_Descending()
        {

            Test document = null;
            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Name, "MyName9");

        }

        [Test]
        //project only the Name in returned documents
        public void MongoCrud_11_Project_Document_Name()
        {
            BsonDocument documents = null;
            Assert.AreNotEqual(documents, null);
            Assert.AreEqual(documents.ElementCount, 1);

        }

        [Test]
        //insert 10 objects of same name, group by name and project name and count of name
        public void MongoCrud_12_Group_Document_By_Name()
        {

            var document = new { Name = "", Count = 0 };
            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Name, "UpdatedName");
            Assert.AreEqual(document.Count, 10);

        }

        [Test]
        //insert 10 objects of same name, group by name and project name and count of name and order by count descending
        public void MongoCrud_12_Group_Document_By_Various_Names()
        {
            var document = new { Name = "", Count = 0 };
            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Name, "UpdatedName");
            Assert.AreEqual(document.Count, 10);

        }
        [TearDown]
        public void CleanUp()
        {
            _runner.Dispose();
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