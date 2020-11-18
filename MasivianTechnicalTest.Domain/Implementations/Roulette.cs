using System.Linq;
using MasivianTechnicalTest.Domain.Contracts;
using ExportModels = MasivianTechnicalTest.Domain.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using MasivianTechnicalTest.DataAccess.Contracts;
using MasivianTechnicalTest.Domain.Models.ViewModels;

namespace MasivianTechnicalTest.Domain.Implementations
{
    public class Roulette : IRoulette
    {
        private IDataAccess _client;

        public void GetDataAccess(IDataAccess client)
        {
            _client = client;
        }

        public IResponse Create()
        {
            IResponse response;
            try
            {
                var roulette = new ExportModels.Roulette
                {
                    id = Guid.NewGuid(),
                    status = ExportModels.Roulette.Status.create
                };
                _client.Set(roulette.id.ToString(), roulette.ToJson());
                response = new ExportModels.Response
                {
                    Status = ExportModels.Response.ResponseStatus.ok,
                    Content = new ResumeResponseContent
                    {
                        Content = roulette.id.ToString()
                    }
                };
            }
            catch (Exception e)
            {
                response = new ExportModels.Response
                {
                    Status = ExportModels.Response.ResponseStatus.fail,
                    Content = new ResumeResponseContent
                    {
                        Content = e.ToJson()
                    }
                };
            }
            return response;
        }

        public IResponse GetAll()
        {
            var response = new List<ExportModels.Roulette>();
            var dataResponse = _client.GetAll();
            foreach (var hash in dataResponse)
            {
                var value = Newtonsoft.Json.JsonConvert.DeserializeObject<ExportModels.Roulette>(hash.Value);
                response.Add(value);
            }

            return new Response
            {
                Status= Response.ResponseStatus.ok,
                Content= response
            };
        }

        public IResponse Open(Guid id)
        {            
            var rouletteJson = _client.Get(id.ToString());
            if (rouletteJson == null)
            {
                return new Response
                {
                    Status = Response.ResponseStatus.fail,
                    Content = new ResumeResponseContent { Content = string.Format("Ruleta {0} no se encuentra disponible!", id) }
                };
            }
            var rouletteObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ExportModels.Roulette>(rouletteJson);
            if (rouletteObject != null && rouletteObject.status.Equals(ExportModels.Roulette.Status.create))
            {
                rouletteObject.status = ExportModels.Roulette.Status.open;
                _client.Set(id.ToString(), rouletteObject.ToJson());
            }
            else if (rouletteObject != null && rouletteObject.status.Equals(ExportModels.Roulette.Status.open))
            {
                return new Response
                {
                    Status = Response.ResponseStatus.fail,
                    Content = new ResumeResponseContent { Content = string.Format("Ruleta {0} ya abierta!", id) }
                };
            }
            else
            {
                return new Response
                {
                    Status = Response.ResponseStatus.fail,
                    Content = new ResumeResponseContent { Content = string.Format("Ruleta {0} en estado no valido", id) }
                };
            }

            return new Response
            {
                Status = Response.ResponseStatus.ok,
                Content = new ResumeResponseContent { Content = string.Format("Ruleta {0} abierta exitosamente", id) }
            };
        }

        public IResponse AddBet(Guid rouletteId, Bet bet)
        {
            var rouletteJson = _client.Get(rouletteId.ToString());
            if (rouletteJson == null)
            {
                return new Response
                {
                    Status = Response.ResponseStatus.fail,
                    Content = new ResumeResponseContent { Content = string.Format("Ruleta {0} no se encuentra disponible!", rouletteId) }
                };
            }
            var rouletteObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ExportModels.Roulette>(rouletteJson);            
            if (ValidateBet(rouletteObject, bet, out var validationResponse))
            {
                if (rouletteObject.bets == null)
                    rouletteObject.bets = new List<Bet>();
                rouletteObject.bets.Add(bet);
                _client.Set(rouletteObject.id.ToString(), rouletteObject.ToJson());
                return new Response
                {
                    Status = Response.ResponseStatus.ok,
                    Content = null
                };
            }
            else
            {
                return validationResponse;
            }
        }

        private bool ValidateBet(ExportModels.Roulette roulette, Bet bet, out Response response)
        {
            if (!roulette.status.Equals(ExportModels.Roulette.Status.open))
            {
                response =  new Response
                {
                    Status = Response.ResponseStatus.fail,
                    Content = new ResumeResponseContent { Content = string.Format("Ruleta {0} no esta en un estado valido para aceptar una apuesta.", roulette.id.ToString()) }              
                };
                return false;
            }

            if (!(bet.Amount > 0 && bet.Amount <= 10000))
            {
                response =  new Response
                {
                    Status = Response.ResponseStatus.fail,
                    Content = new ResumeResponseContent { Content = "El monto de la apuesta debe estar comprendido entre 1 y 10000 dollares." }
                };
                return false;
            }

            if (roulette.bets != null)
            {
                if (roulette.bets.Where(b => b.ClientId.Equals(bet.ClientId)).Any())
                {
                    response = new Response
                    {
                        Status = Response.ResponseStatus.fail,
                        Content = new ResumeResponseContent { Content = string.Format("El cliente {0} tiene ya una apuesta en la Ruleta {1}.", bet.ClientId, roulette.id.ToString()) }
                    };
                    return false;
                }
                if (bet.Type.Equals(Bet.BetType.num) && roulette.bets.Where(b => b.Num.Equals(bet.Num)).Any())
                {
                    response = new Response
                    {
                        Status = Response.ResponseStatus.fail,
                        Content = new ResumeResponseContent { Content = string.Format("El número fue ya seleccionado para la ruleta El monto de la apuesta debe estar comprendido entre 1 y 10000 dollares.", roulette.id.ToString()) }
                    };
                    return false;
                }
            }
            response = new Response();
            return true;
        }

        public IResponse Close(Guid id)
        {
            var rouletteJson = _client.Get(id.ToString());
            if (rouletteJson == null)
            {
                return new Response
                {
                    Status = Response.ResponseStatus.fail,
                    Content = new ResumeResponseContent { Content = string.Format("Ruleta {0} no se encuentra disponible!", id) }
                };
            }
            var rouletteObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ExportModels.Roulette>(rouletteJson);
            if (ValidateRouletteClose(rouletteObject, out var validationResponse))
            {
                rouletteObject.status = ExportModels.Roulette.Status.close;
                _client.Set(id.ToString(), rouletteObject.ToJson());
                SelectWinner(rouletteObject, out var winnerValue);
                _client.Set(id.ToString(), rouletteObject.ToJson());
                return new Response
                {
                    Status = Response.ResponseStatus.ok,
                    Content = new { winnerValue = winnerValue, bets = rouletteObject.bets }
                };
            }
            else
            {
                return validationResponse;
            }
        }

        private bool ValidateRouletteClose(ExportModels.Roulette roulette, out Response response)
        {            
            if (roulette.status == ExportModels.Roulette.Status.close)
            {
                response = new Response
                {
                    Status = Response.ResponseStatus.fail,
                    Content = new ResumeResponseContent { Content = string.Format("Ruleta {0} en estado no valido", roulette.id) }
                };
                return false;
            }
            response = new Response();
            return true;
        }

        private void SelectWinner(ExportModels.Roulette roulette, out int winnerValue)
        {
            var random = new Random();
            winnerValue = random.Next(0, 38);
            foreach (var bet in roulette.bets)
            {
                if (bet.Type.Equals(Bet.BetType.num) && bet.Num.Equals(winnerValue))
                {
                    bet.Amount *= 5;
                }
                else if (bet.Type.Equals(Bet.BetType.color) && (   
                    (winnerValue.IsEwen() && bet.Color.Equals(Bet.BetColor.red)) ||
                    (!winnerValue.IsEwen() && bet.Color.Equals(Bet.BetColor.black))))
                {
                    bet.Amount *= 1.8;
                }
                else
                {
                    bet.Amount *= -1;
                }
            }
        }
    }
}
