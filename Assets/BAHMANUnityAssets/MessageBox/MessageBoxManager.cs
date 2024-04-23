/*
 * Message box Version 1.0
 * 
 * 
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Message structure to show on screen
class MessageStruct
{
    public Color _color;
    public string _message;
    public float _interval;

}


public class MessageBoxManager : MonoBehaviour
{

    // instance to call message box manager
    public static MessageBoxManager _INSTANCE;

    //public procedures to call


    // insert message by single text
    public void _ShowMessage(string iMessage)
    {
        MessageStruct messageStructure = new MessageStruct();

        messageStructure._interval = _DefaultHideIntervalTime;
        messageStructure._message = iMessage;
        _messageQueue.Enqueue(messageStructure);


    }
    // insert message by single text and color
    public void _ShowMessage(string iMessage, Color iColor)
    {
        MessageStruct messageStructure = new MessageStruct();
        messageStructure._color = iColor;
        messageStructure._message = iMessage;
        messageStructure._interval = _DefaultHideIntervalTime;
        _messageQueue.Enqueue(messageStructure);


    }
    // insert message by single text and color and time
    public void _ShowMessage(string iMessage, Color iColor, float iInterval)
    {
        MessageStruct messageStructure = new MessageStruct();
        messageStructure._color = iColor;
        messageStructure._message = iMessage;
        messageStructure._interval = iInterval;
        _messageQueue.Enqueue(messageStructure);


    }

    #region private

    // message text placeholder
    [SerializeField] Text _MessageText;

    // message panel to show or hide
    [SerializeField] GameObject _MessagePanel;

    // default hide interval time
    [SerializeField] [Range(0, 10)] float _DefaultHideIntervalTime = 2f;

    // message queue for storing data
    Queue<MessageStruct> _messageQueue;


    void Awake()
    {

        if (_INSTANCE == null)
        {
            _INSTANCE = this;
        }
        DontDestroyOnLoad(this.gameObject);

    }

    void Start()
    {

        StartCoroutine(_startupRoutine());
    }
    IEnumerator _startupRoutine()
    {
        yield return 0;
        _messageQueue = new Queue<MessageStruct>();
        _MessagePanel.SetActive(false);
        _MessageText.text = string.Empty;
        StartCoroutine(_MessageManager());
        yield return 0;
    }
    IEnumerator _MessageManager()
    {
        while (true)
        {
            if (_messageQueue.Count > 0)
            {
                var message = _messageQueue.Dequeue();
                
                Debug.Log(message._message);
                if (message._interval < 0)
                {
                    if (_messageQueue.Count <= 0)
                    {
                        _MessagePanel.SetActive(true);
                        _MessageText.text = message._message;
                        _MessageText.color = message._color;
                    }

                }
                else
                {
                    _MessagePanel.SetActive(true);
                    _MessageText.text = message._message;
                    _MessageText.color = message._color;
                    yield return new WaitForSeconds(message._interval);
                    _MessagePanel.SetActive(false);
                    _MessageText.text = string.Empty;
                }
            }
            yield return 0;
        }

    }
    #endregion

}
