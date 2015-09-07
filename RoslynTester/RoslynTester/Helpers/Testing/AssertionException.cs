using System;
using System.Diagnostics;

namespace RoslynTester.Helpers.Testing
{
    /// <summary>
    ///     An expected outcome is different from the actual outcome.
    /// </summary>
    [DebuggerDisplay("ToString()")]
    public class AssertionException : Exception
    {
        private readonly object _expected, _result;
        private readonly string _message;

        public AssertionException(object expected, object result, string message) : this(message)
        {
            _expected = expected;
            _result = result;
        }

        public AssertionException(string message) : base(message)
        {
            _message = message;
        }

        public override string ToString()
        {
            if (_expected != null && _result != null)
            {
                return $"Message:\t{_message}\nExpected:\t{_expected}\nResult:\t{_result}\n";
            }
            return $"Message:\t{_message}";
        }
    }
}