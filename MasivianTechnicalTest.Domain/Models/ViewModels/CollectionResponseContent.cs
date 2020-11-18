using MasivianTechnicalTest.Domain.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MasivianTechnicalTest.Domain.Models.ViewModels
{
    [Serializable]
    public class CollectionResponseContent : ResumeResponseContent, IList<IRoulette>
    {
        public IRoulette this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(IRoulette item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(IRoulette item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(IRoulette[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IRoulette> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(IRoulette item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, IRoulette item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IRoulette item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
