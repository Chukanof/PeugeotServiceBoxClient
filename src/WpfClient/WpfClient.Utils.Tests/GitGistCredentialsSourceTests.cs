using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Infrastructure.DataAccess;
using WpfClient.Utils.CredentialsManager;
using WpfClient.Utils.SettingsProviderUtility;

namespace WpfClient.Utils.Tests
{
    [TestFixture]
    class GitGistCredentialsSourceTests
    {
        private ICredentialsManager _sut;
        private ISettingsProviderUtility fakeSettingsProvider;

        [SetUp]
        public void Setup()
        {
            var fakeLogger = Mock.Of<ILogger>();

            fakeSettingsProvider = Mock.Of<ISettingsProviderUtility>();
            fakeSettingsProvider.CredentialsSource = "https://gist.githubusercontent.com/Chukanof/29ec504da91162289b726a372666a564/raw/";
            fakeSettingsProvider.PreviousCredentialSourceHash = null;

            var fakeUow = Mock.Of<IUnitOfWork<SettingsDbContext>>();


            _sut = new GitGistCredentialsManager(fakeLogger, fakeSettingsProvider, fakeUow);
        }

        [Test]
        public async Task CheckIsUpToDateCredentialsAsyncTest_Failed_NoHashForCompare()
        {
            var expected = false;

            var actual = await _sut.CheckIsUpToDateCredentialsAsync();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task CheckIsUpToDateCredentialsAsyncTest()
        {
            fakeSettingsProvider.PreviousCredentialSourceHash = "C0A76C62BED4A9EC732B953D69DFD5DF"; // md5 hash for default credentials

            var expected = true;

            var actual = await _sut.CheckIsUpToDateCredentialsAsync();

            Assert.AreEqual(expected, actual);
        }
    }
}