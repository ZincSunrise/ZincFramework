using UnityEngine;
using YooAsset;
using ZincFramework.Threading.Tasks;

namespace ZincFramework.LoadServices.YooAsset.Packages
{
    public readonly struct EditorSimulateOperation : IPackageInitializeOperation
    {
        public async ZincTask IntializePackageAsync(ResourcePackage resourcePackage)
        {
            var bulid = EditorSimulateModeHelper.SimulateBuild(resourcePackage.PackageName);
            FileSystemParameters fileSystemParameters = FileSystemParameters.CreateDefaultEditorFileSystemParameters(bulid.PackageRootDirectory);

            EditorSimulateModeParameters editorSimulateModeParameters = new EditorSimulateModeParameters()
            {
                EditorFileSystemParameters = fileSystemParameters,
            };

            await resourcePackage.InitializeAsync(editorSimulateModeParameters);

            if (resourcePackage.InitializeStatus == EOperationStatus.Succeed)
            {
                Debug.Log($"{resourcePackage.PackageName}资源包初始化成功");
            }
            else
            {
                Debug.Log($"{resourcePackage.PackageName}资源包初始化失败");
            }

            await UpdatePackageManifestAsync(resourcePackage);
            await UpdatePackageAsync(resourcePackage);
        }

        public async ZincTask UpdatePackageManifestAsync(ResourcePackage resourcePackage)
        {
            var versionOperation = resourcePackage.RequestPackageVersionAsync();
            await versionOperation;

            if (versionOperation.Status == EOperationStatus.Succeed)
            {
                //更新成功
                Debug.Log($"{resourcePackage.PackageName}资源包版本为 : {versionOperation.PackageVersion}");
            }
            else
            {
                //更新失败
                Debug.LogError(versionOperation.Error);
            }

            var updateManifestOperation = resourcePackage.UpdatePackageManifestAsync(versionOperation.PackageVersion);
            await updateManifestOperation;

        }

        public async ZincTask UpdatePackageAsync(ResourcePackage resourcePackage)
        {
            var downloadOperation = resourcePackage.CreateResourceDownloader(10, 3);

            if (downloadOperation.TotalDownloadCount == 0)
            {
                Debug.Log($"{resourcePackage.PackageName} 无需更新，已是最新资源");
                return;
            }

            downloadOperation.BeginDownload();
            await downloadOperation;

            if (downloadOperation.Status == EOperationStatus.Succeed)
            {
                Debug.Log($"{resourcePackage.PackageName} 更新完成");
            }
            else
            {
                Debug.LogError($"{resourcePackage.PackageName} 更新失败");
            }
        }
    }
}