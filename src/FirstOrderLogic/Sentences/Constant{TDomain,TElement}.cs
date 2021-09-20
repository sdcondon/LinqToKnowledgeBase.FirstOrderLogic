﻿using LinqToKB.FirstOrderLogic.InternalUtilities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LinqToKB.FirstOrderLogic.Sentences
{
    /// <summary>
    /// Representation of a constant term within a sentence of first order logic.
    /// </summary>
    /// <typeparam name="TDomain">The type of the domain.</typeparam>
    /// <typeparam name="TElement">The type that all elements of the domain are assignable to.</typeparam>
    public class Constant<TDomain, TElement> : Term<TDomain, TElement>
        where TDomain : IEnumerable<TElement>
    {
        private readonly MemberInfo member;

        /// <summary>
        /// Initializes a new instance of the <see cref="Constant{TDomain, TElement}"/> class.
        /// </summary>
        /// <param name="valueExpr">An expression for obtaining the value of the constant.</param>
        public Constant(MemberInfo memberInfo) => member = memberInfo;

        /// <summary>
        /// Gets the name of the constant.
        /// </summary>
        public string Name => member.Name;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is Constant<TDomain, TElement> otherConstant && MemberInfoEqualityComparer.Instance.Equals(otherConstant.member, member);
        }

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(MemberInfoEqualityComparer.Instance.GetHashCode(member));
    }
}
