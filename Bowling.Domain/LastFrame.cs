namespace Bowling.Domain
{
    internal class LastFrame: IFrame
    {
        public bool IsOver { get; private set; }
        public void Roll(int rolledPins)
        {
            throw new System.NotImplementedException();
        }

        public int Score()
        {
            throw new System.NotImplementedException();
        }
    }
}