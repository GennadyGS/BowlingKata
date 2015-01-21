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
        public void ShouldReturnCorrectScoreFirstRoll(int rolledPins)
        {
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
        public void ShouldCountTheScoreCorrectly()
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
        public void ShouldLimitNumberOfRollsInNormalFramesPerGame()
        {
            var rollGenerator = new RollGenerator(FrameResult.Normal);
            foreach (int pinCount in rollGenerator.Rolls)
            {
                _sut.Roll(pinCount);
            }
            Assert.Throws<BowlingException>(() => _sut.Roll(0));
        }

        [Fact]
        public void ShouldLimitNumberOfRollsInSpareFramesPerGame()
        {
            var rollGenerator = new RollGenerator(FrameResult.Spare);
            foreach (int pinCount in rollGenerator.Rolls)
            {
                _sut.Roll(pinCount);
            }
            Assert.Throws<BowlingException>(() => _sut.Roll(0));
        }
    }
}