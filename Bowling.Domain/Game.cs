using System.Collections.Generic;
using System.Linq;

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
            if (!_frames.Any())
            {
                return 0;
            }
            var initialState = new
            {
                Score = 0,
                LastResult = (FrameResult?)null
            };
            return _frames.Aggregate(initialState, (state, frame) =>
                new
                {
                    Score = state.Score + frame.GetBonusesForPreviousFrame(state.LastResult),
                    LastResult = frame.Result
                }).Score;
        }

        private int GetMainScore()
        {
            return _frames.Sum(frame => frame.Score());
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