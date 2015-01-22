using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bowling.Domain.Utils
{
    internal class RecursiveList<T> : IRecursiveList<T>
    {
        private readonly IEnumerable<T> _source;

        public RecursiveList(IEnumerable<T> source)
        {
            _source = source;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        public bool Empty
        {
            get { return !_source.Any(); }
        }

        public T Head
        {
            get
            {
                VerifyNotEmpty();
                return _source.First();
            }
        }

        public IRecursiveList<T> Tail {
            get
            {
                VerifyNotEmpty();
                return new RecursiveList<T>(_source.Skip(1));
            }
        }

        private void VerifyNotEmpty()
        {
            if (Empty)
            {
                throw new InvalidOperationException("List is empty");
            }
        }
    }
}