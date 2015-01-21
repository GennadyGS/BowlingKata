using System.Collections.Generic;
using System.Linq;

namespace Bowling.Domain
{
    internal class Frame : IFrame
    {
        public const int StartingPinsCount = 10;
        private const int RollsPerFrame = 2;
        private readonly List<int> _scores = new List<int>();
        private int _pinCount;

        private int RollsCount
        {
            get { return _scores.Count; }
        }

        protected FrameResult? Result
        {
            get
            {
                if (_scores.Any() && _scores.First() >= StartingPinsCount)
                {
                    return FrameResult.Strike;
                }
                if (RollsCount >= RollsPerFrame)
                {
                    if (_scores.Take(RollsPerFrame).Sum() >= StartingPinsCount)
                    {
                        return FrameResult.Spare;
                    }
                    return FrameResult.Normal;
                }
                return null;
            }
        }

        public bool IsOver
        {
            get { return RollsCount >= GetAllowedRollsCount(); }
        }

        public void Roll(int rolledPins)
        {
            if (IsOver)
            {
                throw new BowlingException("Frame is over");
            }
            if (_pinCount == 0)
            {
                _pinCount = StartingPinsCount;
            }
            if (rolledPins > _pinCount)
            {
                throw new BowlingException("Rolled pin count is more then accessible");
            }
            _scores.Add(rolledPins);
            _pinCount -= rolledPins;
        }

        public int Score()
        {
            return _scores.Sum();
        }

        protected virtual int GetAllowedRollsCount()
        {
            return RollsPerFrame;
        }
    }
}