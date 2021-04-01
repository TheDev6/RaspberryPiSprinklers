//namespace UnitTests.Authentication
//{
//    using System.Linq;
//    using System.Security.Claims;
//    using FluentAssertions;
//    using Jobz.RootContracts.BehaviorContracts.Configuration;
//    using Jobz.WebUi.Security;
//    using Xunit;
//    using NSubstitute;
//    using Xunit.Abstractions;


//    public class Authenticator_Tests
//    {
//        private readonly ITestOutputHelper _logger;

//        public Authenticator_Tests(ITestOutputHelper logger)
//        {
//            _logger = logger;
//        }

//        [Fact]
//        public void SignIn_HappyPath()
//        {
//            var username = "user";
//            var pass = "pass";

//            var config = Substitute.For<IConfig>();
//            config.AdminUserName().ReturnsForAnyArgs(username);
//            config.AdminUserSalt().ReturnsForAnyArgs("salt");
//            config.AdminUserPassHash()
//                .ReturnsForAnyArgs("HxxvnKpVLWDjTUVgyF3qUZAeAhcM1FZwXzjaqDGA02SyfCcEjgdss7uYPsF3xdVpyZ1x7tVP8jo/9/c78Qwv3g==");
//            var sut = new Authenticator(hashUtil: new HashUtility(), config: config);

//            var result = sut.SignIn(username: username, password: pass);
//            result.IsValidSignIn.Should().BeTrue();
//            result.ClaimsIdentity.Claims.ToList()[0].Type.Should().Be(ClaimTypes.Name);
//            result.ClaimsIdentity.Claims.ToList()[0].Value.Should().Be(username);
//        }

//        [Theory]
//        [InlineData("user", "pass", true)]
//        [InlineData("user_", "pass", false)]
//        [InlineData("user", "pass_", false)]
//        [InlineData(null, "pass_", false)]
//        [InlineData("user", null, false)]
//        [InlineData(null, null, false)]
//        [InlineData("", "", false)]
//        public void SignIn(string userName, string userPass, bool isValidLogin)
//        {
//            var config = Substitute.For<IConfig>();
//            config.AdminUserName().ReturnsForAnyArgs("user");
//            config.AdminUserSalt().ReturnsForAnyArgs("salt");
//            config.AdminUserPassHash()
//                .ReturnsForAnyArgs("HxxvnKpVLWDjTUVgyF3qUZAeAhcM1FZwXzjaqDGA02SyfCcEjgdss7uYPsF3xdVpyZ1x7tVP8jo/9/c78Qwv3g==");
//            var sut = new Authenticator(hashUtil: new HashUtility(), config: config);

//            var result = sut.SignIn(username: userName, password: userPass);
//            result.IsValidSignIn.Should().Be(isValidLogin);
//        }
//    }
//}
