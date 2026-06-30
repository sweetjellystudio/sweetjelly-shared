namespace SWeetJelly.Shared.Tests.EditMode
{
    using System;
    using NUnit.Framework;
    using SWeetJelly.Shared.Policy;
    using SWeetJelly.Shared.Result;

    public sealed class ResultTest
    {
        [Test]
        public void Success_HasNoError()
        {
            var result = Result.Success();

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.IsFailure, Is.False);
            Assert.That(result.Error.IsEmpty, Is.True);
        }

        [Test]
        public void Failure_HasError()
        {
            var error = new Error("Save.NoSlot", "No save slot is available.");
            var result = Result.Failure(error);

            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(error));
            Assert.That(result.Error.ToString(), Is.EqualTo("Save.NoSlot: No save slot is available."));
        }

        [Test]
        public void Failure_ThrowsWhenErrorIsEmpty()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = Result.Failure(Error.None);
            });
        }

        [Test]
        public void ResultTSuccess_ExposesValue()
        {
            var result = Result<int>.Success(10);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.EqualTo(10));
            Assert.That(result.Error.IsEmpty, Is.True);
        }

        [Test]
        public void ResultTFailure_ThrowsWhenValueIsAccessed()
        {
            var result = Result<int>.Failure(new Error("Coins.Missing", "Coins could not be loaded."));

            Assert.That(result.IsFailure, Is.True);
            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = result.Value;
            });
        }

        [Test]
        public void ResultsFactory_CreatesResultValues()
        {
            var success = Results.Success("Ready");
            var failure = Results.Failure<string>(Results.Error("State.Invalid", "State is invalid."));

            Assert.That(success.IsSuccess, Is.True);
            Assert.That(success.Value, Is.EqualTo("Ready"));
            Assert.That(failure.IsFailure, Is.True);
            Assert.That(failure.Error.Code, Is.EqualTo("State.Invalid"));
        }

        [Test]
        public void ResultVerdict_CanRepresentPolicyEvaluation()
        {
            var result = Results.Success(
                Verdict<DungeonEntryBlockReason>.Block(DungeonEntryBlockReason.LevelTooLow));

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.IsBlocked, Is.True);
            Assert.That(result.Value.Reason, Is.EqualTo(DungeonEntryBlockReason.LevelTooLow));
        }

        private enum DungeonEntryBlockReason
        {
            None = 0,
            LevelTooLow = 1
        }
    }
}
