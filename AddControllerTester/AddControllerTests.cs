using FluentAssertions;
using MyCubixAPI.Controllers;

namespace AddControllerTester
{
    public class AddControllerTests
    {
        private readonly AddController _sut = new AddController();

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(123456, 654321, 777777)]
        [InlineData(long.MaxValue - 1, 1, long.MaxValue)]
        public void Get_ShouldReturnSum_ForTwoPositiveNumbers(long a, long b, long expected)
        {
            var result = _sut.Get(a, b);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(-1, -2, -3)]
        [InlineData(-123456, -654321, -777777)]
        [InlineData(long.MinValue + 1, -1, long.MinValue)]
        public void Get_ShouldReturnSum_ForTwoNegativeNumbers(long a, long b, long expected)
        {
            var result = _sut.Get(a, b);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(5, -3, 2)]
        [InlineData(-10, 4, -6)]
        [InlineData(42, -42, 0)]
        public void Get_ShouldReturnSum_ForMixedSigns(long a, long b, long expected)
        {
            var result = _sut.Get(a, b);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 5, 5)]
        [InlineData(-7, 0, -7)]
        public void Get_ShouldReturnSum_WhenOneIsZero(long a, long b, long expected)
        {
            var result = _sut.Get(a, b);
            result.Should().Be(expected);
        }

        // Szélsõséges/túlcsordulásos esetek
        [Fact]
        public void Get_ShouldWrapAround_OnPositiveOverflow()
        {
            var result = _sut.Get(long.MaxValue, 1);
            result.Should().Be(long.MinValue);
        }

        [Fact]
        public void Get_ShouldWrapAround_OnNegativeOverflow()
        {
            var result = _sut.Get(long.MinValue, -1);
            result.Should().Be(long.MaxValue);
        }

        [Fact]
        public void Get_ShouldReturnMaxValue_WhenAddingZeroToMax()
        {
            var result = _sut.Get(long.MaxValue, 0);
            result.Should().Be(long.MaxValue);
        }

        [Fact]
        public void Get_ShouldReturnMinValue_WhenAddingZeroToMin()
        {
            var result = _sut.Get(long.MinValue, 0);
            result.Should().Be(long.MinValue);
        }
    }
}