namespace EFCoreExperiments.TestAPI.Helpers;

public static class RandomHelper
{
    private static Random random = new();
    private static string randomLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string RandomWord(int length)
    {
        return new string([.. Enumerable.Repeat(randomLetters, length).Select(s => s[random.Next(s.Length)])]);
    }

    public static string RandomEmail(int length) => $"{RandomWord(length)}@{RandomWord(length)}.com";
}
