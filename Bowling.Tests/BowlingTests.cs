using System;
using Bowling.Domain;
using FluentAssert;
using Xunit;
using Xunit.Extensions;

namespace Bowling.Tests
{
    public class BowlingTests
    {
        private readonly Game _sut = new Game();
        private readonly Random _random = new Random();

        [Fact]
        public void ShouldReturnZeroScoreOnStart()
        {
            int score = _sut.Score();
            score.ShouldBeEqualTo(0, "Score must be zero on game start");
        }

        [Fact]
        public void ShouldReturnCorrectScoreFirstRoll()
        {
            int rolledPins = _random.Next(Consts.StartingPinsCount);
            _sut.Roll(rolledPins);
            int score = _sut.Score();
            score.ShouldBeEqualTo(rolledPins, string.Format("Score after first roll must be {0}", rolledPins));
        }

        [Fact]
        public void SholdThrowExceptionOnRollLimitViolation()
        {
            Assert.Throws<BowlingException>(() => _sut.Roll(Consts.StartingPinsCount + 1));
        }

        [Theory]
        [InlineData(2, 9)]
        [InlineData(4, 7)]
        public void SholdThrowExceptionOnDoubleRollLimitViolation(int rolledPins1, int rolledPins2)
        {
            Assert.Throws<BowlingException>(() =>
            {
                _sut.Roll(rolledPins1);
                _sut.Roll(rolledPins2);
            });
        }

        [Fact]
        public void ShouldLimitNumberOfZeroRollsPerGame()
        {
            for (int i = 0; i < Consts.FrameCount; i++)
            {
                for (int j = 0; j < Consts.RollsPerFrame; j++)
                {
                    _sut.Roll(0);
                }
            }
            Assert.Throws<BowlingException>(() => _sut.Roll(0));
        }

        [Fact]
        public void ShouldCountTheScorerCorrectlyInNormalFrames()
        {
            int totalPins = 0;
            var rollsGenerator = new RollGenerator(FrameResult.Normal);
            foreach (int pinCount in rollsGenerator.Rolls)
            {
                _sut.Roll(pinCount);
                totalPins += pinCount;
            }
            Assert.Equal(totalPins, _sut.Score());
        }

        [Theory]
        [InlineData(FrameResult.Normal)]
        [InlineData(FrameResult.Spare)]
        [InlineData(FrameResult.Strike)]
        public void ShouldLimitNumberOfRollsPerGame(FrameResult frameResult)
        {
            var rollGenerator = new RollGenerator(FrameResult.Normal);
            foreach (int pinCount in rollGenerator.Rolls)
            {
                _sut.Roll(pinCount);
            }
            Assert.Throws<BowlingException>(() => _sut.Roll(0));
        }
    }
}