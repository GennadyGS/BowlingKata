﻿using System.Collections.Generic;

namespace Bowling.Domain.Frames
{
    internal interface IFrame
    {
        bool IsOver { get; }

        FrameResult? Result { get; }

        void Roll(int rolledPins);

        IEnumerable<int> GetScores();
    }
}