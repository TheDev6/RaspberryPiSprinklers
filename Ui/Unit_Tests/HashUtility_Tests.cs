namespace Unit_Tests
{
    using System;
    using FluentAssertions;
    using Jobz.WebUi.Security;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HashUtility_Tests
    {
        [TestMethod]
        public void GetCreds_ForPasswords()
        {
            var hash = new HashUtility();

            Console.WriteLine(hash.Hash(
                toBeHashed: "[]pigz_flying[]",
                salt: "llYC8kxo/TUugRUTXnhd6BgRz5vgAENrEpx6cin0nbg="));
            var salt = hash.GenerateSalt();
            Console.WriteLine(salt);
            Console.WriteLine(hash.Hash(
                toBeHashed: "sp33nkz",
                salt: salt));
        }

        [TestMethod]
        public void SpanWork()
        {
            var span = new TimeSpan(days: 0, hours: 1, minutes: 65, seconds: 0);
            span.Hours.Should().Be(2);
            span.Minutes.Should().Be(5);
            span.Seconds.Should().Be(0);
        }
    }
}
