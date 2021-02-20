using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Util.Pathfinding;

namespace Tests
{
    public class MotilityTests
    {
        [Test]
        public void Contains_SameReference_True()
        {
            Motility motility = Motility.Fly;
            
            Assert.IsTrue(motility.Contains(motility));
        }
        
        [Test]
        public void Contains_SameType_True()
        {
            Motility motility = Motility.Fly;
            Motility motility2 = Motility.Fly;
            
            Assert.IsTrue(motility.Contains(motility2));
        }
        
        [Test]
        public void Equals_SameReference_True()
        {
            Motility motility = Motility.Fly;
            Motility motility2 = motility;
            
            Assert.IsTrue(motility.Equals(motility2));
        }
        
        [Test]
        public void Equals_SameType_True()
        {
            Motility motility = Motility.Fly;
            Motility motility2 = Motility.Fly;
            
            Assert.IsTrue(motility.Equals(motility2));
        }
        
        [Test]
        public void Equals_DifferentType_False()
        {
            Motility motility = Motility.Fly;
            Motility motility2 = Motility.Climb;
            
            Assert.IsFalse(motility.Equals(motility2));
        }
        
        #region Operators
        
        [Test]
        public void Bitwise_OR()
        {
            Motility motility = Motility.Land | Motility.Swim;
            int landAndSwim = 3;

            Assert.AreEqual(motility.GetHashCode(), landAndSwim.GetHashCode());
        }
        
        [Test]
        public void Bitwise_AND()
        {
            Motility motility = Motility.Land | Motility.Swim;
            int land = 1;

            motility &= Motility.Land;

            Assert.AreEqual(motility.GetHashCode(), land.GetHashCode());
        }
        
        [Test]
        public void Subtract()
        {
            Motility motility = Motility.Land | Motility.Swim;
            int land = 1;

            motility -= Motility.Swim;

            Assert.AreEqual(motility.GetHashCode(), land.GetHashCode());
        }
        
        #endregion
        
    }
}
