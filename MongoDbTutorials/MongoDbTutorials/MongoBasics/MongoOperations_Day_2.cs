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

            var beforeUpdate = mongoCollection.Find(Builders<Test>.Filter.Empty).ToList();
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

            var filter = Builders<AirTravel>.Filter.Eq(x => x.FirstName, "Krishna");
            var foodPrefrence = new List<FoodTypes>() { FoodTypes.Indian_NonVeg };
            var updateOperation = Builders<AirTravel>.Update.Set(x => x.FoodPreferences, foodPrefrence);
            travelCollection.UpdateOne(filter, updateOperation);

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
            var beforeUpdate = mongoCollection.Find(Builders<Test>.Filter.Empty).ToList();
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
            var beforeUpdate = mongoCollection.Find(Builders<Test>.Filter.Empty).ToList();
            var filter = new BsonDocument(new BsonDocument("Name", "MyName0"));
            var filter1 = Builders<Test>.Filter.Eq(x => x.Name, "MyNAme");
            mongoCollection.DeleteMany(filter);

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

            var beforeUpdate = travelCollection.Find(Builders<AirTravel>.Filter.Empty).ToList();
            var filter = Builders<AirTravel>.Filter.Lt(x => x.Age, 18);
            travelCollection.DeleteMany(filter);

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
        // upadte the travel date to today UTC of given booking ID "PGS1789"

        public void MongoCrud_07_2_Find_Customer_with_Booking_ID_AndUpdate_TravelDate()
        {
            #region mongoConnection

            travelCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<AirTravel>("travel");

            #endregion
            #region data preparation
            InsertTravelDetails();
            #endregion
            var beforeUpdate = travelCollection.Find(Builders<AirTravel>.Filter.Empty).ToList();
            var filter = Builders<AirTravel>.Filter.ElemMatch(x => x.TravelHistory, his => his.BookingID == "PGS1789");
            var updateOperation = Builders<AirTravel>.Update.Set(x => x.TravelHistory.ElementAt(-1).TravelDate, DateTime.Now);

            var options = new FindOneAndUpdateOptions<AirTravel>
            {
                ReturnDocument = ReturnDocument.After
            };
            var updatedDocuemnt = travelCollection.FindOneAndUpdate(filter, updateOperation, options);

            #region verification
            TravelOperationVerifier.Verify_Travel_Date_Updated(_runner.ConnectionString);
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