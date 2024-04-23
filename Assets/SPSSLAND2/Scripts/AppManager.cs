using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AppManager : MonoBehaviour
{
    public GameObject _PackageListUI;
    public GameObject _PreviewListUI;
    public GameObject _DownloadListUI;
    public GameObject _BuyUI;
    public GameObject _ExitUI;
    public GameObject _RateUsUI;
    public Text _MessageBox;

    public static AppManager _Instance = null;
    private void Awake()
    {
        //Database.LoadData();
        if (_Instance == null)
            _Instance = this;

    }
    private void OnEnable()
    {

        PackagePreviewManager.OnPackageBought += PackagePreviewManager_OnPackageBought;




    }
    private void OnDisable()
    {
        PackagePreviewManager.OnPackageBought -= PackagePreviewManager_OnPackageBought;



    }

    private void PackagePreviewManager_OnPackageBought(Database.PackageInfo iPackage)
    {
        dl("Start Buying:" + string.Format("{0}.{1}", Application.identifier, iPackage.package_id));

        long price = Mathf.RoundToInt((iPackage.package_price * (100 - iPackage.package_discount)) / 100);

        var desc = iPackage.package_name;
        var productID = iPackage.package_id;
        Zarinpal.Purchase(price, desc, productID);



        //dl();

    }
    public void _SetPackageBought(string iPackageID)
    {



        PlayerPrefs.SetInt(string.Format("{0}_{1}", ab.IsPackageBoughtTag, iPackageID), 1);


        _RearrangeApp();
    }
    private void Start()
    {
        Zarinpal.Initialize();


        Zarinpal.StoreInitialized += Zarinpal_StoreInitialized;

        Zarinpal.StoreInitializeFailed += Zarinpal_StoreInitializeFailed;
        Zarinpal.PurchaseStarted += Zarinpal_PurchaseStarted;
        Zarinpal.PurchaseFailedToStart += Zarinpal_PurchaseFailedToStart;
        Zarinpal.PurchaseSucceed += Zarinpal_PurchaseSucceed;
        Zarinpal.PurchaseFailed += Zarinpal_PurchaseFailed;
        Zarinpal.PurchaseCanceled += Zarinpal_PurchaseCanceled;
        Zarinpal.PaymentVerificationStarted += Zarinpal_PaymentVerificationStarted;
        Zarinpal.PaymentVerificationSucceed += Zarinpal_PaymentVerificationSucceed;
        Zarinpal.PaymentVerificationFailed += Zarinpal_PaymentVerificationFailed;



        //StartCoroutine(_StartFetchingAd());
    }
    void OnDestroy()
    {
        Zarinpal.StoreInitialized -= Zarinpal_StoreInitialized;
        Zarinpal.StoreInitializeFailed -= Zarinpal_StoreInitializeFailed;
        Zarinpal.PurchaseStarted -= Zarinpal_PurchaseStarted;
        Zarinpal.PurchaseFailedToStart -= Zarinpal_PurchaseFailedToStart;
        Zarinpal.PurchaseSucceed -= Zarinpal_PurchaseSucceed;
        Zarinpal.PurchaseFailed -= Zarinpal_PurchaseFailed;
        Zarinpal.PurchaseCanceled -= Zarinpal_PurchaseCanceled;
        Zarinpal.PaymentVerificationStarted -= Zarinpal_PaymentVerificationStarted;
        Zarinpal.PaymentVerificationSucceed -= Zarinpal_PaymentVerificationSucceed;
        Zarinpal.PaymentVerificationFailed -= Zarinpal_PaymentVerificationFailed;
    }

    private void Zarinpal_StoreInitializeFailed(string obj)
    {

    }


    private void Zarinpal_PaymentVerificationFailed()
    {
        throw new NotImplementedException();
    }

    private void Zarinpal_PaymentVerificationSucceed(string obj)
    {
        throw new NotImplementedException();
    }

    private void Zarinpal_PaymentVerificationStarted(string obj)
    {
        throw new NotImplementedException();
    }

    private void Zarinpal_PurchaseCanceled()
    {
        throw new NotImplementedException();
    }

    private void Zarinpal_PurchaseFailed()
    {
        throw new NotImplementedException();
    }

    private void Zarinpal_PurchaseSucceed(string iProductId, string iAutharity)
    {
        _SetPackageBought(iProductId);
    }

    private void Zarinpal_PurchaseFailedToStart(string obj)
    {
        throw new NotImplementedException();
    }

    private void Zarinpal_PurchaseStarted()
    {
        throw new NotImplementedException();
    }

    private void Zarinpal_StoreInitialized()
    {
        throw new NotImplementedException();
    }

    public void _TerminateApp()
    {
        dl("Exit");
        Application.Quit();
    }
    public void _RearrangeApp()
    {

        _PackageListUI.SetActive(true);
        _PreviewListUI.SetActive(false);
        _DownloadListUI.SetActive(false);
        _BuyUI.SetActive(false);
        _ExitUI.SetActive(false);
        _RateUsUI.SetActive(false);
        _PackageListUI.GetComponent<PackageListManager>()._UpdatePackagesInfo();

    }
    public void _ShowRatingUI()
    {
        dl(ab.ShowRatingURL);
        Application.OpenURL(ab.ShowRatingURL);
        //this.gameObject.SetActive(true);
        dl("RatingDone");
        PlayerPrefs.SetInt(ab.IsRatingDoneTag, 1);

    }
    public void _ShareApp()
    {
        AndroidJavaClass intentClass = new
                 AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new
                         AndroidJavaObject("android.content.Intent");

        //set action to that intent object   
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));


        //set the type as text and put extra subject and text to share
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), ab.ShareSubject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), ab.ShareText);
        //create current activity object
        AndroidJavaClass unity = new
                        AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity =
                     unity.GetStatic<AndroidJavaObject>("currentActivity");

        //call createChooser method of activity class     
        AndroidJavaObject chooser =
                intentClass.CallStatic<AndroidJavaObject>("createChooser",
                             intentObject, ab.ShareChooserTitle);
        currentActivity.Call("startActivity", chooser);
    }
    private void dl(string iMessage)
    {
        MessageBoxManager._INSTANCE?._ShowMessage(iMessage);
        if (_MessageBox != null)
        {
            _MessageBox.text = iMessage;
            
        }
    }


}
