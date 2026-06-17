using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace EggCore.Utils;

public static class Il2CppUtils
{
    /// <summary>
    /// Converts an Il2CppSystem.Collections.Generic.List to a .NET List.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="il2CPPList">The Il2CppSystem.Collections.Generic.List to convert.</param>
    /// <returns>A new .NET List containing the same elements as the input list.</returns>
    public static List<T> ConvertToSystemList<T>(Il2CppSystem.Collections.Generic.List<T> il2CPPList)
    {
        List<T> list = new List<T>();
        foreach (var entry in il2CPPList)
        {
            list.Add(entry);
        }
        return list;
    }
    
    /// <summary>
    /// Converts a .NET List to an Il2CppSystem.Collections.Generic.List.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="systemList">The .NET List to convert.</param>
    /// <returns>A new Il2CppSystem.Collections.Generic.List containing the same elements as the input list.</returns>
    public static Il2CppSystem.Collections.Generic.List<T> ConvertToIl2CppList<T>(List<T> systemList)
    {
        Il2CppSystem.Collections.Generic.List<T> list = new Il2CppSystem.Collections.Generic.List<T>();
        foreach (var entry in systemList)
        {
            list.Add(entry);
        }
        return list;
    }

    /// <summary>
    /// Converts an Il2CppArrayBase to a .NET List.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the array</typeparam>
    /// <param name="il2CPPArray">The Il2Cpp array to convert</param>
    /// <returns>A new .NET List containing the same elements as the input array</returns>
    public static List<T> ConvertToSystemList<T>(Il2CppArrayBase<T> il2CPPArray) 
    {
        List<T> list = new List<T>();
        foreach (var entry in il2CPPArray)
        {
            list.Add(entry);
        }
        return list;
    }
    
}