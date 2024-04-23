using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;


public class PackageController : MonoBehaviour
{
    public delegate void MouseClicked(Database.PackageInfo iPackage);
    public static event MouseClicked OnMouseClicked;

    public Image _PackageImage;
    public Text _PackageText;
    public Text _DiscountAmountText;
    public GameObject _FreeText;
    public GameObject _DiscountText;
    public GameObject _BoughtText;
    //public Button _PackageButton;

    string iconLocalLocation, iconURL;



    private Database.PackageInfo _PackageInfo;
    public void _LoadPackageInfo(Database.PackageInfo iPackage)
    {
        _PackageInfo = iPackage;
        iconLocalLocation = Path.Combine(Application.streamingAssetsPath, _PackageInfo.package_icon);
        iconURL = string.Format("{0}{1}", ab.PackageIconURLPrefix, _PackageInfo.package_icon);
        StartCoroutine(_UpdatePackageInfo());

    }

    public IEnumerator _UpdatePackageInfo()
    {
        this.gameObject.SetActive(true);
        MessageBoxManager._INSTANCE._ShowMessage("Start Updating Package Info:"+_PackageInfo.package_id, Color.black, 1);
        _PackageText.text = _PackageInfo.package_name;

        #region doanload or load image
        

        if (!File.Exists(iconLocalLocation))
        {
            MessageBoxManager._INSTANCE._ShowMessage("Fetching From URL: " + iconURL, Color.black, 1);
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(iconURL);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                MessageBoxManager._INSTANCE._ShowMessage("Fetching From URL with Error: " + iconURL,Color.red,1);
                // data connection error
            }
            else
            {

                File.WriteAllBytes(iconLocalLocation, www.downloadHandler.data);
                Texture2D myTexture = DownloadHandlerTexture.GetContent(www);
                _PackageImage.sprite = Sprite.Create(myTexture, new Rect(Vector2.zero, new Vector2(myTexture.width, myTexture.height)), Vector2.zero);
            }
        }
        else
        {
            MessageBoxManager._INSTANCE._ShowMessage("Fetching From Disk: " + iconLocalLocation,Color.black,1);
            UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(iconLocalLocation);
            yield return uwr.SendWebRequest();
            Texture2D myTexture = DownloadHandlerTexture.GetContent(uwr);

            _PackageImage.sprite = Sprite.Create(myTexture, new Rect(Vector2.zero, new Vector2(myTexture.width, myTexture.height)), Vector2.zero);
        }

        #endregion

        _FreeText.SetActive(false);
        _DiscountText.SetActive(false);
        _BoughtText.SetActive(false);
        yield return 0;
        if (PlayerPrefs.GetInt(string.Format("{0}_{1}", ab.IsPackageBoughtTag, _PackageInfo.package_id), 0).Equals(1))
        {
            _BoughtText.SetActive(true);
            _FreeText.SetActive(false);
            _DiscountText.SetActive(false);
            _DiscountAmountText.gameObject.SetActive(false);
        }
        else
        {
            if (_PackageInfo.package_discount > 0)
            {
                if (_PackageInfo.package_discount == 100)
                {
                    _BoughtText.SetActive(false);
                    _FreeText.SetActive(true);
                    _DiscountText.SetActive(false);
                    _DiscountAmountText.gameObject.SetActive(false);
                }
                else
                {

                    _DiscountAmountText.text = string.Format("{0}%", _PackageInfo.package_discount);
                    _DiscountAmountText.gameObject.SetActive(true);
                    _BoughtText.SetActive(false);
                    _FreeText.SetActive(false);
                    _DiscountText.SetActive(true);


                }
            }
            else
            {
                _DiscountAmountText.gameObject.SetActive(false);
            }

        }
    }
    public void _PackageClicked()
    {
        //Debug.Log("PackageClicked");
        OnMouseClicked?.Invoke(_PackageInfo);
    }
}
