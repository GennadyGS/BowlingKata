using System.Collections.Generic;
using System.Linq;

namespace Bowling.Domain.Frames
{
    internal class Frame : IFrame
    {
        private readonly List<int> _scores = new List<int>();
        private int _pinCount;

        protected FrameResult? Result
        {
            get
            {
                if (_scores.Any() && _scores.First() >= Consts.StartingPinsCount)
                {
                    return FrameResult.Strike;
                }
                if (RollsCount >= Consts.RollsPerFrame)
                {
                    if (_scores.Take(Consts.RollsPerFrame).Sum() >= Consts.StartingPinsCount)
                    {
                        return FrameResult.Spare;
                    }
                    return FrameResult.Normal;
                }
                return null;
            }
        }

        private int RollsCount
        {
            get { return _scores.Count; }
        }

        public bool IsOver
        {
            get { return RollsCount >= GetAllowedRollsCount(); }
        }

        FrameResult? IFrame.Result
        {
            get
            {
                if (_scores.FirstOrDefault() >= Consts.StartingPinsCount)
                {
                    return FrameResult.Strike;
                }
                if (_scores.Take(Consts.RollsPerFrame).Sum() >= Consts.StartingPinsCount)
                {
                    return FrameResult.Spare;
                }
                if (_scores.Count() >= Consts.RollsPerFrame)
                {
                    return FrameResult.Normal;
                }
                return null;
            }
        }

        void IFrame.Roll(int rolledPins)
        {
            if (IsOver)
            {
                throw new BowlingException("Frame is over");
            }
            if (_pinCount == 0)
            {
                _pinCount = Consts.StartingPinsCount;
            }
            if (rolledPins > _pinCount)
            {
                throw new BowlingException("Rolled pin count is more then accessible");
            }
            _scores.Add(rolledPins);
            _pinCount -= rolledPins;
        }

        IEnumerable<int> IFrame.GetScores()
        {
            return _scores;
        }

        protected virtual int GetAllowedRollsCount()
        {
            return Result == FrameResult.Strike ? 1 : Consts.RollsPerFrame;
        }
    }
}