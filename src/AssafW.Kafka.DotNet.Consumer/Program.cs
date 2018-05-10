using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace AssafW.Kafka.DotNet.Consumer
{
  class Program
  {
    public static void Main(string[] args)
    {
      Console.WriteLine();
      Console.WriteLine("-----------");
      Console.WriteLine("----------- simple dotnet kafka consumer");
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
        { "enable.auto.commit", "true" },
        { "auto.commit.interval.ms", "1000"},
        { "auto.offset.reset", "earliest" }
      };

      if (args.Length > 1)
      {
        var group = args[1];
        System.Console.WriteLine($"will use consumer group.id: {group}");
        config.Add("group.id", group);
      }

      var topic = args[0];
      ReceiveMessages(config, topic);
    }

    private static void ReceiveMessages(Dictionary<string, object> config, string topic)
    {
      var con = new Consumer<string, string>(config, new StringDeserializer(Encoding.UTF8), new StringDeserializer(Encoding.UTF8));
      con.OnMessage += (s, e) => {
        System.Console.WriteLine($"{e.Partition} | {e.Key}: {e.Value}");
      };

      Console.CancelKeyPress += (s, e) => {
        System.Console.WriteLine("Ctrl+C pressed, quitting");
        Environment.Exit(0);
      };

      con.Subscribe(topic);
      while (true)
      {
        con.Poll(TimeSpan.FromMilliseconds(100));
      }
    }
  }
}