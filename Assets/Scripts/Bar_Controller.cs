using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar_Controller : MonoBehaviour
{
    [SerializeField] private Image mainBar;
    [SerializeField] private Image slowBar;
    [SerializeField] private float maxValue;
    [SerializeField] private float reduceSpeed;
    private float value;
    private float target = 1;
    [SerializeField] private bool showDecrease = true;

    // Start is called before the first frame update
    void Start()
    {
        value = maxValue;
        slowBar.enabled = showDecrease;
    }

    public void updateBar(){
        target = value / maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        updateBar();
        mainBar.fillAmount = target;
        slowBar.fillAmount = Mathf.MoveTowards(slowBar.fillAmount,target,reduceSpeed*Time.deltaTime);
    }
    public float getValue(){return value;}
    public void setValue(float _value){
        value = (_value<maxValue) ? _value : maxValue;
    }
    public float getMaxValue(){return maxValue;}
    public void setMaxValue(float _value){maxValue = _value;}
}
