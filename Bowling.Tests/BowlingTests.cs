using System;
using System.Collections.Generic;
using System.Linq;
using Bowling.Domain;
using Bowling.Tests.Common;
using Bowling.Tests.Generators;
using FluentAssert;
using Xunit;
using Xunit.Extensions;

namespace Bowling.Tests
{
    public class BowlingTests
    {
        private readonly Random _random = new Random();
        private readonly GameRollGenerator _gameRollGenerator;
        private readonly Game _sut = new Game();

        public BowlingTests()
        {
            _gameRollGenerator = new GameRollGenerator(_random);
        }

        [Fact]
        public void ShouldReturnZeroScoreOnStart()
        {
            int score = _sut.GetScore();
            score.ShouldBeEqualTo(0, "Score must be zero on game start");
        }

        [Fact]
        public void ShouldReturnCorrectScoreAfterFirstRoll()
        {
            int rolledPins = _random.Next(Consts.StartingPinsCount);
            _sut.Roll(rolledPins);
            int score = _sut.GetScore();
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
            foreach (int pinCount in _gameRollGenerator.GenerateAllRolls(FrameResult.Normal))
            {
                _sut.Roll(pinCount);
            }
            Assert.Throws<BowlingException>(() => _sut.Roll(0));
        }

        [Fact]
        public void ShouldScoreCorrectlySpareWithNormalFrame()
        {
            RollGeneratedFrame(FrameResult.Spare);
            var frameRolls2 = RollGeneratedFrame(FrameResult.Normal);
            Assert.Equal(Consts.StartingPinsCount + frameRolls2.Sum() + frameRolls2.First(), _sut.GetScore());
        }

        [Fact]
        public void ShouldScoreCorrectlyStrikeWithNormalFrame()
        {
            RollGeneratedFrame(FrameResult.Strike);
            var frameRolls2 = RollGeneratedFrame(FrameResult.Normal);
            Assert.Equal(Consts.StartingPinsCount + frameRolls2.Sum()*2, _sut.GetScore());
        }

        [Fact]
        public void ShouldScoreCorrectlyStrikeWithSpareFrame()
        {
            RollGeneratedFrame(FrameResult.Strike);
            RollGeneratedFrame(FrameResult.Spare);
            var frameRolls3 = RollGeneratedFrame(FrameResult.Normal);
            Assert.Equal(Consts.StartingPinsCount * 3 + frameRolls3.Sum() + frameRolls3.First(), _sut.GetScore());
        }

        [Fact]
        public void ShouldScoreCorrectlyDoubleStrikeWithNormalFrame()
        {
            RollGeneratedFrame(FrameResult.Strike);
            RollGeneratedFrame(FrameResult.Strike);
            var frameRolls3 = RollGeneratedFrame(FrameResult.Normal);
            Assert.Equal(Consts.StartingPinsCount * 3 + frameRolls3.Sum() * 2 + frameRolls3.First(), _sut.GetScore());
        }

        [Fact]
        public void ShouldScoreCorrectlyGameWithNormalFrames()
        {
            int totalPins = 0;
            foreach (int pinCount in _gameRollGenerator.GenerateAllRolls(FrameResult.Normal))
            {
                _sut.Roll(pinCount);
                totalPins += pinCount;
            }
            Assert.Equal(totalPins, _sut.GetScore());
        }

        [Fact]
        public void ShouldScoreCorrectlyGameWithSpareFrames()
        {
            int totalPins = 0;
            var frameRollGenerators = _gameRollGenerator.GenerateAllFrames(FrameResult.Spare).ToList();
            foreach (var frame in frameRollGenerators)
            {
                var pinCounts = frame.GetRolls().ToList();
                foreach (int pinCount in pinCounts)
                {
                    _sut.Roll(pinCount);
                    totalPins += pinCount;
                }
                if (frame != frameRollGenerators.First())
                {
                    totalPins += pinCounts.First();
                }
            }
            Assert.Equal(totalPins, _sut.GetScore());
        }

        private List<int> RollGeneratedFrame(FrameResult frameResult)
        {
            var frameRolls = _gameRollGenerator.GenerateFrame(frameResult).GetRolls().ToList();
            foreach (int pinCount in frameRolls)
            {
                _sut.Roll(pinCount);
            }
            return frameRolls;
        }
    }
}