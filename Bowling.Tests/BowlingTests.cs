using System;
using Bowling.Domain;
using FluentAssert;
using Xunit;
using Xunit.Extensions;

namespace Bowling.Tests
{
    public class BowlingTests
    {
        private readonly Game _sut;

        public BowlingTests()
        {
            _sut = new Game();
        }

        [Fact]
        public void ShouldReturnZeroScoreOnStart()
        {
            int score = _sut.Score();
            score.ShouldBeEqualTo(0, "Score must be zero on game start");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        public void ShouldReturnCorrectCorrectlyScoreFirstRoll(int rolledPins)
        {
            _sut.Roll(rolledPins);
            int score = _sut.Score();
            score.ShouldBeEqualTo(rolledPins, string.Format("Score after first roll must be {0}", rolledPins));
        }

        [Fact]
        public void SholdThrowExceptionOnRollLimitViolation()
        {
            Assert.Throws<BowlingException>(() => _sut.Roll(Consts.MaxPinCount + 1));
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
        public void ShouldCountTheScoreCorrectly()
        {
            const int maxSafePinsCount = Consts.MaxPinCount / 2;
            var random = new Random();
            int totalPins = 0;
            for (int i = 0; i < Consts.MinRollsPerGameCount; i++)
            {
                var pins = random.Next(maxSafePinsCount);
                totalPins += pins;
                _sut.Roll(pins);
            }
            Assert.Equal(totalPins, _sut.Score());
        }

        [Fact]
        public void ShouldLimitNumberOfFailedRollsPerGame()
        {
            for (int i = 0; i < Consts.MinRollsPerGameCount; i++)
            {
                _sut.Roll(0);
            }
            Assert.Throws<BowlingException>(() => _sut.Roll(0));
        }
    }
}