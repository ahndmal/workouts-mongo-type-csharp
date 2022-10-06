using System.Collections.Generic;
using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;

namespace HelloHttp
{
   public class Function : IHttpFunction
   {
       private readonly ILogger _logger;

       public Function(ILogger<Function> logger) =>
           _logger = logger;

       public async Task HandleAsync(HttpContext context)
       {
           var client = new MongoClient(System.Environment.GetEnvironmentVariable("DB_URL"));
           var db = client.GetDatabase("workouts");
           var work_coll = db.GetCollection<BsonDocument>("workouts");
           List<BsonDocument> workouts = work_coll.Find(new BsonDocument()).ToList();
           
           HttpRequest request = context.Request;
           // string name = ((string) request.Query["name"]) ?? "world";

           // using TextReader reader = new StreamReader(request.Body);
           // string text = await reader.ReadToEndAsync();
               try
               {
                   // JsonDocument document = new JsonReader()
                   JsonElement json = JsonSerializer.Deserialize<JsonElement>(workouts[0].ToString());
                   // if (json.TryGetProperty("name", out JsonElement nameElement) &&
                   //     nameElement.ValueKind == JsonValueKind.String)
                   // {
                   //     name = nameElement.GetString();
                   // }
                   await context.Response.WriteAsync(json.ToString());
               }
               catch (JsonException parseException)
               {
                   _logger.LogError(parseException, "Error parsing JSON request");
                   await context.Response.WriteAsync("ERROR");
               }

           
       }
   }
}
