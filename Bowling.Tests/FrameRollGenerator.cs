using System;
using System.Collections.Generic;
using Bowling.Domain;

namespace Bowling.Tests
{
    internal abstract class FrameRollGenerator
    {
        private readonly bool _isLast;
        private readonly Random _randomRoll;

        private int _pinsRemained;

        protected FrameRollGenerator(bool isLast, Random randomRoll)
        {
            _isLast = isLast;
            _randomRoll = randomRoll;
        }

        public abstract IEnumerable<int> Rolls { get; }

        public abstract FrameResult FrameResult { get; }

        protected bool IsLast
        {
            get { return _isLast; }
        }

        protected int GenerateNextRolledPins(bool? cleanUp = null)
        {
            if (_pinsRemained == 0)
            {
                _pinsRemained = Consts.StartingPinsCount;
            }
            bool ultimateCleanup = cleanUp ?? _randomRoll.Next(2) > 0;
            int pinsRolled = ultimateCleanup ? _pinsRemained : _randomRoll.Next(_pinsRemained - 1);
            _pinsRemained -= pinsRolled;
            return pinsRolled;
        }
    }
}