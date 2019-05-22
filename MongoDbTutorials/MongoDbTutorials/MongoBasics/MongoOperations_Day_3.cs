using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using MongoDbTutorials.MongoDbTutorials.MongoBasics.Model;
using WonderTools.JsonSectionReader;
using System.Text;
using MongoDB.Bson.Serialization;

namespace MongoDbTutorials.MongoDbTutorials.MongoBasics
{
    public class MongoOperations_Day_3
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


        #region excercise 1
        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "travel"
        // delete food prefernce for krishna

        public void MongoCrud_07_3_Remove_Food_Preference()
        {
            #region mongoConnection

            travelCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<AirTravel>("travel");

            #endregion
            #region data preparation
            InsertTravelDetails();
            var beforeUpdate = travelCollection.Find(Builders<AirTravel>.Filter.Empty).ToList();
            #endregion

            FilterDefinition<AirTravel> filter = null;
            UpdateDefinition<AirTravel> updateOperation = null;

            #region printfilter

            PrintFilter(filter, updateOperation);
            #endregion



            #region verification
            TravelOperationVerifier.Verify_Removed_Food_Prefrence(_runner.ConnectionString);
            #endregion
            #region data cleanup
            cleanUpTravelCollection();
            #endregion
        }
        #endregion
        #region excercise 2
        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "travel"
        // add food Prefrence "Indian_Veg" to Shubham

        public void MongoCrud_07_4_Add_Food_Preference()
        {
            #region mongoConnection

            travelCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<AirTravel>("travel");

            #endregion
            #region data preparation
            InsertTravelDetails();
            var beforeUpdate = travelCollection.Find(Builders<AirTravel>.Filter.Empty).ToList();

            #endregion




            #region verification
            TravelOperationVerifier.Verify_Add_Food_Preference(_runner.ConnectionString);
            #endregion
            #region data cleanup
            cleanUpTravelCollection();
            #endregion
        }
        #endregion
        #region excercise 3

        [Test]
        // find a traveller with booking id "MYS34394"
        // update the details as present in "FindAndUpdatePassenger" of sirtravel test data
        // hint : use the jsection reader to read the data.
        // how to upsert
        public void MongoCrud_07_5_Update_An_AirTraveller()
        {
            #region mongoConnection

            travelCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<AirTravel>("travel");

            #endregion
            #region data preparation
            InsertTravelDetails();
            #endregion



            #region verification
            TravelOperationVerifier.Verify_Add_Food_Preference(_runner.ConnectionString);
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

            //we can also use SortBy to just pass the condition
            // it provide an interface to club more sort conditions using ThenBy


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

            
            List<AirTravel> result = null;

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

        #region excercise 5
        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "travel"
        // find the recent second-two recent travlers with name starting with S order by travel date desc
        // project first name , last name and latest travelhistory

        public void MongoCrud_10_2_Find_Second_two_Recent_Passengers_Starting_With_S_OrderBy_TravelDate_Dsc_Project()
        {
            #region mongoConnection

            travelCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<AirTravel>("travel");

            #endregion
            #region data preparation
            InsertTravelDetails();
            #endregion

            
            

            #region verification
            //Assert.True(result.ElementAt(0).hist.TravelDate > result.ElementAt(1).hist.TravelDate);
            #endregion
            #region data cleanup
            cleanUpTravelCollection();
            #endregion
        }

        #endregion

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
        #region excercise 7
        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "travel"
        // find the passengers with  age above 26
        public void MongoCrud_12_2_Find_Travellers_Above_26()
        {
            #region mongoConnection

            travelCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<AirTravel>("travel");

            #endregion
            #region data preparation
            InsertTravelDetails();
            #endregion


           
            List<AirTravel> output = null;
            #region verification
            Assert.AreEqual(output.Count(), 1);
            #endregion
            #region data cleanup
            cleanUpTravelCollection();
            #endregion

        }
        #endregion
        #region excercise 8
        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "travel"
        // find the count of passengers of different age older than 25
        public void MongoCrud_12_3_Find_Travellers_Older_Than_25()
        {
            #region mongoConnection

            travelCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<AirTravel>("travel");

            #endregion
            #region data preparation
            InsertTravelDetails();
            #endregion


            
            List<AirTravel> output = null;
            #region verification
            Assert.AreEqual(output.Count(), 2);
            #endregion
            #region data cleanup
            cleanUpTravelCollection();
            #endregion

        }
        #endregion
        #region excercise 9
        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "travel"
        // find the average age  of passengers
        public void MongoCrud_12_3_Find_Average_Age__Of_Travelers()
        {
            #region mongoConnection

            travelCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<AirTravel>("travel");

            #endregion
            #region data preparation
            InsertTravelDetails();
            #endregion


            int AvgAge = 1;
            #region verification
            Assert.AreEqual(AvgAge, 23);
            #endregion
            #region data cleanup
            cleanUpTravelCollection();
            #endregion

        }
        #endregion

        #region excercise 10
        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "travel"
        // find the list of ages order in desc order
        public void MongoCrud_12_4_Find_List_Of_Ages__Of_Travelers()
        {
            #region mongoConnection

            travelCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<AirTravel>("travel");

            #endregion
            #region data preparation
            InsertTravelDetails();
            #endregion


            List<AirTravel> AvgAge = null;
            #region verification
            Assert.AreEqual(AvgAge.ElementAt(0), 29);
            #endregion
            #region data cleanup
            cleanUpTravelCollection();
            #endregion

        }
        #endregion

        #region excercise 11
        [Test]
        // Insert a list of documenst in database named "testdb", in a collection named "travel"
        // find teh count of passengers with similar travel frequency order by count desc

        public void MongoCrud_12_3_Find_The_count_of_pasengers_with_similar_travel_frequency()
        {
            #region mongoConnection

            travelCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<AirTravel>("travel");

            #endregion
            #region data preparation
            InsertTravelDetails();
            #endregion


           

            List<AirTravel> documentsLinq = null;
            #region verification
            //Assert.AreEqual(documentsLinq.ElementAt(1).Freq, 13);
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

        private void PrintFilter(FilterDefinition<AirTravel> filter, UpdateDefinition<AirTravel> updateOperation)
        {
            Console.WriteLine(filter.Render(BsonSerializer.SerializerRegistry.GetSerializer<AirTravel>(), BsonSerializer.SerializerRegistry));
            Console.WriteLine(updateOperation.Render(BsonSerializer.SerializerRegistry.GetSerializer<AirTravel>(), BsonSerializer.SerializerRegistry));
        }
    }

     
}