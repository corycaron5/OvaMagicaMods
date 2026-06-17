using MelonLoader;
using MelonLoader.Logging;

namespace EggCore;

public class EggLogger
{
    public static LogLevel Level { get; set; } = LogLevel.Warning;
    public string Prefix { get; set; } = "";
    // ReSharper disable once UnusedMember.Local
    private EggLogger() { }

    internal EggLogger(string section)
    {
        Prefix = section.Equals("") ? "" : "[" + section + "] ";
    }

    public void Log(LogLevel level, string message)
    {
        if(Level >= level) Melon<EggCore>.Logger.Msg(GetLoggerColor(level),Prefix + message);
    }
    
    public void CriticalMessage(string message) => Log(LogLevel.Critical, message);
    public void ErrorMessage(string message) => Log(LogLevel.Error, message);
    public void WarningMessage(string message) => Log(LogLevel.Warning, message);
    public void InfoMessage(string message) => Log(LogLevel.Info, message);
    public void DebugMessage(string message) => Log(LogLevel.Debug, message);

    public static ColorARGB GetLoggerColor(LogLevel level)
    {
        return level switch
        {
            LogLevel.Critical => ColorARGB.Red,
            LogLevel.Error => ColorARGB.Orange,
            LogLevel.Warning => ColorARGB.Yellow,
            LogLevel.Info => ColorARGB.White,
            LogLevel.Debug => ColorARGB.Gray,
            _ => ColorARGB.White
        };
    }
    
    public static LogLevel GetLogLevel(int level)
    {
        return level switch
        {
            0 => LogLevel.Critical,
            1 => LogLevel.Error,
            2 => LogLevel.Warning,
            3 => LogLevel.Info,
            4 => LogLevel.Debug,
            _ => LogLevel.Warning
        };
    }
    
    public enum LogLevel
    {
        Critical = 0,
        Error = 1,
        Warning = 2,
        Info = 3,
        Debug = 4
    }
}