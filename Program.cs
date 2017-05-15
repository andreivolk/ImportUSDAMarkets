using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace ImportUSDAMarkets
{
    class Program
    {
        private static List<MarketDetail> marketList = new List<MarketDetail>();

        static void Main(string[] args)
        {
            readCSV();
            insertRecords();
            Console.ReadLine();
        }

        private static void readCSV()
        {
            Console.WriteLine("Starting read...");
            try
            {
                using (TextReader sr = new StreamReader(ConfigurationManager.AppSettings["importCSV"]))
                {
                    var csv = new CsvReader(sr);
                    marketList = csv.GetRecords<MarketDetail>().ToList();
                    Console.WriteLine("Finished reading records");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void insertRecords()
        {
            Console.WriteLine("Converting list to BSON for insertion");
            try
            {
                var insertList = new List<BsonDocument>();
                foreach (var market in marketList)
                {
                    string marketJSON = JsonConvert.SerializeObject(market);
                    BsonDocument marketInsert = BsonDocument.Parse(marketJSON);
                    insertList.Add(marketInsert);
                }
                Console.WriteLine("Inserting list");
                var client = new MongoClient(ConfigurationManager.AppSettings["dbUri"]);
                var db = client.GetDatabase(ConfigurationManager.AppSettings["database"]);
                var markets = db.GetCollection<BsonDocument>("Markets");
                markets.InsertMany(insertList);
                Console.WriteLine("Inserting complete, press any key to exit");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
