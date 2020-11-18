using MasivianTechnicalTest.DataAccess.Contracts;
using MasivianTechnicalTest.Domain.Contracts;
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
    }
}
