using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Util;

namespace Tests
{
    public class Rect2Tests
    {
        #region Public static properties
        
        [Test]
        public void Empty_IsZeroed()
        {
            Rect2 r = Rect2.Empty;
            
            Assert.AreEqual(0, r.Position.x);
            Assert.AreEqual(0, r.Position.y);
            Assert.AreEqual(0, r.Size.x);
            Assert.AreEqual(0, r.Size.x);
        }
        
        [Test]
        public void TestRow()
        {
            Rect2 r = Rect2.Row(4);
            
            Assert.AreEqual(0, r.Position.x);
            Assert.AreEqual(0, r.Position.y);
            Assert.AreEqual(4, r.Size.x);
            Assert.AreEqual(1, r.Size.y);
        }
        
        [Test]
        public void TestRowPos()
        {
            Rect2 r = Rect2.Row(-2, 3, 4);
            
            Assert.AreEqual(-2, r.Position.x);
            Assert.AreEqual(3, r.Position.y);
            Assert.AreEqual(4, r.Size.x);
            Assert.AreEqual(1, r.Size.y);
        }
        
        [Test]
        public void TestRowVec2Pos()
        {
            Rect2 r = Rect2.Row(new Vec2(-2, 3), 4);
            
            Assert.AreEqual(-2, r.Position.x);
            Assert.AreEqual(3, r.Position.y);
            Assert.AreEqual(4, r.Size.x);
            Assert.AreEqual(1, r.Size.y);
        }
        
        [Test]
        public void TestColumn()
        {
            Rect2 r = Rect2.Column(4);

            Assert.AreEqual(0, r.Position.x);
            Assert.AreEqual(0, r.Position.y);
            Assert.AreEqual(1, r.Size.x);
            Assert.AreEqual(4, r.Size.y);
        }
        
        [Test]
        public void TestColumnPos()
        {
            Rect2 r = Rect2.Column(-2, 3, 4);

            Assert.AreEqual(-2, r.Position.x);
            Assert.AreEqual(3, r.Position.y);
            Assert.AreEqual(1, r.Size.x);
            Assert.AreEqual(4, r.Size.y);
        }
        
        [Test]
        public void TestColumnVecPos()
        {
            Rect2 r = Rect2.Column(new Vec2(-2, 3), 4);

            Assert.AreEqual(-2, r.Position.x);
            Assert.AreEqual(3, r.Position.y);
            Assert.AreEqual(1, r.Size.x);
            Assert.AreEqual(4, r.Size.y);
        }
        
        #endregion
        
        #region Constructors

        [Test]
        public void TestConstructorDefault()
        {
            Rect2 r = new Rect2();

            Assert.AreEqual(0, r.Position.x);
            Assert.AreEqual(0, r.Position.y);
            Assert.AreEqual(0, r.Size.x);
            Assert.AreEqual(0, r.Size.y);
        }

        [Test]
        public void TestConstructorSizeVec2()
        {
            Rect2 r = new Rect2(new Vec2(3, 5));

            Assert.AreEqual(0, r.Position.x);
            Assert.AreEqual(0, r.Position.y);
            Assert.AreEqual(3, r.Size.x);
            Assert.AreEqual(5, r.Size.y);
        }

        [Test]
        public void TestConstructorPositionVec2SizeVec2()
        {
            Rect2 r = new Rect2(new Vec2(2, 4), new Vec2(3, 5));

            Assert.AreEqual(2, r.Position.x);
            Assert.AreEqual(4, r.Position.y);
            Assert.AreEqual(3, r.Size.x);
            Assert.AreEqual(5, r.Size.y);
        }

        [Test]
        public void TestConstructorPositionIntSizeInt()
        {
            Rect2 r = new Rect2(2, 4, 3, 5);

            Assert.AreEqual(2, r.Position.x);
            Assert.AreEqual(4, r.Position.y);
            Assert.AreEqual(3, r.Size.x);
            Assert.AreEqual(5, r.Size.y);
        }

        [Test]
        public void TestConstructorPositionVec2SizeInt()
        {
            Rect2 r = new Rect2(new Vec2(2, 4), 3, 5);

            Assert.AreEqual(2, r.Position.x);
            Assert.AreEqual(4, r.Position.y);
            Assert.AreEqual(3, r.Size.x);
            Assert.AreEqual(5, r.Size.y);
        }

        [Test]
        public void TestConstructorPositionIntSizeVec2()
        {
            Rect2 r = new Rect2(2, 4, new Vec2(3, 5));

            Assert.AreEqual(2, r.Position.x);
            Assert.AreEqual(4, r.Position.y);
            Assert.AreEqual(3, r.Size.x);
            Assert.AreEqual(5, r.Size.y);
        }

        #endregion
        
        #region Enumeration

        [Test]
        public void TestEnumerateNegativeWidth()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => TestEnumeration(new Rect2(3, 2, -1, 1)));
        }

        [Test]
        public void TestEnumerateNegativeHeight()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => TestEnumeration(new Rect2(3, 2, 1, -1)));
        }

        [Test]
        public void TestEnumerateEmpty()
        {
            TestEnumeration(Rect2.Empty);
        }

        [Test]
        public void TestEnumerateZeroWidth()
        {
            TestEnumeration(new Rect2(-3, 2, 0, 10));
        }

        [Test]
        public void TestEnumerateZeroHeight()
        {
            TestEnumeration(new Rect2(3, -2, 10, 0));
        }

        [Test]
        public void TestEnumerateRow()
        {
            TestEnumeration(Rect2.Row(4, 5, 3),
                new Vec2(4, 5),
                new Vec2(5, 5),
                new Vec2(6, 5));
        }

        [Test]
        public void TestEnumerateColumn()
        {
            TestEnumeration(Rect2.Column(4, 5, 3),
                new Vec2(4, 5),
                new Vec2(4, 6),
                new Vec2(4, 7));
        }

        [Test]
        public void TestEnumerate()
        {
            TestEnumeration(new Rect2(4, 5, 3, 2),
                new Vec2(4, 5),
                new Vec2(5, 5),
                new Vec2(6, 5),
                new Vec2(4, 6),
                new Vec2(5, 6),
                new Vec2(6, 6));
        }

        /*[Test]
        public void TestEnumerateTrace()
        {
            // tracing a zero-dimension does nothing
            TestEnumeration(new Rect2(4, 5, 0, 0).Trace());
            TestEnumeration(new Rect2(4, 5, 4, 0).Trace());
            TestEnumeration(new Rect2(4, 5, 0, 3).Trace());

            // a single unit
            TestEnumeration(new Rect2(4, 5, 1, 1).Trace(),
                new Vec2(4, 5));

            // a row
            TestEnumeration(new Rect2(4, 5, 3, 1).Trace(),
                new Vec2(4, 5),
                new Vec2(5, 5),
                new Vec2(6, 5));

            // a column
            TestEnumeration(new Rect2(4, 5, 1, 3).Trace(),
                new Vec2(4, 5),
                new Vec2(4, 6),
                new Vec2(4, 7));

            // a 3x3 square
            TestEnumeration(new Rect2(4, 5, 3, 3).Trace(),
                new Vec2(4, 7),
                new Vec2(5, 7),
                new Vec2(6, 7),
                
                new Vec2(6, 6),
                
                new Vec2(6, 5),
                new Vec2(5, 5),
                new Vec2(4, 5),
                
                new Vec2(4, 6));
        }*/

        #endregion
        
        [Test]
        public void Contains()
        {
            // identical rect is inside
            Assert.IsTrue(new Rect2(0, 0, 3, 4).Contains(new Rect2(0, 0, 3, 4)));

            // zero size rect can still be inside
            Assert.IsTrue(new Rect2(0, 0, 3, 4).Contains(new Rect2(1, 2, 0, 0)));

            // outer corners of rect are included
            Assert.IsTrue(new Rect2(0, 0, 3, 4).Contains(new Rect2(0, 0, 0, 0)));
            Assert.IsTrue(new Rect2(0, 0, 3, 4).Contains(new Rect2(3, 4, 0, 0)));

            // point must be in
            Assert.IsFalse(new Rect2(0, 0, 3, 4).Contains(new Rect2(-1, 0, 0, 0)));

            // off left side
            Assert.IsFalse(new Rect2(0, 0, 3, 4).Contains(new Rect2(-1, 1, 2, 2)));

            // off right side
            Assert.IsFalse(new Rect2(0, 0, 3, 4).Contains(new Rect2(5, 1, 2, 2)));

            // off bottom side
            Assert.IsFalse(new Rect2(0, 0, 3, 4).Contains(new Rect2(1, -3, 2, 2)));

            // off top side
            Assert.IsFalse(new Rect2(0, 0, 3, 4).Contains(new Rect2(1, 5, 2, 2)));

            // completely surrounded
            Assert.IsFalse(new Rect2(0, 0, 3, 4).Contains(new Rect2(-1, -1, 5, 6)));

            // off two sides
            Assert.IsFalse(new Rect2(0, 0, 3, 4).Contains(new Rect2(-1, -1, 6, 2)));
        }

        [Test]
        public void TestCoordinates()
        {
            Rect2 rect = new Rect2(1, 2, 3, 4);

            // x, y
            Assert.AreEqual(1, rect.x);
            Assert.AreEqual(2, rect.y);

            // size
            Assert.AreEqual(3, rect.Width);
            Assert.AreEqual(4, rect.Height);

            // ltrb
            Assert.AreEqual(1, rect.Left);
            Assert.AreEqual(2 + 4, rect.Top);
            Assert.AreEqual(1 + 3, rect.Right);
            Assert.AreEqual(2, rect.Bottom);

            // ltrb vecs
            Assert.AreEqual(new Vec2(1, 2 + 4), rect.TopLeft);
            Assert.AreEqual(new Vec2(1 + 3, 2 + 4), rect.TopRight);
            Assert.AreEqual(new Vec2(1, 2), rect.BottomLeft);
            Assert.AreEqual(new Vec2(1 + 3, 2), rect.BottomRight);
        }

        private void TestEnumeration(IEnumerable<Vec2> enumerable, params Vec2[] expected)
        {
            // build the queue of expected vectors
            Queue<Vec2> queue = new Queue<Vec2>();
            foreach (Vec2 pos in expected)
            {
                queue.Enqueue(pos);
            }

            // enumerate and compare
            foreach (Vec2 pos in enumerable)
            {
                Assert.AreEqual(queue.Dequeue(), pos);
            }

            // make sure we got as many as expected
            Assert.AreEqual(0, queue.Count);
        }
    }
}
