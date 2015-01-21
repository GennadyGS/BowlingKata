using System;
using System.Collections.Generic;
using System.Linq;
using Bowling.Domain;
using FluentAssert;
using Xunit;
using Xunit.Extensions;

namespace Bowling.Tests
{
    public class BowlingTests
    {
        private readonly Random _random = new Random();
        private readonly GameRollGenerator _rollsGenerator;
        private readonly Game _sut = new Game();

        public BowlingTests()
        {
            _rollsGenerator = new GameRollGenerator(_random);
        }

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
            foreach (int pinCount in _rollsGenerator.GenerateRolls(FrameResult.Normal))
            {
                _sut.Roll(pinCount);
            }
            Assert.Throws<BowlingException>(() => _sut.Roll(0));
        }

        [Fact]
        public void ShouldScoreCorrectlyGameWithNormalFrames()
        {
            int totalPins = 0;
            foreach (int pinCount in _rollsGenerator.GenerateRolls(FrameResult.Normal))
            {
                _sut.Roll(pinCount);
                totalPins += pinCount;
            }
            Assert.Equal(totalPins, _sut.Score());
        }

        [Fact]
        public void ShouldScoreCorrectlySpareFrame()
        {
            List<int> frameRolls = _rollsGenerator.CreateFrameRollGenerator(FrameResult.Spare).GetRolls().ToList();
            foreach (int pinCount in frameRolls)
            {
                _sut.Roll(pinCount);
            }
            int nextRollPinCount = _random.Next(1, Consts.StartingPinsCount);
            _sut.Roll(nextRollPinCount);
            Assert.Equal(frameRolls.Sum() + nextRollPinCount + nextRollPinCount, _sut.Score());
        }

        [Fact]
        public void ShouldScoreCorrectlyStrikeFrame()
        {
            List<int> frameRolls = _rollsGenerator.CreateFrameRollGenerator(FrameResult.Strike).GetRolls().ToList();
            foreach (int pinCount in frameRolls)
            {
                _sut.Roll(pinCount);
            }
            List<int> nextFrameRolls = _rollsGenerator.CreateFrameRollGenerator(FrameResult.Normal).GetRolls().ToList();
            foreach (int pinCount in nextFrameRolls)
            {
                _sut.Roll(pinCount);
            }
            Assert.Equal(frameRolls.Sum() + nextFrameRolls.Sum() * 2, _sut.Score());
        }
    }
}