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
            Assert.That(verdict.IsBlocked, Is.False);
        }

        [Test]
        public void Block_HasNoSharedReason()
        {
            var verdict = Verdict.Block();

            Assert.That(verdict.IsBlocked, Is.True);
            Assert.That(verdict.IsAllowed, Is.False);
        }

        [Test]
        public void VerdictTAllow_HasDefaultReason()
        {
            var verdict = Verdict<DungeonEntryBlockReason>.Allow();

            Assert.That(verdict.IsAllowed, Is.True);
            Assert.That(verdict.IsBlocked, Is.False);
            Assert.That(verdict.Reason, Is.EqualTo(DungeonEntryBlockReason.None));
        }

        [Test]
        public void VerdictTBlock_HasPackageOwnedReason()
        {
            var verdict = Verdict<DungeonEntryBlockReason>.Block(DungeonEntryBlockReason.LevelTooLow);

            Assert.That(verdict.IsBlocked, Is.True);
            Assert.That(verdict.IsAllowed, Is.False);
            Assert.That(verdict.Reason, Is.EqualTo(DungeonEntryBlockReason.LevelTooLow));
        }

        [Test]
        public void VerdictTBlock_ThrowsWhenReasonIsDefault()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = Verdict<DungeonEntryBlockReason>.Block(DungeonEntryBlockReason.None);
            });
        }

        private enum DungeonEntryBlockReason
        {
            None = 0,
            LevelTooLow = 1,
            DungeonClosed = 2
        }
    }
}
