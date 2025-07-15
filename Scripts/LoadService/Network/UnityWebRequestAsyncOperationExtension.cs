using UnityEngine.Networking;


namespace ZincFramework.LoadServices.Network
{
    public static class UnityWebRequestAsyncOperationExtension
    {
        public static UnityWebRequestAsyncOperationAwaiter GetAwaiter(this UnityWebRequestAsyncOperation unityWebRequestAsyncOperation)
        {
            return new UnityWebRequestAsyncOperationAwaiter(unityWebRequestAsyncOperation);
        }
    }
}