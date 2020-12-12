using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CardTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void CardTestSimplePasses()
        {
            // Use the Assert class to test conditions
            var x = new Card(Suit.Club, 10);
            var y = new Card(Suit.Diamond, 10);
            var z = new Card(Suit.Diamond, 5);

            Assert.AreEqual(-1, Card.CompareSuit(x, y));
            Assert.AreEqual(1, Card.CompareSuit(y, x));
            Assert.AreEqual(0, Card.CompareSuit(y, z));
            
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator CardTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
