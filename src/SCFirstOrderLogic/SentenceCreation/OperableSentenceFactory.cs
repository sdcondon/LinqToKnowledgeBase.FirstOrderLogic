﻿// Copyright (c) 2021-2025 Simon Condon.
// You may use this file in accordance with the terms of the MIT license.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SCFirstOrderLogic.SentenceCreation;

/// <summary>
/// <para>
/// Shorthand static factory methods for <see cref="Sentence"/> instances. Intended to be used with a 'using static' directive to make method invocations acceptably succinct:
/// <code>using static SCFirstOrderLogic.SentenceCreation.OperableSentenceFactory;</code>
/// This class provides a compromise between <see cref="SentenceFactory"/> and the full language integration provided by the types in the LanguageIntegration namespace.
/// The objects returned by this factory (are implicitly convertible to <see cref="Sentence"/>s and) override operators in a way that is perhaps intuitive: | for disjunctions, &amp; for conjunctions, ! for negations, and == for the equality predicate.
/// For domain-specific sentence elements (predicates &amp; functions), their operable surrogate is implicitly convertible from (and to) the regular type, so the recommendation is to create appropriate properties and methods to create them - declaring
/// the type as Operable.. so that they can be operated on. For example:
/// <code>OperablePredicate MyBinaryPredicate(Term arg1, Term arg2) => new Predicate(nameof(MyBinaryPredicate), arg1, arg2);</code>
/// ..which means you can then write things like:
/// <code>ForAll(X, ThereExists(Y, !MyBinaryPredicate(X, Y) | MyOtherPredicate(Y)));</code>
/// </para>
/// <para>
/// <strong>N.B. #1:</strong> The real sentence classes do not define these operators to keep them as lean and mean as possible.
/// </para>
/// <para>
/// <strong>N.B. #2:</strong> It's also probably worth noting that this approach's reliance on implicit conversion will result in some memory overhead compared to using SentenceFactory -
/// as each conversion creates a new object. Probably not something worth worrying about 99.9% of the time, but.. the more you know.
/// </para>
/// </summary>
public static class OperableSentenceFactory
{
    /// <summary>
    /// Shorthand factory method for a new <see cref="OperableUniversalQuantification"/> instance.
    /// </summary>
    /// <param name="variableDeclaration">The variable declaration.</param>
    /// <param name="sentence">The body sentence that refers to the declared variable.</param>
    /// <returns>A new <see cref="OperableUniversalQuantification"/> instance.</returns>
    public static OperableSentence ForAll(OperableVariableDeclaration variableDeclaration, OperableSentence sentence) =>
        new OperableUniversalQuantification(variableDeclaration, sentence);

    /// <summary>
    /// Shorthand factory method for a (tree of) new <see cref="OperableUniversalQuantification"/> instances that declares two universally quantified variables.
    /// </summary>
    /// <param name="variableDeclaration1">The first variable declaration.</param>
    /// <param name="variableDeclaration2">The second variable declaration.</param>
    /// <param name="sentence">The body sentence that refers to the declared variables.</param>
    /// <returns>A new <see cref="OperableUniversalQuantification"/> instance.</returns>
    public static OperableSentence ForAll(OperableVariableDeclaration variableDeclaration1, OperableVariableDeclaration variableDeclaration2, OperableSentence sentence) =>
        new OperableUniversalQuantification(variableDeclaration1, new OperableUniversalQuantification(variableDeclaration2, sentence));

    /// <summary>
    /// Shorthand factory method for a (tree of) new <see cref="OperableUniversalQuantification"/> instances that declares three universally quantified variables.
    /// </summary>
    /// <param name="variableDeclaration1">The first variable declaration.</param>
    /// <param name="variableDeclaration2">The second variable declaration.</param>
    /// <param name="variableDeclaration3">The third variable declaration.</param>
    /// <param name="sentence">The body sentence that refers to the declared variables.</param>
    /// <returns>A new <see cref="OperableUniversalQuantification"/> instance.</returns>
    public static OperableSentence ForAll(OperableVariableDeclaration variableDeclaration1, OperableVariableDeclaration variableDeclaration2, OperableVariableDeclaration variableDeclaration3, OperableSentence sentence) =>
        new OperableUniversalQuantification(variableDeclaration1, new OperableUniversalQuantification(variableDeclaration2, new OperableUniversalQuantification(variableDeclaration3, sentence)));

    /// <summary>
    /// Shorthand factory method for a new <see cref="OperableExistentialQuantification"/> instance.
    /// </summary>
    /// <param name="variableDeclaration">The variable declaration.</param>
    /// <param name="sentence">The body sentence that refers to the declared variable.</param>
    /// <returns>A new <see cref="OperableExistentialQuantification"/> instance.</returns>
    public static OperableSentence ThereExists(OperableVariableDeclaration variableDeclaration, OperableSentence sentence) =>
        new OperableExistentialQuantification(variableDeclaration, sentence);

    /// <summary>
    /// Shorthand factory method for a (tree of) new <see cref="OperableExistentialQuantification"/> instances that declares two universally quantified variables.
    /// </summary>
    /// <param name="variableDeclaration1">The first variable declaration.</param>
    /// <param name="variableDeclaration2">The second variable declaration.</param>
    /// <param name="sentence">The body sentence that refers to the declared variables.</param>
    /// <returns>A new <see cref="OperableExistentialQuantification"/> instance.</returns>
    public static OperableSentence ThereExists(OperableVariableDeclaration variableDeclaration1, OperableVariableDeclaration variableDeclaration2, OperableSentence sentence) =>
        new OperableExistentialQuantification(variableDeclaration1, new OperableUniversalQuantification(variableDeclaration2, sentence));

    /// <summary>
    /// Shorthand factory method for a (tree of) new <see cref="OperableExistentialQuantification"/> instances that declares three universally quantified variables.
    /// </summary>
    /// <param name="variableDeclaration1">The first variable declaration.</param>
    /// <param name="variableDeclaration2">The second variable declaration.</param>
    /// <param name="variableDeclaration3">The third variable declaration.</param>
    /// <param name="sentence">The body sentence that refers to the declared variables.</param>
    /// <returns>A new <see cref="OperableExistentialQuantification"/> instance.</returns>
    public static OperableSentence ThereExists(OperableVariableDeclaration variableDeclaration1, OperableVariableDeclaration variableDeclaration2, OperableVariableDeclaration variableDeclaration3, OperableSentence sentence) =>
        new OperableExistentialQuantification(variableDeclaration1, new OperableExistentialQuantification(variableDeclaration2, new OperableExistentialQuantification(variableDeclaration3, sentence)));

    /// <summary>
    /// Shorthand factory method for a new <see cref="OperableImplication"/> instance.
    /// </summary>
    /// <param name="antecedent">The antecedent sentence of the implication.</param>
    /// <param name="consequent">The consequent sentence of the implication.</param>
    /// <returns>A new <see cref="OperableImplication"/> instance.</returns>
    public static OperableSentence If(OperableSentence antecedent, OperableSentence consequent) =>
        new OperableImplication(antecedent, consequent);

    /// <summary>
    /// Shorthand factory method for a new <see cref="OperableEquivalence"/> instance.
    /// </summary>
    /// <param name="left">The left-hand operand of the equivalence.</param>
    /// <param name="right">The right-hand operand of the equivalence.</param>
    /// <returns>A new <see cref="OperableEquivalence"/> instance.</returns>
    public static OperableSentence Iff(OperableSentence left, OperableSentence right) =>
        new OperableEquivalence(left, right);

    /// <summary>
    /// Shorthand factory method for a new <see cref="OperablePredicate"/> instance with the <see cref="EqualityIdentifier.Instance"/> identifier.
    /// </summary>
    /// <param name="left">The left-hand operand of the equality.</param>
    /// <param name="right">The right-hand operand of the equality.</param>
    /// <returns>A new <see cref="OperablePredicate"/> instance.</returns>
    public static OperableSentence AreEqual(OperableTerm left, OperableTerm right) =>
        new OperablePredicate(EqualityIdentifier.Instance, left, right);

    /// <summary>
    /// Shorthand factory method for a new <see cref="OperableVariableDeclaration"/> instance.
    /// </summary>
    /// <param name="identifier">The identifier of the variable.</param>>
    /// <returns>A new <see cref="OperableVariableDeclaration"/> instance.</returns>
    public static OperableVariableDeclaration Var(object identifier) => new(identifier);

    #region VariableDeclarations

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "A".
    /// </summary>
    public static OperableVariableDeclaration A { get; } = new(nameof(A));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "B".
    /// </summary>
    public static OperableVariableDeclaration B { get; } = new(nameof(B));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "C".
    /// </summary>
    public static OperableVariableDeclaration C { get; } = new(nameof(C));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "D".
    /// </summary>
    public static OperableVariableDeclaration D { get; } = new(nameof(D));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "E".
    /// </summary>
    public static OperableVariableDeclaration E { get; } = new(nameof(E));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "F".
    /// </summary>
    public static OperableVariableDeclaration F { get; } = new(nameof(F));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "G".
    /// </summary>
    public static OperableVariableDeclaration G { get; } = new(nameof(G));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "H".
    /// </summary>
    public static OperableVariableDeclaration H { get; } = new(nameof(H));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "I".
    /// </summary>
    public static OperableVariableDeclaration I { get; } = new(nameof(I));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "J".
    /// </summary>
    public static OperableVariableDeclaration J { get; } = new(nameof(J));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "K".
    /// </summary>
    public static OperableVariableDeclaration K { get; } = new(nameof(K));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "L".
    /// </summary>
    public static OperableVariableDeclaration L { get; } = new(nameof(L));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "M".
    /// </summary>
    public static OperableVariableDeclaration M { get; } = new(nameof(M));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "N".
    /// </summary>
    public static OperableVariableDeclaration N { get; } = new(nameof(N));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "O".
    /// </summary>
    public static OperableVariableDeclaration O { get; } = new(nameof(O));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "P".
    /// </summary>
    public static OperableVariableDeclaration P { get; } = new(nameof(P));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "Q".
    /// </summary>
    public static OperableVariableDeclaration Q { get; } = new(nameof(Q));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "R".
    /// </summary>
    public static OperableVariableDeclaration R { get; } = new(nameof(R));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "S".
    /// </summary>
    public static OperableVariableDeclaration S { get; } = new(nameof(S));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "T".
    /// </summary>
    public static OperableVariableDeclaration T { get; } = new(nameof(T));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "U".
    /// </summary>
    public static OperableVariableDeclaration U { get; } = new(nameof(U));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "V".
    /// </summary>
    public static OperableVariableDeclaration V { get; } = new(nameof(V));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "W".
    /// </summary>
    public static OperableVariableDeclaration W { get; } = new(nameof(W));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "X".
    /// </summary>
    public static OperableVariableDeclaration X { get; } = new(nameof(X));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "Y".
    /// </summary>
    public static OperableVariableDeclaration Y { get; } = new(nameof(Y));

    /// <summary>
    /// Gets a <see cref="OperableVariableDeclaration"/> for a variable with the identifier "Z".
    /// </summary>
    public static OperableVariableDeclaration Z { get; } = new(nameof(Z));

    #endregion

    /// <summary>
    /// Surrogate for <see cref="Sentence"/> instances that defines |, &amp; and ! operators to create disjunctions, conjunctions and negations respectively.
    /// Instances are implicitly convertible to the equivalent <see cref="Sentence"/> instance.
    /// </summary>
    public abstract class OperableSentence
    {
        /// <summary>
        /// Composes two <see cref="OperableSentence"/> instances together by forming a conjunction from them.
        /// </summary>
        /// <param name="left">The left-hand operand of the conjunction.</param>
        /// <param name="right">The right-hand operand of the conjunction.</param>
        /// <returns>A new <see cref="OperableConjunction"/> instance.</returns>
        public static OperableSentence operator &(OperableSentence left, OperableSentence right) => new OperableConjunction(left, right);

        /// <summary>
        /// Composes two <see cref="OperableSentence"/> instances together by forming a disjunction from them.
        /// </summary>
        /// <param name="left">The left-hand operand of the disjunction.</param>
        /// <param name="right">The right-hand operand of the disjunction.</param>
        /// <returns>A new <see cref="OperableDisjunction"/> instance.</returns>
        public static OperableSentence operator |(OperableSentence left, OperableSentence right) => new OperableDisjunction(left, right);

        /// <summary>
        /// Creates the negation of an <see cref="OperableSentence"/> instance.
        /// </summary>
        /// <param name="operand">The sentence to negate.</param>
        /// <returns>A new <see cref="OperableNegation"/> instance.</returns>
        public static OperableSentence operator !(OperableSentence operand) => new OperableNegation(operand);

        /// <summary>
        /// Implicitly converts an <see cref="OperableSentence"/> instance to an equivalent <see cref="Sentence"/>.
        /// </summary>
        /// <param name="sentence">The sentence to convert.</param>
        public static implicit operator Sentence(OperableSentence sentence) => sentence switch
        {
            null => throw new ArgumentNullException(nameof(sentence)),
            OperableConjunction conjunction => new Conjunction(conjunction.Left, conjunction.Right),
            OperableDisjunction disjunction => new Disjunction(disjunction.Left, disjunction.Right),
            OperableEquivalence equivalence => new Equivalence(equivalence.Left, equivalence.Right),
            OperableExistentialQuantification existentialQuantification => new ExistentialQuantification(existentialQuantification.Variable, existentialQuantification.Sentence),
            OperableImplication implication => new Implication(implication.Antecedent, implication.Consequent),
            OperableNegation negation => new Negation(negation.Sentence),
            OperablePredicate predicate => new Predicate(predicate.Identifier, predicate.Arguments.Select(a => (Term)a).ToArray()),
            OperableUniversalQuantification universalQuantification => new UniversalQuantification(universalQuantification.Variable, universalQuantification.Sentence),
            _ => throw new ArgumentException($"Unsupported OperableSentence type '{sentence.GetType()}'", nameof(sentence))
        };
    }

    /// <summary>
    /// Surrogate for <see cref="Conjunction"/> instances that derives from <see cref="OperableSentence"/> and can thus be operated on with the |, &amp; and ! operators
    /// to create disjunctions, conjunctions and negations respectively. NB constructor is intentionally not public - should only be created via the &amp; operator acting on
    /// <see cref="OperableSentence"/> instances.
    /// </summary>
    public sealed class OperableConjunction : OperableSentence
    {
        internal OperableConjunction(OperableSentence left, OperableSentence right) => (Left, Right) = (left, right);

        internal OperableSentence Left { get; }

        internal OperableSentence Right { get; }
    }

    /// <summary>
    /// Surrogate for <see cref="Conjunction"/> instances that derives from <see cref="OperableSentence"/> and can thus be operated on with |, &amp; and ! operators
    /// to create disjunctions, conjunctions and negations respectively. N.B. constructor is intentionally not public - should only be created via the | operator acting on
    /// <see cref="OperableSentence"/> instances.
    /// </summary>
    public sealed class OperableDisjunction : OperableSentence
    {
        internal OperableDisjunction(OperableSentence left, OperableSentence right) => (Left, Right) = (left, right);

        internal OperableSentence Left { get; }

        internal OperableSentence Right { get; }
    }

    /// <summary>
    /// Surrogate for <see cref="Equivalence"/> instances that derives from <see cref="OperableSentence"/> and can thus be operated on with |, &amp; and ! operators
    /// to create disjunctions, conjunctions and negations respectively. N.B. constructor is intentionally not public - should only be created via the
    /// <see cref="Iff"/> method.
    /// </summary>
    public sealed class OperableEquivalence : OperableSentence
    {
        internal OperableEquivalence(OperableSentence left, OperableSentence right) => (Left, Right) = (left, right);

        internal OperableSentence Left { get; }

        internal OperableSentence Right { get; }
    }

    /// <summary>
    /// Surrogate for <see cref="ExistentialQuantification"/> instances that derives from <see cref="OperableSentence"/> and can thus be operated on with |, &amp; and ! operators
    /// to create disjunctions, conjunctions and negations respectively. N.B. constructor is intentionally not public - should only be created via the
    /// <see cref="ThereExists(OperableVariableDeclaration, OperableSentence)"/> method (or its overrides).
    /// </summary>
    public sealed class OperableExistentialQuantification : OperableSentence
    {
        internal OperableExistentialQuantification(OperableVariableDeclaration variable, OperableSentence sentence) => (Variable, Sentence) = (variable, sentence);

        internal OperableVariableDeclaration Variable { get; }

        internal OperableSentence Sentence { get; }
    }

    /// <summary>
    /// Surrogate for <see cref="Function"/> instances that derives from <see cref="OperableTerm"/> and can thus be operated on with the  == operator
    /// to create equality predicate instances. N.B. constructor is intentionally not public - can be implicitly converted from (and to) <see cref="Function"/>
    /// instances. E.g.
    /// <code>OperableFunction MyFunction(OperableTerm arg1) => new Function(nameof(MyFunction), arg1);</code>
    /// </summary>
    public class OperableFunction : OperableTerm
    {
        internal OperableFunction(object identifier, params OperableTerm[] arguments)
            : this(identifier, (IList<OperableTerm>)arguments)
        {
        }

        internal OperableFunction(object identifier, IList<OperableTerm> arguments)
        {
            Identifier = identifier;
            Arguments = new ReadOnlyCollection<OperableTerm>(arguments);
        }

        internal ReadOnlyCollection<OperableTerm> Arguments { get; }

        internal object Identifier { get; }

        /// <summary>
        /// Implicitly converts a <see cref="Function"/> instance to an equivalent <see cref="OperableFunction"/>.
        /// </summary>
        /// <param name="function">The function to convert.</param>
        public static implicit operator OperableFunction(Function function) => new(function.Identifier, function.Arguments.Select(a => (OperableTerm)a).ToArray());

        /// <summary>
        /// Implicitly converts an <see cref="OperableFunction"/> instance to an equivalent <see cref="Function"/>.
        /// </summary>
        /// <param name="function">The function to convert.</param>
        public static implicit operator Function(OperableFunction function) => new(function.Identifier, function.Arguments.Select(a => (Term)a).ToArray());

        /// <summary>
        /// Implicitly converts an <see cref="OperableFunction"/> instance to an equivalent <see cref="Term"/>.
        /// </summary>
        /// <param name="function">The function to convert.</param>
        public static implicit operator Term(OperableFunction function) => new Function(function.Identifier, function.Arguments.Select(a => (Term)a).ToArray());
    }

    /// <summary>
    /// Surrogate for <see cref="Implication"/> instances that derives from <see cref="OperableSentence"/> and can thus be operated on with |, &amp; and ! operators
    /// to create disjunctions, conjunctions and negations respectively. N.B. constructor is intentionally not public - should only be created via the
    /// <see cref="If"/> method.
    /// </summary>
    public sealed class OperableImplication : OperableSentence
    {
        internal OperableImplication(OperableSentence antecedent, OperableSentence consequent) => (Antecedent, Consequent) = (antecedent, consequent);

        internal OperableSentence Antecedent { get; }

        internal OperableSentence Consequent { get; }
    }

    /// <summary>
    /// Surrogate for <see cref="Negation"/> instances that derives from <see cref="OperableSentence"/> and can thus be operated on with |, &amp; and ! operators
    /// to create disjunctions, conjunctions and negations respectively. N.B. constructor is intentionally not public - should only be created via the ! operator acting on
    /// <see cref="OperableSentence"/> instances.
    /// </summary>
    public sealed class OperableNegation : OperableSentence
    {
        internal OperableNegation(OperableSentence sentence) => Sentence = sentence;

        internal OperableSentence Sentence { get; }
    }

    /// <summary>
    /// Surrogate for <see cref="Predicate"/> instances that derives from <see cref="OperableSentence"/> and can thus be operated on with |, &amp; and ! operators
    /// to create disjunctions, conjunctions and negations respectively. N.B. constructor is intentionally not public - can be implicitly converted from (and to) <see cref="Predicate"/>
    /// instances. E.g.
    /// <code>OperablePredicate MyPredicate(OperableTerm arg1) => new Predicate(nameof(MyPredicate), arg1);</code>
    /// </summary>
    public class OperablePredicate : OperableSentence
    {
        internal OperablePredicate(object identifier, params OperableTerm[] arguments)
            : this(identifier, (IList<OperableTerm>)arguments)
        {
        }

        internal OperablePredicate(object identifier, IList<OperableTerm> arguments)
        {
            Identifier = identifier;
            Arguments = new ReadOnlyCollection<OperableTerm>(arguments);
        }

        internal ReadOnlyCollection<OperableTerm> Arguments { get; }

        internal object Identifier { get; }

        /// <summary>
        /// Implicitly converts a <see cref="Predicate"/> instance to an equivalent <see cref="OperablePredicate"/>.
        /// </summary>
        /// <param name="predicate">The predicate to convert.</param>
        public static implicit operator OperablePredicate(Predicate predicate) => new(predicate.Identifier, predicate.Arguments.Select(a => (OperableTerm)a).ToArray());

        /// <summary>
        /// Implicitly converts an <see cref="OperablePredicate"/> instance to an equivalent <see cref="Predicate"/>.
        /// </summary>
        /// <param name="predicate">The operable predicate to convert.</param>
        public static implicit operator Predicate(OperablePredicate predicate) => new(predicate.Identifier, predicate.Arguments.Select(a => (Term)a).ToArray());

        /// <summary>
        /// Implicitly converts an <see cref="OperablePredicate"/> instance to an equivalent <see cref="Sentence"/>.
        /// </summary>
        /// <param name="predicate">The operable predicate to convert.</param>
        public static implicit operator Sentence(OperablePredicate predicate) => new Predicate(predicate.Identifier, predicate.Arguments.Select(a => (Term)a).ToArray());
    }

    /// <summary>
    /// Surrogate for <see cref="Term"/> instances that defines an == operator to create equality predicates.
    /// Instances are implicitly convertible from (and to) the equivalent <see cref="Term"/> instance.
    /// </summary>
#pragma warning disable CS0660, CS0661
    // Overrides == but not Equals and HashCode. Overriding these would make no sense. Needing to disable these warnings is
    // hopefully a good example of why I want to keep this approach to creating sentences well away from the actual sentence classes..
    public abstract class OperableTerm
    {
        /// <summary>
        /// Composes two <see cref="OperableTerm"/> instances together by creating an equality predicate. That is, an <see cref="OperablePredicate"/> with the <see cref="EqualityIdentifier.Instance"/> identifier.
        /// </summary>
        /// <param name="left">The left-hand side of the equality.</param>
        /// <param name="right">The right-hand side of the equality.</param>
        /// <returns>A new <see cref="OperablePredicate"/> instance.</returns>
        public static OperableSentence operator ==(OperableTerm left, OperableTerm right) => new OperablePredicate(EqualityIdentifier.Instance, left, right);

        /// <summary>
        /// Composes two <see cref="OperableTerm"/> instances together by creating a negation of the equality predicate. That is, an <see cref="OperableNegation"/> acting on an <see cref="OperablePredicate"/> with the <see cref="EqualityIdentifier.Instance"/> identifier.
        /// </summary>
        /// <param name="left">The left-hand side of the inequality.</param>
        /// <param name="right">The right-hand side of the inequality.</param>
        /// <returns>A new <see cref="OperableNegation"/> instance.</returns>
        public static OperableSentence operator !=(OperableTerm left, OperableTerm right) => new OperableNegation(new OperablePredicate(EqualityIdentifier.Instance, left, right));

        /// <summary>
        /// Implicitly converts a <see cref="OperableTerm"/> instance to an equivalent <see cref="Term"/>.
        /// </summary>
        /// <param name="term">The term to convert.</param>
        public static implicit operator Term(OperableTerm term) => term switch
        {
            null => throw new ArgumentNullException(nameof(term)),
            OperableFunction function => new Function(function.Identifier, function.Arguments.Select(a => (Term)a).ToArray()),
            OperableVariableReference variableReference => new VariableReference(variableReference.Declaration.Identifier),
            _ => throw new ArgumentException($"Cannot convert unsupported OperableTerm subtype {term.GetType()} to Term"),
        };

        /// <summary>
        /// Implicitly converts a <see cref="OperableTerm"/> instance to an equivalent <see cref="Term"/>.
        /// </summary>
        /// <param name="term">The term to convert.</param>
        public static implicit operator OperableTerm(Term term) => term switch
        {
            null => throw new ArgumentNullException(nameof(term)),
            Function function => new OperableFunction(function.Identifier, function.Arguments.Select(a => (OperableTerm)a).ToArray()),
            VariableReference variableReference => new OperableVariableReference(variableReference.Declaration.Identifier),
            _ => throw new ArgumentException($"Cannot convert unsupported Term subtype {term.GetType()} to OperableTerm"),
        };

        /// <summary>
        /// Implicitly converts a <see cref="Function"/> instance to an equivalent <see cref="OperableTerm"/>.
        /// </summary>
        /// <param name="function">The function to convert.</param>
        public static implicit operator OperableTerm(Function function) => new OperableFunction(function.Identifier, function.Arguments.Select(a => (OperableTerm)a).ToArray());

        /// <summary>
        /// Implicitly converts a <see cref="VariableReference"/> instance to an equivalent <see cref="OperableTerm"/>.
        /// </summary>
        /// <param name="variableReference">The variable reference to convert.</param>
        public static implicit operator OperableTerm(VariableReference variableReference) => new OperableVariableReference(variableReference.Declaration.Identifier);
    }
#pragma warning restore CS0660, CS0661

    /// <summary>
    /// Surrogate for <see cref="UniversalQuantification"/> instances that derives from <see cref="OperableSentence"/> and can thus be operated on with |, &amp; and ! operators
    /// to create disjunctions, conjunctions and negations respectively. N.B. constructor is intentionally not public - should only be created via the
    /// <see cref="ForAll(OperableVariableDeclaration, OperableSentence)"/> method (or its overrides).
    /// </summary>
    public sealed class OperableUniversalQuantification : OperableSentence
    {
        internal OperableUniversalQuantification(OperableVariableDeclaration variable, OperableSentence sentence) => (Variable, Sentence) = (variable, sentence);

        internal OperableVariableDeclaration Variable { get; }

        internal OperableSentence Sentence { get; }
    }

    /// <summary>
    /// Surrogate for <see cref="VariableDeclaration"/> instances.
    /// </summary>
    public sealed class OperableVariableDeclaration
    {
        internal OperableVariableDeclaration(object identifier) => Identifier = identifier;

        internal object Identifier { get; }

        /// <summary>
        /// Implicitly converts an <see cref="OperableVariableDeclaration"/> instance to an equivalent <see cref="VariableDeclaration"/>.
        /// </summary>
        /// <param name="declaration">The declaration to convert.</param>
        public static implicit operator VariableDeclaration(OperableVariableDeclaration declaration) => new(declaration.Identifier);

        /// <summary>
        /// Implicitly converts a <see cref="VariableDeclaration"/> instance to an equivalent <see cref="OperableVariableDeclaration"/>.
        /// </summary>
        /// <param name="declaration">The declaration to convert.</param>
        public static implicit operator OperableVariableDeclaration(VariableDeclaration declaration) => new(declaration.Identifier);

        /// <summary>
        /// Implicitly converts an <see cref="OperableVariableDeclaration"/> instance to an <see cref="OperableVariableReference"/> referring to that variable.
        /// </summary>
        /// <param name="declaration">The declaration to convert.</param>
        public static implicit operator OperableVariableReference(OperableVariableDeclaration declaration) => new(declaration);

        /// <summary>
        /// Implicitly converts an <see cref="OperableVariableDeclaration"/> instance to an <see cref="VariableReference"/> referring to that variable.
        /// </summary>
        /// <param name="declaration">The declaration to convert.</param>
        public static implicit operator VariableReference(OperableVariableDeclaration declaration) => new(declaration);

        /// <summary>
        /// Implicitly converts an <see cref="OperableVariableDeclaration"/> instance to an <see cref="Term"/> (that is a <see cref="VariableReference"/> referring to that variable).
        /// </summary>
        /// <param name="declaration">The declaration to convert.</param>
        public static implicit operator Term(OperableVariableDeclaration declaration) => new VariableReference(declaration);
    }

    /// <summary>
    /// Surrogate for <see cref="VariableReference"/> instances that derives from <see cref="OperableTerm"/> and can thus be operated on with the == and != operators
    /// to create equality and negation of equality respectively.
    /// </summary>
    public sealed class OperableVariableReference : OperableTerm
    {
        internal OperableVariableReference(OperableVariableDeclaration declaration) => Declaration = declaration;

        internal OperableVariableReference(object identifier) => Declaration = new OperableVariableDeclaration(identifier);

        internal OperableVariableDeclaration Declaration { get; }

        internal object Identifier => Declaration.Identifier;

        /// <summary>
        /// Implicitly converts an <see cref="OperableVariableReference"/> instance to an equivalent <see cref="VariableReference"/>.
        /// </summary>
        /// <param name="reference">The reference to convert.</param>
        public static implicit operator VariableReference(OperableVariableReference reference) => new(reference.Declaration);

        /// <summary>
        /// Implicitly converts an <see cref="VariableReference"/> instance to an equivalent <see cref="OperableVariableReference"/>.
        /// </summary>
        /// <param name="reference">The reference to convert.</param>
        public static implicit operator OperableVariableReference(VariableReference reference) => new(reference.Declaration);

        /// <summary>
        /// Implicitly converts an <see cref="OperableVariableReference"/> instance to an equivalent <see cref="Term"/>.
        /// </summary>
        /// <param name="reference">The reference to convert.</param>
        public static implicit operator Term(OperableVariableReference reference) => new VariableReference(reference.Declaration);
    }
}
