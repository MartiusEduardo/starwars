using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using StarWars.Data;
using StarWars.Models;

namespace StarWars.Controllers
{
    public class PlanetasController: Controller {
        private IMongoDatabase dbContext;
        public PlanetasController() {
            dbContext = ApplicationDbContextFactory.Create();
        }
        private List<PlanetaViewModel> listarPlanetas(List<Planeta> planetas) {
            List<PlanetaViewModel> lpvm = new List<PlanetaViewModel>();
            //Adiciono os planetas ao PlanetaViewModel aqui para que todos apareçam na lista e não somente
            //os que estão no site da swapi.co
            foreach (var planeta in planetas) {
                PlanetaViewModel pvm = new PlanetaViewModel {
                    Id = planeta.Id, name = planeta.Nome, climate = planeta.Clima, terrain = planeta.Terreno
                };
                lpvm.Add(pvm);
            }
            lpvm = contarFilmes("https://swapi.co/api/planets/?page=1", lpvm);
            int i = 2;
            while (lpvm.Count() != planetas.Count() && i <= 7) {
                lpvm = contarFilmes("https://swapi.co/api/planets/?page=" + i, lpvm);
                i++;
            }
            return lpvm;
        }
        private List<PlanetaViewModel> contarFilmes(string URL, List<PlanetaViewModel> lpvm) {
            using (var client = new WebClient()) {
                var conteudo = client.DownloadString(URL);
                SwapiApiViewModel savm = JsonConvert.DeserializeObject<SwapiApiViewModel>(conteudo);
                foreach (var planetaSwapi in savm.results) {
                    foreach (var planeta in lpvm) {
                        if(planetaSwapi.name == planeta.name) {
                            planeta.QtdFilmes = planetaSwapi.films.Count();
                        }
                    }
                }
            }
            return lpvm;
        }
        public ActionResult Index() {
            return View(listarPlanetas(dbContext.GetCollection<Planeta>("Planeta").Find(p => true).ToList()));
        }
        public ActionResult PagInserir() {
            return View("Inserir");
        }
        public ActionResult Inserir(Planeta planeta) {
            dbContext.GetCollection<Planeta>("Planeta").InsertOne(planeta);
            return View("Index", listarPlanetas(dbContext.GetCollection<Planeta>("Planeta").Find(p => true).ToList()));
        }
        public ActionResult PagModificar(string Id) {
            ObjectId oId = ObjectId.Parse(Id);
            return View("Modificar", dbContext.GetCollection<Planeta>("Planeta").Find(p => p.Id == oId).SingleOrDefault());
        }
        public ActionResult Modificar(string IdAlvo, Planeta planeta) {
            ObjectId oId = ObjectId.Parse(IdAlvo);
            planeta.Id = oId;
            dbContext.GetCollection<Planeta>("Planeta").ReplaceOne(p => p.Id == oId, planeta);
            return View("Index", listarPlanetas(dbContext.GetCollection<Planeta>("Planeta").Find(p => true).ToList()));
        }
        public ActionResult Excluir(string Id) {
            ObjectId oId = ObjectId.Parse(Id);
            dbContext.GetCollection<Planeta>("Planeta").DeleteOne(p => p.Id == oId);
            return View("Index", listarPlanetas(dbContext.GetCollection<Planeta>("Planeta").Find(p => true).ToList()));
        }
        public ActionResult Buscar(string palavra) {
            List<PlanetaViewModel> alvos = new List<PlanetaViewModel>();
            if (palavra != null) {
                alvos = listarPlanetas(dbContext.GetCollection<Planeta>("Planeta").Find(p => p.Nome.Contains(palavra)).ToList());
                if (alvos.Count == 0) {
                    if(palavra.Length == 24) {
                        ObjectId oId = ObjectId.Parse(palavra);
                        alvos.AddRange(listarPlanetas(dbContext.GetCollection<Planeta>("Planeta").Find(p => p.Id == oId).ToList()));
                    }
                }
            } else {
                alvos = listarPlanetas(dbContext.GetCollection<Planeta>("Planeta").Find(p => true).ToList());
            }
            return View("Index", alvos);
        }
    }
}