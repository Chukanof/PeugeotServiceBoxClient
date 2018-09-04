using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Utils.PageScriptManager;

namespace WpfClient.Utils.Tests
{
    [TestFixture]
    class PageScriptManagerTests
    {
        private PageScriptManagerImpl _sut;

        [SetUp]
        public void Setup()
        {
            var fakeCredMngr = Mock.Of<ICredentialsManager>();

            _sut = new PageScriptManagerImpl(fakeCredMngr);
        }

        [Test]
        public void InitializeContainerTest()
        {
            var dirStr = Path.Combine(Environment.CurrentDirectory, @"WpfClient.Shell\PageScripts");

            var expected = new Dictionary<string, ExtScript>
            {
                {"/socle/",new ExtScript{ ScriptType=ExtScriptType.Origin} },
                {"/pages/index.jsp", new ExtScript{ScriptType=ExtScriptType.Template } }
            };

            _sut.InitializeContainer(dirStr);


            Assert.AreEqual(expected.Count, _sut.Container.Count);
        }

        [Test]
        public async Task ResolveScriptTest_Templated()
        {
            _sut.Container = new Dictionary<string, ExtScript>
            {
                {"/scl/", new ExtScript{ScriptType=ExtScriptType.Template, Script=@"alert(""Hello"");"} }
            };

            var script = await _sut.ResolveScriptAsync(new Uri("http://www.ya.ru/scl/"));
        }

        [Test]
        public void ResolveScriptTest_Origin()
        {
            //_sut.ResolveScriptAsync(new Uri())
        }
    }
}
