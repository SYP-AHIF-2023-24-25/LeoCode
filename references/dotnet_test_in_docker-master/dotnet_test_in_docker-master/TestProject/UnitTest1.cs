using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using Xunit;

namespace TestProject
{
    public class UnitTest1
    {
        private readonly Converter _output;

        public UnitTest1()
        {
            this._output = new();
            Console.SetOut(this._output);
        }

        [Fact]
        public void Test1()
        {
            ToBeDockerTested.Program.Main(null);
            var res = this._output.Lines[0];
            res.Should().Be("Hello World!");
        }

        private class Converter : TextWriter
        {
            public List<string> Lines { get; }
            public Converter()
            {
                this.Lines = new();
            }
            public override Encoding Encoding => Encoding.UTF32;

            public override void WriteLine(string message)
            {
                Lines.Add(message);
            }
            public override void WriteLine(string format, params object[] args)
            {
                throw new NotSupportedException();
            }
        }
    }
}
