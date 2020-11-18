using MasivianTechnicalTest.DataAccess.Contracts;
using MasivianTechnicalTest.Domain.Contracts;
using MasivianTechnicalTest.Domain.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MasivianTechnicalTest.WebService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private readonly IRoulette _domain;
        public RouletteController(IDataAccess client, IRoulette domain)
        {
            domain.GetDataAccess(client);
            _domain = domain;
        }

        [HttpPost]
        public object Open(Guid id)
        {
            return _domain.Open(id);
        }

        [HttpGet]
        public object GetAll()
        {
            return _domain.GetAll();
        }

        [HttpPost]
        public IResponse Create()
        {
            return _domain.Create();
        }

        [HttpPost]
        public IResponse AddBet(Guid rouletteId, Bet bet)
        {
            Request.Headers.TryGetValue("clientId", out var clientIdValue);
            bet.ClientId = Guid.Parse(clientIdValue);

            return _domain.AddBet(rouletteId, bet);
        }
    
        [HttpPost]
        public IResponse Close(Guid id)
        {
            return _domain.Close(id);
        }
    }
}
