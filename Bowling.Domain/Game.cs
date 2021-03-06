﻿using System.Collections.Generic;
using System.Linq;
using Bowling.Domain.Frames;
using Bowling.Domain.Utils;

namespace Bowling.Domain
{
    public class Game
    {
        private readonly ICollection<IFrame> _frames = new LinkedList<IFrame>();

        private IFrame _currentFrame;

        private IFrame CurrentFrame
        {
            get
            {
                if (_currentFrame == null || _currentFrame.IsOver)
                {
                    if (_frames.Count >= Consts.FrameCount)
                    {
                        throw new BowlingException("Game is over");
                    }
                    _currentFrame = CreateFrame();
                    _frames.Add(_currentFrame);
                }
                return _currentFrame;
            }
        }

        public int GetScore()
        {
            return GetFrameListScore(new RecursiveList<IFrame>(_frames));
        }

        public void Roll(int rolledPins)
        {
            CurrentFrame.Roll(rolledPins);
        }

        private IFrame CreateFrame()
        {
            return _frames.Count < Consts.FrameCount - 1 ? new Frame() : new LastFrame();
        }

        private int GetFrameListScore(IRecursiveList<IFrame> frameList)
        {
            if (frameList.Empty)
            {
                return 0;
            }
            return GetFrameScore(frameList.Head, frameList.Tail) + GetFrameListScore(frameList.Tail);
        }

        private int GetFrameScore(IFrame frame, IEnumerable<IFrame> nextFrames)
        {
            return frame.GetScores().Sum() +
                   GetRolls(nextFrames).Take(GetBonusCountFromResult(frame.Result)).Sum();
        }

        private IEnumerable<int> GetRolls(IEnumerable<IFrame> frames)
        {
            return frames.SelectMany(frame => frame.GetScores());
        }

        private static int GetBonusCountFromResult(FrameResult? frameResult)
        {
            switch (frameResult)
            {
                case FrameResult.Spare:
                    return Consts.SpareBonusRolls;
                case FrameResult.Strike:
                    return Consts.StrikeBonusRolls;
                default:
                    return 0;
            }
        }
    }
}