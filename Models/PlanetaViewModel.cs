using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StarWars.Models {
    public class PlanetaViewModel {
        public ObjectId Id { get; set; }
        public string name { get; set; }
        public string climate { get; set; }
        public string terrain { get; set; }
        public List<string> films { get; set; }
        public int QtdFilmes { get; set; }
    }
}