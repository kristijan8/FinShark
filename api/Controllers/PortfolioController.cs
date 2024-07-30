using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{    
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;

        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo)
        {
            _userManager=userManager;
            _stockRepo=stockRepo;
            _portfolioRepo=portfolioRepo;
        }

        [HttpGet("Get Portfolio")]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username= User.GetUsername();
            var appUser=await _userManager.FindByNameAsync(username);
            if(appUser==null)
            {
                return BadRequest("User null");
            }
            var userPortfolio = await _portfolioRepo.GetUserProtfolio(appUser);
            if(userPortfolio==null)
            {
                return BadRequest("No portfolio");
            }
            return Ok(userPortfolio);
        }

        [HttpPost("Add to portfolio")]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username=User.GetUsername();
            var appUser=await _userManager.FindByNameAsync(username);
            var stock=await _stockRepo.GetBySymbolAsync(symbol);
            if(stock==null) return BadRequest("Stock not found");
            var userPortfolio=await _portfolioRepo.GetUserProtfolio(appUser);
            if(userPortfolio.Any(p => p.Symbol.ToLower().Equals(symbol.ToLower())))
                return BadRequest("Alredt exists");

            var portfolioModel=new Portfolio{
                AppUserID=appUser.Id,
                StockId=stock.Id
            };
            await _portfolioRepo.CreateAsync(portfolioModel);
            if (portfolioModel==null)
            {
                return StatusCode(500, "Could not create");
            }
            return Ok(portfolioModel);


        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username=User.GetUsername();
            var appUser=await _userManager.FindByNameAsync(username);
            var userPortfolio=await _portfolioRepo.GetUserProtfolio(appUser);
            var filterredStock= userPortfolio.Where(s => s.Symbol.ToLower().Equals(symbol.ToLower()));
            if(filterredStock.Count()==1)
            {
                await _portfolioRepo.DeletePortfolio(appUser, symbol);
            }
            else
            {
                return BadRequest("Stock not in portfolio");
            }
            return Ok();

        }

    }
}