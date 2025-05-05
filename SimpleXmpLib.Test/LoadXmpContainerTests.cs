using FluentAssertions;
using SharedTestData;
using SimpleXmpLib.FileEmbedding;

namespace SimpleXmpLib.Test;

public class LoadXmpContainerTests : FileBasedTestBase
{
    [Theory]
    [MemberData(nameof(TestFilesWithEmbeddedXmp))]
    public void WhenEmbedded(string filePath)
    {
        var file = EmbeddedXmpMiner.GetXmpSource(filePath);
        file.Should().NotBeNull("A valid file was provided");

        var (exists, container) =  file.GetContainer();

        exists.Should().BeTrue("The file should contain embedded XMP data");
        container.Should().NotBeNull("A valid file was provided");
    }

    [Theory]
    [MemberData(nameof(TestFilesWithoutEmbeddedXmp))]
    public void WhenNotEmbedded(string filePath)
    {
        var file = EmbeddedXmpMiner.GetXmpSource(filePath);
        file.Should().NotBeNull("A valid file was provided");

        var (exists, container) = file.GetContainer();

        exists.Should().BeFalse("The file should not contain embedded XMP data");
        container.Should().NotBeNull("A valid file was provided");
    }
}

