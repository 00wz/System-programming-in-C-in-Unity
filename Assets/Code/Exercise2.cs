using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Exercise2 : MonoBehaviour
{
    async void Start()
    {
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken cancelToken = cancelTokenSource.Token;
        Task task1 = Task1(cancelToken);
        Task task2 = Task2(cancelToken);
        await Task.WhenAll(task1, task2);
        cancelTokenSource.Cancel();
        cancelTokenSource.Dispose();
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
        for(int i=0;i<60;i++)
        {
            if (cancelToken.IsCancellationRequested)
                return;
            await Task.Yield();
        }
        Debug.Log("end task2");
    }
}
