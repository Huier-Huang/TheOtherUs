using System;
using System.Text;
using BepInEx.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using Hazel;
using InnerNet;


namespace TheOtherUs.Helper;

internal static class LogHelper
{
    private static ManualLogSource logSource { get; set; }

    internal static void SetLogSource(ManualLogSource Source)
    {
        logSource = Source;
    }

    /// <summary>
    ///     一般信息
    /// </summary>
    /// <param name="Message"></param>
    public static void Info(string Message)
    {
        FastLog(LogLevel.Info, Message);
    }

    /// <summary>
    ///     报错
    /// </summary>
    /// <param name="Message"></param>
    public static void Error(string Message)
    {
        FastLog(LogLevel.Error, Message);
    }

    /// <summary>
    ///     测试
    /// </summary>
    /// <param name="Message"></param>
    public static void Debug(string Message)
    {
        FastLog(LogLevel.Debug, Message);
    }

    public static void Fatal(string Message)
    {
        FastLog(LogLevel.Fatal, Message);
    }

    /// <summary>
    ///     警告
    /// </summary>
    /// <param name="Message"></param>
    public static void Warn(string Message)
    {
        FastLog(LogLevel.Warning, Message);
    }


    public static void Message(string Message)
    {
        FastLog(LogLevel.Message, Message);
    }

    public static void Exception(Exception exception)
    {
        Error(exception.ToString());
    }

    public static void FastLog(LogLevel errorLevel, object @object)
    {
        var Logger = logSource;
        var Message = @object as string;
        switch (errorLevel)
        {
            case LogLevel.Message:
                Logger.LogMessage(Message);
                break;
            case LogLevel.Error:
                Logger.LogError(Message);
                break;
            case LogLevel.Warning:
                Logger.LogWarning(Message);
                break;
            case LogLevel.Fatal:
                Logger.LogFatal(Message);
                break;
            case LogLevel.Info:
                Logger.LogInfo(Message);
                break;
            case LogLevel.Debug:
                Logger.LogDebug(Message);
                break;
            case LogLevel.None:
            case LogLevel.All:
            default:
                goto Writer;
        }
        Writer:
        LogFileWriter?.WriteLine($"[FastLog, Level: {errorLevel}] {Message}");

    }

    public static void LogObject(object @object)
    {
        FastLog(LogLevel.Error, @object);
    }
    

    public static void InitConsole()
    {
        System.Console.OutputEncoding = Encoding.UTF8;
    }

    public static readonly string LogDir = Path.Combine(Paths.GameRootPath, "NextLogs");
    internal static StreamWriter LogFileWriter;
    
    public static async void InitLogFile(string fileName)
    {
        if (!Directory.Exists(LogDir))
            Directory.CreateDirectory(LogDir);

        LogFileWriter = new StreamWriter(File.Open(Path.Combine(LogDir, fileName + Main.ModEx), FileMode.OpenOrCreate, FileAccess.Write))
        {
            AutoFlush = true,
        };

        await LogFileWriter.WriteLineAsync("NextLog Listener Start");
        await LogFileWriter.WriteLineAsync($"CurrentTime: {DateTime.Now:g}");
    }
}

internal static class LogListener
{
    #nullable enable
    private static bool Started;
    public static readonly HarmonyMethod OnRPCMethod = new(typeof(LogListener), nameof(OnRpc));
    public static Harmony? _Harmony { get; private set; }
    internal static void Start(Harmony? harmony = null)
    {
        if (Started)
            return;
        _Harmony = harmony ?? new Harmony("LogListener.TargetMethod.HandleRpc");
        foreach (var method in targetMethodBases())
        {
            _Harmony.Patch(method, postfix: OnRPCMethod);
        }
        Started = true;
    }

    internal static void UnPatch()
    {
        if (!Started)
            return;

        foreach (var method in targetMethodBases())
        {
            _Harmony?.Unpatch(method, OnRPCMethod.method);
        }
        
        Started = false;
    }
    #nullable disable
    
    private static IEnumerable<MethodBase> targetMethodBases() => typeof(AmongUsClient).Assembly.GetTypes()
        .Where(n => n.IsSubclassOf(typeof(InnerNetObject)))
        .Select(x => x.GetMethod(nameof(InnerNetObject.HandleRpc), AccessTools.allDeclared))
        .Where(m => m != null);

    internal static void OnRpc(InnerNetObject __instance, [HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
    {
        Info($"{__instance.name} {callId} {reader.Length} {reader.Tag}");
    }
}