// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using System;

namespace Yoakke.Collections.Intervals
{
    /// <summary>
    /// Represents the upper-bound of an interval.
    /// </summary>
    /// <typeparam name="T">The type of the endpoint value.</typeparam>
    public abstract record UpperBound<T> : IComparable<LowerBound<T>>, IComparable<UpperBound<T>>
    {
        /// <inheritdoc/>
        public int CompareTo(LowerBound<T> other) => BoundComparer<T>.Default.Compare(this, other);

        /// <inheritdoc/>
        public int CompareTo(UpperBound<T> other) => BoundComparer<T>.Default.Compare(this, other);

        /// <summary>
        /// Checks, if this bound is in touching relation with a lower bound.
        /// </summary>
        /// <param name="other">The <see cref="LowerBound{T}"/> to compare with.</param>
        /// <returns>True, if this is touching <paramref name="other"/>.</returns>
        public bool IsTouching(LowerBound<T> other) => BoundComparer<T>.Default.IsTouching(this, other);

        /// <inheritdoc/>
        public override int GetHashCode() => BoundComparer<T>.Default.GetHashCode(this);

        /// <summary>
        /// The touching bound of this one.
        /// </summary>
        public abstract LowerBound<T>? Touching { get; }

        /// <summary>
        /// Unbounded endpoint.
        /// </summary>
        public record Unbounded : UpperBound<T>
        {
            /// <summary>
            /// A singleton instance to use.
            /// </summary>
            public static readonly Unbounded Instance = new();

            /// <inheritdoc/>
            public override LowerBound<T>? Touching => null;
        }

        /// <summary>
        /// Exclusive endpoint.
        /// </summary>
        public record Exclusive(T Value) : UpperBound<T>
        {
            /// <inheritdoc/>
            public override LowerBound<T>? Touching => LowerBound.Inclusive(this.Value);
        }

        /// <summary>
        /// Inclusive endpoint.
        /// </summary>
        public record Inclusive(T Value) : UpperBound<T>
        {
            /// <inheritdoc/>
            public override LowerBound<T>? Touching => LowerBound.Exclusive(this.Value);
        }

        #region Operators

        /// <summary>
        /// Less-than compares two <see cref="UpperBound{T}"/>s.
        /// </summary>
        /// <param name="a">The first <see cref="UpperBound{T}"/> to compare.</param>
        /// <param name="b">The second <see cref="UpperBound{T}"/> to compare.</param>
        /// <returns>True, if <paramref name="a"/> comes before <paramref name="b"/>.</returns>
        public static bool operator <(UpperBound<T> a, UpperBound<T> b) => a.CompareTo(b) < 0;

        /// <summary>
        /// Greater-than compares two <see cref="UpperBound{T}"/>s.
        /// </summary>
        /// <param name="a">The first <see cref="UpperBound{T}"/> to compare.</param>
        /// <param name="b">The second <see cref="UpperBound{T}"/> to compare.</param>
        /// <returns>True, if <paramref name="a"/> comes after <paramref name="b"/>.</returns>
        public static bool operator >(UpperBound<T> a, UpperBound<T> b) => a.CompareTo(b) > 0;

        /// <summary>
        /// Less-than or equals compares two <see cref="UpperBound{T}"/>s.
        /// </summary>
        /// <param name="a">The first <see cref="UpperBound{T}"/> to compare.</param>
        /// <param name="b">The second <see cref="UpperBound{T}"/> to compare.</param>
        /// <returns>True, if <paramref name="a"/> comes before <paramref name="b"/> or they are equal.</returns>
        public static bool operator <=(UpperBound<T> a, UpperBound<T> b) => a.CompareTo(b) <= 0;

        /// <summary>
        /// Greater-than or equals compares two <see cref="UpperBound{T}"/>s.
        /// </summary>
        /// <param name="a">The first <see cref="UpperBound{T}"/> to compare.</param>
        /// <param name="b">The second <see cref="UpperBound{T}"/> to compare.</param>
        /// <returns>True, if <paramref name="a"/> comes after <paramref name="b"/> or they are equal.</returns>
        public static bool operator >=(UpperBound<T> a, UpperBound<T> b) => a.CompareTo(b) >= 0;

        /// <summary>
        /// Less-than compares an <see cref="UpperBound{T}"/> with a <see cref="LowerBound{T}"/>.
        /// </summary>
        /// <param name="a">The <see cref="UpperBound{T}"/> to compare.</param>
        /// <param name="b">The <see cref="LowerBound{T}"/> to compare.</param>
        /// <returns>True, if <paramref name="a"/> comes before <paramref name="b"/>.</returns>
        public static bool operator <(UpperBound<T> a, LowerBound<T> b) => a.CompareTo(b) < 0;

        /// <summary>
        /// Greater-than compares an <see cref="UpperBound{T}"/> with a <see cref="LowerBound{T}"/>.
        /// </summary>
        /// <param name="a">The <see cref="UpperBound{T}"/> to compare.</param>
        /// <param name="b">The <see cref="LowerBound{T}"/> to compare.</param>
        /// <returns>True, if <paramref name="a"/> comes after <paramref name="b"/>.</returns>
        public static bool operator >(UpperBound<T> a, LowerBound<T> b) => a.CompareTo(b) > 0;

        #endregion
    }
}
