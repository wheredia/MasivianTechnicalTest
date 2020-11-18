using MasivianTechnicalTest.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasivianTechnicalTest.Domain.Models.ViewModels
{
    [Serializable]
    public class ResumeResponseContent :  IResponseContent
    {
        public String Content { get; set; }
    }
}
