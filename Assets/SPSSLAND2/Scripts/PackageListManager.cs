using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageListManager : MonoBehaviour
{
    public GameObject _ListPlaceHolder;
    public GameObject _ListItem;
    public GameObject _RatingUI;
    public GameObject _ExitUI;
    public PackagePreviewManager _PreviewManager;
    public DownloadManager _DownloadManager;

    private List<GameObject> _Packages;
    private void Start()
    {
        //PlayerPrefs.SetInt("pkg1", 1);
        PackageController.OnMouseClicked += PackageController_OnMouseClicked;
        StartCoroutine(_PopulateList());
    }
    private IEnumerator _PopulateList()
    {
        //for(int i = 0; i < _ListPlaceHolder.transform.childCount;i++)
        //{
        //    Destroy(_ListPlaceHolder.transform.GetChild(i).gameObject);
        //}
        _Packages = new List<GameObject>();
        foreach(Database.PackageInfo pi in Database.SPSSLandPackes.all_packages)
        {

            yield return 0;
            GameObject go = Instantiate(_ListItem, _ListPlaceHolder.transform);
            go.GetComponent<PackageController>()._LoadPackageInfo(pi);
            _Packages.Add(go);
        }
    }
    public void _UpdatePackagesInfo()
    {
        foreach(GameObject go in _Packages.ToArray())
        {
            StartCoroutine( go.GetComponent<PackageController>()._UpdatePackageInfo());

        }
    }
    private void OnEnable()
    {

        BackButtonReader.OnBackTapped += BackButtonReader_OnBackTapped;
    }

    private void BackButtonReader_OnBackTapped()
    {
        if (_ExitUI.activeInHierarchy == false)
        {
            _ExitUI.SetActive(true);
        }
        else
        {
            _ExitUI.SetActive(false);
        }
    }

    private void OnDisable()
    {
        BackButtonReader.OnBackTapped -= BackButtonReader_OnBackTapped;
    }
    private void PackageController_OnMouseClicked(Database.PackageInfo iPackage)
    {
        
        if (PlayerPrefs.GetInt(string.Format("{0}_{1}",ab.IsPackageBoughtTag, iPackage.package_id), 0).Equals(1) || iPackage.package_discount==100 || iPackage.package_price==0)
        {
            if (PlayerPrefs.GetInt(ab.IsRatingDoneTag, 0) != 1 && iPackage.package_discount == 100)
            {
                _RatingUI.SetActive(true);

            }
            
            _DownloadManager._LoadPackageInf(iPackage);
        }
        else
        {
            _PreviewManager._LoadPackageInf(iPackage);
            
            
            
        }
        gameObject.SetActive(false);

    }
    
}
