using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDbTutorials.MongoDbTutorials.MongoBasics.Model;
using NUnit.Framework;

namespace MongoDbTutorials.MongoDbTutorials.MongoBasics
{
    public class TravelOperationVerifier
    {
        
        public static void VerifyUpdateFoodPrefrence(string connectionString)
        {

            var filter = Builders<AirTravel>.Filter.Eq(x => x.FirstName, "Krishna");
            var result = GetCollection(connectionString).FindAsync(filter);
            var resultData = result.Result.ToList().FirstOrDefault();
            var foodPrefrence = resultData.FoodPrefrence;
            Assert.AreEqual(1, foodPrefrence.Count, "More than one food prefrence exists for passenger");
            Assert.AreEqual(FoodTypes.Indian_NonVeg, foodPrefrence.FirstOrDefault(), "food prefrence not equal to Indian_NonVeg");
        }

        public static void Verify_No_Minor_Passenger(string connectionString)
        {

            var filter = Builders<AirTravel>.Filter.Lt(x => x.Age, 18);
            var result = GetCollection(connectionString).FindAsync(filter);
            var resultData = result.Result.ToList();
            Assert.AreEqual(0, resultData.Count, "Minor Passengers present");
        }

        public static void Verify_Travel_Date_Updated(string connectionString)
        {

            var filter = Builders<AirTravel>.Filter.ElemMatch(x => x.TravelHistory,
                his => his.BookingID == "PGS1789");
            var result = GetCollection(connectionString).FindAsync(filter);
            var resultData = result.Result.ToList();
            var travelHistoryForGivenBookingId = resultData.FirstOrDefault().
                TravelHistory.Where(h => h.BookingID == "PGS1789").FirstOrDefault().TravelDate;
            Assert.AreEqual(true,(DateTime.UtcNow - travelHistoryForGivenBookingId).TotalMinutes < 10 , "Failed to update travel date");
        }
        

        public static void Verify_Passengers_NameStarting_with_S(string connectionString, List<AirTravel> TravelDocument)
        {

            var filter = Builders<AirTravel>.Filter.Regex(x => x.FirstName, BsonRegularExpression.Create(new Regex("S.*")));
            var sortDefinition = Builders<AirTravel>.Sort.Descending(x => x.FirstName);
            var result = GetCollection(connectionString).Find(filter).Sort(sortDefinition)
                .Skip(2)
                .Limit(2).ToList();
            Assert.AreEqual(2, TravelDocument.Count);
            Assert.AreEqual(TravelDocument.ElementAt(1).FirstName, result.ElementAt(1).FirstName, "sorting not proper");
        }

      
        private static IMongoCollection<AirTravel> GetCollection(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var coll = client.GetDatabase("testdb").GetCollection<AirTravel>("travel");
            return coll;
        }

        
    }
}