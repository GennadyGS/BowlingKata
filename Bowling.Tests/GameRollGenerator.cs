﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bowling.Domain;

namespace Bowling.Tests
{
    internal class GameRollGenerator
    {
        private readonly Random _randomRoll = new Random();

        public IEnumerable<int> GenerateRolls(FrameResult frameResult)
        {
            return GenerateFrames(frameResult).SelectMany(generator => generator.GetRolls());
        }

        public FrameRollGenerator CreateFrameRollGenerator(FrameResult frameResult, bool isLast = false)
        {
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

        private IEnumerable<FrameRollGenerator> GenerateFrames(FrameResult frameResult)
        {
            for (int i = 0; i < Consts.FrameCount; i++)
            {
                yield return CreateFrameRollGenerator(frameResult, i == Consts.FrameCount - 1);
            }
        }
    }
}