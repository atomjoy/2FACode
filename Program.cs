using Newtonsoft.Json;
using OtpNet;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class Item
    {        
        public string name = "";
        public string code = "";
        // public DateTime datetime;
    }

    class Program
    {
        static void Main(string[] args)
        {   
            // Random hash
            var key = KeyGeneration.GenerateRandomKey(20);
            var base32String = Base32Encoding.ToString(key);
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Random {0}", base32String);
            Console.WriteLine("-------------------------------------");

            // using StreamReader r = new(@"D:\Git\file.json");
            // From JSON
            using StreamReader r = new(@"secrets\secrets.json");
            string? json = r.ReadToEnd();
            if (json != null)
            {
                List<Item>? items = JsonConvert.DeserializeObject<List<Item>>(json);

                while (true)
                {
                    dynamic? array = JsonConvert.DeserializeObject(json) ?? Array.Empty<string>();

                    foreach (var item in array)
                    {
                        string itemName = item.name;
                        string itemCode = item.code;
                        var otpCode = new Totp(Base32Encoding.ToBytes(itemCode)).ComputeTotp(DateTime.UtcNow);
                        Console.WriteLine("{0}", DateTime.UtcNow.ToString());
                        Console.WriteLine("===> {0}", itemName);
                        Console.WriteLine("======> {0}", otpCode);
                        Console.WriteLine("----------------------------------------");
                    }                 

                    Thread.Sleep(15000); // 30 second delay
                    Console.Clear();
                }

            }
            
            Console.WriteLine("\nPress any key to exit.");            
            Console.ReadKey();
        }
    }
}
