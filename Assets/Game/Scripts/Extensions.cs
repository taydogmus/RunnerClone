using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Taydogmus
{
    public static class Extensions
    {
        public static List<T> GetEnumValuesList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
        
        public static void Shuffle<T>(this List<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Range(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}