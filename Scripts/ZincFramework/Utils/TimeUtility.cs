using System;
using System.Diagnostics;


namespace ZincFramework
{
    /// <summary>
    /// 整个TimeUtility类都不受到Unity的Time.timeScale影响
    /// </summary>
    public class TimeUtility
    {
        public static readonly DateTime UTCFirstYear = new DateTime(1970, 1, 1);

        /// <summary>
        /// 存储时间间隔之前进行的转换函数,得到的是一个时间戳
        /// </summary>
        /// <param name="day">距离现在多少日</param>
        /// <param name="hour">距离现在多少时</param>
        /// <param name="minute">距离现在多少分钟</param>
        /// <param name="second">距离现在多少秒</param>
        /// <returns></returns>
        public static int GetTimeOffset(int day = 0, int hour = 0,int minute = 0,int second = 0)
        {
            DateTime achievementDate = DateTime.Now + new TimeSpan(day, hour, minute, second);

            TimeSpan timeSpan = achievementDate - UTCFirstYear;
            //得到时间的秒数间隔
            int timeStamp = (int)(timeSpan.Ticks / TimeSpan.TicksPerSecond);
            return timeStamp;
        }

        /// <summary>
        /// 存储时间间隔之前进行的转换函数,得到的是一个时间戳
        /// </summary>
        /// <param name="day">距离现在多少日</param>
        /// <returns></returns>
        public static int GetTimeOffset(int day)
        {
            TimeSpan achievementDate = DateTime.Now.AddDays(day) - UTCFirstYear;
            //得到时间的秒数间隔

            int timeStamp = (int)(achievementDate.Ticks / TimeSpan.TicksPerSecond);
            return timeStamp;
        }

        /// <summary>
        /// 存储时间间隔之前进行的转换函数,得到的是一个时间戳
        /// </summary>
        /// <param name="day">距离现在多少日</param>
        /// <param name="hour">距离现在多少时</param>
        /// <returns></returns>
        public static int GetTimeOffset(int day,int hour)
        {
            TimeSpan achievementDate = DateTime.Now.AddDays(day).AddHours(hour) - UTCFirstYear;
            //得到时间的秒数间隔

            int timeStamp = (int)(achievementDate.Ticks / TimeSpan.TicksPerSecond);
            return timeStamp;
        }

        /// <summary>
        /// 存储时间间隔之前进行的转换函数,得到的是一个时间戳
        /// </summary>
        /// <param name="day">距离现在多少日</param>
        /// <param name="hour">距离现在多少时</param>
        /// <param name="minute">距离现在多少分钟</param>
        /// <returns></returns>
        public static int GetTimeOffset(int day, int hour, int minute)
        {
            TimeSpan achievementDate = DateTime.Now.AddDays(day).AddHours(hour).AddMinutes(minute) - UTCFirstYear;
            //得到时间的秒数间隔

            int timeStamp = (int)(achievementDate.Ticks / TimeSpan.TicksPerSecond);
            return timeStamp;
        }


        /// <summary>
        /// 存储时间间隔之前进行的转换函数,得到的是一个时间戳(使用的是中国古代的计时方式)
        /// </summary>
        /// <param name="day">距离现在多少日</param>
        /// <param name="hour">距离现在多少时辰</param>
        /// <param name="quarter">距离现在多少刻</param>
        /// <returns></returns>
        public static int GetTimeOffsetAnciently(int day, int hour, int quarter)
        {
            TimeSpan achievementDate = DateTime.Now.AddDays(day).AddHours(hour * 2).AddMinutes(quarter * 15) - UTCFirstYear;
            //得到时间的秒数间隔

            int timeStamp = (int)(achievementDate.Ticks / TimeSpan.TicksPerSecond);
            return timeStamp;
        }


        /// <summary>
        /// 检测是否达到时间,需要传入一个时间戳,需要存储时间之后在使用
        /// </summary>
        /// <param name="timeStamp">传入的时间戳, 从GetTimeOffset中得到</param>
        /// <returns></returns>
        public static bool IsReachedTime(int timeStamp)
        {
            //得到时间的秒数间隔
            TimeSpan timeSpan = new TimeSpan(timeStamp * TimeSpan.TicksPerSecond);
            DateTime achievement = UTCFirstYear + timeSpan;

            return (achievement - DateTime.Now).TotalSeconds <= 0;
        }


        /// <summary>
        /// 得到距离时间戳还有多少秒
        /// </summary>
        /// <param name="timeStamp">传入的时间戳</param>
        /// <returns></returns>
        public static int GetCountdown(int timeStamp)
        {
            //得到时间的秒数间隔
            TimeSpan timeSpan = new TimeSpan(timeStamp * TimeSpan.TicksPerSecond);
            DateTime achievement = UTCFirstYear + timeSpan;

            int countdown = (int)(achievement - DateTime.Now).TotalSeconds;
            if(countdown < 0)
            {
                countdown = 0;
            }
            return countdown;
        }

        /// <summary>
        /// 得到当前时间的季节
        /// </summary>
        /// <param name="dateTime">传入的日期</param>
        /// <returns></returns>
        public static string GetSeaSon(DateTime dateTime) => dateTime.Month switch
        {
            >= 3 and <= 5 => "春季",
            >= 6 and <= 8 => "夏季",
            >= 9 and <= 11 => "秋季",
            12 or 1 or 2 => "冬季",
            _ => throw new ArgumentException("传入的日期错误,你不是三体人,不要传入一个小于1或者大于12的月数"),
        };

        /// <summary>
        /// 获取函数运行的时间
        /// </summary>
        /// <returns></returns>
        public static long GetFunctionTime(Action action)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            action.Invoke();
            stopwatch.Stop();

            return stopwatch.ElapsedTicks;
        }
    }
}
