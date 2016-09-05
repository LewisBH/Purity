using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[Serializable]
public class Bar : MonoBehaviour {

    private float fillAmount;
    private Image bar;

    [SerializeField]
    private float lerpSpeed;

    public int MaxValue { get; set; }

    public float Value
    {
        set
        {
            fillAmount = Map(value, 0, MaxValue, 0, 1);
        }
    }

    void Start()
    {
        bar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleBar();
    }

    private void HandleBar()
    {
        if (fillAmount != bar.fillAmount)
        {
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
        }
    }

    private float Map(float health, float inMinHealth, float inMaxHealth, float outMinHealth, float outMaxHealth)
    {
        return (health - inMinHealth) * (outMaxHealth - outMinHealth) / (inMaxHealth - inMinHealth) + outMinHealth;
    }
}
