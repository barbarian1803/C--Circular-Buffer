using System;

namespace CircularBufferLib;

public static class Helper
{
    public static int CalculateCapacity(int n){
        // Calculate the number of bits in the given integer
        int numBits = 0;
        int temp = n;
        while (temp > 0)
        {
            temp >>= 1;
            numBits++;
        }

        // Calculate the nearest number with all ones in binary representation
        int allOnes = (1 << numBits) - 1;

        // If the given number is greater than the all ones number, 
        // we need to add one more bit and subtract the given number from it
        if (n > allOnes)
        {
            allOnes = (1 << (numBits + 1)) - 1;
        }

        return allOnes;
    }

    public static int IncrementCounterWithMask(int n, int mask){
        return ((n+1) & mask);
    }
}
