using System;
using System.Collections.Generic;
using System.Linq;
using Bowling.Domain;
using Bowling.Tests.Common;

namespace Bowling.Tests.Generators
{
    internal class GameRollGenerator
    {
        private readonly Random _random;

        public GameRollGenerator(Random random)
        {
            _random = random;
        }

        public IEnumerable<int> GenerateAllRolls(FrameResult frameResult)
        {
            return GenerateAllFrames(frameResult).SelectMany(frame => frame.GetRolls());
        }

        public FrameRollGenerator GenerateFrame(FrameResult frameResult, bool isLast = false)
        {
            switch (frameResult)
            {
                case FrameResult.Normal:
                    return new NormalFrameRollGenerator(isLast, _random);
                case FrameResult.Spare:
                    return new SpareFrameRollGenerator(isLast, _random);
                case FrameResult.Strike:
                    return new StrikeFrameRollGenerator(isLast, _random);
            }
            throw new ArgumentOutOfRangeException("frameResult");
        }

        public IEnumerable<FrameRollGenerator> GenerateAllFrames(FrameResult frameResult)
        {
            for (int i = 0; i < Consts.FrameCount; i++)
            {
                yield return GenerateFrame(frameResult, i == Consts.FrameCount - 1);
            }
        }
    }
}