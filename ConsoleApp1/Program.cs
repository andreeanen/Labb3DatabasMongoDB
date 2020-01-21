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
            //var filterCafe = Builders<BsonDocument>.Filter.Eq("categories", "Cafe");
            //var projectionCafe = Builders<BsonDocument>.Projection.Include("name").Exclude("_id");
            //var resultCafe = restaurantsCollection.Find<BsonDocument>(filterCafe).Project(projectionCafe).ToList();
            //foreach (var restaurant in resultCafe)
            //{
            //    Console.WriteLine(restaurant.ToString());
            //}

            //Skriv en metod som uppdaterar genom increment “stars” för den restaurang 
            //som har “name” “XYZ Coffee Bar” så att nya värdet på stars blir 6.
            //OBS! Ni ska använda increment . 
            //OBS! Skriv ut alla restauranger igen, så att jag kan se att “stars” blivit 6, för denna restaurang när jag kör ert program. 

            var filterInc = Builders<BsonDocument>.Filter.Eq("name", "XYZ Coffee Bar");
            var update = Builders<BsonDocument>.Update.Inc("stars", 1);
            var result = restaurantsCollection.UpdateOne(filterInc, update);

            foreach (var restaurant in restaurantsCollection.Find(new BsonDocument()).ToEnumerable())
            {
                Console.WriteLine(restaurant.ToString());
            }







            Console.ReadKey(true);
        }
    }
}
