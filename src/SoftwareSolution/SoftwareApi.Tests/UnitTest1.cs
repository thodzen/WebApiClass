namespace SoftwareApi.Tests;

[Trait("Category", "Unit")]
public class UnitTest1
{
    [Fact]
    public void CanAddTenAndTwenty()
    {
        // Given
        int a = 10; int b = 20; int answer;
        // When
        answer = a + b;
        // Then
        Assert.Equal(30, answer);
    }

    [Theory]
    [InlineData(10, 20, 30)]
    [InlineData(2, 2, 4)]
    [InlineData(10, 2, 12)]
    public void CanAddAnyTwoIntegers(int a, int b, int expected)
    {
        int answer = a + b;

        Assert.Equal(expected, answer);
    }

}