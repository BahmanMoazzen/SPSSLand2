using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public static PlayerController _INSTANCE;

    private void Awake()
    {
        if (_INSTANCE == null)
            _INSTANCE = this;
    }

    [SerializeField] VideoPlayer _Player;
    [SerializeField] Slider _PlayerProgress;
    [SerializeField] Image _PlayPauseButton;
    [SerializeField] GameObject _PlayerScreen, _ButtonPanel;

    bool _ProgressMouseClicked = false, _isButtonVisible = false;
    float _HideButtonInterval = 2f;

    public void _Stop()
    {
        StopAllCoroutines();
        _Player.Stop();
        _ButtonPanel.SetActive(false);
        _PlayerScreen.SetActive(false);
    }
    public void _ProgressPointerDown()
    {
        _ProgressMouseClicked = true;
        StopCoroutine(_updateProgressBar());
    }
    public void _ProgressPointerUp()
    {
        _ProgressMouseClicked = false;
        StartCoroutine(_updateProgressBar());
    }
    public void _ProgressValueChanged(float iNewValue)
    {
        if (_ProgressMouseClicked)
        {

            _Player.frame = Mathf.RoundToInt(iNewValue);
            _Player.Play();
        }
    }
    public void _ShowButtons()
    {

        _isButtonVisible = !_isButtonVisible;
        _ButtonPanel.SetActive(_isButtonVisible);


    }
    IEnumerator _HideButtonRoutine()
    {
        yield return new WaitForSeconds(_HideButtonInterval);
        _ButtonPanel.SetActive(false);
    }
    private void OnEnable()
    {
        _PlayerProgress.onValueChanged.AddListener(_ProgressValueChanged);
        _Player.loopPointReached += _Player_loopPointReached;
        _Player.prepareCompleted += _Player_prepareCompleted;

    }
    private void OnDisable()
    {
        _PlayerProgress.onValueChanged.RemoveListener(_ProgressValueChanged);
        _Player.loopPointReached -= _Player_loopPointReached;
        _Player.prepareCompleted -= _Player_prepareCompleted;
    }
    public void _PlayVideo(string iPath)
    {
#if UNITY_EDITOR
        string path = string.Format("{0}", iPath);
#elif UNITY_ANDROID
        string path = string.Format("{0}", iPath);
#endif

        _PlayerScreen.SetActive(true);
        //_ButtonPanel.SetActive(true);

        //_Player.source = VideoSource.Url;
        _Player.url = path;
        _Player.Prepare();


        StartCoroutine(_updateProgressBar());


    }

    private void _Player_prepareCompleted(VideoPlayer source)
    {
        _PlayerProgress.minValue = 0;
        _PlayerProgress.maxValue = _Player.frameCount;

        //_Player.frame = Mathf.RoundToInt(_Player.frameCount / 2);
        _Player.Play();
    }

    private void _Player_loopPointReached(VideoPlayer source)
    {
        _Stop();

    }

    IEnumerator _updateProgressBar()
    {
        while (true)
        {
            _PlayerProgress.value = _Player.frame;
            yield return 0;
        }
    }

}
