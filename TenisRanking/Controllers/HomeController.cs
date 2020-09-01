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
using TenisRanking.Email;
using TenisRanking.MatchProvider;
using TenisRanking.Models;

namespace TenisRanking.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDbContextWrapper context;
        private readonly IMatchProvider matchProvider;
        private readonly IEmailController emailController;

        public HomeController(IDbContextWrapper context, IMatchProvider matchProvider, IEmailController emailController)
        {
            this.context = context;
            this.matchProvider = matchProvider;
        }


        public IActionResult Index()
        {
            var models = new ViewModel()
            {
                Players = this.context.GetAllPlayers()
            };

            if (this.User.Identity.IsAuthenticated)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                models.PlayedMatches = this.context.GetAllPlayedMatchesForPlayer(userId);
                models.PlannedMatches = this.context.GetAllPlannedMatchesForPlayer(userId);
                models.ChallengedMatches = this.context.GetAllChellangedMatchesForPlayer(userId);
                models.AllMatches = this.context.GetAllMatches();
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

            this.emailController.SendChallangeEmail();

            return RedirectToAction("Index");
        } 

        public IActionResult AcceptChallange(string matchId)
        {
            var match = this.context.GetMatch(matchId);
            match.Status = MatchStatus.Accepted;

            this.context.SaveMatch(match);

            this.emailController.SendChallangeAcceptedEmail();

            return RedirectToAction("Index");
        }

        public IActionResult RefuseChallenge(string matchId)
        {
            var match = this.context.GetMatch(matchId);
            match.Status = MatchStatus.Refused;

            this.context.SaveMatch(match);

            this.emailController.SendChallangeRefusedEmail();

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
