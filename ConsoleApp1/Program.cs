using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace Labb3
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("lab3");
            database.DropCollection("restaurants");
            var restaurantsCollection = database.GetCollection<BsonDocument>("restaurants");

            //Create bson array 
            BsonDocument[] restaurants = new BsonDocument[]
            {
                new BsonDocument
                {
                    { "_id",ObjectId.Parse("5c39f9b5df831369c19b6bca")},
                    { "name" , "Sun Bakery Trattoria" },
                    { "stars" , 4},
                    { "categories" , new BsonArray {"Pizza", "Pasta", "Italian", "Coffee", "Sandwiches" } }
                },
                new BsonDocument
                {
                    { "_id", ObjectId.Parse("5c39f9b5df831369c19b6bcb")},
                    { "name" , "Blue Bagels Grill" },
                    { "stars" , 3 },
                    { "categories" , new BsonArray {"Bagels", "Cookies", "Sandwiches" } }
                },
                new BsonDocument
                {
                    { "_id" , ObjectId.Parse("5c39f9b5df831369c19b6bcc") },
                    { "name","Hot Bakery Cafe" },
                    { "stars" , 4},
                    { "categories" , new BsonArray {"Bakery","Cafe","Coffee","Dessert" } }
                },
                new BsonDocument
                {
                    { "_id" , ObjectId.Parse("5c39f9b5df831369c19b6bcd") },
                    { "name" , "XYZ Coffee Bar"},
                    { "stars" , 5 },
                    { "categories" , new BsonArray {"Coffee", "Cafe", "Bakery", "Chocolates" } }
                },
                new BsonDocument
                {
                    { "_id" , ObjectId.Parse("5c39f9b5df831369c19b6bce") },
                    { "name" , "456 Cookies Shop" },
                    { "stars" , 4},
                    {"categories", new BsonArray {"Bakery", "Cookies", "Cake", "Coffee"} }
                }
            };
            restaurantsCollection.InsertMany(restaurants);

            //Print out the restaurants
            //var restaurantsFind = restaurantsCollection.Find(new BsonDocument());
            //foreach (var restaurant in restaurantsFind.ToEnumerable())
            //{
            //    Console.WriteLine(restaurant.ToString()); //or
            //    //Console.WriteLine(restaurant.ToJson());
            //}

            //Filter restaurants that hace "Cafe" among its categories
            var filterCafe = Builders<BsonDocument>.Filter.Eq("categories", "Cafe");
            var projectionCafe = Builders<BsonDocument>.Projection.Include("name").Exclude("_id");
            var resultCafe = restaurantsCollection.Find<BsonDocument>(filterCafe).Project(projectionCafe).ToList();
            foreach (var restaurant in resultCafe)
            {
                Console.WriteLine(restaurant.ToString());
            }









            Console.ReadKey(true);
        }
    }
}
