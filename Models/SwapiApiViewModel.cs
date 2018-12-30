using System.Collections.Generic;

namespace StarWars.Models {
    public class SwapiApiViewModel {
        public int count { get; set; }
        public string next { get; set; }
        public string previous { get; set; }
        public List<PlanetaViewModel> results { get; set; }
    }
}