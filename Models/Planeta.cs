using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StarWars.Models {
    public class Planeta {
        [BsonId]
        [BsonElement("Id")]
        public ObjectId Id { get; set; }
        [BsonRequired]
        [BsonElement("Nome")]
        public string Nome { get; set; }
        [BsonRequired]
        [BsonElement("Clima")]
        public string Clima { get; set; }
        [BsonRequired]
        [BsonElement("Terreno")]
        public string Terreno { get; set; }
    }
}