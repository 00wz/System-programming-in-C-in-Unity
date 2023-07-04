using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Exercise3 : MonoBehaviour
{
    static CancellationTokenSource cancelTokenSource;
    async void Start()
    {
        cancelTokenSource = new CancellationTokenSource();
        CancellationToken cancelToken = cancelTokenSource.Token;
        Task task1 = Task1(cancelToken);
        Task task2 = Task2(cancelToken);
        Debug.Log(await WhatTaskFasterAsync(cancelToken, task1, task2));

        cancelTokenSource.Dispose();
    }

    public async static Task<bool> WhatTaskFasterAsync(CancellationToken ct, Task task1, Task task2)
    {
        var resultTask = await Task.WhenAny(task1, task2);
        if (ct.IsCancellationRequested) return false;
        cancelTokenSource.Cancel();
        if (resultTask == task1) return true;
        return false;
    }

    async Task Task1(CancellationToken cancelToken)
    {
        for (int i = 0; i < 100; i++)
        {
            if (cancelToken.IsCancellationRequested)
                return;
            await Task.Delay(10);
        }
        Debug.Log("end task1");
    }
    async Task Task2(CancellationToken cancelToken)
    {
        for (int i = 0; i < 60; i++)
        {
            if (cancelToken.IsCancellationRequested)
                return;
            await Task.Yield();
        }
        Debug.Log("end task2");
    }
}
