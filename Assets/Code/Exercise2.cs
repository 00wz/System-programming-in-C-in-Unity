using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Exercise2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NativeArray<Vector3> Positions = new NativeArray<Vector3>(3, Allocator.TempJob);
        NativeArray<Vector3> Velocities = new NativeArray<Vector3>(3, Allocator.TempJob);
        NativeArray<Vector3> FinalPositions = new NativeArray<Vector3>(3, Allocator.TempJob);
        SomeJob job = new SomeJob();
        job.Positions = Positions;
        job.Velocities = Velocities;
        job.FinalPositions = FinalPositions;
        SetArray(Positions);
        SetArray(Velocities);
        LogArray(Positions);
        LogArray(Velocities);

        var handle = job.Schedule(3,0);
        handle.Complete();
        LogArray(FinalPositions);
        Positions.Dispose();
        Velocities.Dispose();
        FinalPositions.Dispose();
    }

    private void SetArray(NativeArray<Vector3> arr)
    {
        int length = arr.Length;
        for (int i = 0; i < length; i++)
        {
            arr[i] = Random.insideUnitSphere;
        }
    }

    private void LogArray(NativeArray<Vector3> arr)
    {
        StringBuilder strBild = new StringBuilder();
        int length = arr.Length;
        for (int i = 0; i < length; i++)
        {
            strBild.Append(arr[i] + " ");
        }
        Debug.Log(strBild);
    }

    public struct SomeJob : IJobParallelFor
    {
        public NativeArray<Vector3> Positions;
        public NativeArray<Vector3> Velocities;
        public NativeArray<Vector3> FinalPositions;
        public void Execute(int index)
        {
            FinalPositions[index] = Velocities[index] + Positions[index];
        }
    }
}
