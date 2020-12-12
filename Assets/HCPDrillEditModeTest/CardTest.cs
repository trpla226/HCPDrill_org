using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CardTest
    {
        readonly Card Club10 =      new Card(Suit.Club, 10);
        readonly Card Diamond10 =   new Card(Suit.Diamond, 10);
        readonly Card Diamond5 =    new Card(Suit.Diamond, 5);

        readonly Card SpadeA = new Card(Suit.Spade, 1);
        readonly Card SpadeK = new Card(Suit.Spade, 13);
        readonly Card SpadeQ = new Card(Suit.Spade, 12);
        readonly Card SpadeJ = new Card(Suit.Spade, 11);
        readonly Card Spade2 = new Card(Suit.Spade, 2);

        // A Test behaves as an ordinary method
        [Test]
        public void CompareSuitTest()
        {
            Assert.AreEqual(-1, Card.CompareBySuit(Club10, Diamond10));
            Assert.AreEqual(1, Card.CompareBySuit(Diamond10, Club10));
            Assert.AreEqual(0, Card.CompareBySuit(Diamond10, Diamond5));
        }

        [Test]
        public void CompareNumberTest()
        {
            Assert.AreEqual(5, Card.CompareByScore(Diamond10, Diamond5));
            Assert.AreEqual(-5, Card.CompareByScore(Diamond5, Diamond10));
            Assert.AreEqual(0, Card.CompareByScore(Club10, Diamond10));

            Assert.AreEqual(1, Card.CompareByScore(SpadeA, SpadeK));
            Assert.AreEqual(0, Card.CompareByScore(Diamond10, Club10));
        }

        [Test]
        public void ScoreTest()
        {
            Assert.AreEqual(12, SpadeA.Rank);
            Assert.AreEqual(11, SpadeK.Rank);
            Assert.AreEqual(10, SpadeQ.Rank);
            Assert.AreEqual(0, Spade2.Rank);
        }

        [Test]
        public void CompareBySuitAndNumberTest()
        {
            Assert.True(Card.CompareBySuitAndNumber(SpadeA, SpadeK) > 0);
            Assert.True(Card.CompareBySuitAndNumber(Club10, Diamond10) < 0);
        }

        [Test]
        public void HCPTest()
        {
            Assert.AreEqual(4, SpadeA.HCP);
            Assert.AreEqual(3, SpadeK.HCP);
            Assert.AreEqual(2, SpadeQ.HCP);
            Assert.AreEqual(1, SpadeJ.HCP);
            Assert.AreEqual(0, Diamond10.HCP);
            Assert.AreEqual(0, Spade2.HCP);
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
