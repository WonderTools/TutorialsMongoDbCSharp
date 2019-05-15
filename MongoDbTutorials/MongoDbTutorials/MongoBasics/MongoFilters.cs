using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDbTutorials.MongoDbTutorials.MongoBasics.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WonderTools.JsonSectionReader;

namespace MongoDbTutorials.MongoDbTutorials.MongoBasics
{
    class MongoFilters
    {

            private MongoDbRunner _runner;
            private IMongoCollection<Test> mongoCollection;

            private JSection testData;


        [SetUp]
            public void Setup()
            {
                _runner = MongoDbRunner.Start();
                mongoCollection = new MongoClient(_runner.ConnectionString).GetDatabase("testdb").GetCollection<Test>("testcollection");
                testData = JSectionReader.Section("AirTravel.json", Encoding.UTF8);
             }

            [Test]
            //design a  schema model for airtravel documents
            //try to read the dat ainto the model
            public void MongoCrud_00_Filters_Document()
            {
                var airTravelDocument = testData.GetSection("AirTravel").GetObject<List<AirTravel>>();
                Assert.AreNotEqual(null, airTravelDocument);
            }

             [Test]
            public void MongoCrud_01_Filters_Passengers_Having_Age_Above_25()
            {
                var filter = Builders<AirTravel>.Filter.Gt(x => x.Age, 25);
                var json = filter.ToJson();

        }
            [Test]
            public void MongoCrud_02_Filters_Passengers_Having_Delhi_In_TravelHistory()
            {
                var filter = Builders<AirTravel>.Filter.ElemMatch(x => x.TravelHistory, his => his.From == "Delhi")
                            | Builders<AirTravel>.Filter.ElemMatch(x => x.TravelHistory, his => his.Destination == "Delhi");

                var json = filter.ToJson();
                
            }
            
            [Test]
            public void MongoCrud_03_Filters_Passengers_Frequently_Travelling_BangaloreToDelhi()
            {
                var filter = Builders<AirTravel>.Filter.ElemMatch(x => x.TravelFrequency, fre => fre.ToFro == "Bangalore-Delhi")
                            | Builders<AirTravel>.Filter.ElemMatch(x => x.TravelFrequency, fre => fre.Frequency > 1);
                var json = filter.ToJson();

        }
            
             [Test]
            public void MongoCrud_04_Filters_Passengers_NonVeg_Food_Prefence()
            {
                var filter = Builders<AirTravel>.Filter.AnyEq(x => x.FoodPrefrence,FoodTypes.Indian_NonVeg);
                var json = filter.ToJson();

        }
        }
}
