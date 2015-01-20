namespace Bowling.Tests
{
    internal static class Consts
    {
        public const int MaxPinCount = 10;

        public const int FrameCount = 10;

        public const int MinAttemptsPerFrame = 2;

        public const int MinRollsPerGameCount = FrameCount*MinAttemptsPerFrame;
    }
}