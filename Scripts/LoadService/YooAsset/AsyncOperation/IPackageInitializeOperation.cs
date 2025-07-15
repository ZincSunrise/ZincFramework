using YooAsset;
using ZincFramework.Threading.Tasks;

namespace ZincFramework.LoadServices.YooAsset.Packages
{
    public interface IPackageInitializeOperation
    {
        public ZincTask IntializePackageAsync(ResourcePackage resourcePackage);

        public ZincTask UpdatePackageManifestAsync(ResourcePackage resourcePackage);

        public ZincTask UpdatePackageAsync(ResourcePackage resourcePackage);
    }
}