namespace Unit_Tests.ValidatorTests
{
    using System;
    using FluentAssertions;
    using Jobz.WebUi.Models;
    using Jobz.WebUi.Validation.Validators;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RainDelayUpdateModelValidator_Tests
    {
        [DataTestMethod]
        [DataRow(int.MinValue, false)]
        [DataRow(-1, false)]
        [DataRow(1, true)]
        [DataRow(int.MaxValue, true)]
        [DataRow(null, true)]
        public void Validate_HappyPath(int? relativeSeconds, bool isValid)
        {
            var sut = new RainDelayUpdateModelValidator();
            var model = new RainDelayUpdateModel();
            if (relativeSeconds != null)
            {
                model.RainDelayExpire = DateTime.Now.AddSeconds(relativeSeconds.Value);
            }
            var result = sut.Validate(instance: model);
            result.IsValid.Should().Be(isValid);
        }
    }
}
