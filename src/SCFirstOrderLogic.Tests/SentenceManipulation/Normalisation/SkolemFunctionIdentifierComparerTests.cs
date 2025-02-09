using FluentAssertions;
using FlUnit;

namespace SCFirstOrderLogic.SentenceManipulation.Normalisation
{
    public static class SkolemFunctionIdentifierComparerTests
    {
        public static Test SmokeTests => TestThat
            .GivenEachOf<ComparisonTestCase>(() =>
            [
            ])
            .When(tc =>
            {
                var cnfX = tc.SentenceX.ToCNF();
                var cnfY = tc.SentenceY.ToCNF();
                return 0;
            })
            .ThenReturns()
            .And((tc, rv) => rv.Should().Be(tc.ExpectedResult));

        private record ComparisonTestCase(
            Sentence SentenceX,
            object IdentifierX,
            Sentence SentenceY,
            object IdentifierY,
            int ExpectedResult);
    }
}
