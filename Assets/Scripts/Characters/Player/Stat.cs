using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Stat  {

    [SerializeField]
    private Bar bar;

    [SerializeField]
    private int maxVal;

    [SerializeField]
    private int curVal;

    public int MaxVal
    {
        get
        {
            return maxVal;
        }

        set
        {
            this.maxVal = value;
            bar.MaxValue = maxVal;
        }
    }

    public int CurVal
    {
        get
        {
            return curVal;
        }

        set
        {
            this.curVal = Mathf.Clamp(value, 0, MaxVal);
            bar.Value = curVal;
        }
    }

    public void Initilise()
    {
        this.MaxVal = maxVal;
        this.CurVal = curVal;
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
