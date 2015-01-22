using System;
using System.Collections.Generic;

namespace Bowling.Tests.Generators
{
    internal class NormalFrameRollGenerator : FrameRollGenerator
    {
        public NormalFrameRollGenerator(bool isLast, Random randomRoll)
            : base(isLast, randomRoll)
        {
        }

        public override IEnumerable<int> GetRolls()
        {
            yield return GenerateNextRolledPins(false);
            yield return GenerateNextRolledPins(false);
        }
    }
}