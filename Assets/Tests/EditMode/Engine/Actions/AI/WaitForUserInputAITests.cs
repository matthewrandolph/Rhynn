using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rhynn.Engine.AI;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class WaitForUserInputAITests
    {
        private WaitForUserInputAI _ai;

        [SetUp]
        public void Init()
        {
            _ai = new WaitForUserInputAI();
        }
        
        [Test]
        public void NeedsUserInput_True()
        {
            Assert.IsTrue(_ai.NeedsUserInput);
        }

        [Test]
        public void GetNextBehaviour_IsNull()
        {
            Assert.IsNull(_ai.GetNextActivity());
        }
    }
}
