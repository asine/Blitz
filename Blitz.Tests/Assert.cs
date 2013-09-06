using System;
using System.Diagnostics;

namespace Blitz.Tests
{
    internal static class Assert
    {
        public static void IsNotNull(object component)
        {
            Debug.Assert(component != null);
        }

        private static void IsNotNull(object component, string message)
        {
            Debug.Assert(component != null, message);
        }

        public static void IsFalse(bool condition)
        {
            Debug.Assert(condition == false);
        }

        public static void IsTrue(bool condition)
        {
            Debug.Assert(condition);
        }

        public static void ShouldThrow(Type exceptionType, Action method)
        {
            Exception exception = GetException(method);

            IsNotNull(exception, string.Format("Exception of type[{0}] was not thrown.", exceptionType.FullName));
            Debug.Assert(exceptionType == exception.GetType());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static Exception GetException(Action method)
        {
            Exception exception = null;

            try
            {
                method();
            }
            catch (Exception e)
            {
                exception = e;
            }

            return exception;
        }
    }
}