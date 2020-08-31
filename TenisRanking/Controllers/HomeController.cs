using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TenisRanking.Data;
using TenisRanking.MatchProvider;
using TenisRanking.Models;

namespace TenisRanking.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDbContextWrapper context;
        private readonly IMatchProvider matchProvider;

        public HomeController(IDbContextWrapper context, IMatchProvider matchProvider)
        {
            this.context = context;
            this.matchProvider = matchProvider;
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
                    PlayedMatches = this.context.GetAllMatchesForPlayer(player)
                };

                models.Add(model);
            }

            return View(models);
        }

        public IActionResult Challenge(string deffenderId)
        {
            var match = new Match()
            {
                Chellanger = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                Defender = deffenderId,
                Status = MatchStatus.Chellanged
            };

            this.context.SaveMatch(match);

            //this.emailController.SendChallangeEmail();

            return RedirectToAction("Index");
        } 

        public IActionResult AcceptChallange(string matchId)
        {
            var match = this.context.GetMatch(matchId);
            match.Status = MatchStatus.Accepted;

            this.context.SaveMatch(match);

            //this.emailController.SendChallangeAcceptedEmail();

            return RedirectToAction("Index");
        }

        public IActionResult RefuseChallenge(string matchId)
        {
            var match = this.context.GetMatch(matchId);
            match.Status = MatchStatus.Refused;

            this.context.SaveMatch(match);

            //this.emailController.SendChallangeAcceptedEmail();

            return RedirectToAction("Index");
        }

        public IActionResult DeleteMatch(string matchId)
        {
            this.context.DeleteMatch(matchId);

            return RedirectToAction("Index");
        }

        public IActionResult AddMatchResult(string deffenderId, string challengerId, string matchId, string firstSet, string secondSet, string thirdSet)
        {
            this.matchProvider.SetFinalMatchResult(deffenderId, challengerId, matchId, firstSet, secondSet, thirdSet);

            return RedirectToAction("Index");
        }

        
    }
}
