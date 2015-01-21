using System;
using System.Collections.Generic;

namespace Bowling.Tests
{
    internal class NormalFrameRollGenerator : FrameRollGenerator
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
}