using System;
using System.Collections.Generic;
using Bowling.Domain;

namespace Bowling.Tests.Generators
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

        protected bool IsLast
        {
            get { return _isLast; }
        }

        public abstract IEnumerable<int> GetRolls();

        protected int GenerateNextRolledPins(bool? cleanUp = null)
        {
            if (_pinsRemained == 0)
            {
                _pinsRemained = Consts.StartingPinsCount;
            }
            bool ultimateCleanup = cleanUp ?? NextBoolean();
            int pinsRolled = ultimateCleanup ? _pinsRemained : _randomRoll.Next(_pinsRemained - 1);
            _pinsRemained -= pinsRolled;
            return pinsRolled;
        }

        private bool NextBoolean()
        {
            return _randomRoll.Next(2) > 0;
        }
    }
}