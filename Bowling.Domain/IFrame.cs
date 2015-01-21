namespace Bowling.Domain
{
    internal interface IFrame
    {
        bool IsOver { get; }
        
        void Roll(int rolledPins);
        
        int Score();
    }
}