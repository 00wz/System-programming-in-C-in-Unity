using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Exercise1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NativeArray<int> array = new NativeArray<int>(10, Allocator.TempJob);
        SomeJob job = new SomeJob();
        job.array = array;
        SetArray(array, 0, 20);
        LogArray(array);

        var handle = job.Schedule();
        handle.Complete();
        LogArray(array);
        array.Dispose();
    }

    private void SetArray(NativeArray<int> arr,int min, int max)
    {
        System.Random rnd = new System.Random();
        int length = arr.Length;
        for (int i = 0; i < length; i++)
        {
            arr[i] = rnd.Next(min, max);
        }
    }

    private void LogArray(NativeArray<int> arr)
    {
        StringBuilder strBild = new StringBuilder();
        int length = arr.Length;
        for (int i = 0; i < length; i++)
        {
            strBild.Append(arr[i] + " ");
        }
        Debug.Log(strBild);
    }

    public struct SomeJob : IJob
    {
        public NativeArray<int> array;
        public void Execute()
        {
            int length = array.Length;
            for(int i = 0; i < length; i++)
            {
                if (array[i] > 10)
                    array[i] = 0;
            }
        }
    }
}
