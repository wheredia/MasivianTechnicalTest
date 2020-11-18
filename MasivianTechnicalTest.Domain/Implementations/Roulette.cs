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
            //throw new NotImplementedException();
            IResponse response;
            try
            {
                var o = new ExportModels.Roulette
                {
                    id = Guid.NewGuid(),
                    status = ExportModels.Roulette.Status.create
                };

                _client.Set(o.id.ToString(), o.ToJson());

                response = new ExportModels.Response
                {
                    Status = ExportModels.Response.ResponseStatus.ok,
                    Content = new ResumeResponseContent
                    {
                        Content = o.id.ToString()
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

        public IList<ExportModels.Roulette> GetAll()
        {
            var response = new List<ExportModels.Roulette>();
            var dataResponse = _client.GetAll();

            foreach (var hash in dataResponse)
            {
                var value = Newtonsoft.Json.JsonConvert.DeserializeObject<ExportModels.Roulette>(hash.Value);
                response.Add(value);
            }
            
            return response;
        }

        public IResponse Open(Guid id)
        {
            var response = new Response
            {
                Status = Response.ResponseStatus.ok,
                Content = null
            };
            var rouletteJson = _client.Get(id.ToString());
            var rouletteObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ExportModels.Roulette>(rouletteJson);

            if (rouletteObject.status.Equals(ExportModels.Roulette.Status.create))
            {
                rouletteObject.status = ExportModels.Roulette.Status.open;
                _client.Set(id.ToString(), rouletteObject.ToJson());
            }
            else if (rouletteObject.status.Equals(ExportModels.Roulette.Status.open))
            {
                response = new Response
                {
                    Status = Response.ResponseStatus.fail,
                    Content = new ResumeResponseContent { Content = string.Format("Ruleta {0} ya abierta!", id) }
                };
            }
            else
            {
                response = new Response
                {
                    Status = Response.ResponseStatus.ok,
                    Content = new ResumeResponseContent { Content = string.Format("Ruleta {0} en estado no valido", id) }
                };
            }
            return response;
        }

        public IResponse AddBet(Guid rouletteId, Bet bet)
        {
            var response = new Response
            {
                Status = Response.ResponseStatus.ok,
                Content = null
            };
            var rouletteJson = _client.Get(rouletteId.ToString());
            var rouletteObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ExportModels.Roulette>(rouletteJson);
            var validationResponse = ValidateBet(rouletteObject, bet);
            if (validationResponse == null)
            {

                if (rouletteObject.bets == null)
                    rouletteObject.bets = new List<Bet>();

                rouletteObject.bets.Add(bet);
                _client.Set(rouletteObject.id.ToString(), rouletteObject.ToJson());
                return response;
            }
            else
            {
                return validationResponse;
            }
        }

        private Response ValidateBet(ExportModels.Roulette roulette, Bet bet)
        {
            if (!roulette.status.Equals(ExportModels.Roulette.Status.open))
            {
                return new Response
                {
                    Status = Response.ResponseStatus.fail,
                    Content = new ResumeResponseContent { Content = string.Format("Ruleta {0} no esta en un estado valido para aceptar una apuesta.", roulette.id.ToString()) }
                };
            }

            if (!(bet.Amount > 0 && bet.Amount <= 10000))
            {
                return new Response
                {
                    Status = Response.ResponseStatus.fail,
                    Content = new ResumeResponseContent { Content = "El monto de la apuesta debe estar comprendido entre 1 y 10000 dollares." }
                };
            }

            if (roulette.bets != null)
            {
                if (roulette.bets.Where(b => b.ClientId.Equals(bet.ClientId)).Any())
                {
                    return new Response
                    {
                        Status = Response.ResponseStatus.fail,
                        Content = new ResumeResponseContent { Content = string.Format("El cliente {0} tiene ya una apuesta en la Ruleta {1}.", bet.ClientId, roulette.id.ToString()) }
                    };
                }
                if (bet.Type.Equals(Bet.BetType.num) && roulette.bets.Where(b => b.Num.Equals(bet.Num)).Any())
                {
                    return new Response
                    {
                        Status = Response.ResponseStatus.fail,
                        Content = new ResumeResponseContent { Content = string.Format("El número fue ya seleccionado para la ruleta El monto de la apuesta debe estar comprendido entre 1 y 10000 dollares.", roulette.id.ToString()) }
                    };
                }
            }

            return null;
        }

        public IResponse Close(Guid id)
        {
            var response = new Response
            {
                Status = Response.ResponseStatus.ok,
                Content = null
            };
            var rouletteJson = _client.Get(id.ToString());
            var rouletteObject = Newtonsoft.Json.JsonConvert.DeserializeObject<ExportModels.Roulette>(rouletteJson);
            var validationResponse = ValidateRouletteClose(rouletteObject);

            if (validationResponse.Status == Response.ResponseStatus.ok)
            {
                rouletteObject.status = ExportModels.Roulette.Status.close;
                _client.Set(id.ToString(), rouletteObject.ToJson());
                var winnerValue = SelectWinner(rouletteObject);
                _client.Set(id.ToString(), rouletteObject.ToJson());
                response.Content = new { winnerValue = winnerValue, bets = rouletteObject.bets };
            }
            else
            {
                response = new Response
                {
                    Status = Response.ResponseStatus.fail,
                    Content = new ResumeResponseContent { Content = string.Format("Ruleta {0} en estado no valido", id) }
                };
            }

            return response;
        }

        private Response ValidateRouletteClose(ExportModels.Roulette roulette)
        {
            var response = new Response
            {
                Status = Response.ResponseStatus.ok,
                Content = null
            };

            if (roulette.status == ExportModels.Roulette.Status.close)
            {

                response = new Response
                {
                    Status = Response.ResponseStatus.fail,
                    Content = new ResumeResponseContent { Content = string.Format("Ruleta {0} en estado no valido", roulette.id) }
                };
            }
            return response;
        }

        private int SelectWinner(ExportModels.Roulette roulette)
        {
            var random = new Random();
            var winnerValue = random.Next(0, 38);
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

            return winnerValue;
        }
    }
}
