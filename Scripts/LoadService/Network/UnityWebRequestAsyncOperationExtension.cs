using UnityEngine.Networking;


namespace ZincFramework.LoadSerivice.Network
{
    public static class UnityWebRequestAsyncOperationExtension
    {
        public static UnityWebRequestAsyncOperationAwaiter GetAwaiter(this UnityWebRequestAsyncOperation unityWebRequestAsyncOperation)
        {
            return new UnityWebRequestAsyncOperationAwaiter(unityWebRequestAsyncOperation);
        }
    }
}