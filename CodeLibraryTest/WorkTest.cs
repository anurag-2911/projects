using CodeLibrary;
using NUnit.Framework;
using System;

namespace CodeLibraryTest
{
    [TestFixture]
    public class WorkTest
    {
        TimeScheduler timeScheduler;
        [Test]
        public virtual void TestMorningTime()
        {
            IDateTimeProvider dateTimeProvider = new MorningTime();
            timeScheduler = new TimeScheduler(dateTimeProvider);
            bool shallIwork=timeScheduler.ShallIWork01();
            Assert.AreEqual(true, shallIwork);

            shallIwork = (new TimeScheduler(new EveningTime())).ShallIWork01();

            Assert.AreEqual(false, shallIwork);

            

        }
            

    }

    public class MorningTime : IDateTimeProvider
    {
        public DateTime GetDateTime()
        {
            return new DateTime(2020, 09, 12, 11, 0, 0);
        }
    }
    public class EveningTime : IDateTimeProvider
    {
        public DateTime GetDateTime()
        {
            return new DateTime(2020, 09, 19, 11, 0, 0);
        }
    }
}
