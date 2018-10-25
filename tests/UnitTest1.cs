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
            dataSvcMock.Setup(m => m.GetPeopleFromGedcom(It.IsAny<String>()))
                .Returns(new[]
                {
                    new Person("BogusLast", "BogusFirst")
                });
            dataSvcMock.Setup(m => m.FileExists(It.IsAny<String>())).Returns(true);

            var vm = new MainViewModel(dataSvcMock.Object);

            vm.GedcomPath = "totally random";
            Assert.IsTrue(vm.LoadGedcom.CanExecute(this));

            vm.LoadGedcom.Execute(this);
            Assert.AreEqual(vm.Output, $"We found {vm.People.Count} people in {vm.GedcomPath}!");
        }
    }
}