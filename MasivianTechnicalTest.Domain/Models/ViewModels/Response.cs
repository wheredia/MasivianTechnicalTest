using MasivianTechnicalTest.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasivianTechnicalTest.Domain.Models.ViewModels
{
    [Serializable]
    public class Response : IResponse
    {
        public enum ResponseStatus { ok, fail };
        public ResponseStatus Status { get; set; }
        //public IResponseContent Content { get; set; }
        public Object Content { get; set; }
    }
}
