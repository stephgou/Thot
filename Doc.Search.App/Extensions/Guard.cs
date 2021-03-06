﻿/// <summary>
/// Validation of argument static class and methods
/// <credits>This class was developed by Orktane http://www.orktane.com/Blog/Default.aspx and is included in nRoute Framework http://nroute.codeplex.com
/// </credits>
/// <summary>
using System;
using System.Diagnostics;
using System.Globalization;


namespace Doc.Search.App.Extensions
{
    internal static class Guard
    {
        private const string PARAMETER_NOT_VALID = "The parameter '{0}' value is not valid";
        private const string PARAMETER_NOT_NULLOREMPTY = "The parameter '{0}' cannot be null or empty";
        private const string PARAMETER_MUSTBE_OFTYPE = "The parameter '{0}' must be of type '{1}";

        // ArgumentNotNull

        [DebuggerStepThrough]
        public static void ArgumentNotNull<T>(T value, string parameterName)
            where T
                : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentNotNull<T>(T value, string parameterName, string message)
            where T
                : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName, message);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentNotNull<T>(T value, string parameterName, string messageFormat, params Object[] messageArgs)
            where T
                : class
        {
            ArgumentNotNull<T>(value, parameterName, string.Format(CultureInfo.CurrentCulture, messageFormat, messageArgs));
        }

        // ArgumentNotDefault

        [DebuggerStepThrough]
        public static void ArgumentNotDefault<T>(T value, string parameterName)
        {
            if (Object.Equals(value, default(T)))
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentNotDefault<T>(T value, string parameterName, string message)
        {
            if (Object.Equals(value, default(T)))
            {
                throw new ArgumentNullException(parameterName, message);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentNotDefault<T>(T value, string parameterName, string messageFormat, params Object[] messageArgs)
        {
            ArgumentNotDefault<T>(value, parameterName, string.Format(CultureInfo.CurrentCulture, messageFormat, messageArgs));
        }

        // ArgumentNotNullOrEmpty

        [DebuggerStepThrough]
        public static void ArgumentNotNullOrEmpty(string value, string parameterName)
        {
            ArgumentNotNullOrEmpty(value, parameterName, string.Format(CultureInfo.CurrentCulture,
                PARAMETER_NOT_NULLOREMPTY, parameterName));
        }

        [DebuggerStepThrough]
        public static void ArgumentNotNullOrEmpty(string value, string parameterName, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(message, parameterName);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentNotNullOrEmpty(string value, string parameterName, string messageFormat, params Object[] messageArgs)
        {
            ArgumentNotNullOrEmpty(value, parameterName, string.Format(CultureInfo.CurrentCulture, messageFormat, messageArgs));
        }

        // ArgumentNotNullOrWhiteSpace

        [DebuggerStepThrough]
        public static void ArgumentNotNullOrWhiteSpace(string value, string parameterName)
        {
            ArgumentNotNullOrWhiteSpace(value, parameterName, string.Format(CultureInfo.CurrentCulture,
                    PARAMETER_NOT_NULLOREMPTY, parameterName));
        }

        [DebuggerStepThrough]
        public static void ArgumentNotNullOrWhiteSpace(string value, string parameterName, string message)
        {
            if (value == null || string.IsNullOrEmpty(value.Trim()))
            {
                throw new ArgumentNullException(message, parameterName);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentNotNullOrWhiteSpace(string value, string parameterName, string messageFormat, params Object[] messageArgs)
        {
            ArgumentNotNullOrWhiteSpace(value, parameterName, string.Format(CultureInfo.CurrentCulture, messageFormat, messageArgs));
        }

        // ArgumentOutOfRange

        [DebuggerStepThrough]
        public static void ArgumentOutOfRange(bool outOfRange, string parameterName)
        {
            if (outOfRange)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentOutOfRange(bool outOfRange, string parameterName, string message)
        {
            if (outOfRange)
            {
                throw new ArgumentOutOfRangeException(parameterName, message);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentOutOfRange(bool outOfRange, string parameterName, string messageFormat, params Object[] messageArgs)
        {
            ArgumentOutOfRange(outOfRange, parameterName, string.Format(CultureInfo.CurrentCulture, messageFormat, messageArgs));
        }

        // ArgumentValue

        [DebuggerStepThrough]
        public static void ArgumentIsType(object argument, Type type, string parameterName)
        {
            ArgumentIsType(argument, type, parameterName, string.Format(CultureInfo.CurrentCulture,
                    PARAMETER_MUSTBE_OFTYPE, parameterName, type.FullName));
        }

        [DebuggerStepThrough]
        public static void ArgumentIsType(object argument, Type type, string parameterName, string message)
        {
            //if (argument == null || !type.IsAssignableFrom(argument.GetType()))
            if (argument == null)
                {
                    throw new ArgumentException(message, parameterName);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentIsType(object argument, Type type, string parameterName, string messageFormat,
            params Object[] messageArgs)
        {
            ArgumentIsType(argument, type, parameterName, string.Format(CultureInfo.CurrentCulture, messageFormat, messageArgs));
        }

        // ArgumentValue

        [DebuggerStepThrough]
        public static void ArgumentValue(bool throwException, string parameterName)
        {
            ArgumentValue(throwException, parameterName, string.Format(CultureInfo.CurrentCulture,
                PARAMETER_NOT_VALID, parameterName));
        }

        [DebuggerStepThrough]
        public static void ArgumentValue(bool throwException, string parameterName, string message)
        {
            if (throwException)
            {
                throw new ArgumentException(message, parameterName);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentValue(bool throwException, string parameterName, string messageFormat, params Object[] messageArgs)
        {
            ArgumentValue(throwException, parameterName, string.Format(CultureInfo.CurrentCulture, messageFormat, messageArgs));
        }
    }
}
