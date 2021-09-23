﻿using System;
using System.Collections.Generic;

namespace LinqToKB.FirstOrderLogic.Sentences
{
    /// <summary>
    /// Representation of a disjunction sentence of first order logic. In typical FOL syntax, this is written as:
    /// <code>{sentence} ∨ {sentence}</code>
    /// In LinqToKB, the equivalent expression acting on the domain (as well as any relevant variables and constants) is:
    /// <code>{expression} {|| or |} {expression}</code>
    /// </summary>
    /// <typeparam name="TDomain">The type of the domain.</typeparam>
    /// <typeparam name="TElement">The type that all elements of the domain are assignable to.</typeparam>
    public class Disjunction<TDomain, TElement> : Sentence<TDomain, TElement>
        where TDomain : IEnumerable<TElement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Disjunction{TDomain, TElement}"/> class.
        /// </summary>
        /// <param name="left">The left side of the disjunction.</param>
        /// <param name="right">The right side of the disjunction.</param>
        public Disjunction(Sentence<TDomain, TElement> left, Sentence<TDomain, TElement> right) => (Left, Right) = (left, right);

        /// <summary>
        /// Gets the left side of the disjunction.
        /// </summary>
        public Sentence<TDomain, TElement> Left { get; }

        /// <summary>
        /// Gets the right side of the disjunction.
        /// </summary>
        public Sentence<TDomain, TElement> Right { get; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is Disjunction<TDomain, TElement> otherDisjunction))
            {
                return false;
            }

            (var low, var high) = Left.GetHashCode() < Right.GetHashCode() ? (Left, Right) : (Right, Left);
            (var otherLow, var otherHigh) = otherDisjunction.Left.GetHashCode() < otherDisjunction.Right.GetHashCode() ? (otherDisjunction.Left, otherDisjunction.Right) : (otherDisjunction.Right, otherDisjunction.Left);

            return low.Equals(otherLow) && high.Equals(otherHigh);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            (var low, var high) = Left.GetHashCode() < Right.GetHashCode() ? (Left, Right) : (Right, Left);

            return HashCode.Combine(low, high);
        }
    }
}
