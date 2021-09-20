﻿using LinqToKB.FirstOrderLogic.InternalUtilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace LinqToKB.FirstOrderLogic.Sentences
{
    /// <summary>
    /// Representation of an predicate sentence of first order logic, In typical FOL syntax, this is written as:
    /// <code>Predicate({term}, ..)</code>
    /// In C#, the equivalent expression acting on the domain (as well as any relevant variables and constants) is a boolean-valued property or method call
    /// on a TElement, or a boolean-valued property or method call on TDomain.
    /// </summary>
    /// <typeparam name="TDomain">The type of the domain.</typeparam>
    /// <typeparam name="TElement">The type that all elements of the domain are assignable to.</typeparam>
    public class Predicate<TDomain, TElement> : Sentence<TDomain, TElement>
        where TDomain : IEnumerable<TElement>
    {
        private readonly MemberInfo member;

        /// <summary>
        /// Initializes a new instance of the <see cref="Predicate{TDomain, TElement}"/> class.
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="arguments">The arguments of this predicate.</param>
        public Predicate(MemberInfo memberInfo, IList<Term<TDomain, TElement>> arguments)
        {
            member = memberInfo;
            Arguments = new ReadOnlyCollection<Term<TDomain, TElement>>(arguments);
        }

        /// <summary>
        /// Gets the name of this predicate.
        /// </summary>
        public string Name => member.Name;

        /// <summary>
        /// Gets the arguments of this predicate.
        /// </summary>
        public ReadOnlyCollection<Term<TDomain, TElement>> Arguments { get; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is Predicate<TDomain, TElement> otherPredicate)
                || !MemberInfoEqualityComparer.Instance.Equals(member, otherPredicate.member)
                || otherPredicate.Arguments.Count != Arguments.Count)
            {
                return false;
            }

            for (int i = 0; i < Arguments.Count; i++)
            {
                if (!Arguments[i].Equals(otherPredicate.Arguments[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();

            hashCode.Add(MemberInfoEqualityComparer.Instance.GetHashCode(member));
            foreach (var argument in Arguments)
            {
                hashCode.Add(argument);
            }

            return hashCode.ToHashCode();
        }
    }
}
