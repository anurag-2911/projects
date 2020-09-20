using System;

namespace CodeLibrary
{
    public class TimeScheduler
    {


        #region Step 1
        
        public string GetTimeOfDay()
        {
            DateTime time = DateTime.Now;

            if (time.Hour >= 0 && time.Hour < 6)
            {
                return "Night";
            }
            if (time.Hour >= 6 && time.Hour < 12)
            {
                return "Morning";
            }
            if (time.Hour >= 12 && time.Hour < 18)
            {
                return "Afternoon";
            }
            return "Evening";
        } 

        #endregion

        #region Step 2

        public string GetTimeOfDay(DateTime dateTime)
        {
            if (dateTime.Hour >= 0 && dateTime.Hour < 6)
            {
                return "Night";
            }
            if (dateTime.Hour >= 6 && dateTime.Hour < 12)
            {
                return "Morning";
            }
            if (dateTime.Hour >= 12 && dateTime.Hour < 18)
            {
                return "Noon";
            }
            return "Evening";
        }

        #endregion

        #region Step 3
        
        public bool ShallIWork()
        {
            DateTime dateTime = DateTime.Now;
            if (GetTimeOfDay(dateTime) == "Morning" || GetTimeOfDay(dateTime) == "Afternoon")
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        #endregion

        #region Step 4
        
        IDateTimeProvider dateTimeProvider;
        public TimeScheduler(IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Step 5

        public bool ShallIWork01()
        {
            DateTime dateTime = dateTimeProvider.GetDateTime();
            if (GetTimeOfDay(dateTime) == "Morning" || GetTimeOfDay(dateTime) == "Afternoon")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
    #region Step 4-a

    
    public interface IDateTimeProvider
    {
        DateTime GetDateTime();
    } 

    #endregion
}
