namespace RoslynTester.Helpers.Testing
{
    internal static class Assert
    {
        public static void AreEqual(object one, object two, string message)
        {
            if (!one.Equals(two))
            {
                throw new AssertionException(one, two, message);
            }
        }

        public static void Fail(string message)
        {
            throw new AssertionException(message);
        }
    }
}
