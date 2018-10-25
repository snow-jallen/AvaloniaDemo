using NUnit.Framework;
using System;
using Demo;

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
            
        }
    }
}