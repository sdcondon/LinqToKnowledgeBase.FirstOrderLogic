// Copyright � 2023-2024 Simon Condon.
// You may use this file in accordance with the terms of the MIT license.
using System;
using System.Collections.Generic;

namespace SCFirstOrderLogic.ClauseIndexing;

/// <summary>
/// An implementation of <see cref="IFeatureVectorIndexNode{TFeature, TValue}"/> that just stores things in memory. 
/// Uses a <see cref="Dictionary{TKey, TValue}"/> for the children of a node.
/// </summary>
/// <typeparam name="TFeature">The type of the keys of the feature vectors.</typeparam>
/// <typeparam name="TValue">The type of the value associated with each stored clause.</typeparam>
// todo: ordered lists make way more sense than dictionaries here..
public class FeatureVectorIndexDictionaryNode<TFeature, TValue> : IFeatureVectorIndexNode<TFeature, TValue>
    where TFeature : notnull
{
    private readonly Dictionary<KeyValuePair<TFeature, int>, IFeatureVectorIndexNode<TFeature, TValue>> children;
    private readonly Dictionary<CNFClause, TValue> values = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="FeatureVectorIndexDictionaryNode{TFeature, TValue}"/> class.
    /// </summary>
    public FeatureVectorIndexDictionaryNode()
        : this(EqualityComparer<KeyValuePair<TFeature, int>>.Default)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="FeatureVectorIndexDictionaryNode{TFeature, TValue}"/> class.
    /// </summary>
    /// <param name="equalityComparer">
    /// The equality comparer that should be used by the child dictionary.
    /// For correct behaviour, index instances accessing this node should be using an <see cref="IComparer{T}"/> that is consistent with it. 
    /// That is, one that only returns zero for features considered equal by the equality comparer used by this instance.
    /// </param>
    public FeatureVectorIndexDictionaryNode(IEqualityComparer<KeyValuePair<TFeature, int>> equalityComparer)
    {
        children = new(equalityComparer);
    }

    /// <inheritdoc/>
    // NB: we don't bother wrapping children in a ReadOnlyDict to stop unscrupulous
    // users from casting. Would be more memory for a real edge case.
    public IReadOnlyDictionary<KeyValuePair<TFeature, int>, IFeatureVectorIndexNode<TFeature, TValue>> Children => children;

    /// <inheritdoc/>
    // NB: we don't bother wrapping values in a read-only class to stop unscrupulous
    // users from casting. Would be more mem for a real edge case.
    public IReadOnlyDictionary<CNFClause, TValue> Values => values;

    /// <inheritdoc/>
    public IFeatureVectorIndexNode<TFeature, TValue> GetOrAddChild(KeyValuePair<TFeature, int> vectorElement)
    {
        if (!children.TryGetValue(vectorElement, out var node))
        {
            node = new FeatureVectorIndexDictionaryNode<TFeature, TValue>();
            children.Add(vectorElement, node);
        }

        return node;
    }

    /// <inheritdoc/>
    public void DeleteChild(KeyValuePair<TFeature, int> vectorElement)
    {
        children.Remove(vectorElement);
    }

    /// <inheritdoc/>
    public void AddValue(CNFClause clause, TValue value)
    {
        // todo: unify, or expect ordinalisation? tricky bit in ordinalisation will be ensuring consistent ordering of literals in clause..
        if (!values.TryAdd(clause, value))
        {
            throw new ArgumentException("Key already present", nameof(clause));
        }
    }

    /// <inheritdoc/>
    public bool RemoveValue(CNFClause clause)
    {
        // todo: unify, or expect ordinalisation? tricky bit in ordinalisation will be ensuring consistent ordering of literals in clause..
        return values.Remove(clause);
    }
}
