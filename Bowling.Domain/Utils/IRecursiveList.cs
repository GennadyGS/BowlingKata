using System.Collections.Generic;

namespace Bowling.Domain.Utils
{
    internal interface IRecursiveList<out T>: IEnumerable<T>
    {
        bool Empty { get; }

        T Head { get; }
        
        IRecursiveList<T> Tail { get; }
    }
}