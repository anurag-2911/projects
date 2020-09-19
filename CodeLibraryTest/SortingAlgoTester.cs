using CodeLibrary;
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
        public virtual void TestBubbleSort()
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
    }
}
