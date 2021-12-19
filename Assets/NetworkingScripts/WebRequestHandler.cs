using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System;
using UnityEngine.Networking;

public class WebRequestHandler : MonoBehaviour
    {
        public static WebRequestHandler Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);
        }

        public void Get(string url, Action<string, bool> OnRequestProcessed)
        {
            StartCoroutine(GetRequest(url, OnRequestProcessed));
        }
        public void Post(string url, string json, Action<string, bool> OnRequestProcessed)
        {
            StartCoroutine(PostRequest(url, json, OnRequestProcessed));
        }

        private IEnumerator GetRequest(string url, Action<string, bool> OnRequestProcessed)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            if(request.result==UnityWebRequest.Result.ConnectionError || request.result==UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("web request error in Get method with responce code : " + request.responseCode);
                OnRequestProcessed(request.error, false);
            }
            else
            {
                OnRequestProcessed(request.downloadHandler.text, true);
            }

            request.Dispose();
        }

        private IEnumerator PostRequest(string url, string json, Action<string, bool> OnRequestProcessed)
        {
            Debug.Log("Digvijay Url:  " + url + " json request: " + json);
            var request = new UnityWebRequest(url, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result==UnityWebRequest.Result.ConnectionError || request.result==UnityWebRequest.Result.ProtocolError)
            {
                OnRequestProcessed(request.error, false);
            }
            else
            {
                Debug.Log("Akash Url:  " + url + " json response: " + request.downloadHandler.text);
                OnRequestProcessed(request.downloadHandler.text, true);
            }
            //Hide Loader. Dummy Image
            request.Dispose();
        }

        public void DownloadSprite(string url, Action<Sprite> OnDownloadComplete)
        {
            StartCoroutine(LoadFromWeb(url, OnDownloadComplete));
        }

        public void DownloadTexture(string url, Action<Texture> OnDownloadComplete)
        {
            StartCoroutine(LoadFromWeb(url, OnDownloadComplete));
        }

        IEnumerator LoadFromWeb(string url, Action<Texture> OnDownloadComplete)
        {
            UnityWebRequest webRequest = new UnityWebRequest(url);
            DownloadHandlerTexture textureDownloader = new DownloadHandlerTexture(true);
            webRequest.downloadHandler = textureDownloader;
            yield return webRequest.SendWebRequest();
            if (!(webRequest.result==UnityWebRequest.Result.ConnectionError || webRequest.result==UnityWebRequest.Result.ProtocolError))
            {
                OnDownloadComplete(textureDownloader.texture);
            }
            else
            {
                Debug.LogError("failed to download image");
            }
        }

        IEnumerator LoadFromWeb(string url, Action<Sprite> OnDownloadComplete)
        {
            UnityWebRequest webRequest = new UnityWebRequest(url);
            DownloadHandlerTexture textureDownloader = new DownloadHandlerTexture(true);
            webRequest.downloadHandler = textureDownloader;
            yield return webRequest.SendWebRequest();
            if(!(webRequest.result==UnityWebRequest.Result.ConnectionError || webRequest.result==UnityWebRequest.Result.ProtocolError))
            {
                Texture2D texture = textureDownloader.texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1f);
                OnDownloadComplete(sprite);
            }
            else
            {
                Debug.LogError("failed to download image");
            }
        }

        public static int GetVersionCode()
        {
            try
            {
                AndroidJavaClass contextCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject context = contextCls.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject packageMngr = context.Call<AndroidJavaObject>("getPackageManager");
                string packageName = context.Call<string>("getPackageName");
                AndroidJavaObject packageInfo = packageMngr.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
                return packageInfo.Get<int>("versionCode");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 2;
            }
        }

        public void SaveTextureToFile(Texture2D texture, string filename)
        {
            //then Save To Disk as PNG
            byte[] bytes = texture.EncodeToPNG();
            var dirPath = Application.persistentDataPath + "/../ProfilePicture/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            File.WriteAllBytes(dirPath + filename + ".png", bytes);
        }
    }

