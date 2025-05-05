namespace SharedTestData;

public static class TestData
{
    public static readonly IReadOnlyList<string> WithoutEmbeddedXmp = 
    [
        @"test_files/without_embedded_xmp/file1.jpg",
        @"test_files/without_embedded_xmp/file1.png",
        @"test_files/without_embedded_xmp/file1.tif",
        @"test_files/without_embedded_xmp/file1.wav"
    ];

    public static readonly IReadOnlyList<string> WithEmbeddedXmp =
    [
        @"test_files/with_embedded_xmp/BlueSquare.jpg",
        @"test_files/with_embedded_xmp/BlueSquare.png",
        @"test_files/with_embedded_xmp/BlueSquare.tif",
        @"test_files/with_embedded_xmp/BlueSquare.mov",
        @"test_files/with_embedded_xmp/BlueSquare.mp3",
        @"test_files/with_embedded_xmp/BlueSquare.pdf",
        @"test_files/with_embedded_xmp/BlueSquare.wav",
    ];

    public static readonly IReadOnlyList<string> UnsupportedFileType =
    [
        @"test_files/unsupported_file_type/file1.txt",
    ];

    public const string NonExistentFile = "test_files/i_should_not_exist_delete_me.jpg";
}