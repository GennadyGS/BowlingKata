namespace Bowling.Domain
{
    internal class LastFrame : Frame
    {
        private const int MaxRollsPerLastFrame = 3;

        private bool IsSpecialResult(FrameResult? result)
        {
            return result == FrameResult.Spare || result == FrameResult.Strike;
        }

        protected override int GetAllowedRollsCount()
        {
            return IsSpecialResult(Result)
                ? MaxRollsPerLastFrame
                : base.GetAllowedRollsCount();
        }
    }
}