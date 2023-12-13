// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using StackExchange.Redis;

var configuration = ConfigurationOptions.Parse("localhost:6379");
var redisConnection = ConnectionMultiplexer.Connect(configuration);
var redisCache = redisConnection.GetDatabase();

Console.WriteLine("Fetching data with caching:");

var cachedData = GetDataWithCaching(redisCache);
Console.WriteLine($"Result: {cachedData}");

Console.WriteLine("Fetching data without caching:");
var uncachedData = GetDataFromDatabase();

Console.WriteLine($"Result: {uncachedData}");
redisConnection.Close(); //It is important to close the connection


static string GetDataFromDatabase()
{
    // Simulate fetching data from the database
    // Replace this with your actual database fetching logic
    Thread.Sleep(10000); // Simulating latency

    return "Data from database";
}

static string GetDataWithCaching(IDatabase redisCache)
{
    string cachedData = redisCache.StringGet("cachedData");
    if (string.IsNullOrEmpty(cachedData))
    {
        cachedData = GetDataFromDatabase();
        redisCache.StringSet("cachedData", cachedData, TimeSpan.FromMinutes(10));
    }
    return cachedData;
}