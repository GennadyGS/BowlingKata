using System.Collections.Generic;
using System.Linq;

namespace Bowling.Domain
{
    public class Game
    {
        private const int FrameCount = 10;

        private readonly ICollection<IFrame> _frames = new LinkedList<IFrame>();
        private IFrame _currentFrame;

        public int Score()
        {
            return _frames.Sum(frame => frame.Score());
        }

        public void Roll(int rolledPins)
        {
            CurrentFrame.Roll(rolledPins);
        }

        private IFrame CurrentFrame
        {
            get
            {
                if (_currentFrame == null || _currentFrame.IsOver)
                {
                    if (_frames.Count >= FrameCount)
                    {
                        throw new BowlingException("Game is over");
                    }
                    _currentFrame = CreateFrame();
                    _frames.Add(_currentFrame);
                }
                return _currentFrame;
            }
        }

        private IFrame CreateFrame()
        {
            return _frames.Count < FrameCount - 1 ? new Frame() : new LastFrame();
        }
    }
}