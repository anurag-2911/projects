using CodeLibrary;
using Moq;
using NUnit.Framework;
using System;

namespace CodeLibraryTest
{
    [TestFixture]
    public class SortingAlgoTester
    {
        SortingAlgo sort = null;
        [TestFixtureSetUp]
        public void ClassSetup()
        {

        }
        [SetUp]
        public void MethodSetup()
        {

        }

        [TestFixtureTearDown]
        public void CleanUp()
        {

        }

        [Test]
        public virtual void TestBasicSortingAlgo()
        {

            // Arrange

            int[] array = { 23, 4, 56, 1 };

            int[] bubbleSortData = new int[array.Length];
            int[] insertionSortData = new int[array.Length];
            int[] mergeSortData = new int[array.Length];
            int[] copyArray = new int[array.Length];

            

            Array.Copy(array, copyArray, array.Length);
            Array.Copy(array, bubbleSortData, array.Length);
            Array.Copy(array, insertionSortData, array.Length);
            Array.Copy(array, mergeSortData, array.Length);




            //Act

            Array.Sort(copyArray);


            sort = new BubbleSort();
            sort.Sort(bubbleSortData);

            sort = new InsertionSort();
            sort.Sort(insertionSortData);

            sort = new MergeSort();
            sort.Sort(mergeSortData);


            //Assert

            bool areEqual;
            areEqual = AreTwoArraysEqual(bubbleSortData, copyArray);

            Assert.AreEqual(true, areEqual);

           

            areEqual = AreTwoArraysEqual(insertionSortData, copyArray);

            Assert.AreEqual(true, areEqual);

            
            areEqual = AreTwoArraysEqual(mergeSortData, copyArray);

            Assert.AreEqual(true, areEqual);



        }

        [TestCase(new int[] { 23, 44, 6, 99, 1 })]
        [TestCase(new int[] { 6, 98, 1, 22, 77 })]
        [TestCase(new int[] { 99, 44, 66, 99, 11 })]
        [TestCase(new int[] { 2, 44, 608, 9, 1 })]
        public virtual void TestSortAlgo_ParameterizedInput(int[] array)
        {
            int[] unsortedArray = array;
            sort = new BubbleSort();
            sort.Sort(unsortedArray);
            int[] copy = array;
            Array.Sort(copy);

            bool isEqual = true;
            for (int i = 0; i < array.Length; i++)
            {
                if(unsortedArray[i]!=copy[i])
                {
                    isEqual = false;
                    break;
                }
            }
            Assert.AreEqual(true, isEqual);
        }
        private static bool AreTwoArraysEqual(int[] array, int[] copyArray)
        {
            bool areEqual = true;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != copyArray[i])
                {
                    areEqual = false;
                }
            }

            return areEqual;
        }

        [Test]
        public virtual void TestMock()
        {
            var session = new Mock<ISession>();
            
            session.Setup(x => x.sessionID).Returns("12345");
            session.Setup(_ => _.IsUserSession()).Returns(true);

            Assert.AreEqual(true,session.Object.IsUserSession());
            session.Verify(x => x.IsUserSession(), Times.Once());
        }
    }
}
