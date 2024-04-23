using UnityEngine;
using UnityEngine.UI;

public class PackagePreviewManager : MonoBehaviour
{
    public delegate void PackageBought(Database.PackageInfo iPackage);
    public static event PackageBought OnPackageBought;

    public GameObject _ListPlaceHolder;
    public GameObject _ListItem;
    public GameObject _BuyPanel;
    public GameObject _PackagesListUI;
    private GameObject _myPlaceholder;
    private Database.PackageInfo _myInfo;

    public void _LoadPackageInf(Database.PackageInfo iPackageInfo)
    {
        _myInfo = iPackageInfo;
        this.gameObject.SetActive(true);
        _myPlaceholder = Instantiate(_ListPlaceHolder, this.transform);
        GetComponent<ScrollRect>().content = _myPlaceholder.GetComponent<RectTransform>();
        foreach (Database.PreviewInfo pi in iPackageInfo.package_previews)
        {
            GameObject go = Instantiate(_ListItem, _myPlaceholder.transform);
            go.GetComponent<PackagePreviewController>()._LoadPreview(pi);
        }
        _BuyPanel.SetActive(true);
        _BuyPanel.GetComponentInChildren<Button>().onClick.AddListener(_BuyClicked);
        _BuyPanel.GetComponent<BuyPanellController>()._UpdatePannel(iPackageInfo);
    }
    
    private void OnEnable()
    {
        BackButtonReader.OnBackTapped += BackButtonReader_OnBackTapped;
    }

    private void BackButtonReader_OnBackTapped()
    {
        _PackagesListUI.SetActive(true);
        Destroy(_myPlaceholder.gameObject);
        _BuyPanel.SetActive(false);
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        BackButtonReader.OnBackTapped -= BackButtonReader_OnBackTapped;
    }
    public void _BuyClicked()
    {
        
        OnPackageBought?.Invoke(_myInfo);
    }
}
