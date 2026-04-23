using UnityEngine;

public class WebView : MonoBehaviour
{
    public GameObject dataManager;
    public SceneButton scene;
    public string backUrl = "208cpt/detail.html";
    public string initialUrl = "208cpt/home_default_.html";
    WebViewObject webViewObject;
    private string _currentUrl; // 记录当前显示的网址

    void Start()
    {
        SceneButton scene = dataManager.GetComponent<SceneButton>();

        if (!DataMana.isFirst)
        {
            initialUrl = backUrl;
        }

        webViewObject = new GameObject("WebViewObject").AddComponent<WebViewObject>();

        // 初始化WebView（和你原有的代码保持一致）
        webViewObject.Init(
            cb: (msg) => { Debug.Log("HTML消息: " + msg); },
            started: (msg) => { Debug.Log("WebView启动"); },
            ld: (msg) => { Debug.Log("页面加载完成"); }
        );

        // 等待初始化
        while (!webViewObject.IsInitialized())
            return;

        webViewObject.SetMargins(0, 0, 0, 0);
        webViewObject.SetVisibility(true);

        // 初始加载URL，同时记录下来
        string fullInitialUrl = "file://" + Application.streamingAssetsPath + "/" + initialUrl;
        _currentUrl = fullInitialUrl;
        webViewObject.LoadURL(fullInitialUrl);

        DataMana.ResetData();
    }

    // 核心：封装加载URL的方法，每次加载都更新记录
    public void LoadNewUrl(string newUrl)
    {
        if (webViewObject == null || !webViewObject.IsInitialized())
            return;

        // 更新记录的当前网址
        _currentUrl = newUrl;
        // 调用包自带的LoadURL方法加载新页面
        webViewObject.LoadURL(newUrl);
    }

    // 你要的：返回当前显示的网址的方法
    public string GetCurrentDisplayedUrl()
    {
        return _currentUrl;
    }

    private void Update()
    {
        string targetPath = "208cpt/ar.html";

        if (_currentUrl.Contains(targetPath))
        {
            Debug.Log("跳转");
            scene.GoToTargetScene();
        }
    }
}