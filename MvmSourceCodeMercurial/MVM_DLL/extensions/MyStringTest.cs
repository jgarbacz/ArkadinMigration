using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
namespace MVM.extensions
{
    [TestFixture]
    class MyStringTest
    {
        [Test]
        public static void TestSubstr()
        {
            // from http://perldoc.perl.org/functions/substr.html
            string s1 = "The black cat climbed the green tree";
            Assert.AreEqual(s1,s1.Substr(0));
            Assert.AreEqual("black",s1.Substr(4,5));
            Assert.AreEqual("black cat climbed the",s1.Substr(4, -11));
            Assert.AreEqual("climbed the green tree",s1.Substr(14));
            Assert.AreEqual( "tree",s1.Substr(-4));
            Assert.AreEqual("tr",s1.Substr(-4, 2));
            Assert.AreEqual("The black cat jumped from the green tree",s1.Substr(14, 7, "jumped from"));
            
            // extra ones:
            Assert.AreEqual("A black cat climbed the green tree",s1.Substr(0, 3, "A"));
            Assert.AreEqual("The black cat climbed the green pole",s1.Substr(-4, 4, "pole"));

            // TBD: investigate boundary conditions when outside the string
           
        }
    }
}
