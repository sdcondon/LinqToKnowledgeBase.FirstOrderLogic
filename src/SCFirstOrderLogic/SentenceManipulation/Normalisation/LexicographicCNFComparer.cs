using System;
using System.Collections.Generic;
using System.Linq;

namespace SCFirstOrderLogic.SentenceManipulation.Normalisation
{
    /// <summary>
    /// A comparer for CNF clauses (and the elements thereof), intended to be of use in index structures such an feature vector indices.
    /// Broadly adheres 
    /// </summary>
    internal class LexicographicCNFComparer : IComparer<CNFClause>, IComparer<Literal>, IComparer<SkolemFunctionIdentifier>
    {
        private readonly IComparer<object> identifierComparer;

        public LexicographicCNFComparer(IComparer<object> identifierComparer)
        {
            this.identifierComparer = identifierComparer;
        }

        public int Compare(CNFClause? x, CNFClause? y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else if (y == null)
            {
                return 1;
            }

            var xSortedLiterals = x.Literals.OrderBy(l => l, this);
            var ySortedLiterals = y.Literals.OrderBy(l => l, this);
            return CompareLexicographically(xSortedLiterals, ySortedLiterals, this);
        }

        public int Compare(Literal? x, Literal? y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else if (y == null)
            {
                return 1;
            }

            var predicateComparison = identifierComparer.Compare(x.Predicate.Identifier, y.Predicate.Identifier);
            if (predicateComparison != 0)
            {
                return predicateComparison;
            }

            var argsComparison = CompareLexicographically(x.Predicate.Arguments, y.Predicate.Arguments, this);
            if (argsComparison != 0)
            {
                return argsComparison;
            }

            // We go by positive/negative last to give primacy to the user-provided identifier comparer - i.e. to 
            // make this ordering less impacted by arbitrary positive/negative comparisons, and more by what the
            // consumer provides.
            return x.IsNegated.CompareTo(y.IsNegated);
        }

        public int Compare(Term? x, Term? y)
        {
            // variables (by ...? this is the problem if ultimate goal is skm fun id comparison) > function (by id then by args lexico)
            throw new NotImplementedException();
        }

        private static int CompareLexicographically<T>(IEnumerable<T> x, IEnumerable<T> y, IComparer<T> comparer)
        {
            using var xEnumerator = x.GetEnumerator();
            using var yEnumerator = y.GetEnumerator();
            
            while (true)
            {
                var xHadNext = xEnumerator.MoveNext();
                var yHadNext = yEnumerator.MoveNext();

                if (xHadNext)
                {
                    if (yHadNext)
                    {
                        var comparison = comparer.Compare(xEnumerator.Current, yEnumerator.Current);
                        if (comparison != 0)
                        {
                            return comparison;
                        }
                    }
                    else
                    {
                        return 1;
                    }
                }
                else if (yHadNext)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
