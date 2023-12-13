# How to Integrate Redis Cache in .NET Core 6

Caching is an essential technique in modern software development that helps improve the performance and responsiveness of applications. Redis, a popular in-memory data store, can be seamlessly integrated into .NET Core 6 applications to provide efficient caching capabilities. In this article, we’ll explore how to integrate Redis cache into a .NET Core 6 application, including code snippets and step-by-step instructions. We’ll also discuss how to verify if we are indeed retrieving results from the cache and not the database.

# Prerequisites

Before we begin, ensure you have the following prerequisites:

1. Visual Studio 2022 or later, or Visual Studio Code.
2. .NET 6 SDK installed.
3. Redis server up and running. You can install Redis using Docker or download it from the official website.
4. Redis server up and running. You can install Redis using Docker or download it from the official website.

## Download

### You can download the last Redis source files here -> [Download | Redis](https://redis.io/download/?source=post_page-----4c1714d825df--------------------------------).

# Step 1: Create a .NET Core 6 Application

Start by creating a new .NET Core 6 application. You can do this using the following command:

```
dotnet new console -n RedisCacheDemo
```

Navigate to the project folder:

```
cd RedisCacheDemo
```

# Step 2: Install Required Packages

To work with Redis cache, we need to install the `StackExchange.Redis` package. Run the following command:

```
dotnet add package StackExchange.Redis
```

# Step 3: Configure Redis Connection

In the `Program.cs` file, add the necessary using statement and configure the Redis connection in the `Main` method:

```
using StackExchange.Redis;

class Program
{
    static void Main(string[] args)
    {
        var configuration = ConfigurationOptions.Parse("localhost:6379");
        var redisConnection = ConnectionMultiplexer.Connect(configuration);
        
        // Your caching logic here
        
        redisConnection.Close();
    }
}
```

Replace `"localhost:6379"` with your Redis server connection information if it's different.

# Step 4: Implement Redis Caching

Let’s create a simple example to demonstrate caching. Suppose we have a method that fetches data from a database, and we want to cache the results.

```
static string GetDataFromDatabase()
{
    // Simulate fetching data from the database
    // Replace this with your actual database fetching logic
    Thread.Sleep(2000); // Simulating latency
    
    return "Data from database";
}
```

Now, let’s integrate Redis caching into this method:

```
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
```

In the above code, we first check if the data is present in the cache. If not, we fetch the data from the database, store it in the cache with a 10-minute expiration time, and return the data.

# Step 5: Verify Cache Usage

To verify if the cache is indeed being used, you can add some console output messages. Modify the `Main` method to include the following:

```
static void Main(string[] args)
{
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
}
```

When you run the application, you should see that the first fetch (with caching) takes some time due to the simulated delay, but the second fetch (without caching) is consistently slower.

# Conclusion

In this article, we’ve learned how to integrate Redis cache into a .NET Core 6 application. We covered the installation of required packages, configuration of the Redis connection, implementation of caching logic, and verification of cache usage. By leveraging Redis caching, you can significantly improve the performance and responsiveness of your applications, especially when dealing with frequently accessed data.

Remember that this example provides a basic understanding of how to integrate Redis cache into a .NET Core 6 application. In real-world scenarios, you would need to adapt and extend this approach to fit your specific application requirements and architecture.



**FUENTE**: https://medium.com/@vndpal/how-to-integrate-redis-cache-in-net-core-6-4c1714d825df

