using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;
    public GameObject shakeMessage;
    public GameObject tiltMessage;

    public bool shakeMsgIsOn;
    public bool tiltMsgIsOn;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        } else {
            //Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    public void TurnOnShakeMessage() {
        shakeMessage.SetActive(true);
        shakeMsgIsOn = true;
    }

    public void TurnOffShakeMessage() {
        shakeMessage.SetActive(false);
        shakeMsgIsOn = false;
    }

    public void TurnOnTiltMessage() {
        tiltMessage.SetActive(true);
        tiltMsgIsOn = true;
    }

    public void TurnOffTiltMessage() {
        tiltMessage.SetActive(false);
        tiltMsgIsOn = false;
    }
}
