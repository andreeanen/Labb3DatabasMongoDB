using MongoDB.Bson;
using MongoDB.Bson.IO;
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

            PopulateRestaurantsCollection(restaurantsCollection);            

            PrintRestaurants(restaurantsCollection);

            PrintCafes(restaurantsCollection);

            UpdateStarsForXYZCoffeeBar(restaurantsCollection);

            UpdateNameForCookiesShop(restaurantsCollection);

            AggregateFourPlusStarsRestaurants(restaurantsCollection);

            Console.ReadKey(true);
        }

        private static void PopulateRestaurantsCollection(IMongoCollection<BsonDocument> restaurantsCollection)
        {
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
        }

        private static void AggregateFourPlusStarsRestaurants(IMongoCollection<BsonDocument> restaurantsCollection)
        {
            var Matching = new BsonDocument {
                {
                    "$match", new BsonDocument
                    {
                         { "stars", new BsonDocument
                            {
                                {"$gte", 4 }
                            }
                         }
                    }
                }
            };

            var Projection = new BsonDocument(
                "$project", new BsonDocument 
                {
                    {"_id", 0},
                    {"name", "$name"},
                    {"stars","$stars"}
                }
            );

            var pipeline = new[] { Matching, Projection };
            var fourPlusStarsRestaurants = restaurantsCollection.Aggregate<BsonDocument>(pipeline).ToList();

            Console.WriteLine("\n\nRestaurants with four or more stars:");
            foreach (var restaurant in fourPlusStarsRestaurants)
            {
                Console.WriteLine(restaurant.ToString());
            }
        }

        private static void UpdateNameForCookiesShop(IMongoCollection<BsonDocument> restaurantsCollection)
        {
            var filterName = Builders<BsonDocument>.Filter.Eq("name", "456 Cookies Shop");
            var updateName = Builders<BsonDocument>.Update.Set("name", "123 Cookies Heaven");
            var resultUpdateName = restaurantsCollection.UpdateOne(filterName, updateName);
            Console.WriteLine("\n\nList with restaurants after changing name for 456 Cookies Shop:");
            PrintRestaurants(restaurantsCollection);
        }

        private static void UpdateStarsForXYZCoffeeBar(IMongoCollection<BsonDocument> restaurantsCollection)
        {
            var filterInc = Builders<BsonDocument>.Filter.Eq("name", "XYZ Coffee Bar");
            var updateInc = Builders<BsonDocument>.Update.Inc("stars", 1);
            var result = restaurantsCollection.UpdateOne(filterInc, updateInc);
            Console.WriteLine("\n\nList with restaurants after increasing number of stars for XYZ Coffee Bar:");
            PrintRestaurants(restaurantsCollection);
        }

        private static void PrintCafes(IMongoCollection<BsonDocument> restaurantsCollection)
        {
            var filterCafe = Builders<BsonDocument>.Filter.Eq("categories", "Cafe");
            var projectionCafe = Builders<BsonDocument>.Projection.Include("name").Exclude("_id");
            var resultCafe = restaurantsCollection.Find<BsonDocument>(filterCafe).Project(projectionCafe).ToList();
            Console.WriteLine("\n\nCafes: ");
            foreach (var restaurant in resultCafe)
            {
                Console.WriteLine(restaurant.ToString());
            }
        }

        private static void PrintRestaurants(IMongoCollection<BsonDocument> restaurantsCollection)
        {
            var restaurantsFind = restaurantsCollection.Find(new BsonDocument());
            foreach (var restaurant in restaurantsFind.ToEnumerable())
            {
                Console.WriteLine(restaurant.ToJson(new JsonWriterSettings { Indent = true }));
            }
        }
    }
}
