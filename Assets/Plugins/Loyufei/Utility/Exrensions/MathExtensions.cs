using System;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    public static class MathExtensions
    {
        public static int Pow(this int self, int pow) 
        {
            return (int)Math.Pow(self, pow);
        }

        public static float Pow(this float self, float pow)
        {
            return (float)Math.Pow(self, pow);
        }

        public static double Pow(this double self, double pow)
        {
            return Math.Pow(self, pow);
        }

        public static int Clamp(this int self, int min, int max) 
        {
            return Math.Clamp(self, min, max);
        }

        public static float Clamp(this float self, float min, float max)
        {
            return Math.Clamp(self, min, max);
        }

        public static double Clamp(this double self, double min, double max)
        {
            return Math.Clamp(self, min, max);
        }

        public static int Clamp01(this int self)
        {
            return Math.Clamp(self, 0, 1);
        }

        public static float Clamp01(this float self)
        {
            return Math.Clamp(self, 0f, 1f);
        }

        public static double Clamp01(this double self)
        {
            return Math.Clamp(self, 0d, 1d);
        }

        public static bool IsClamp(this int self, int min, int max) 
        {
            return self == self.Clamp(min, max);
        }

        public static bool IsClamp(this float self, float min, float max)
        {
            return self == self.Clamp(min, max);
        }

        public static bool IsClamp(this double self, double min, double max)
        {
            return self == self.Clamp(min, max);
        }

        public static int Abs(this int self) 
        {
            return self >= 0 ? self : -self;
        }

        public static float Abs(this float self)
        {
            return self >= 0 ? self : -self;
        }

        public static double Abs(this double self)
        {
            return self >= 0 ? self : -self;
        }
    }
}