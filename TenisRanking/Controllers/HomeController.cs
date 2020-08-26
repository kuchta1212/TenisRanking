using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TenisRanking.Data;
using TenisRanking.Models;

namespace TenisRanking.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDbContextWrapper context;

        public HomeController(IDbContextWrapper context)
        {
            this.context = context;
        }


        public IActionResult Index()
        {
            var models = new List<PlayerViewModel>();
            var players = this.context.GetAllPlayers();

            foreach (var player in players)
            {
                var model = new PlayerViewModel()
                {
                    Player = player,
                    Rank = JsonConvert.DeserializeObject<Rank>(player.Rank),
                    PlayedMatches = this.context.GetAllMatchesForPlayer(player)
                };

                models.Add(model);
            }

            return View(models);
        }


        public IActionResult AddMatch()
        {
            return View("Match");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
