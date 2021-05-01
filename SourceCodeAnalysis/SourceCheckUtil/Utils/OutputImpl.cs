using System;
using System.IO;

namespace SourceCheckUtil.Utils
{
    // TODO (std_string) : think about using of logger - alternate approach
    internal class OutputImpl
    {
        public OutputImpl(TextWriter output, TextWriter error, Boolean verbose)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            if (error == null)
                throw new ArgumentNullException(nameof(error));
            _output = output;
            _error = error;
            _verbose = verbose;
        }

        public void WriteOutputLine(String value)
        {
            if (_verbose)
                _output.WriteLine(value);
        }

        public void WriteOutputLine(String filename, Int32 line, String value)
        {
            // line is zero-based
            if (_verbose)
                _output.WriteLine($"{filename}({line + 1}): {value}");
        }

        public void WriteOutputLine()
        {
            if (_verbose)
                _output.WriteLine();
        }

        public void WriteErrorLine(String value)
        {
            _error.WriteLine(value);
        }

        public void WriteErrorLine(String filename, Int32 line, String value)
        {
            // line is zero-based
            _error.WriteLine($"{filename}({line + 1}): {value}");
        }

        public void WriteErrorLine()
        {
            _error.WriteLine();
        }

        private readonly TextWriter _output;
        private readonly TextWriter _error;
        private readonly Boolean _verbose;
    }
}
