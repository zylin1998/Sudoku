using System;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei 
{
    public interface IOffset2D 
    {
        public float X { get; }
        public float Y { get; }
    }

    public interface IOffset : IOffset2D
    {
        public float Z { get; }
    }

    public interface IOffset2DInt
    {
        public int X { get; }
        public int Y { get; }
    }

    public interface IOffsetInt : IOffset2DInt
    {
        public int Z { get; }
    }

    [Serializable]
    public struct Offset : IOffset, IOffsetInt
    {
        public Offset(float x, float y, float z)
            => (X, Y, Z) = (x, y, z);

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        int IOffset2DInt.X => (int)X;
        int IOffset2DInt.Y => (int)Y;
        int IOffsetInt  .Z => (int)Z;

        public static Offset operator +(Offset first, Offset second)
        {
            return new(first.X + second.X, first.Y + second.Y, first.Z + second.Z);
        }

        public static Offset operator -(Offset first, Offset second)
        {
            return new(first.X - second.X, first.Y - second.Y, first.Z - second.Z);
        }

        public static Offset operator -(Offset first)
        {
            return new(-first.X, -first.Y, -first.Z);
        }
    }

    [Serializable]
    public struct Offset2D : IOffset2D, IOffset2DInt
    {
        public Offset2D(float x, float y)
            => (X, Y) = (x, y);

        public float X { get; set; }
        public float Y { get; set; }

        int IOffset2DInt.X => (int)X;
        int IOffset2DInt.Y => (int)Y;

        public static Offset2D operator +(Offset2D first, Offset2D second)
        {
            return new(first.X + second.X, first.Y + second.Y);
        }

        public static Offset2D operator -(Offset2D first, Offset2D second)
        {
            return new(first.X - second.X, first.Y - second.Y);
        }

        public static Offset2D operator -(Offset2D first)
        {
            return new(-first.X, -first.Y);
        }
    }

    [Serializable]
    public struct OffsetInt : IOffsetInt
    {
        public OffsetInt(int x, int y, int z)
            => (X, Y, Z) = (x, y, z);

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public static OffsetInt operator +(OffsetInt first, OffsetInt second)
        {
            return new(first.X + second.X, first.Y + second.Y, first.Z + second.Z);
        }

        public static OffsetInt operator -(OffsetInt first, OffsetInt second)
        {
            return new(first.X - second.X, first.Y - second.Y, first.Z - second.Z);
        }

        public static OffsetInt operator -(OffsetInt first)
        {
            return new(-first.X, -first.Y, -first.Z);
        }
    }

    [Serializable]
    public struct Offset2DInt : IOffset2DInt
    {
        public Offset2DInt(int x, int y)
            => (X, Y) = (x, y);

        public int X { get; set; }
        public int Y { get; set; }

        public static Offset2DInt operator +(Offset2DInt first, Offset2DInt second)
        {
            return new(first.X + second.X, first.Y + second.Y);
        }

        public static Offset2DInt operator -(Offset2DInt first, Offset2DInt second)
        {
            return new(first.X - second.X, first.Y - second.Y);
        }

        public static Offset2DInt operator -(Offset2DInt first)
        {
            return new(-first.X, -first.Y);
        }

        public static bool operator ==(Offset2DInt first, Offset2DInt second) 
        {
            return first.X == second.X && first.Y == second.Y;
        }

        public static bool operator !=(Offset2DInt first, Offset2DInt second)
        {
            return first.X != second.X || first.Y != second.Y;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }
    }
}
