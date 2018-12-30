using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StarWars.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace StarWars.Data
{
    class ApplicationDbContextFactory
    {
        public static IMongoDatabase Create()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("StarWars");
            return database;  
        }
    }
}
