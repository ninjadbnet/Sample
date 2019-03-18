using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp
{
    using Newtonsoft.Json;
    using NinjaDBLib;
    using NinjaDBLib.Models;

    class Program
    {
        static void Main(string[] args)
        {
            var tableName = "Orders";
            var db = new NinjaDb(new NinjaDbOptions() { StorageType = NinjaStorageType.InMemory });

            Console.WriteLine($"Existing Data ...");
            db.QueryAll(tableName).Results.ForEach(x => Console.WriteLine(((dynamic)x).OrderId));

            var id = db.Save(tableName, new { OrderDate = DateTime.UtcNow, Sku = "ABC889", SerialNumber = 100, OrderId=Guid.NewGuid().ToString() });
            object order=db.Load(tableName, id);
            Console.WriteLine($"Before...");
            Console.WriteLine(order.Serialize());
            var order1 = db.Load(tableName, id);
            order1.SerialNumber++;
            db.Update(tableName, order1);
            object order2 = db.Load(tableName, id);
            Console.WriteLine($"After ...");
            Console.WriteLine(order2.Serialize());

            Console.WriteLine($"All Data ...");
            db.QueryAll(tableName).Results.ForEach(x => Console.WriteLine(((dynamic)x).OrderId));

            Console.ReadLine();
        }
    }

    public static class SerializationExtensions
    {
        public static string Serialize(this object obj)
        {
           return  JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}
