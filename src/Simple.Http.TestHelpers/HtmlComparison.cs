namespace Simple.Http.TestHelpers
{
    public static class HtmlComparison
    {
        public static string Cleanse(string result)
        {
            return result.Trim().Replace("\n", "").Replace("\r", "");
        }
    }
}
