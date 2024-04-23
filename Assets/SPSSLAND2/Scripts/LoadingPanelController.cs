using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;

public class LoadingPanelController : MonoBehaviour
{
    public Button _StartButton;
    public Text _StartButtonText;
    public GameObject _LoadingImage;
    public GameObject _MenuUI;
    public GameObject _PackageListUI;


    void Start()
    {
        _StartButtonText.text = ab.StartText;


    }


    public void _EnterClicked()
    {
        StartCoroutine(_startLoading());
    }
    private IEnumerator _startLoading()
    {
        _StartButton.interactable = false;
        _StartButtonText.text = ab.ConnectingText;
        _LoadingImage.SetActive(true);

        MessageBoxManager._INSTANCE?._ShowMessage("Start Fetching Package", Color.blue, 2f);
        UnityWebRequest www = UnityWebRequest.Get(ab.PackageURL);

        // sending data request
        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError)
        {
            MessageBoxManager._INSTANCE?._ShowMessage("Package Request Failed", Color.red, 2f);
            // data connection error
            _StartButton.interactable = true;
            _StartButtonText.text = ab.NotConnectedText;
            _LoadingImage.SetActive(false);
        }
        else
        {
            try
            {
                Database.LoadData(www.downloadHandler.text);
                _LoadingImage.SetActive(false);
                _StartButtonText.text = ab.StartText;
                _showMenu();
            }
            catch
            {
                _StartButton.interactable = true;
                _StartButtonText.text = ab.NotConnectedText;
                _LoadingImage.SetActive(false);
            }
        }

        yield return 0;
    }
    
    private void _showMenu()
    {
        _MenuUI.SetActive(true);
        _PackageListUI.SetActive(true);
        gameObject.SetActive(false);

    }

}
