using System;
using System.Collections.Generic;

namespace Bowling.Tests
{
    public class RollGenerator
    {
        private readonly FrameResult _frameResult;
        private readonly Random _random = new Random();

        public RollGenerator(FrameResult frameResult)
        {
            _frameResult = frameResult;
        }

        public IEnumerable<int> Rolls {
            get
            {
                const int maxSafePinsCount = (Consts.MaxPinCount - 1) / 2;
                for (int i = 0; i < Consts.MinRollsPerGameCount; i++)
                {
                    yield return _random.Next(maxSafePinsCount);
                }
            }
        }
    }
}