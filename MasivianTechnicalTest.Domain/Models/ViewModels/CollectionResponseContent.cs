using MasivianTechnicalTest.Domain.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MasivianTechnicalTest.Domain.Models.ViewModels
{
    [Serializable]
    public class CollectionResponseContent : ResumeResponseContent, IEnumerable<IRoulette>
    {
        public IEnumerator<IRoulette> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
