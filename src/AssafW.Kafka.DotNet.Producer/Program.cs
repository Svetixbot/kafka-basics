using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace AssafW.Kafka.DotNet.Producer
{
  class Program
  {
    public static void Main(string[] args)
    {
      Console.WriteLine();
      Console.WriteLine("-----------");
      Console.WriteLine("----------- simple dotnet kafka producer");
      Console.WriteLine("-----------");
      Console.WriteLine();

      if (args == null || !args.Any())
      {
        System.Console.WriteLine("ERROR: missing topic name argument");
        Environment.Exit(-1);
      }

      var config = new Dictionary<string, object>
      {
        { "bootstrap.servers", "localhost:9092" },
        { "acks", "1" },
        { "retries", "3" },
      };

      var topic = args[0];
      WarnIfTopicDoesnotExist(config, topic);
      PushMessages(config, topic);
    }

    private static void WarnIfTopicDoesnotExist(Dictionary<string, object> config, string topic)
    {
      var producer = new Confluent.Kafka.Producer(config);
      var meta = producer.GetMetadata();

      if (!meta.Topics.Any(t => t.Topic == topic))
      {
        System.Console.WriteLine($"topic '{topic}' was not found, will create it with default configuration");
      }
    }

    private static void PushMessages(Dictionary<string, object> config, string topic)
    {

      Console.WriteLine("type text and hit enter to produce a message, exit with Ctrl+C");

      using (var producer = new Producer<string, string>(config, new StringSerializer(Encoding.UTF8), new StringSerializer(Encoding.UTF8)))
      {
        Console.CancelKeyPress += (s, e) => {
          System.Console.WriteLine("Ctrl+C pressed, quitting");
          producer.Flush(TimeSpan.FromSeconds(10));

          Environment.Exit(0);
        };

        while (true)
        {
          producer.ProduceAsync(topic, Guid.NewGuid().ToString(), Console.ReadLine());
        }
      }
    }
  }
}