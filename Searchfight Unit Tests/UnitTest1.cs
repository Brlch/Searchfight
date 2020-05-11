using NUnit.Framework;
using Searchfight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Searchfight_Unit_Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task NoParametersTest()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                await Searchfight.Program.Main(new string[] { });
                Assert.IsTrue(sw.ToString().Contains("No terms provided."));
            }
        }

        [Test]
        public async Task TestForCSharp()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                await Searchfight.Program.Main(new string[] {"C#"});
                Assert.IsTrue(sw.ToString().Contains("C#"));
            }
        }

        [Test]
        public async Task SupportForSpaces()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                await Searchfight.Program.Main(new string[] { "C#", "Objective C" });
                Assert.IsTrue(sw.ToString().Contains("C#") && sw.ToString().Contains("Objective C"));
            }
        }
    }
}