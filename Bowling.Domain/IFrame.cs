using System.Collections.Generic;

namespace Bowling.Domain
{
    internal interface IFrame
    {
        bool IsOver { get; }
        
        FrameResult? Result { get; }

        void Roll(int rolledPins);

        int GetBonusesForPreviousFrame(FrameResult? lastResult);

        int GetScore();
        
        IEnumerable<int> GetScores();
    }
}