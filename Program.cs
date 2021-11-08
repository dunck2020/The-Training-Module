using System;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization;

namespace MongoDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            const string DATABASE_NAME = "MyFirstMongoDatabase";
            const string CONNECTION_STRING = "mongodb+srv:// *** Your User Name*** : ***Your User Password*** @democluster.krulx.mongodb.net/ ***Your Cluster Name*** ?retryWrites=true&w=majority";
            const string COLLECTION_NAME = "People";

            MongoConnect demoMongoDb = new MongoConnect(DATABASE_NAME, CONNECTION_STRING);
            Person person1 = new Person() { PersonId=100, FirstName = "Jenny", LastName = "Banana" };
            Person person2 = new Person() { PersonId=200, FirstName = "Mike", LastName = "Smith" };

            //demoMongoDb.CreateRecord(COLLECTION_NAME, person1);
            //demoMongoDb.CreateRecord(COLLECTION_NAME, person2);
            //demoMongoDb.ReadFirstRecord(COLLECTION_NAME);
            //demoMongoDb.FindSpecificRecord(COLLECTION_NAME, 200);
            //demoMongoDb.UpdateRecordLastName(COLLECTION_NAME, 100, "Smith");
            demoMongoDb.DeleteRecord(COLLECTION_NAME, "Mike");
            Console.ReadKey();

        }
    }
    public class MongoConnect
    {
        private IMongoDatabase db;
        public MongoConnect(string databaseName, string connectionString)
        {
            MongoClient dbClient = new MongoClient(connectionString);
            db = dbClient.GetDatabase(databaseName);
        }
        public void CreateRecord(string collectionName, object record)
        {
            var collection = db.GetCollection<BsonDocument>(collectionName);
            var bsonDoc = record.ToBsonDocument();
            Console.WriteLine("Inserting Document");
            collection.InsertOne(bsonDoc);
            Console.WriteLine("Document Inserted");
        }
        public void ReadFirstRecord(string collectionName)
        {
            var collection = db.GetCollection<BsonDocument>(collectionName);
            var record = collection.Find(new BsonDocument()).FirstOrDefault();
            var person = BsonSerializer.Deserialize<Person>(record);
            person.Display();

        }
        public void FindSpecificRecord(string collectionName, int personIdToFind)
        {
            var collection = db.GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", personIdToFind);
            var record = collection.Find(filter).FirstOrDefault();
            var person = BsonSerializer.Deserialize<Person>(record);
            person.Display();

        }
        public void UpdateRecordLastName(string collectionName, int personToUpdate, string newLastName)
        {
            var collection = db.GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", personToUpdate);
            var update = Builders<BsonDocument>.Update.Set("LastName", newLastName);
            var result = collection.UpdateOne(filter, update);
            Console.WriteLine(result);
        }
        public void DeleteRecord(string collectionName, string personToDelete)
        {
            var collection = db.GetCollection<BsonDocument>(collectionName);
            Console.WriteLine("Deleting the person record");
            var deleteFilter = Builders<BsonDocument>.Filter.Eq("FirstName", personToDelete);
            var deleteResult = collection.DeleteOne(deleteFilter);
            Console.WriteLine(deleteResult);
        }
    }
    public class Person
    {  
        [BsonId]
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public void Display()
        {
            Console.WriteLine(PersonId);
            Console.WriteLine(FirstName);
            Console.WriteLine(LastName);
        }
    }
}
