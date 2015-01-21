using System;
using Bowling.Domain;
using FluentAssert;
using Xunit;
using Xunit.Extensions;

namespace Bowling.Tests
{
    public class BowlingTests
    {
        private readonly Random _random = new Random();
        private readonly Game _sut = new Game();

        [Fact]
        public void ShouldReturnZeroScoreOnStart()
        {
            int score = _sut.Score();
            score.ShouldBeEqualTo(0, "Score must be zero on game start");
        }

        [Fact]
        public void ShouldReturnCorrectScoreAfterFirstRoll()
        {
            int rolledPins = _random.Next(Consts.StartingPinsCount);
            _sut.Roll(rolledPins);
            int score = _sut.Score();
            score.ShouldBeEqualTo(rolledPins, string.Format("Score after first roll must be {0}", rolledPins));
        }

        [Fact]
        public void SholdThrowExceptionOnRolledPinsLimitViolation()
        {
            Assert.Throws<BowlingException>(() => _sut.Roll(Consts.StartingPinsCount + 1));
        }

        [Fact]
        public void SholdThrowExceptionOnRolledPinsLimitViolationByTwoRolls()
        {
            int rolledPins1 = _random.Next(Consts.StartingPinsCount);
            int rolledPins2 = Consts.StartingPinsCount + 1 - rolledPins1;
            Assert.Throws<BowlingException>(() =>
            {
                _sut.Roll(rolledPins1);
                _sut.Roll(rolledPins2);
            });
        }

        [Fact]
        public void ShouldLimitNumberOfUnsuccessfulRollsPerGame()
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

        [Fact]
        public void ShouldScoreCorrectlyPerGameInNormalFrames()
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
    }
}