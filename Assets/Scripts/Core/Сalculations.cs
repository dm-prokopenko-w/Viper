using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public static class Сalculations
    {
        public static float AspectRatio => Screen.width > Screen.height ? Screen.height : Screen.width;

        public static int GetSize(int count) => count % 2 > 0 ? (count - 1) / 2 : count / 2;

        public static bool IsUseZero(int count) => count % 2 > 0;
        
        public static List<int> GetLineList(int count)
        {
            int size = GetSize(count);

            List<int> nums = new();
            for (int i = -size; i < 0; i++)
            {
                nums.Add(i);
            }

            if (count % 2 > 0)
            {
                nums.Add(0);
            }

            for (int i = 1; i < size + 1; i++)
            {
                nums.Add(i);
            }

            return nums;
        }
    }
}