using System.Collections.Generic;
using System.Linq;
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
            return GetMainScore() + GetBonuses();
        }

        private int GetBonuses()
        {
            return GetBonusesForFrameList(new RecursiveList<IFrame>(_frames));
        }

        private int GetBonusesForFrameList(IRecursiveList<IFrame> frameList)
        {
            if (frameList.Empty)
            {
                return 0;
            }
            return GetBonusesForFrame(frameList.Head.Result, frameList.Tail) + GetBonusesForFrameList(frameList.Tail);
        }

        private int GetBonusesForFrame(FrameResult? frameResult, IEnumerable<IFrame> nextFrames)
        {
            return GetRolls(nextFrames).Take(GetBonusCountFromResult(frameResult)).Sum();
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

        private IEnumerable<int> GetRolls(IEnumerable<IFrame> frames)
        {
            return frames.SelectMany(frame => frame.GetScores());
        }

        private int GetMainScore()
        {
            return _frames.Sum(frame => frame.GetScores().Sum());
        }

        public void Roll(int rolledPins)
        {
            CurrentFrame.Roll(rolledPins);
        }

        private IFrame CreateFrame()
        {
            return _frames.Count < Consts.FrameCount - 1 ? new Frame() : new LastFrame();
        }
    }
}