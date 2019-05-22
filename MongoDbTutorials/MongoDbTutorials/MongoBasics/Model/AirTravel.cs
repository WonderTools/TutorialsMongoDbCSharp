using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDbTutorials.MongoDbTutorials.MongoBasics.Model
{
    public class AirTravel
    {
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        [BsonRepresentation(BsonType.String)]
        public List<FoodTypes> FoodPreferences{ get; set; }
        public List<TravelHistory> TravelHistory { get; set; }
        public List<TravelFrequency> TravelFrequency { get; set; }


    }

    public enum FoodTypes
    {
        Chinese,
        Indian_NonVeg,
        Indian_Veg

    }
}
