using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace MongoDbTutorials.MongoDbTutorials.MovieDetailsSchema.Model
{
    public class MovieDetail
    {
        [BsonRepresentation(BsonType.String)]
        public String Id { get; set; }
        [BsonRequired]
        public String Title { get; set; }
        public int Year { get; set; }
        public String ImdbId { get; set; }
        //possible values G, PG, PG_13, R, NC17, NOT_RATED
        [BsonRepresentation(BsonType.String)]
        [BsonDefaultValue(MpaaRatings.NOT_RATED)]
        [BsonElement("criticRating")]
        public String MpaaRating { get; set; }
        [BsonDefaultValue(0.0)]
        public Double ViewerRating { get; set; }
        public int ViewerVotes { get; set; }
        public int Runtime { get; set; }
        public List<Genre> Genres { get; set; }
        public String Director { get; set; }
        public List<String> Cast { get; set; }
        public String Plot { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
    public enum MpaaRatings
    {
        G,
        PG,
        PG_13,
        R,
        NC17,
        NOT_RATED
    }
    public class Genre
    {
        public int Id
        {
            get; set;
        }
        public String Type
        {
            get; set;
        }
    }
}
