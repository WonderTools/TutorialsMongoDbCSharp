using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace MongoDbTutorials.MongoDbTutorials.MovieDetailsSchema.Model
{
    public class MovieDetail
    {
        [BsonId]
        public String Id { get; set; }
        public String Title { get; set; }
        public int Year { get; set; }
        public String ImdbId { get; set; }
        //possible values G, PG, PG_13, R, NC17, NOT_RATED
        public String MpaaRating { get; set; }
        public Double ViewerRating { get; set; }
        public int ViewerVotes { get; set; }
        public int Runtime { get; set; }
        public String Genres { get; set; }
        public String Director { get; set; }
        public List<String> Cast { get; set; }
        public String Plot { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
