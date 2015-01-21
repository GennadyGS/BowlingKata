﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bowling.Domain;

namespace Bowling.Tests
{
    internal class RollGenerator
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
                    yield return CreateFrameRollGenerator(_frameResult, i == Consts.FrameCount - 1);
                }
            }
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
    }
}