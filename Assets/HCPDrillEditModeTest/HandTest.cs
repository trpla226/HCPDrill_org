using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class HandTest
    {

        // A Test behaves as an ordinary method
        [Test]
        public void HandTestSimplePasses()
        {
            var cards = new List<Card>
            {
                new Card(Suit.Club, 2),
                new Card(Suit.Club, 3),
                new Card(Suit.Club, 4),
                new Card(Suit.Club, 5),
                new Card(Suit.Club, 6),
                new Card(Suit.Club, 7),
                new Card(Suit.Diamond, 10),
                new Card(Suit.Diamond, 5),
                new Card(Suit.Spade, 13), // 3
                new Card(Suit.Spade, 12), // 2
                new Card(Suit.Spade, 11), // 1
                new Card(Suit.Spade, 2),
                new Card(Suit.Spade, 1) // 4
            };

            var hand = new Hand(cards);
            Assert.AreEqual(10, hand.HCP);   
        }

        [Test]
        public void HandArgumentExceptionTest()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Hand(new List<Card>() { new Card(Suit.Spade, 1) });
            });
        }

        [Test]
        public void HighCardCountTest()
        {
            var cards = new List<Card>
            {
                new Card(Suit.Club, 2),
                new Card(Suit.Club, 3),
                new Card(Suit.Club, 4),
                new Card(Suit.Club, 5),
                new Card(Suit.Club, 6),
                new Card(Suit.Club, 7),
                new Card(Suit.Diamond, 10),
                new Card(Suit.Diamond, 5),
                new Card(Suit.Spade, 13), // 3
                new Card(Suit.Spade, 12), // 2
                new Card(Suit.Spade, 11), // 1
                new Card(Suit.Spade, 2),
                new Card(Suit.Spade, 1) // 4
            };

            var hand = new Hand(cards);
            Assert.AreEqual(4, hand.HighCardCount);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator HandTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
