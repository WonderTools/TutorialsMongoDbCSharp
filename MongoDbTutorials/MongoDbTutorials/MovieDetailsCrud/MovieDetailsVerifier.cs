using System;
using System.Collections.Generic;
using System.Linq;
using MongoDbTutorials.MongoDbTutorials.MovieDetailsCrud.Model;
using MongoDB.Driver;
using NUnit.Framework;

namespace MongoDbTutorials.MongoDbTutorials.MovieDetailsCrud
{
    class MovieDetailsVerifier
    {
        public static IMongoCollection<MovieDetail> getCollection(string connectionString)
        {
            var client = new MongoClient(connectionString);
            return client.GetDatabase("testdb").GetCollection<MovieDetail>("MovieDetail");
        }

        public static void VerifyMovieDetaailInsertOne(string connectionString, MovieDetail movieDetailTestData)
        {
            var coll = getCollection(connectionString);
            var result = coll.FindAsync(FilterDefinition<MovieDetail>.Empty);
            var resultData = result.Result.ToList();
            Assert.AreEqual(1, resultData.Count, "No document found in the collection testdb.testcollection");
            var data = resultData.ElementAt(0);
            //Assert.AreEqual("Superman", data.Title, "The inserted data is not same");
            Assert.AreEqual(movieDetailTestData.Title, data.Title, "The inserted data is not same");
        }
    }
}
