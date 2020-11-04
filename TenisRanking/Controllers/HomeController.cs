using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite.Internal.IISUrlRewrite;
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
                WaitingForConfirmationMatches = this.User.Identity.IsAuthenticated
                    ? this.context.GetAllWaitingForConfirmationMatchesForPlayer(userId)
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
                    Status = MatchStatus.Challanged
                };

                this.context.SaveMatch(match);

                var challenger = this.context.GetPlayer(match.Chellanger);
                var deffender = this.context.GetPlayer(match.Defender);
                this.emailController.SendChallangeEmail(deffender.UserName, deffender.PlayerName, challenger.PlayerName);

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

                var challenger = this.context.GetPlayer(match.Chellanger);
                var deffender = this.context.GetPlayer(match.Defender);
                this.emailController.SendChallangeAcceptedEmail(challenger.UserName, challenger.PlayerName, deffender.PlayerName);

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

                var challenger = this.context.GetPlayer(match.Chellanger);
                var deffender = this.context.GetPlayer(match.Defender);
                this.emailController.SendChallangeRefusedEmail(challenger.UserName, challenger.PlayerName, deffender.PlayerName);

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

            if (match.Status == MatchStatus.WaitingForConfirmation)
            {
                viewModel.FirstSetDefender = match.Result.Sets.First().Deffender;
                viewModel.FirstSetTieBreakDeffender = match.Result.Sets.First().DeffenderTieBreak;

                viewModel.FirstSetChellanger = match.Result.Sets.First().Challanger;
                viewModel.FirstSetTieBreakChallanger = match.Result.Sets.First().ChallengerTieBreak;

                if (match.Result.Type == MatchResultType.TwoSets)
                {
                    viewModel.FirstSetDefender = match.Result.Sets[1].Deffender;
                    viewModel.FirstSetTieBreakDeffender = match.Result.Sets[1].DeffenderTieBreak;

                    viewModel.FirstSetChellanger = match.Result.Sets[1].Challanger;
                    viewModel.FirstSetTieBreakChallanger = match.Result.Sets[1].ChallengerTieBreak;
                }
            }

            return PartialView("AddMatchResultPartial", viewModel);
        }

        [HttpPost]
        public IActionResult AddMatchResult(MatchViewModel matchViewModel)
        {
            try
            {
                this.matchProvider.SetFinalMatchResult(matchViewModel, this.User.FindFirstValue(ClaimTypes.NameIdentifier));

                return RedirectToAction("Index", new { status = MessageStatus.SUCCESS.ToString(), message = Messages.ResultAdded });
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", new { status = MessageStatus.ERROR.ToString(), message = e.ToString() });
            }
        }

        public IActionResult ConfirmResult(string matchId)
        {
            try
            {
                this.matchProvider.ConfirmResult(matchId);

                return RedirectToAction("Index", new { status = MessageStatus.SUCCESS.ToString(), message = Messages.ResultConfirmed });
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", new { status = MessageStatus.ERROR.ToString(), message = e.ToString() });
            }
        }
    }
}
