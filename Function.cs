using System;
using System.Collections.Generic;
using System.IO;
using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;


namespace HelloHttp
{
    public class Function : IHttpFunction
    {
        private readonly ILogger _logger;

        public Function(ILogger<Function> logger) =>
            _logger = logger;

        class Workout
        {
            public string workout_type { get; set; }
            public int record  { get; set; }
            public int sets  { get; set; }
            public int user_id  { get; set; }
            public int week  { get; set; }
            public string comments { get; set; }
            public string workout_date { get; set; }
        }
        
        public async Task HandleAsync(HttpContext context)
        {
            var client = new MongoClient(System.Environment.GetEnvironmentVariable("DB_URL"));
            var db = client.GetDatabase("workouts");
            var work_coll = db.GetCollection<BsonDocument>("workouts");
            List<Workout> workouts = new List<Workout>();
            // List<BsonDocument> workouts = work_coll.Find(new BsonDocument()).ToList();
                var cursor = work_coll.Find(new BsonDocument()).ToCursor();
                foreach (var document in cursor.ToEnumerable())
                {
                    Workout workout = new Workout();
                    workout.workout_type = document.GetValue("workout_type").AsString;
                    workout.sets = document.GetValue("sets").AsInt32;
                    workout.week = document.GetValue("week").AsInt32;
                    workout.workout_date = document.GetValue("workout_date").AsString;
                    workouts.Add(workout);
                }

            HttpRequest request = context.Request;
            Console.WriteLine(">> req " + request);
            // string name = ((string) request.Query["name"]) ?? "world";

            // using TextReader reader = new StreamReader(request.Body);
            // string text = await reader.ReadToEndAsync();
            try
            {
                string json = JsonSerializer.Serialize(workouts);
                context.Response.Headers.Add("Content-Type", "application/json");
                await context.Response.WriteAsync(json);
            }
            catch (JsonException parseException)
            {
                _logger.LogError(parseException, "Error parsing JSON request");
                await context.Response.WriteAsync("ERROR");
            }
        }
    }
}