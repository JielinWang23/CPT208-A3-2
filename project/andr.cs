using UnityEngine;
using System.Collections;

public class WebView : MonoBehaviour
{
    public GameObject dataManager;
    public SceneButton scene;
    public string backUrl = "208cpt/detail.html";
    public string initialUrl = "208cpt/home_default_.html";
    WebViewObject webViewObject;
    private string _currentUrl;

    void Start()
    {
        StartCoroutine(InitWebViewCoroutine());
    }

    private IEnumerator InitWebViewCoroutine()
    {
        yield return null;

        if (dataManager != null)
        {
            SceneButton sceneComp = dataManager.GetComponent<SceneButton>();
            if (sceneComp != null)
                scene = sceneComp;
        }

        if (!DataMana.isFirst)
            initialUrl = backUrl;

        webViewObject = new GameObject("WebViewObject").AddComponent<WebViewObject>();
        webViewObject.Init(
            cb: (msg) => { Debug.Log(msg); },
            started: (msg) => { Debug.Log(msg); },
            ld: (msg) => { Debug.Log(msg); }
        );

        while (!webViewObject.IsInitialized())
            yield return null;

        webViewObject.SetMargins(0, 0, 0, 0);
        webViewObject.SetVisibility(true);

        string fullUrl = GetPlatformStreamingAssetsPath(initialUrl);
        _currentUrl = fullUrl;
        webViewObject.LoadURL(fullUrl);

        DataMana.ResetData();
    }

    private string GetPlatformStreamingAssetsPath(string relativePath)
    {
        string path = "";
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = "file:///android_asset/" + relativePath;
                break;
            default:
                path = "file://" + Application.streamingAssetsPath + "/" + relativePath;
                break;
        }
        return path.Replace("\\", "/");
    }

    public void LoadNewUrl(string newUrl)
    {
        if (webViewObject == null || !webViewObject.IsInitialized())
            return;

        if (newUrl.Contains("208cpt/"))
            newUrl = GetPlatformStreamingAssetsPath(newUrl);

        _currentUrl = newUrl;
        webViewObject.LoadURL(newUrl);
    }

    public string GetCurrentDisplayedUrl()
    {
        return _currentUrl;
    }

    private void Update()
    {
        if (string.IsNullOrEmpty(_currentUrl)) return;

        if (_currentUrl.Contains("208cpt/ar.html"))
        {
            scene?.GoToTargetScene();
        }
    }

    private void OnDestroy()
    {
        if (webViewObject != null)
        {
            webViewObject.SetVisibility(false);
            Destroy(webViewObject.gameObject);
            webViewObject = null;
        }
    }
}
