using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Net;
using System;

public class DownloadController : MonoBehaviour
{
    public Text _FileDescriptionText;
    [Header("Download Setting:")]
    public string _UrlToDownload = "https://spssland.ir/dl/app/";
    public string _PackageID;

    [Header("Progress Setting:")]
    public Slider _ProgressSlider;
    public Text _PercentageText;

    [Header("Buttons to Handle:")]
    public GameObject _DownloadButton;
    public GameObject _CancelButton;
    public GameObject _PlayButton;
    public GameObject _LoadingImage;

    private string _Pathtosave;
    private string _FileName = "";
    private string _fileDescription;
    
    private UnityWebRequest _myFileDownloader;
    void Start()
    {



        //Debug.Log(Application.persistentDataPath);


    }
    public void UpdateDownloadController(string iPackageId,Database.DownloadInfo iDownloadInfo)
    {
        _PackageID = iPackageId;
        _FileDescriptionText.text = _fileDescription = iDownloadInfo.download_name;
        _UrlToDownload = iDownloadInfo.download_url;



        _FileName = _UrlToDownload.Substring(_UrlToDownload.LastIndexOf("/") + 1, _UrlToDownload.Length - _UrlToDownload.LastIndexOf("/") - 1);


        _Pathtosave = string.Format("{0}/{2}_{1}", Application.persistentDataPath, _FileName, _PackageID);

        _DownloadButton.GetComponent<Button>().onClick.AddListener(StartDownload);
        _PlayButton.GetComponent<Button>().onClick.AddListener(PlayMovie);
        _CancelButton.GetComponent<Button>().onClick.AddListener(CancelDownload);

        checkIfDownloaded();
    }
    public void StartDownload()
    {

        
        StartCoroutine(downloadNow(_UrlToDownload));



    }
    public void CancelDownload()
    {
        _myFileDownloader.Abort();
        StopAllCoroutines();
        _myFileDownloader.Dispose();


        checkIfDownloaded();
        
    }
    public void PlayMovie()
    {

        
            StartCoroutine(startPlayback(_Pathtosave, FullScreenMovieScalingMode.Fill));
        
        

    }

    private void checkIfDownloaded()
    {
        
        _LoadingImage.SetActive(false);
        if (_PercentageText != null)
            _PercentageText.gameObject.SetActive(false);
        if (_ProgressSlider != null)
            _ProgressSlider.gameObject.SetActive(false);
        if (!File.Exists(_Pathtosave))
        {
            if (_CancelButton != null)
                _CancelButton.SetActive(false);
            _DownloadButton.SetActive(true);
            _PlayButton.SetActive(false);
        }
        else
        {
            _CancelButton.SetActive(false);
            _ProgressSlider.gameObject.SetActive(false);
            _PercentageText.gameObject.SetActive(false);
            _DownloadButton.SetActive(false);
            _PlayButton.SetActive(true);
        }
    }

    private IEnumerator startPlayback(string iPath, FullScreenMovieScalingMode iScalingMode)
    {
        yield return 0;
        PlayerController._INSTANCE._PlayVideo(iPath);


        //Screen.orientation = ScreenOrientation.LandscapeLeft;
        
        //yield return Handheld.PlayFullScreenMovie(iPath, Color.black, FullScreenMovieControlMode.Full, iScalingMode);

        //yield return new WaitForEndOfFrame();
        

        //yield return Screen.orientation = ScreenOrientation.Portrait;
    }
    private bool URLExists(string url)
    {
        bool result = false;
        Debug.Log("URLEXISTS:" + "Preparin");
        WebRequest webRequest = WebRequest.Create(url);
        webRequest.Timeout = 1200; // miliseconds
        webRequest.Method = "HEAD";

        HttpWebResponse response = null;

        try
        {
            Debug.Log("URLEXISTS:" + "GettingResponse");
            response = (HttpWebResponse)webRequest.GetResponse();
            result = true;
            Debug.Log("URLEXISTS:" + "ResponseGot");
        }
        catch (WebException webException)
        {
            Debug.Log(url + " doesn't exist: " + webException.Message);
        }
        finally
        {
            if (response != null)
            {
                Debug.Log("URLEXISTS:" + "ClosingResponse");
                response.Close();
            }
        }

        return result;
    }
    private IEnumerator downloadNow(string iURL)
    {
        _LoadingImage.SetActive(true);
        _CancelButton.SetActive(false);
        _DownloadButton.SetActive(false);
        _PlayButton.SetActive(false);
        yield return new WaitForFixedUpdate();
        bool isURLExists = false;
        Debug.Log("DOWNLOADNOW:" + "ChechingULRExistance");
        int count = 0;
        do
        {
            count++;
            yield return isURLExists = URLExists(iURL);

            
            yield return new WaitForFixedUpdate();
            Debug.Log("Count="+count);
            if (count > ab.MaxNumberOfDownloadRetry)
                break;
        } while (!isURLExists);
        _LoadingImage.SetActive(false);
        if (isURLExists)
        {
            Debug.Log("URL IS Exists");
            _CancelButton.SetActive(true);
            yield return new WaitForFixedUpdate();

            _myFileDownloader = new UnityWebRequest(iURL);
            _myFileDownloader.downloadHandler = new DownloadHandlerBuffer();
            StartCoroutine(ShowDownloadProgress(_myFileDownloader));
            yield return _myFileDownloader.SendWebRequest();

            Debug.Log("DOWNLOADNOW:" + _myFileDownloader.downloadHandler.isDone);
            if (_myFileDownloader.isNetworkError || _myFileDownloader.isHttpError)
            {
                Debug.Log("DOWNLOADNOW:" + "Error:" + _myFileDownloader.error);



            }
            else
            {
                Debug.Log("DOWNLOADNOW:" + "DownloadOK");
                File.WriteAllBytes(_Pathtosave, _myFileDownloader.downloadHandler.data);

            }
            yield return new WaitForFixedUpdate();

            checkIfDownloaded();
        }
        else
        {
            Debug.Log("URL Not Exists");
            _DownloadButton.SetActive(true);
        }
        
        yield return new WaitForFixedUpdate();
        //_PercentageText.text = string.Format("{0}%", 0);
        //yield return 0;

        //WebRequest wr = WebRequest.Create(iURL);
        //WebResponse response = null;
        //yield return response = wr.GetResponse();
        //response.GetResponseStream()



    }
    private IEnumerator ShowDownloadProgress(UnityWebRequest iRequest)
    {
        while (!iRequest.downloadHandler.isDone)
        {
            
            yield return new WaitForFixedUpdate();

            if (_ProgressSlider != null)
            {
                _ProgressSlider.gameObject.SetActive(true);
                _ProgressSlider.value = iRequest.downloadProgress;

            }
            if (_PercentageText != null)
            {
                _PercentageText.gameObject.SetActive(true);
                _PercentageText.text = string.Format("{0}%", Mathf.Round(iRequest.downloadProgress * 100));
            }
            
            

        }
        _PercentageText.text = string.Format("{0}%", 100);
        yield return 0;
    }


}
