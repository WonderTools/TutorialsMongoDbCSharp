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
using WonderTools.JsonSectionReader;
using System.Text;

namespace MongoDbTutorials.MongoDbTutorials.MongoBasics
{
    public class MongoOperations_Day_2
    {
        private MongoDbRunner _runner;
        private IMongoCollection<Test> mongoCollection;
        private IMongoCollection<AirTravel> travelCollection;
        private JSection testData;


        [SetUp]
        public void Setup()
        {
            _runner = MongoDbRunner.Start();
            testData = JSectionReader.Section("AirTravel.json", Encoding.UTF8);
        }


        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "testcollection"
        // update the column Name with value "MyName1" to "UpdatedName"
        // although multiple documents match only first document will be updated

        public void MongoCrud_04_1_UpdateOne_SetDocumentWithName_MyName1_To_UpdatedName()
        {
            #region mongoConnection

            mongoCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<Test>("testcollection");

            #endregion
            #region data preparation
            var documents = InsertMany();
            #endregion
            
            var filter = Builders<Test>.Filter.Eq(x => x.Name, "MyName1");
            var updateOperation = Builders<Test>.Update.Set(x => x.Name, "UpdatedName");
            mongoCollection.UpdateOne(filter, updateOperation);

            #region verification
            MongoOperationsVerifier.VerifyUpdateOne(_runner.ConnectionString, documents);
            #endregion
        }

        #region excercise 1
        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "travel"
        // update the passenger krishna foodprefrence to only "Indian_NonVeg"

        public void MongoCrud_04_2_Update_Food_Preference()
        {
            #region mongoConnection

            travelCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<AirTravel>("travel");

            #endregion
            #region data preparation
            InsertTravelDetails();
            #endregion

            ///insert code here///

            #region verification
            TravelOperationVerifier.VerifyUpdateFoodPrefrence(_runner.ConnectionString);
            #endregion
            #region data cleanup
            cleanUpTravelCollection();
            #endregion
        }

        #endregion

        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "testcollection"
        // update all the Documents with Name column to "UpdatedName"

        public void MongoCrud_05_UpdateMany_Set_All_Document_Name_To_UpdatedName()
        {
            #region mongoConnection

            mongoCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<Test>("testcollection");

            #endregion
            #region data preparation
            var documents = InsertMany();
            #endregion

            var filter = Builders<Test>.Filter.Empty;
            var updateOperation = Builders<Test>.Update.Set(x => x.Name, "UpdatedName");
            mongoCollection.UpdateMany(filter, updateOperation);

            #region verification
            MongoOperationsVerifier.VerifyUpdateMany(_runner.ConnectionString, documents);


            #endregion
        }

        
        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "testcollection"
        // delete the document with name "MyName0"

        public void MongoCrud_06_1_DeleteOne_DocumentWithName_MyName0()
        {
            #region mongoConnection

            mongoCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<Test>("testcollection");

            #endregion
            #region data preparation
            var documents = InsertMany();
            #endregion

            var filter = new BsonDocument(new BsonDocument("Name", "MyName0"));
            mongoCollection.DeleteOne(filter);

            #region verification
            MongoOperationsVerifier.VerifyDeleteOne(_runner.ConnectionString, documents);

            #endregion
        }

        #region excercise 2
        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "travel"
        // remove all minor passengers

        public void MongoCrud_06_2_Customers_With_Age_below_18()
        {
            #region mongoConnection

            travelCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<AirTravel>("travel");

            #endregion
            #region data preparation
            InsertTravelDetails();
            #endregion

           ///insert code here

            #region verification
            TravelOperationVerifier.Verify_No_Minor_Passenger(_runner.ConnectionString);
            #endregion
            #region data cleanup
            cleanUpTravelCollection();
            #endregion
        }

        #endregion
        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "testcollection"
        // find a document with name "MyName0" and updated it to "UpdatedName"
        // support for three type of operation FindOneAndUpdate, FindOneAndReplace, FindOneAndDelete
        // Returns the document before or after operation

        public void MongoCrud_07_1_FindAndDoOperation_Update_MyName0_UpdatedName()
        {
            #region mongoConnection

            mongoCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<Test>("testcollection");

            #endregion
            #region data preparation
            var documents = InsertMany();
            #endregion

            var filterDefinitionBuilder = Builders<Test>.Filter;
            var filter = filterDefinitionBuilder.Eq(x => x.Name, "MyName0");
            var updateOperation = Builders<Test>.Update.Set(x => x.Name, "UpdatedName");
            var options = new FindOneAndUpdateOptions<Test>
            {
                ReturnDocument = ReturnDocument.After
            };
            var document = mongoCollection.FindOneAndUpdate(filter, updateOperation, options);

            #region verification
            MongoOperationsVerifier.VerifyFindOneAndUpdate(_runner.ConnectionString, document);

            #endregion

        }

        #region excercise 3
        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "travel"
        // upadte the travel of given booking ID "PGS1789"

        public void MongoCrud_07_2_Find_Customer_with_Booking_ID_AndUpdate_TravelDate()
        {
            #region mongoConnection

            travelCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<AirTravel>("travel");

            #endregion
            #region data preparation
            InsertTravelDetails();
            #endregion

           //insert code here

            #region verification
            TravelOperationVerifier.Verify_Travel_Date_Updated(_runner.ConnectionString);
            #endregion
            #region data cleanup
            cleanUpTravelCollection();
            #endregion
        }

        #endregion
        [Test]
        // Insert a list of Documents
        // Find All Documents with Name starting with Myname
        // return only first 2 documents
        public void MongoCrud_08_Limit_The_Number_Of_Records()
        {
            #region mongoConnection

            mongoCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<Test>("testcollection");

            #endregion
            #region data preparation
            InsertMany();
            #endregion

            var filter = Builders<Test>.Filter.Regex(x => x.Name, BsonRegularExpression.Create(new Regex("MyName.*")));
            var documents = mongoCollection.Find(filter).Limit(2).ToList();

            #region verification
            Assert.AreNotEqual(documents, null);
            Assert.AreEqual(documents.Count, 2);

            #endregion

        }

        [Test]
        // Insert a list of Documents
        // Find All Documents with Name starting with Myname
        // skip the first 5 documents
        // select * from test where name like "Myname%"
        public void MongoCrud_09_Skip_First_Five_Documents()
        {
            #region mongoConnection

            mongoCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<Test>("testcollection");

            #endregion
            #region data preparation
            InsertMany();
            #endregion

            var filter = Builders<Test>.Filter.Regex(x => x.Name, BsonRegularExpression.Create(new Regex("MyName.*")));
            var documents = mongoCollection.Find(filter).Limit(2).ToList();

            #region verification
            Assert.AreNotEqual(documents, null);
            Assert.AreEqual(documents.Count, 2);

            #endregion

        }

        [Test]
        // Insert a list of Documents
        // find the first document order by name in desc order
        // select * from test order by name
        public void MongoCrud_10_1_Sort_Document_By_Name_Descending()
        {

            #region mongoConnection

            mongoCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<Test>("testcollection");

            #endregion
            #region data preparation
            InsertMany();
            #endregion

            var filter = Builders<Test>.Filter.Empty;
            var document = mongoCollection.Find(filter).Sort(Builders<Test>.Sort.Descending(x => x.Name)).FirstOrDefault();

            #region verification
            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Name, "MyName9");

            #endregion


        }

        #region excercise 4
        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "travel"
        // find the recent second-two travlers with name starting with S order by name desc

        public void MongoCrud_10_2_Find_Second_two_Recent_Passengers_with_Name_Starting_With_S_Order_By_Name_Dsc()
        {
            #region mongoConnection

            travelCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<AirTravel>("travel");

            #endregion
            #region data preparation
            InsertTravelDetails();
            #endregion

            //insert code here

            var result = new List<AirTravel> { };
            #region verification
            TravelOperationVerifier.Verify_Passengers_NameStarting_with_S(_runner.ConnectionString, result);
            #endregion
            #region data cleanup
            cleanUpTravelCollection();
            #endregion
        }

        #endregion

        [Test]
        // fetch The list user names
        // select name from test
        public void MongoCrud_11_Project_Document_Name()
        {
            #region mongoConnection

            mongoCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<Test>("testcollection");

            #endregion
            #region data preparation
            InsertMany();
            #endregion

            var filter = Builders<Test>.Filter.Empty;
            var document = mongoCollection.Find(filter).Project(Builders<Test>.Projection.Include(x => x.Name).Exclude(x => x.Id))
                .ToList().FirstOrDefault();

            #region verification
            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.ElementCount, 1);

            #endregion


        }

       
        [Test]
        //insert 10 objects of same name, group by name and project name and count of name and order by count descending
        // select name, count(*)  as count from test group by name order by count desc
        public void MongoCrud_12_1_Group_Document_By_Various_Names()
        {
            #region mongoConnection

            mongoCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<Test>("testcollection");

            #endregion
            #region data preparation
            UpdateMany();
            InsertMany();
            #endregion

            var query = mongoCollection.AsQueryable()
                   .GroupBy(p => p.Name)
                   .Select(g => new { Name = g.Key, Count = g.Count() })
                   .OrderByDescending(x => x.Count);
            var documents = query.ToList();
            var document = documents.FirstOrDefault();

            #region verification
            Assert.AreNotEqual(document, null);
            Assert.AreEqual(document.Name, "UpdatedName");
            Assert.AreEqual(document.Count, 10);

            #endregion


        }

        #region excercise 5
        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "travel"
        // find the count of passengers with similar travel frequency order by count desc

        public void MongoCrud_12_2_Find_Th_count_pasengers_with_similar_travel_frequency()
        {
            #region mongoConnection

            travelCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<AirTravel>("travel");

            #endregion
            #region data preparation
            InsertTravelDetails();
            #endregion
                        
            //insert code here

            var documents = new List<AirTravel> { };

            #region verification
            Assert.AreEqual(documents.Count, 3);
           // Assert.AreEqual(documents.ElementAt(1).Count, 2);
            #endregion
            #region data cleanup
            cleanUpTravelCollection();
            #endregion
        }

        #endregion
        [TearDown]
        public void CleanUp()
        {
            _runner.Dispose();
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
        private List<Test> UpdateOne()
        {
            var documents = InsertMany();
            // var filter = new BsonDocument();
            // var update = new BsonDocument("$set", new BsonDocument("Name","UpdatedName" ));
            
            var filter = Builders<Test>.Filter.Eq(x => x.Name, "MyName1");
            var updateOperation = Builders<Test>.Update.Set(x => x.Name, "UpdatedName");
            mongoCollection.UpdateOne(filter, updateOperation);
            return documents;
        }

        private List<Test> UpdateMany()
        {
            var documents = InsertMany();
            var filter = Builders<Test>.Filter.Empty;
            var updateOperation = Builders<Test>.Update.Set(x => x.Name, "UpdatedName");
            mongoCollection.UpdateMany(filter, updateOperation);
            return documents;
        }

        private List<Test> DeleteOne()
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

            var filterDefinitionBuilder = Builders<Test>.Filter;
            var filter = filterDefinitionBuilder.Eq(x => x.Name, "MyName0");
            var updateOperation = Builders<Test>.Update.Set(x => x.Name, "UpdatedName");
            var options = new FindOneAndUpdateOptions<Test>
            {
                ReturnDocument = ReturnDocument.After
            };
            return mongoCollection.FindOneAndUpdate(filter, updateOperation, options);

        }

        private void InsertTravelDetails()
        {
            var travelData = testData.GetSection("AirTravel").GetObject<List<AirTravel>>();
            travelCollection.InsertMany(travelData);

        }
        private void cleanUpTravelCollection()
        {
            travelCollection.Database.DropCollection("travel");
        }
    }

     
}