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

        public IResponse AddBet(Guid id, object bet)
        {
            throw new NotImplementedException();
        }

        public IResponse Close(Guid id)
        {
            throw new NotImplementedException();
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

            //response =

            //return new ExportModels.Response
            //{
            //    Status = ExportModels.Response.ResponseStatus.ok,
            //    Content = response
            //};

            return response;
        }

        public IResponse Open(Guid id)
        {
            var response = new Response { 
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
                //throw new Exception(string.Format("Ruleta {0} ya abierta!", id));
                response = new Response
                {
                    Status = Response.ResponseStatus.fail,
                    Content = new ResumeResponseContent { Content = string.Format("Ruleta {0} ya abierta!", id) }
                };
            }
            else
            {
                //throw new Exception(string.Format("Ruleta {0} en estado no valido", id));
                response = new Response
                {
                    Status = Response.ResponseStatus.ok,
                    Content = new ResumeResponseContent { Content = string.Format("Ruleta {0} en estado no valido", id) }
                };
            }
            return response;
        }
    }
}
