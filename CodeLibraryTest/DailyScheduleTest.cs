using Moq;
using Novell.Zenworks.Tess.Scheduler.Schedules;
using NUnit.Framework;
using System;
using System.Globalization;

namespace TessUnitTest
{
    enum DaysBitMap
    {
        Sunday = 1,
        Monday = 2,
        Tuesday = 4,
        Wednesday = 8,
        Thursday = 16,
        Friday = 32,
        Saturday = 64
    }
    [TestFixture]
    public class DailyScheduleTest
    {
        DailySchedule dailySchedule;
        DateTime currentTime;
        GregorianCalendar gregorianCalendar = new GregorianCalendar(System.Globalization.GregorianCalendarTypes.Localized);
        
        /// <summary>
        /// This method will setup the DailySchedule class with some default data like start time,end time and current time
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            dailySchedule = new DailySchedule();

            //start time as 11.00 am
            dailySchedule.StartDateTime = new DateTime(2020, 9, 21, 11, 0, 0);
            //end time as 11.30 am
            dailySchedule.EndDateTime = new DateTime(2020, 9, 21, 11, 30, 0);
            // current time as 11.15 am
            currentTime = new DateTime(2020, 9, 21, 11, 15, 0);
            // schedule has start and end time so randomdispatch is true

            dailySchedule.RandomDispatch = true;
            // as this is schedule to run on a friday ,so days bit map is 32
            dailySchedule.DaysBitmap = (Int32)DaysBitMap.Monday;

        }
        /// <summary>
        /// This will test isInSchedule method of Daily Schedule
        /// current time is between start and end time: it should return true
        /// </summary>
        [Test]
        public virtual void IsInSchedule_CurrentTimeBetweenStartAndEndTime()
        {
            //Arrange
            var dailysch = new Mock<DailySchedule>();
            //start time as 11.00 am
            dailysch.Setup(x=>x.StartDateTime).Returns(new DateTime(2020, 9, 21, 11, 0, 0));
            //end time as 11.30 am
            dailysch.Setup(x=>x.EndDateTime).Returns(new DateTime(2020, 9, 21, 11, 30, 0));
            // current time as 11.15 am
            currentTime = new DateTime(2020, 9, 21, 11, 15, 0);
            // schedule has start and end time so randomdispatch is true
            
            dailysch.Setup(x=>x.RandomDispatch).Returns(true);
            // as this is schedule to run on a friday ,so days bit map is 32
            dailysch.Setup(x=>x.DaysBitmap).Returns((Int32)DaysBitMap.Monday);

            // this schedule is not yet triggered so make below flag as false
            dailysch.Setup(_ => _.IsWithinTheScheduleAndAlreadyTriggered(gregorianCalendar)).Returns(false);



            bool isInSchedule = dailysch.Object.isInSchedule(currentTime);//Act
            dailysch.Verify();
            
            
            Assert.AreEqual(true, isInSchedule);//Assert

        }

        /// <summary>
        /// Current time is lesser than start time: it should return false
        /// </summary>
      
        [Test]

        public virtual void IsInSchedule_CurrentTimeBeforeStartTime()
        {
            
            currentTime = currentTime.AddMinutes(-17);//Arrange
                                                      //Arrange
            var dailysch = new Mock<DailySchedule>();

            // this schedule is not yet triggered so make below flag as false
            dailysch.Setup(x => x.IsWithinTheScheduleAndAlreadyTriggered(gregorianCalendar)).Returns(false);

            bool isInSchedule = dailysch.Object.isInSchedule(currentTime);//Act
           

            Assert.AreNotEqual(true, isInSchedule);//Assert
        }
        
        
       
        /// <summary>
        /// Tests getDaysToNextFire method of DailySchedule when only one day is scheduled.
        /// </summary>
        [Test]
        [Ignore]
        public virtual void NextDayToFire_OnlyOneDayIsScheduled()
        {
            dailySchedule.DaysBitmap = (Int32)DaysBitMap.Monday;
            int nextdayTofire = dailySchedule.getDaysToNextFire((int)DateTime.Now.DayOfWeek, true);// it means it has fired for monday calc next day

            Assert.AreEqual(7, nextdayTofire);

        }
        /// <summary>
        /// Tests getDaysToNextFire method when multiple days are scheduled.
        /// </summary>
        [Test]
        [Ignore]
        public virtual void NextDayToFire_MultipleDaysScheduled()
        {
            dailySchedule.DaysBitmap = (Int32)DaysBitMap.Monday + (Int32)DaysBitMap.Thursday;
            int nextdayTofire = dailySchedule.getDaysToNextFire((int)DateTime.Now.DayOfWeek, true);// it means it has fired for monday ,calc next day

            Assert.AreEqual(3, nextdayTofire);
        }
        /// <summary>
        /// Test initial due time when current time is lesser than scheduled time :it should be scheduled time
        /// </summary>
        [Test]
        public virtual void InitialDueTime_CurrentTimeBeforeScheduledTime()
        {
            dailySchedule.DaysBitmap = (Int32)DaysBitMap.Friday;
            dailySchedule.EndDateTime = GetDefaultEndDateTimeWhenRandomTimeisOff();
            dailySchedule.RandomDispatch = false;
            //for case curret time <start time so in that case start time should be initial due time
            currentTime = new DateTime(2018, 8, 10, 10, 15, 0);

            dailySchedule.calculateInitialDueTime(currentTime);
            long initialDueTime = dailySchedule.getCurrentDueTime();

            long startTime = dailySchedule.StartDateTime.Ticks;

            Assert.AreEqual(startTime, initialDueTime);

        }
        /// <summary>
        /// Test initial due time when current time> scheduled time :it should be next scheduled time
        /// </summary>
        [Test]
        public virtual void InitialDueTime_CurrentTimeAfterScheduledTime()
        {
            dailySchedule.DaysBitmap = (Int32)DaysBitMap.Friday;
            dailySchedule.RandomDispatch = false;
            dailySchedule.EndDateTime = GetDefaultEndDateTimeWhenRandomTimeisOff();
            currentTime = new DateTime(2018, 8, 10, 12, 30, 0);

            dailySchedule.calculateInitialDueTime(currentTime);
            long initialDueTime = dailySchedule.getCurrentDueTime();
            long nextdueTime = dailySchedule.StartDateTime.AddDays(7).Ticks;// next friday

            Assert.AreEqual(nextdueTime, initialDueTime);

        }

        /// <summary>
        /// Test initial due time when schedule is for 2 days lets say friday and saturday and current time is after schedule time of friday
        /// </summary>
        [Test]
        public virtual void InitialDueTime_CurrentTimeAfterScheduledTimeAndMultipleDays()
        {
            dailySchedule.DaysBitmap = (Int32)DaysBitMap.Friday + (Int32)DaysBitMap.Saturday;
            dailySchedule.RandomDispatch = false;
            dailySchedule.EndDateTime = GetDefaultEndDateTimeWhenRandomTimeisOff();
            currentTime = new DateTime(2018, 8, 10, 12, 30, 0);

            dailySchedule.calculateInitialDueTime(currentTime);
            long initialDueTime = dailySchedule.getCurrentDueTime();
            long nextdueTime = dailySchedule.StartDateTime.AddDays(1).Ticks;// next day

            Assert.AreEqual(nextdueTime, initialDueTime);

        }


        /// <summary>
        /// Tests when current time==start time of the schedule
        /// </summary>
        [Test]
        public virtual void InitialDueTime_CurrentTimeIsEqualToStartTime()
        {
            dailySchedule.DaysBitmap = (Int32)DaysBitMap.Friday;
            dailySchedule.RandomDispatch = false;
            dailySchedule.EndDateTime = GetDefaultEndDateTimeWhenRandomTimeisOff();

            dailySchedule.calculateInitialDueTime(dailySchedule.StartDateTime);
            long initialDueTime = dailySchedule.getCurrentDueTime();

            Assert.AreEqual(dailySchedule.StartDateTime.Ticks, initialDueTime);
        }
        /// <summary>
        /// tests calculateNextDueTime method when current time > scheduled time so that next due time is next scheduled time of the daily schedule
        /// </summary>
        [Test]
        public virtual void CalculateNextDueTime_CurrentTimeAfterScheduledTime()
        {
            dailySchedule.DaysBitmap = (Int32)DaysBitMap.Friday;
            dailySchedule.RandomDispatch = false;
            dailySchedule.EndDateTime = new DateTime(9999, 12, 31, 11, 59, 59);
            currentTime = new DateTime(2018, 8, 10, 12, 30, 0);

            dailySchedule.calculateNextDueTime(currentTime);
            long nextdueTime = dailySchedule.getCurrentDueTime();

            Assert.AreEqual(dailySchedule.StartDateTime.AddDays(7).Ticks, nextdueTime);
        }

        /// <summary>
        /// tests calculateNextDueTimeFromDate method when current time > scheduled time so that next due time is next scheduled time of the daily schedule
        /// </summary>
        [Test]
        public virtual void CalculateNextDueTimeFromDate_CurrentTimeAfterScheduledTime()
        {
            dailySchedule.DaysBitmap = (Int32)DaysBitMap.Friday + (Int32)DaysBitMap.Wednesday;
            dailySchedule.RandomDispatch = false;
            dailySchedule.EndDateTime = new DateTime(9999, 12, 31, 11, 59, 59);

            dailySchedule.calculateNextDueTimeFromDate(new DateTime(2018, 8, 10, 12, 30, 0));
            long nextdueTime = dailySchedule.getCurrentDueTime();

            Assert.AreEqual(dailySchedule.StartDateTime.AddDays(5).Ticks, nextdueTime);

        }

       

        private DateTime GetDefaultEndDateTimeWhenRandomTimeisOff()
        {
            return new DateTime(9999, 12, 31, 11, 59, 59);
        }


    }
}
