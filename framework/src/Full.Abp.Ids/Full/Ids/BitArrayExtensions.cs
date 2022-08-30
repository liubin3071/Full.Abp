using System.Collections;

namespace Full.Ids;

public static class BitArrayExtensions
{
    public static BitArray CopySlice(this BitArray source, int offset, int length)
    {
        // Urgh: no CopyTo which only copies part of the BitArray
        var ret = new BitArray(length);
        for (var i = 0; i < length; i++)
        {
            ret[i] = source[offset + i];
        }

        return ret;
    }

    public static void CopyTo(this BitArray source, int offset, BitArray dest, int destOffset, int length)
    {
        for (var i = 0; i < length; i++)
        {
            dest[destOffset + i] = source[offset + i];
        }
    }

    public static void PadLeft(this BitArray source, int length, bool value = false)
    {
        var srcCount = source.Count;
        var count = length - srcCount;
        if (count <= 0)
        {
            return;
        }

        source.Length = length;
        source.LeftShift(count);
        if (!value)
        {
            return;
        }

        var or = new BitArray(length, true);
        new BitArray(srcCount, false).CopyTo(0, or, count, srcCount);
        source.Or(or);
    }

    public static void PadRight(this BitArray source, int length, bool value = false)
    {
        var srcCount = source.Count;
        var count = length - srcCount;
        if (count <= 0)
        {
            return;
        }

        source.Length = length;
        // source.LeftShift(count);
        if (!value)
        {
            return;
        }

        var or = new BitArray(length, true);
        new BitArray(srcCount, false).CopyTo(0, or, 0, srcCount);
        source.Or(or);
    }
}