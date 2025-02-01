﻿using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using static SCFirstOrderLogic.SentenceCreation.SentenceFactory;

namespace SCFirstOrderLogic.SentenceManipulation;

[MemoryDiagnoser]
[InProcess]
public class RecursiveSentenceTransformationBenchmarks
{
    private static Predicate IsAnimal(Term term) => new(nameof(IsAnimal), term);
    private static Predicate Loves(Term term1, Term term2) => new(nameof(Loves), term1, term2);

    private static Sentence NonTrivialSentence { get; } = ForAll(X, If(
            ForAll(Y, If(IsAnimal(Y), Loves(X, Y))),
            ThereExists(Y, Loves(Y, X))));

    public record TestCase(string Label, bool DoSomething, Sentence Sentence)
    {
        public override string ToString() => Label;
    }

    public static IEnumerable<TestCase> TestCases { get; } = new TestCase[]
    {
        new(
            Label: "Non-Trivial NO-OP",
            DoSomething: false,
            Sentence: NonTrivialSentence),

        new(
            Label: "Non-Trivial ALL-LEAFS-OP",
            DoSomething: true,
            Sentence: NonTrivialSentence),
    };

    [ParamsSource(nameof(TestCases))]
    public TestCase? CurrentTestCase { get; set; }

    [Benchmark(Baseline = true)]
    public Sentence CurrentImpl() => new VarTransform(CurrentTestCase!.DoSomething).ApplyTo(CurrentTestCase!.Sentence);

    [Benchmark]
    public Sentence Linq() => new VarTransform_Linq(CurrentTestCase!.DoSomething).ApplyTo(CurrentTestCase!.Sentence);

    [Benchmark]
    public Sentence LinqIterateTwice() => new VarTransform_LinqIterateTwice(CurrentTestCase!.DoSomething).ApplyTo(CurrentTestCase!.Sentence);

    private class VarTransform_LinqIterateTwice : RecursiveSentenceTransformation_LinqIterateTwice
    {
        private readonly bool doSomething;

        public VarTransform_LinqIterateTwice(bool doSomething) => this.doSomething = doSomething;

        public override VariableDeclaration ApplyTo(VariableDeclaration variableDeclaration)
        {
            return doSomething ? new VariableDeclaration("X") : variableDeclaration;
        }
    }

    private class VarTransform_Linq : RecursiveSentenceTransformation_Linq
    {
        private readonly bool doSomething;

        public VarTransform_Linq(bool doSomething) => this.doSomething = doSomething;

        public override VariableDeclaration ApplyTo(VariableDeclaration variableDeclaration)
        {
            return doSomething ? new VariableDeclaration("X") : variableDeclaration;
        }
    }

    private class VarTransform : RecursiveSentenceTransformation
    {
        private readonly bool doSomething;

        public VarTransform(bool doSomething) => this.doSomething = doSomething;

        public override VariableDeclaration ApplyTo(VariableDeclaration variableDeclaration)
        {
            return doSomething ? new VariableDeclaration("X") : variableDeclaration;
        }
    }
}
