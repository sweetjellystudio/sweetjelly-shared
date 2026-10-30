namespace SWeetJelly.Shared.Tests.EditMode
{
    using System;
    using NUnit.Framework;
    using SWeetJelly.Shared.Policy;

    public sealed class VerdictTest
    {
        [Test]
        public void Allow_IsAllowed()
        {
            var verdict = Verdict.Allow();

            Assert.That(verdict.IsAllowed, Is.True);
            Assert.That(verdict.IsRejected, Is.False);
        }

        [Test]
        public void Reject_HasNoSharedReason()
        {
            var verdict = Verdict.Reject();

            Assert.That(verdict.IsRejected, Is.True);
            Assert.That(verdict.IsAllowed, Is.False);
        }

        [Test]
        public void VerdictTAllow_HasDefaultReason()
        {
            var verdict = Verdict<DungeonEntryRejectReason>.Allow();

            Assert.That(verdict.IsAllowed, Is.True);
            Assert.That(verdict.IsRejected, Is.False);
            Assert.That(verdict.Reason, Is.EqualTo(DungeonEntryRejectReason.None));
        }

        [Test]
        public void VerdictTReject_HasPackageOwnedReason()
        {
            var verdict = Verdict<DungeonEntryRejectReason>.Reject(DungeonEntryRejectReason.LevelTooLow);

            Assert.That(verdict.IsRejected, Is.True);
            Assert.That(verdict.IsAllowed, Is.False);
            Assert.That(verdict.Reason, Is.EqualTo(DungeonEntryRejectReason.LevelTooLow));
        }

        [Test]
        public void VerdictTReject_ThrowsWhenReasonIsDefault()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = Verdict<DungeonEntryRejectReason>.Reject(DungeonEntryRejectReason.None);
            });
        }

        private enum DungeonEntryRejectReason
        {
            None = 0,
            LevelTooLow = 1,
            DungeonClosed = 2
        }
    }
}
