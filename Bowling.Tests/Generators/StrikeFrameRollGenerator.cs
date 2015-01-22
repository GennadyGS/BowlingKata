using System;
using System.Collections.Generic;

namespace Bowling.Tests.Generators
{
    internal class StrikeFrameRollGenerator : FrameRollGenerator
    {
        public StrikeFrameRollGenerator(bool isLast, Random randomRoll)
            : base(isLast, randomRoll)
        {
        }

        public override IEnumerable<int> GetRolls()
        {
            yield return GenerateNextRolledPins(true);
            if (IsLast)
            {
                yield return GenerateNextRolledPins(true);
                yield return GenerateNextRolledPins(true);
            }
        }
    }
}