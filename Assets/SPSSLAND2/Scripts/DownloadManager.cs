using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownloadManager : MonoBehaviour
{
    public GameObject _ListPlaceHolder;
    public GameObject _ListItem;
    public GameObject _PackagesListUI;
    private GameObject _myPlaceholder;
    public void _LoadPackageInf(Database.PackageInfo iPackageInfo)
    {
        this.gameObject.SetActive(true);
        _myPlaceholder = Instantiate(_ListPlaceHolder, this.transform);
        GetComponent<ScrollRect>().content = _myPlaceholder.GetComponent<RectTransform>();
        foreach (Database.DownloadInfo di in iPackageInfo.package_downloads)
        {
            GameObject go = Instantiate(_ListItem, _myPlaceholder.transform);
            go.GetComponent<DownloadController>().UpdateDownloadController(iPackageInfo.package_id,di);
        }
    }

    
    private void OnEnable()
    {
        BackButtonReader.OnBackTapped += BackButtonReader_OnBackTapped;
    }

    private void BackButtonReader_OnBackTapped()
    {
        _PackagesListUI.SetActive(true);
        Destroy(_myPlaceholder.gameObject);
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        BackButtonReader.OnBackTapped -= BackButtonReader_OnBackTapped;
    }
}
