using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

#region Define

using UnityRandom = UnityEngine.Random;

#endregion

namespace Loyufei
{
    public class Random
    {
        private Queue<int> _Seeds;

        public bool HasSeed => _Seeds.Any();
        public int  Seed    => HasSeed ? _Seeds.Dequeue() : 0;

        public void SetSeeds(IEnumerable<int> seeds) 
        {
            _Seeds = new Queue<int>(seeds);
        }
    }

    public static class RandomExtensions 
    {
        public static float Range(this Random self, float min, float max, bool useSeed = false) 
        {
            return useSeed && self.HasSeed ? self.Range(min, max, self.Seed) : UnityRandom.Range(min, max);
        }
        
        public static float Range(this Random self, float min, float max, int seed)
        {
            UnityRandom.InitState(seed);

            return self.Range(min, max);
        }

        public static int Range(this Random self, int min, int max, bool useSeed = false) 
        {
            return useSeed && self.HasSeed ? self.Range(min, max, self.Seed) : UnityRandom.Range(min, max);
        }

        public static int Range(this Random self, int min, int max, int seed) 
        {
            UnityRandom.InitState(seed);

            return self.Range(min, max);
        }

        public static int[] GetArray(this Random self, int min, int max, int count, bool useSeed = false) 
        {
            var array = new int[count];

            for (var index = 0; index < array.Length; index++) 
            {
                array[index] = self.Range(min, max, useSeed);
            }

            return array;
        }

        public static float[] GetArray(this Random self, float min, float max, int count, bool useSeed = false)
        {
            var array = new float[count];

            for (var index = 0; index < array.Length; index++)
            {
                array[index] = self.Range(min, max, useSeed);
            }

            return array;
        }

        public static int[] UniqueArray(this Random self, int min, int max, int count, bool useSeed = false) 
        {
            var array   = new int[count];
            var outLoop = count.Pow(3);
            var repeat  = 0;

            for (var index = 0; index < array.Length;)
            {
                var random = self.Range(min, max, useSeed);

                if (array.Any(n => Equals(n, random))) 
                {
                    repeat++;

                    if (repeat >= outLoop) { break; }

                    continue;
                }

                array[index] = random;

                index++;
            }

            return array;
        }

        public static float[] UniqueArray(this Random self, float min, float max, int count, bool useSeed = false)
        {
            var array   = new float[count];
            var outLoop = count.Pow(3);
            var repeat  = 0;

            for (var index = 0; index < array.Length;)
            {
                var random = self.Range(min, max, useSeed);

                if (array.Any(n => Equals(n, random)))
                {
                    repeat++;

                    if (repeat >= outLoop) { break; }

                    continue;
                }

                array[index] = random;

                index++;
            }

            return array;
        }

        public static int[] EvenlyDistribute(this Random self, int min, int max, int count, int region, bool addUp = true, bool useSeed = false) 
        {
            var list = new List<int>();

            for (var r = 0; r < region; r++) 
            {
                var unique = self.UniqueArray(min, max, count, useSeed).ToList();

                if (addUp) { unique = unique.ConvertAll(n => n + r); }

                list.AddRange(unique);
            }

            return list.ToArray();
        }

        public static float[] EvenlyDistribute(this Random self, float min, float max, int count, int region, bool addUp = true, bool useSeed = false)
        {
            var list = new List<float>();

            for (var r = 0; r < region; r++)
            {
                var unique = self.UniqueArray(min, max, count, useSeed).ToList();

                if (addUp) { unique = unique.ConvertAll(n => n + r); }

                list.AddRange(unique);
            }

            return list.ToArray();
        }
    }
}