using System;

namespace Bowling.Domain
{
    public class BowlingException : Exception
    {
        public BowlingException()
        {
        }

        public BowlingException(string message)
            : base(message)
        {
        }
    }
}