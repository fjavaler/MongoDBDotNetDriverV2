using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace MongoDBDotNetDriverPractice
{
  class Program
  {
    static void Main(string[] args)
    {
      /*
      // To directly connect to a single MongoDB server
      // (this will not auto-discover the primary even if it's a member of a replica set)
      var client = new MongoClient();

      // or use a connection string
      var client = new MongoClient("mongodb://localhost:27017");

      // or, to connect to a replica set, with auto-discovery of the primary, supply a seed list of members
      var client = new MongoClient("mongodb://localhost:27017,localhost:27018,localhost:27019");
      */

      // Declares and initializes the client (i.e. the local MongoDB instance running).
      var client = new MongoClient("mongodb://localhost:27017");

      // Declares and initializes the DB. Creates if doesn't currently exist.
      var database = client.GetDatabase("MarvelHeroDB");

      // Declares and initializes the collection. Creates if doesn't currently exist.
      var collection = database.GetCollection<BsonDocument>("HeroCollection");

      // Document to insert into the collection.
      // TODO: retrieve data from marvel API instead of hard-coded example below.
      var heroDocument = new BsonDocument
      {
        { "marvelId", 1009351 },
        { "name", "Hulk" },
        { "description", "Caught in a gamma bomb explosion while trying to save the life of a teenager, Dr. Bruce Banner was transformed into the incredibly powerful creature called the Hulk. An all too often misunderstood hero, the angrier the Hulk gets, the stronger the Hulk gets." },
        { "thumbnail", new BsonDocument
          {
            { "path", "http://i.annihil.us/u/prod/marvel/i/mg/5/a0/538615ca33ab0" },
            { "extension", "jpg" }
          }
        },
        { "resourceURI", "http://gateway.marvel.com/v1/public/characters/1009351" },
      };

      // Filter document by field and value.
      var field = "marvelId";
      var value = heroDocument.GetValue(field).ToInt32();
      var filter = Builders<BsonDocument>.Filter.Eq(field, value);
      // Find first document matching filter constraints.
      var document = collection.Find(filter).First();
      // If nothing is returned (i.e. document doesn't currently exist in DB)
      if (document == null)
      {
        // Insert the document and print message.
        collection.InsertOne(heroDocument);
        Console.WriteLine("Success!");
      }
      else
      {
        // Notify document already exists.
        Console.WriteLine("Already exists.");
      }
    }
  }
}