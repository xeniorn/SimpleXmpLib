using FluentAssertions;

namespace SimpleXmpLib.Test;

public class DateTime
{
    [Theory]
    [InlineData(DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Unspecified)]
    public void Test(DateTimeKind dtk)
    {
        var dt = new System.DateTime(2023, 10, 1, 12, 0, 0, dtk);

        var dtString = XmpHelper.GetUtcDateTimeXmpString(dt);

        dtString.Should().StartWith("2023-10-01");
    }
}