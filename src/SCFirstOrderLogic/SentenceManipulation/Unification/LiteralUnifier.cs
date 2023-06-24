﻿// Copyright (c) 2021-2023 Simon Condon.
// You may use this file in accordance with the terms of the MIT license.
using SCFirstOrderLogic;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SCFirstOrderLogic.SentenceManipulation.Unification
{
    /// <summary>
    /// <para>
    /// Utility class for creating unifiers for literals.
    /// </para>
    /// <para>
    /// This implementation includes an occurs check.
    /// See §9.2.2 ("Unification") of 'Artificial Intelligence: A Modern Approach' for an explanation of this algorithm.
    /// </para>
    /// </summary>
    // TODO-V5-BREAKING: rename me, now that methods for unification of Terms etc are now public. Perhaps just Unifier? MostGeneralUnifier? UnifierFactory?
    public static class LiteralUnifier
    {
        /// <summary>
        /// Attempts to create the most general unifier for two literals.
        /// </summary>
        /// <param name="x">One of the two literals to attempt to create a unifier for.</param>
        /// <param name="y">One of the two literals to attempt to create a unifier for.</param>
        /// <param name="unifier">If the literals can be unified, this out parameter will be the unifier - a transformation that will yield identical results when applied to both literals.</param>
        /// <returns>True if the two literals can be unified, otherwise false.</returns>
        public static bool TryCreate(Literal x, Literal y, [MaybeNullWhen(returnValue: false)] out VariableSubstitution unifier)
        {
            var unifierAttempt = new VariableSubstitution();

            if (TryUpdateUnsafe(x, y, unifierAttempt))
            {
                unifier = unifierAttempt;
                return true;
            }

            unifier = null;
            return false;
        }

        /// <summary>
        /// Attempts to update a unifier so that it (also) unifies two given literals.
        /// </summary>
        /// <param name="x">One of the two literals to attempt to unify.</param>
        /// <param name="y">One of the two literals to attempt to unify.</param>
        /// <param name="unifier">The unifier to update. Will be updated to refer to a new unifier on success, or be unchanged on failure.</param>
        /// <returns>True if the two literals can be unified, otherwise false.</returns>
        public static bool TryUpdate(Literal x, Literal y, ref VariableSubstitution unifier)
        {
            var updatedUnifier = new VariableSubstitution(unifier);

            if (TryUpdateUnsafe(x, y, updatedUnifier))
            {
                unifier = updatedUnifier;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to update a unifier so that it (also) unifies two given literals.
        /// </summary>
        /// <param name="x">One of the two literals to attempt to unify.</param>
        /// <param name="y">One of the two literals to attempt to unify.</param>
        /// <param name="unifier">The unifier to update. Will be unchanged on failure.</param>
        /// <returns>True if the two literals can be unified, otherwise false.</returns>
        public static bool TryUpdate(Literal x, Literal y, VariableSubstitution unifier)
        {
            var updatedUnifier = new VariableSubstitution(unifier);

            if (TryUpdateUnsafe(x, y, updatedUnifier))
            {
                // TODO-PERFORMANCE: Ugh. slower than is needed. Refactor this whole class at some point - after adding some good benchmarks..
                foreach (var binding in updatedUnifier.Bindings)
                {
                    if (!unifier.Bindings.ContainsKey(binding.Key))
                    {
                        unifier.AddBinding(binding.Key, binding.Value);
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to update a unifier (in place) so that it (also) unifies two given literals. Faster than <see cref="TryUpdate(Literal, Literal, VariableSubstitution)"/>,
        /// but can partially update the substitution on failure. Use only when you don't care about the substitution being modified if it fails.
        /// </summary>
        /// <param name="x">One of the two literals to attempt to unify.</param>
        /// <param name="y">One of the two literals to attempt to unify.</param>
        /// <param name="unifier">The unifier to update. NB: Can be partially updated on failure.</param>
        /// <returns>True if the two literals can be unified, otherwise false.</returns>
        public static bool TryUpdateUnsafe(Literal x, Literal y, VariableSubstitution unifier)
        {
            if (x.IsNegated != y.IsNegated 
                || !x.Predicate.Symbol.Equals(y.Predicate.Symbol)
                || x.Predicate.Arguments.Count != y.Predicate.Arguments.Count)
            {
                return false;
            }

            foreach (var args in x.Predicate.Arguments.Zip(y.Predicate.Arguments, (x, y) => (x, y)))
            {
                if (!TryUpdateUnsafe(args.x, args.y, unifier))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Attempts to update a unifier (in place) so that it (also) unifies two given terms. Can partially update the substitution on failure.
        /// Use only when you don't care about the substitution being modified if it fails.
        /// </summary>
        /// <param name="x">One of the two terms to attempt to unify.</param>
        /// <param name="y">One of the two terms to attempt to unify.</param>
        /// <param name="unifier">The unifier to update. NB: Can be partially updated on failure.</param>
        /// <returns>True if the two terms can be unified, otherwise false.</returns>
        public static bool TryUpdateUnsafe(Term x, Term y, VariableSubstitution unifier)
        {
            return (x, y) switch
            {
                (VariableReference variable, _) => TryUpdateUnsafe(variable, y, unifier),
                (_, VariableReference variable) => TryUpdateUnsafe(variable, x, unifier),
                (Function functionX, Function functionY) => TryUpdateUnsafe(functionX, functionY, unifier),
                // Below, the only potential for equality is if they're both constants. Perhaps worth testing this
                // versus that explicitly and a default that just returns false. Similar from a performance
                // perspective.
                _ => x.Equals(y),
            };
        }

        /// <summary>
        /// Attempts to update a unifier (in place) so that it (also) binds a particular variable to a particular term. Can partially update the substitution on failure.
        /// Use only when you don't care about the substitution being modified if it fails.
        /// </summary>
        /// <param name="variable">The variable reference to bind.</param>
        /// <param name="other">The term to bind the variable reference to.</param>
        /// <param name="unifier">The unifier to update. NB: Can be partially updated on failure.</param>
        /// <returns>True if the variable can be consistently bound to the term, otherwise false.</returns>
        public static bool TryUpdateUnsafe(VariableReference variable, Term other, VariableSubstitution unifier)
        {
            if (variable.Equals(other))
            {
                return true;
            }
            else if (unifier.Bindings.TryGetValue(variable, out var variableValue))
            {
                // The variable is already mapped to something - we need to make sure that the
                // mapping is consistent with the "other" value.
                return TryUpdateUnsafe(variableValue, other, unifier);
            }
            else if (other is VariableReference otherVariable && unifier.Bindings.TryGetValue(otherVariable, out var otherVariableValue))
            {
                // The other value is also a variable that is already mapped to something - we need to make sure that the
                // mapping is consistent with the "other" value.
                return TryUpdateUnsafe(variable, otherVariableValue, unifier);
            }
            else if (Occurs(variable, unifier.ApplyTo(other)))
            {
                // We'd be adding a cycle if we added this binding - so fail instead.
                return false;
            }
            else
            {
                unifier.AddBinding(variable, other);
                return true;
            }
        }

        private static bool TryUpdateUnsafe(Function x, Function y, VariableSubstitution unifier)
        {
            if (!x.Symbol.Equals(y.Symbol) || x.Arguments.Count != y.Arguments.Count)
            {
                return false;
            }

            foreach (var args in x.Arguments.Zip(y.Arguments, (x, y) => (x, y)))
            {
                if (!TryUpdateUnsafe(args.x, args.y, unifier))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool Occurs(VariableReference variableReference, Term term)
        {
            return term switch
            {
                Constant c => false,
                VariableReference v => variableReference.Equals(v),
                Function f => f.Arguments.Any(a => Occurs(variableReference, a)),
                _ => throw new ArgumentException($"Unexpected term type '{term.GetType()}' encountered", nameof(term)),
            };
        }
    }
}
