namespace Bowling.Domain
{
    internal class NormalFrame : IFrame
    {
        private const int StartingPinsCount = 10;
        private const int MinRollsPerFrameCount = 2;

        private int _rollCount;
        private int _score;

        public bool IsOver
        {
            get { return _rollCount >= MinRollsPerFrameCount; }
        }

        public void Roll(int rolledPins)
        {
            _rollCount += 1;
            _score += rolledPins;
            if (_score > StartingPinsCount)
            {
                throw new BowlingException("Max rolled pins violation");
            }
        }

        public int Score()
        {
            return _score;
        }
    }
}