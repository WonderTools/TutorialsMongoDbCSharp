using System;
using System.Collections.Generic;
using System.Text;
using Mongo2Go;
using MongoDbTutorials.MongoDbTutorials.MovieDetailsSchema.Model;
using MongoDB.Driver;
using NUnit.Framework;
using WonderTools.JsonSectionReader;

namespace MongoDbTutorials.MongoDbTutorials.MovieDetailsSchema
{
    class MovieDetailsOperations
    {
        private MongoDbRunner _runner;
        private JSection testData;

        [SetUp]
        public void Setup()
        {
            _runner = MongoDbRunner.Start();

            testData = JSectionReader.Section("movieDetailsTestData.json", Encoding.UTF8);
        }

        [Test]
        // Insert a document in database named "testdb", in a collection named "MovieDetail"
        //The document to be inserted should have a property similar to MovieDetail model
        public void Check_Model_Schema_Validity_001()
        {
            var collection = getCollection();
            var movieDetails = testData.GetSection("InsertOne").GetObject<MovieDetail>();
            collection.InsertOne(movieDetails);

            MovieDetailsVerifier.VerifyMovieDetaailInsertOne(_runner.ConnectionString, movieDetails);
        }

        

        [TearDown]
        public void CleanUp()
        {
            _runner.Dispose();
        }

        private IMongoCollection<MovieDetail> getCollection()
        {
            var client = new MongoClient(_runner.ConnectionString);
            return client.GetDatabase("testdb").GetCollection<MovieDetail>("MovieDetail");
        }
    }
}
