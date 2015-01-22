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

        public IEnumerable<int> GenerateRolls(FrameResult frameResult)
        {
            return GenerateFrames(frameResult).SelectMany(generator => generator.GetRolls());
        }

        public FrameRollGenerator CreateFrameRollGenerator(FrameResult frameResult, bool isLast = false)
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

        private IEnumerable<FrameRollGenerator> GenerateFrames(FrameResult frameResult)
        {
            for (int i = 0; i < Consts.FrameCount; i++)
            {
                yield return CreateFrameRollGenerator(frameResult, i == Consts.FrameCount - 1);
            }
        }
    }
}