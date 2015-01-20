using System.Collections.Generic;
using System.Linq;

namespace Bowling.Domain
{
    public class Game
    {
        private const int FrameCount = 10;

        private readonly ICollection<Frame> _frames = new LinkedList<Frame>();
        private Frame _currentFrame;

        public int Score()
        {
            return _frames.Sum(frame => frame.Score());
        }

        public void Roll(int rolledPins)
        {
            CurrentFrame.Roll(rolledPins);
        }

        private Frame CurrentFrame
        {
            get
            {
                if (_currentFrame == null || _currentFrame.IsOver)
                {
                    if (_frames.Count >= FrameCount)
                    {
                        throw new BowlingException("Game is over");
                    }
                    _currentFrame = new Frame();
                    _frames.Add(_currentFrame);
                }
                return _currentFrame;
            }
        }
    }
}