using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volumeslider : MonoBehaviour
{
    [SerializeField] private Slider s;

    void Start(){
        AudioManager.instance.ChangeMasterVolume(s.value);
        s.onValueChanged.AddListener(val=> AudioManager.instance.ChangeMasterVolume(val));
    }
}
