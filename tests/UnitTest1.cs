using NUnit.Framework;
using System;
using Demo;
using Interfaces;
using Moq;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        public static string TestString = null;

        [Test]
        public void TestSimpleCommand()
        {
            var command = new SimpleCommand(()=>Tests.TestString="I worked!");
            command.Execute(this);
            Assert.AreEqual("I worked!", TestString);
        }

        [Test]
        public void TestReadingGedcomFile()
        {
            var dataSvcMock = new Mock<IDataService>();
            dataSvcMock.Setup(m => m.GetPeopleFromGedcomAsync(It.IsAny<String>()))
                .ReturnsAsync(new []
                {
                    new Person("BogusLast", "BogusFirst")
                });
            dataSvcMock.Setup(m => m.FileExists(It.IsAny<String>())).Returns(true);

            var trackingSvcMock = new Mock<ITrackingService>();

            var vm = new MainViewModel(dataSvcMock.Object, trackingSvcMock.Object);

            vm.GedcomPath = "totally random";
            Assert.IsTrue(vm.LoadGedcom.CanExecute(this));

            vm.LoadGedcom.Execute(this);
            Assert.AreEqual(vm.Output, $"We found {vm.People.Count} people in {vm.GedcomPath}!");
        }

        [Test]
        public void CanLoadAfterFindingValidFile()
        {
            var dataSvcMock = new Mock<IDataService>();
            dataSvcMock.Setup(m => m.FindFileAsync()).ReturnsAsync("/fake/file");
            dataSvcMock.Setup(m => m.FileExists(It.IsAny<String>())).Returns(true);
            var trackingSvcMock = new Mock<ITrackingService>();
            var vm = new MainViewModel(dataSvcMock.Object, trackingSvcMock.Object);

            vm.FindFile.Execute(this);

            Assert.IsTrue(vm.LoadGedcom.CanExecute(this));
        }
    }
}