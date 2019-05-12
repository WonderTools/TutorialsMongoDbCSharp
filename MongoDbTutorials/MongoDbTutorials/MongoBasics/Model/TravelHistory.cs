using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDbTutorials.MongoDbTutorials.MongoBasics.Model
{
    public class TravelHistory
    {
        public string BookingID { get; set; }

        //[BsonType(DateTimeKind.Local)]
        public DateTime BookingDate { get; set; }

        public string From { get; set; }
        public string Destination { get; set; }
        //[BsonRepresentation(BsonType.String)]
        public DateTime TravelDate { get; set; }

    }
}
