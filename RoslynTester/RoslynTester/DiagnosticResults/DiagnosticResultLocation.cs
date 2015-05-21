using System;

namespace RoslynTester.DiagnosticResults
{
    /// <summary>
    ///     Location where the diagnostic appears, as determined by path, line number, and column number.
    /// </summary>
    public struct DiagnosticResultLocation
    {
        public int Column;
        public int Line;
        public string Path;

        public DiagnosticResultLocation(string path, int line, int column)
        {
            if (line < 0 && column < 0)
            {
                throw new ArgumentOutOfRangeException("At least one of line and column must be > 0");
            }
            if (line < -1 || column < -1)
            {
                throw new ArgumentOutOfRangeException("Both line and column must be >= -1");
            }

            Path = path;
            Line = line;
            Column = column;
        }
    }
}