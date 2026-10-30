#pragma warning disable CS0618 // Deliberately verifies deprecated IsBlocked compatibility.
namespace SWeetJelly.Shared.Tests.EditMode
{
    using System;
    using NUnit.Framework;
    using SWeetJelly.Shared.Policy;
    using SWeetJelly.Shared.Result;

    public sealed class ContractRegressionTest
    {
        [Test]
        public void Reject_HasRejectedStateAndCompatibilityAlias()
        {
            var verdict = Verdict.Reject();
            Assert.That(verdict.IsRejected, Is.True);
            Assert.That(verdict.IsAllowed, Is.False);
            Assert.That(verdict.IsBlocked, Is.True);
        }

        [Test]
        public void RejectWithReason_PreservesReasonAndAlias()
        {
            var verdict = Verdict<string>.Reject("closed");
            Assert.That(verdict.IsRejected, Is.True);
            Assert.That(verdict.Reason, Is.EqualTo("closed"));
            Assert.That(verdict.RejectReason, Is.EqualTo(verdict.Reason));
            Assert.That(verdict.IsBlocked, Is.EqualTo(verdict.IsRejected));
        }

        [Test]
        public void RejectWithReason_RejectsDefaultReason()
        {
            Assert.Throws<ArgumentException>(() => Verdict<int>.Reject(0));
            Assert.Throws<ArgumentException>(() => Verdict<string>.Reject(null));
        }

        [Test]
        public void AllowWithValue_PreservesPayloadAndHasDefaultReason()
        {
            var payload = new object();
            var verdict = Verdict<object, string>.Allow(payload);
            Assert.That(verdict.IsAllowed, Is.True);
            Assert.That(verdict.IsRejected, Is.False);
            Assert.That(verdict.Value, Is.SameAs(payload));
            Assert.That(verdict.Reason, Is.Null);
        }

        [Test]
        public void AllowWithValue_AcceptsDefaultPayload()
        {
            Assert.That(Verdict<int, string>.Allow(0).Value, Is.Zero);
            Assert.That(Verdict<object, string>.Allow(null).Value, Is.Null);
        }

        [Test]
        public void RejectWithValue_HasDefaultPayloadAndPreservesReason()
        {
            var verdict = Verdict<int, string>.Reject("closed");
            Assert.That(verdict.IsRejected, Is.True);
            Assert.That(verdict.IsAllowed, Is.False);
            Assert.That(verdict.IsBlocked, Is.True);
            Assert.That(verdict.Value, Is.Zero);
            Assert.That(verdict.RejectReason, Is.EqualTo("closed"));
        }

        [Test]
        public void RejectWithValue_RejectsDefaultReason()
        {
            Assert.Throws<ArgumentException>(() => Verdict<string, int>.Reject(0));
            Assert.Throws<ArgumentException>(() => Verdict<int, string>.Reject(null));
        }

        [Test]
        public void RemovedBlockFactories_AreAbsentFromPublicContract()
        {
            Assert.That(typeof(Verdict).GetMethod("Block"), Is.Null);
            Assert.That(typeof(Verdict<>).GetMethod("Block"), Is.Null);
            Assert.That(typeof(Verdict<,>).GetMethod("Block"), Is.Null);
        }
        [Test]
        public void ResultSuccess_AcceptsNullPayload()
        {
            var result = Result<string>.Success(null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.Null);
        }

        [Test]
        public void ResultFailure_RejectsDefaultOrWhitespaceError()
        {
            Assert.Throws<ArgumentException>(() => Result<int>.Failure(default));
            Assert.Throws<ArgumentException>(() => Result.Failure(new Error(" ", "\t")));
        }

        [Test]
        public void Error_NormalizesNullAndFormatsPartialErrors()
        {
            var empty = new Error(null, null);
            Assert.That(empty.IsEmpty, Is.True);
            Assert.That(empty.Code, Is.Empty);
            Assert.That(empty.Message, Is.Empty);
            Assert.That(new Error("Code", "").ToString(), Is.EqualTo("Code"));
            Assert.That(new Error("", "Message").ToString(), Is.EqualTo("Message"));
        }

        [Test]
        public void DefaultResult_IsFailureWithEmptyErrorAndNoValueAccess()
        {
            Assert.That(default(Result).IsFailure, Is.True);
            Assert.That(default(Result).Error.IsEmpty, Is.True);
            var result = default(Result<int>);
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.IsEmpty, Is.True);
            Assert.Throws<InvalidOperationException>(() => { _ = result.Value; });
        }

        [Test]
        public void DefaultVerdict_IsRejectedWithDefaultPayloads()
        {
            Assert.That(default(Verdict).IsRejected, Is.True);
            var reasoned = default(Verdict<string>);
            Assert.That(reasoned.IsRejected, Is.True);
            Assert.That(reasoned.Reason, Is.Null);
            var valued = default(Verdict<int, string>);
            Assert.That(valued.IsRejected, Is.True);
            Assert.That(valued.Value, Is.Zero);
            Assert.That(valued.Reason, Is.Null);
        }
    }
}

#pragma warning restore CS0618
