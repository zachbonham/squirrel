using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using squirrel.contracts;
using squirrel.businesslogic;

namespace squirrel.businesslogic.tests
{

    [TestFixture]
    public class NutTests
    {

        readonly string TestAccount = "Testing";
        readonly string TestContainer = "TestContainer";

        
        [Test]
        public void Nut_CreateNut_ShouldCreate()
        {

            var nut = new Nut() 
            { 
                Database = TestAccount, 
                Key = Guid.NewGuid().ToString(), 
                Value = Guid.NewGuid().ToString() 
            };

            bool created = SquirrelBusinessLogic.CreateNut(TestAccount, TestContainer, nut);

            Assert.IsTrue(created);
        }

        [Test]
        public void Nut_FindNut_NutShouldNotBeNull()
        {
            var nut = new Nut()
            {
                Database = TestAccount,
                Key = Guid.NewGuid().ToString(),
                Value = Guid.NewGuid().ToString()
            };

            SquirrelBusinessLogic.CreateNut(TestAccount, TestContainer, nut);

            var result = SquirrelBusinessLogic.FindNut(TestAccount, TestContainer, nut.Key, new Dictionary<string, string>());

            Assert.IsNotNull(result);
        }
    }
}
