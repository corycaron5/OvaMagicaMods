using Il2CppOvaMagica;

namespace EggCore.Utils;

public static class EggTimeUtils
{
    //See Il2CppOvaMagica -> GameTimeUtil for additional utility functions

    public const int TimePerYear = 57600;
    public const int TimePerSeason = 19200;
    public const int TimePerWeek = 3840;
    public const int TimePerDay = 960;
    public const int TimePerHour = 60;

    /// <summary>
    /// Calculates the number of complete years that have passed given a specific time value.
    /// </summary>
    /// <param name="time">The time value to be converted into years.</param>
    /// <returns>The number of complete years that have passed.</returns>
    public static int GetTimeInYears(int time)
    {
        return (int)Math.Floor((time - TimePerYear) / (float)TimePerYear);
    }

    /// <summary>
    /// Calculates the number of complete seasons that have passed given a specific time value.
    /// </summary>
    /// <param name="time">The time value to be converted into seasons.</param>
    /// <returns>The number of complete seasons that have passed.</returns>
    public static int GetTimeInSeasons(int time)
    {
        return (int)Math.Floor((time - TimePerYear) / (float)TimePerSeason);
    }

    /// <summary>
    /// Calculates the number of complete weeks that have passed given a specific time value.
    /// </summary>
    /// <param name="time">The time value to be converted into weeks.</param>
    /// <returns>The number of complete weeks that have passed.</returns>
    public static int GetTimeInWeeks(int time)
    {
        return (int)Math.Floor((time - TimePerYear) / (float)TimePerWeek);
    }

    /// <summary>
    /// Calculates the number of complete days that have passed given a specific time value.
    /// </summary>
    /// <param name="time">The time value to be converted into days.</param>
    /// <returns>The number of complete days that have passed.</returns>
    public static int GetTimeInDays(int time)
    {
        return (int)Math.Floor((time - TimePerYear) / (float)TimePerDay);
    }

    /// <summary>
    /// Calculates the number of complete hours that have passed given a specific time value.
    /// </summary>
    /// <param name="time">The time value to be converted into hours.</param>
    /// <returns>The number of complete hours that have passed.</returns>
    public static int GetTimeInHours(int time)
    {
        return (int)Math.Floor((time - TimePerYear) / (float)TimePerHour);
    }

    /// <summary>
    /// Converts a given time value into a formatted string representing the elapsed time
    /// in terms of years, seasons, weeks, days, hours, and minutes.
    /// </summary>
    /// <param name="time">The time value to be converted, expressed in minutes since a starting point.</param>
    /// <returns>A formatted string in the format "Xy Ys Ww Dd Hh Mm" where X, Y, W, D, H, and M
    /// represent the number of years, seasons, weeks, days, hours, and minutes respectively.</returns>
    public static string GetFormattedTime(int time)
    {
        int total = 0;
        int years = GetTimeInYears(time);
        total += years * TimePerYear;
        int seasons = GetTimeInSeasons(time - total);
        total += seasons * TimePerSeason;
        int weeks = GetTimeInWeeks(time - total);
        total += weeks * TimePerWeek;
        int days = GetTimeInDays(time - total);
        total += days * TimePerDay;
        int hours = GetTimeInHours(time - total);
        total += hours * TimePerHour;
        int minutes = time - TimePerYear - total;
        return (years + 1) + "y " + (seasons + 1) + "s " + (weeks + 1) + "w " + (days + 1) + "d " + (hours + 8) + "h " +
               minutes + "m";
    }

    public static bool TogglePause()
    {
        TimeLogic.current.stopTime = !TimeLogic.current.stopTime;
        return TimeLogic.current.stopTime;
    }

    public static void PauseGame(bool pause)
    {
        TimeLogic.current.stopTime = pause;
    }
    
    public static bool IsGamePaused()
    {
        return TimeLogic.current.stopTime;
    }

    public static int GetCurrentTime()
    {
        return GameData.current.time;
    }

    public static void SetTimeOfDay(int hour, int minute)
    {
        int day = GetTimeInDays(GetCurrentTime());
        EggCore.DebugMessage("Day: " + day);
        TimeLogic.SetTime(TimePerYear + day * TimePerDay + hour * TimePerHour + minute);
    }

    public static void SetDay(int day)
    {
        int hour = GameTimeUtil.GetHour(GetCurrentTime());
        TimeLogic.SetTime(TimePerYear + day * TimePerDay + hour * TimePerHour);
    }

    public enum Season
    {
        Solis = 0,
        Ceres = 1,
        Frigus = 2,
        Invalid = 3
    }

    public static Season GetSeason(int season)
    {
        return (season % 3) switch
        {
            0 => Season.Solis,
            1 => Season.Ceres,
            2 => Season.Frigus,
            _ => Season.Invalid
        };
    }

    public static List<int> GetFallingStarDays(int season)
    {
        return GetFallingStarDays(GetSeason(season));
    }
    
    public static List<int> GetFallingStarDays(Season season)
    {
        return season switch
        {
            Season.Solis => Il2CppUtils.ConvertToSystemList(GameLogic.Current.festivalDatabase.fallingStarDaysSolis),
            Season.Ceres => Il2CppUtils.ConvertToSystemList(GameLogic.Current.festivalDatabase.fallingStarDaysCeres),
            Season.Frigus => Il2CppUtils.ConvertToSystemList(GameLogic.Current.festivalDatabase.fallingStarDaysFrigus),
            _ => new List<int>()
        };
    }

    private static readonly List<int> DefaultFallingStarDays = new List<int>{4,14};

    public static void ResetDefaultFallingStarDays()
    {
        GameLogic.Current.festivalDatabase.fallingStarDaysSolis = Il2CppUtils.ConvertToIl2CppList(DefaultFallingStarDays);
        GameLogic.Current.festivalDatabase.fallingStarDaysCeres = Il2CppUtils.ConvertToIl2CppList(DefaultFallingStarDays);
        GameLogic.Current.festivalDatabase.fallingStarDaysFrigus = Il2CppUtils.ConvertToIl2CppList(DefaultFallingStarDays);
    }

    public static bool AddFallingStarDay(int season, int day)
    {
        return AddFallingStarDay(GetSeason(season), day);
    }

    public static bool AddFallingStarDay(Season season, int day)
    {
        if((int)season is < 0 or > 2)
        {
            EggCore.ErrorMessage("Invalid season.");
            return false;
        }
        if (day is < 0 or > 19)
        {
            EggCore.ErrorMessage("Day must be between 0 and 19.");
            return false;
        }
        FestivalDatabase database = GameLogic.Current.festivalDatabase;
        if (database.IssFallingStars((int)season, day))
        {
            EggCore.InfoMessage("Falling star day already exists.");
            return false;
        }
        if (database.GetFestival((int) season, day) != null)
        {
            EggCore.InfoMessage("Festival already exists on this day.");
            return false;
        }
        switch (season)
        {
            case Season.Solis:
                database.fallingStarDaysSolis.Add(day);
                EggCore.DebugMessage("Added falling star day to Solis: " + day);
                return true;
            case Season.Ceres:
                database.fallingStarDaysCeres.Add(day);
                EggCore.DebugMessage("Added falling star day to Ceres: " + day);
                return true;
            case Season.Frigus:
                database.fallingStarDaysFrigus.Add(day);
                EggCore.DebugMessage("Added falling star day to Frigus: " + day);
                return true;
            default:
                EggCore.ErrorMessage("Invalid season.");
                return false;
        }
    }
}