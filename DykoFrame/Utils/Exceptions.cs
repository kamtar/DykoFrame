using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DykoFrame
{
    class BadUsageException : DykoFrameException
    {
        public BadUsageException() 
            : base("Bad usage")
        {            
        }

        public BadUsageException(string message)
            : base(message)
        {
        }

        public BadUsageException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    class InvalidMethodException : DykoFrameException
    {
        public InvalidMethodException()
            : base("Error in system logic")
        {
        }

        public InvalidMethodException(string message)
            : base(message)
        {
        }

        public InvalidMethodException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    abstract class DykoFrameException : Exception
    {
        public DykoFrameException()
            : base("DykoFrame: Empty exception")
        {           
        }

        public DykoFrameException(string message)
            : base("DykoFrame: " + message)
        {
        }

        public DykoFrameException(string message, Exception inner)
            : base("DykoFrame: " + message, inner)
        {
        }
    }
}
