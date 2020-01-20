using MongoDB.Bson;
using MongoDB.Driver;

namespace Labb3
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("lab3");
            database.DropCollection("restaurants");
            var collection = database.GetCollection<BsonDocument>("restaurants");

            BsonDocument[] documents = new BsonDocument[]
            {
                new BsonDocument
                {
                    { "_id",1},
                    { "name" , "Sun Bakery Trattoria" },
                    { "stars" , 4},
                    { "categories" , new BsonArray {"Pizza", "Pasta", "Italian", "Coffee", "Sandwiches" } }
                },
                new BsonDocument
                {
                    { "_id", 2},
                    { "name" , "Blue Bagels Grill" },
                    { "stars" , 3 },
                    { "categories" , new BsonArray {"Bagels", "Cookies", "Sandwiches" } }
                },
                new BsonDocument
                {
                    { "_id" , 3 },
                    { "name","Hot Bakery Cafe" },
                    { "stars" , 4},
                    { "categories" , new BsonArray {"Bakery","Cafe","Coffee","Dessert" } }
                },
                new BsonDocument
                {
                    { "_id" , 4 },
                    { "name" , "XYZ Coffee Bar"},
                    { "stars" , 5 },
                    { "categories" , new BsonArray {"Coffee", "Cafe", "Bakery", "Chocolates" } }
                },
                new BsonDocument
                {
                    { "_id" , 5 },
                    { "name" , "456 Cookies Shop" },
                    { "stars" , 4},
                    {"categories", new BsonArray {"Bakery", "Cookies", "Cake", "Coffee"} }
                }
            };
            collection.InsertMany(documents);
        }
    }
}
