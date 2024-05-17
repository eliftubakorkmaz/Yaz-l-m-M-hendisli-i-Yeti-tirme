namespace CalculatorLibrary.Tests.UnitTest;

public class UnitTest1
{
    private readonly Calculator _sut = new(); // system under test
    [Fact]
    public void Test1()
    {
        var result = _sut.Add(2, 7);

        Assert.Equal(9, result);
    }

    [Fact]
    public void Test2()
    {
        var result = _sut.Subtract(7, 2);

        Assert.Equal(5, result);
    }
}