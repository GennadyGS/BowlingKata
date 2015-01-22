using System;
using System.Collections.Generic;
using Bowling.Tests.Common;

namespace Bowling.Tests.Generators
{
    internal class SpareFrameRollGenerator : FrameRollGenerator
    {
        public SpareFrameRollGenerator(bool isLast, Random randomRoll)
            : base(isLast, randomRoll)
        {
        }

        public override IEnumerable<int> GetRolls()
        {
            yield return GenerateNextRolledPins(false);
            yield return GenerateNextRolledPins(true);
            if (IsLast)
            {
                yield return GenerateNextRolledPins();
            }
        }

        public override FrameResult FrameResult
        {
            get { return FrameResult.Spare; }
        }
    }
}