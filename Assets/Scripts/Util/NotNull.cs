/*
NotNull<T> class is distributed under the MIT License

Copyright (c) 2009-2016 Robert Nystrom

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;

namespace Util
{
    /// <summary>
    /// <para>
    /// Wrapper around a reference that ensures the reference is not <c>null</c>. Provides implicit cast operators
    /// to automatically wrap and unwrap values.
    /// </para>
    /// <para>
    /// NotNull{T} can be used as an argument to a method to ensure that no <c>null</c> values are passed to the
    /// method in place of manually throwing on an <see cref="ArgumentNullException"/>. It has an added benefit over
    /// that because using it as an argument type clearly communicates to the caller the expectation of the method.
    /// </para>
    /// </summary>
    /// <typeparam name="T">Type being unwrapped.</typeparam>
    public struct NotNull<T>
    {
        /// <summary>
        /// Automatically unwraps the non-<c>null</c> object being wrapped by this NotNull{T}.
        /// </summary>
        /// <param name="notNull">The wrapper.</param>
        /// <returns>The raw object being wrapped.</returns>
        public static implicit operator T(NotNull<T> notNull)
        {
            return notNull.Value;
        }

        /// <summary>
        /// Automatically wraps an object in a NotNull{T}. Will throw an <see cref="ArgumentNullException"/> exception
        /// if the value being wrapped is <c>null</c>. 
        /// </summary>
        /// <param name="maybeNull">The raw reference to wrap.</param>
        /// <returns>A new NotNull{T} that wraps the value, provided the value is not <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">If <c>value</c> is <c>null</c>.</exception>
        public static implicit operator NotNull<T>(T maybeNull)
        {
            return new NotNull<T>(maybeNull);
        }

        /// <summary>
        /// Gets and sets the non-null references being wrapped by this NotNull{T}.
        /// </summary>
        /// <param name="maybeNull"></param>
        /// <exception cref="ArgumentNullException">If <c>value</c> is <c>null</c>.</exception>
        public T Value => _value;

        /// <summary>
        /// Creates a new wrapper around the given reference.
        /// </summary>
        /// <remarks>Explictly calling the constructor is rarely needed. Usually the implicit cast is simpler.</remarks>
        /// <param name="maybeNull">The reference to wrap.</param>
        /// <exception cref="ArgumentNullException">If <c>value</c> is <c>null</c>.</exception>
        public NotNull(T maybeNull)
        {
            if (maybeNull == null) throw new ArgumentNullException(nameof(maybeNull));

            _value = maybeNull;
        }

        private T _value;
    }
}