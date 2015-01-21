using System;
using System.Collections.Generic;
using System.Linq;
using Bowling.Domain;

namespace Bowling.Tests
{
    public class RollGenerator
    {
        private readonly FrameResult _frameResult;
        private readonly Random _randomRoll = new Random();

        public RollGenerator(FrameResult frameResult)
        {
            _frameResult = frameResult;
        }

        public IEnumerable<int> Rolls
        {
            get { return Frames.SelectMany(generator => generator.Rolls); }
        }

        private IEnumerable<FrameRollGenerator> Frames
        {
            get
            {
                for (int i = 0; i < Consts.FrameCount; i++)
                {
                    yield return CreateFrameRollGenerator(_frameResult, i);
                }
            }
        }

        private FrameRollGenerator CreateFrameRollGenerator(FrameResult frameResult, int i)
        {
            bool isLast = i == Consts.FrameCount - 1;
            switch (frameResult)
            {
                case FrameResult.Normal:
                    return new NormalFrameRollGenerator(isLast, _randomRoll);
                case FrameResult.Spare:
                    return new SpareFrameRollGenerator(isLast, _randomRoll);
                case FrameResult.Strike:
                    return new StrikeFrameRollGenerator(isLast, _randomRoll);
            }
            throw new ArgumentOutOfRangeException("frameResult");
        }

        private abstract class FrameRollGenerator
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

        private class NormalFrameRollGenerator : FrameRollGenerator
        {
            public NormalFrameRollGenerator(bool isLast, Random randomRoll)
                : base(isLast, randomRoll)
            {
            }

            public override IEnumerable<int> Rolls
            {
                get
                {
                    yield return GenerateNextRolledPins(false);
                    yield return GenerateNextRolledPins(false);
                }
            }

            public override FrameResult FrameResult
            {
                get { return FrameResult.Normal; }
            }
        }

        private class SpareFrameRollGenerator : FrameRollGenerator
        {
            public SpareFrameRollGenerator(bool isLast, Random randomRoll)
                : base(isLast, randomRoll)
            {
            }

            public override IEnumerable<int> Rolls
            {
                get
                {
                    yield return GenerateNextRolledPins(false);
                    yield return GenerateNextRolledPins(true);
                    if (IsLast)
                    {
                        yield return GenerateNextRolledPins();
                    }
                }
            }

            public override FrameResult FrameResult
            {
                get { return FrameResult.Spare; }
            }
        }

        private class StrikeFrameRollGenerator : FrameRollGenerator
        {
            public StrikeFrameRollGenerator(bool isLast, Random randomRoll)
                : base(isLast, randomRoll)
            {
            }

            public override IEnumerable<int> Rolls
            {
                get
                {
                    yield return GenerateNextRolledPins(true);
                    if (IsLast)
                    {
                        yield return GenerateNextRolledPins();
                        yield return GenerateNextRolledPins();
                    }
                }
            }

            public override FrameResult FrameResult
            {
                get { return FrameResult.Strike; }
            }
        }
    }
}