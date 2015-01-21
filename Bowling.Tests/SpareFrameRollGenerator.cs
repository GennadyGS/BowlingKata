using System;
using System.Collections.Generic;

namespace Bowling.Tests
{
    internal class SpareFrameRollGenerator : FrameRollGenerator
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
}