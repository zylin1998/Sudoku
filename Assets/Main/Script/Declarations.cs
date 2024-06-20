using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Declarations
{
    #region Const Field

    public const string Sudoku = "Sudoku";

    #endregion

    #region Static Field

    public static readonly int[] Sizes = { 3, 4 };

    #endregion

    #region Function

    public static int GetRandom(int min, int max) 
    {
        return Random.Range(min, max);
    }

    public static List<int> RandomList(int min, int max, int length)
    {
        return RandomList(min, max, length, (n) => false);
    }

    public static List<int> RandomList(int min, int max, int length, System.Func<int, bool> ignore)
    {
        var list = new List<int>();

        var repeat = 0;
        var limit = length * length * length;
        for (var num = 0; num < length;)
        {
            var random = Random.Range(min, max);

            var contains = list.Contains(random);
            var isIgnore = ignore(random);
            
            if (contains || isIgnore)
            {
                repeat += contains ? 0 : 1;
                
                if (repeat >= limit) { break; }

                continue;
            }

            list.Add(random);

            num++;
        }

        return list;
    }

    public static List<int> EvenlyDistributed(int min, int max, int region) 
    {
        var list   = new List<int>();
        var length = max - min;
        
        for (var i = 0; i < region; i++) 
        {
            var add = RandomList(min, max, length).ConvertAll(num => num + length * i);

            list.AddRange(add);
        }

        return list;
    }

    public static List<int> RandomLine(int min, int max, int shuffleTime)
    {
        return GetLine(min, max).Shuffle(shuffleTime);
    }

    public static List<int> GetLine(int min, int max) 
    {
        var list = new List<int>();

        for (var num = min; num <= max; num++)
        {
            list.Add(num);
        }

        return list;
    }

    public static List<T> Shuffle<T>(this List<T> list, int shuffleTime = 10) 
    {
        var max = list.Count - 1;

        for (var suffle = 0; suffle <= shuffleTime;)
        {
            var first  = Random.Range(0, max);
            var second = Random.Range(0, max);

            if (Equals(first, second)) { continue; }

            var temp = list[first];

            list[first]  = list[second];
            list[second] = temp;

            suffle++;
        }

        return list;
    }

    #endregion
}
