namespace Bowling.Domain.Frames
{
    internal class LastFrame : Frame
    {
        private static bool IsSpecialResult(FrameResult? result)
        {
            return result == FrameResult.Spare || result == FrameResult.Strike;
        }

        protected override int GetAllowedRollsCount()
        {
            return IsSpecialResult(Result)
                ? Consts.MaxRollsPerLastFrame
                : base.GetAllowedRollsCount();
        }
    }
}