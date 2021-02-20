using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rhynn.UI;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameUIStateTests
    {
        #region Operators

        [Test]
        public void OperatorEquals_SameReference_True()
        {
            GameObject gameObject = new GameObject();
            GameScreen screen = gameObject.AddComponent<GameScreen>();
            GameUIState state1 = new MoveState(screen);
            GameUIState state2 = state1;
            
            Assert.IsTrue(state1 == state2);
        }
        
        [Test]
        public void OperatorEquals_SameType_True()
        {
            GameObject gameObject = new GameObject();
            GameScreen screen = gameObject.AddComponent<GameScreen>();
            GameUIState state1 = new MoveState(screen);
            GameUIState state2 = new MoveState(screen);
            
            Assert.IsTrue(state1 == state2);
        }
        
        [Test]
        public void OperatorEquals_BothNull_True()
        {
            GameUIState state1 = null;
            GameUIState state2 = null;
            
            Assert.IsTrue(state1 == state2);
        }
        
        [Test]
        public void OperatorEquals_OneNull_False()
        {
            GameObject gameObject = new GameObject();
            GameScreen screen = gameObject.AddComponent<GameScreen>();
            GameUIState state1 = new MoveState(screen);
            GameUIState state2 = null;
            
            Assert.IsFalse(state1 == state2);
        }
        
        [Test]
        public void OperatorEquals_DifferentTypes_False()
        {
            GameObject gameObject = new GameObject();
            GameScreen screen = gameObject.AddComponent<GameScreen>();
            GameUIState state1 = new MoveState(screen);
            GameUIState state2 = new AnimatingState(screen);
            
            Assert.IsFalse(state1 == state2);
        }
        
        [Test]
        public void OperatorNotEquals_SameReference_False()
        {
            GameObject gameObject = new GameObject();
            GameScreen screen = gameObject.AddComponent<GameScreen>();
            GameUIState state1 = new MoveState(screen);
            GameUIState state2 = state1;
            
            Assert.IsFalse(state1 != state2);
        }
        
        [Test]
        public void OperatorNotEquals_SameType_False()
        {
            GameObject gameObject = new GameObject();
            GameScreen screen = gameObject.AddComponent<GameScreen>();
            GameUIState state1 = new MoveState(screen);
            GameUIState state2 = new MoveState(screen);
            
            Assert.IsFalse(state1 != state2);
        }
        
        [Test]
        public void OperatorNotEquals_BothNull_False()
        {
            GameUIState state1 = null;
            GameUIState state2 = null;
            
            Assert.IsFalse(state1 != state2);
        }
        
        [Test]
        public void OperatorNotEquals_OneNull_True()
        {
            GameObject gameObject = new GameObject();
            GameScreen screen = gameObject.AddComponent<GameScreen>();
            GameUIState state1 = new MoveState(screen);
            GameUIState state2 = null;
            
            Assert.IsTrue(state1 != state2);
        }
        
        [Test]
        public void OperatorNotEquals_DifferentTypes_True()
        {
            GameObject gameObject = new GameObject();
            GameScreen screen = gameObject.AddComponent<GameScreen>();
            GameUIState state1 = new MoveState(screen);
            GameUIState state2 = new AnimatingState(screen);
            
            Assert.IsTrue(state1 != state2);
        }
        
        #endregion
    }
}
