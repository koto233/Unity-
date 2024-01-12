using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public class HotfixManager : MonoBehaviour
{

    public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;
    public string packageName = "DefaultPackage";
    void Start()
    {



    }

    void HotFixInitialize()
    {
        // 初始化资源系统
        YooAssets.Initialize();

        // 创建默认的资源包
        var package = YooAssets.CreatePackage(packageName);

        // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
        YooAssets.SetDefaultPackage(package);

        if (PlayMode == EPlayMode.EditorSimulateMode)
        {
            StartCoroutine(InitializeYooAsset());
        }
        else
        {
            return;
        }

    }

    // private IEnumerator InitializeYooAsset()
    // {
    //     // 注意：GameQueryServices.cs 太空战机的脚本类，详细见StreamingAssetsHelper.cs
    //     string defaultHostServer = "http://127.0.0.1/CDN/Android/v1.0";
    //     string fallbackHostServer = "http://127.0.0.1/CDN/Android/v1.0";
    //     var initParameters = new HostPlayModeParameters();
    //     initParameters.BuildinQueryServices = new GameQueryServices();
    //     initParameters.DecryptionServices = new FileOffsetDecryption();
    //     initParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
    //     var initOperation = package.InitializeAsync(initParameters);
    //     yield return initOperation;

    //     if (initOperation.Status == EOperationStatus.Succeed)
    //     {
    //         Debug.Log("资源包初始化成功！");
    //     }
    //     else
    //     {
    //         Debug.LogError($"资源包初始化失败：{initOperation.Error}");
    //     }
    // }

    private IEnumerator InitializeYooAsset()
    {
        var package = YooAssets.TryGetPackage(packageName);
        if (package == null)
            package = YooAssets.CreatePackage(packageName);
        var initParameters = new EditorSimulateModeParameters();
        var simulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, "DefaultPackage");
        initParameters.SimulateManifestFilePath = simulateManifestFilePath;
        yield return package.InitializeAsync(initParameters);
    }
}
