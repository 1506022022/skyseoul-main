using System;
using System.Collections.Generic;

namespace Util
{
    public static class Enumerator
    {
        public static void InvokeFor<T>(T[] arr, Action<T> action)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                action(arr[i]);
            }
        }
        public static void InvokeFor<T>(IList<T> arr, Action<T> action)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                action(arr[i]);
            }
        }
    }

}
