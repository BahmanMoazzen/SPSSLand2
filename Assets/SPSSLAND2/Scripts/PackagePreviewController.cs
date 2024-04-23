using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackagePreviewController : MonoBehaviour
{
    public Text _PackageInfoText;
    public Text _PackageInfoTextFull;
    public Button _PackageInfoYoutubeButton;
    public Button _PackageInfoAparatButton;
    public GameObject _PreviewLabel;
    private Database.PreviewInfo _PreviewInfo;
    public void _LoadPreview(Database.PreviewInfo iPreviewInfo)
    {
        _PreviewInfo = iPreviewInfo;
        _PackageInfoTextFull.text =  _PackageInfoText.text = iPreviewInfo.preview_name;
        if (iPreviewInfo.preview_aparat_url == string.Empty)
        {
            _PackageInfoAparatButton.gameObject.SetActive(false);
        }
        else
        {
            _PackageInfoAparatButton.onClick.RemoveAllListeners();
            _PackageInfoAparatButton.onClick.AddListener(_OpenAparat);
        }
        if (iPreviewInfo.preview_youtube_url == string.Empty)
        {
            _PackageInfoYoutubeButton.gameObject.SetActive(false);
        }
        else
        {
            _PackageInfoYoutubeButton.onClick.RemoveAllListeners();
            _PackageInfoYoutubeButton.onClick.AddListener(_OpenYoutube);
        }
        if (iPreviewInfo.preview_aparat_url == string.Empty && iPreviewInfo.preview_youtube_url == string.Empty)
        {
            _PreviewLabel.SetActive(false);
            _PackageInfoTextFull.gameObject.SetActive(true);
            _PackageInfoText.gameObject.SetActive(false);
        }

    }
    public void _OpenAparat()
    {
        Application.OpenURL(_PreviewInfo.preview_aparat_url);

    }
    public void _OpenYoutube()
    {
        Application.OpenURL(_PreviewInfo.preview_youtube_url);

    }

}
