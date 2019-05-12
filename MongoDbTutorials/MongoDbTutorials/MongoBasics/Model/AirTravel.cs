using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDbTutorials.MongoDbTutorials.MongoBasics.Model
{
    public class AirTravel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public List<string> FoodPrefrence { get; set; }
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
