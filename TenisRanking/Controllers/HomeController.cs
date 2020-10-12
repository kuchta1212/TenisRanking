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
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using TenisRanking.Data;
using TenisRanking.Email;
using TenisRanking.MatchProvider;
using TenisRanking.Models;
using TenisRanking.Utils;

namespace TenisRanking.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDbContextWrapper context;
        private readonly IMatchProvider matchProvider;
        private readonly IEmailController emailController;
        private readonly IViewMessageFactory viewMessageFactory;

        public HomeController(IDbContextWrapper context, IMatchProvider matchProvider, IEmailController emailController, IViewMessageFactory viewMessageFactory)
        {
            this.context = context;
            this.matchProvider = matchProvider;
            this.emailController = emailController;
            this.viewMessageFactory = viewMessageFactory;

        }


        public IActionResult Index(MessageStatus status, string message)
        {
            var userId = this.User.Identity.IsAuthenticated ? this.User.FindFirstValue(ClaimTypes.NameIdentifier) : string.Empty;

            var models = new ViewModel
            {
                Player = this.User.Identity.IsAuthenticated 
                    ? this.context.GetPlayer(this.User.FindFirstValue(ClaimTypes.NameIdentifier))
                    : new Player(),
                Players = this.context.GetAllPlayers(),
                PlayedMatches = this.User.Identity.IsAuthenticated
                    ? this.context.GetAllPlayedMatchesForPlayer(userId)
                    : new List<Match>(),
                PlannedMatches = this.User.Identity.IsAuthenticated
                    ? this.context.GetAllPlannedMatchesForPlayer(userId)
                    : new List<Match>(),
                ChallengedMatches = this.User.Identity.IsAuthenticated
                    ? this.context.GetAllChellangedMatchesForPlayer(userId)
                    : new List<Match>(),
                RefusedMatches = this.User.Identity.IsAuthenticated
                    ? this.context.GetAllRefusedMatches(userId)
                    : new List<Match>(),
                AllMatches = this.context.GetAllMatches(),
                ViewMessage = this.viewMessageFactory.Create(status, message)
            };

            return View(models);
        }

        public IActionResult Challenge(string deffenderId)
        {
            try
            {
                var match = new Match()
                {
                    Chellanger = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Defender = deffenderId,
                    Status = MatchStatus.Chellanged
                };

                this.context.SaveMatch(match);

                //this.emailController.SendChallangeEmail();

                return RedirectToAction("Index", new { status = MessageStatus.SUCCESS.ToString(), message = Messages.ChallengeSended });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction("Index", MessageStatus.ERROR.ToString(), e.ToString());
            }
            
        } 

        public IActionResult AcceptChallange(string matchId)
        {
            try
            {
                var match = this.context.GetMatch(matchId);
                match.Status = MatchStatus.Accepted;

                this.context.UpdateMatch(match);

                //this.emailController.SendChallangeAcceptedEmail();

                return RedirectToAction("Index", new { status = MessageStatus.SUCCESS.ToString(), message = Messages.ChallangeAccpeted});
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", new { status = MessageStatus.ERROR.ToString(), message = e.ToString()});
            }
            
        }

        public IActionResult RefuseChallenge(string matchId)
        {
            try
            {
                var match = this.context.GetMatch(matchId);
                match.Status = MatchStatus.Refused;

                this.context.UpdateMatch(match);

                //this.emailController.SendChallangeRefusedEmail();

                return RedirectToAction("Index", new { status = MessageStatus.SUCCESS.ToString(), message = Messages.ChallengeRefused });
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", new { status = MessageStatus.ERROR.ToString(), message = e.ToString() });
            }
           
        }

        public IActionResult DeleteMatch(string matchId)
        {
            try
            {
                this.context.DeleteMatch(matchId);

                return RedirectToAction("Index", new { status = MessageStatus.SUCCESS.ToString(), message = Messages.MatchRemoved });
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", new { status = MessageStatus.ERROR.ToString(), message = e.ToString() });
            }
            
        }

        [HttpGet]
        public PartialViewResult InitMatchResult(string matchId)
        {
            var match = this.context.GetMatch(matchId);
            var challanger = this.context.GetPlayer(match.Chellanger);
            var deffender = this.context.GetPlayer(match.Defender);

            var viewModel = new MatchViewModel()
            {
                MatchId = matchId,
                Challanger = challanger.PlayerName,
                Deffender = deffender.PlayerName
            };

            return PartialView("AddMatchResultPartial", viewModel);
        }

        [HttpPost]
        public IActionResult AddMatchResult(string MatchId, string FirstSetChellanger, string SecondSetChellanger, string ThirdSetChellanger, string FirstSetDefender, string SecondSetDefender, string ThirdSetDefender)
        {
            try
            {
                this.matchProvider.SetFinalMatchResult(MatchId, FirstSetChellanger, SecondSetChellanger, ThirdSetChellanger, FirstSetDefender, SecondSetDefender, ThirdSetDefender);

                return RedirectToAction("Index", new { status = MessageStatus.SUCCESS.ToString(), message = Messages.ResultAdded });
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", new { status = MessageStatus.ERROR.ToString(), message = e.ToString() });
            }
        }
    }
}
