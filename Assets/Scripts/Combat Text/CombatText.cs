using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CombatText : MonoBehaviour {

    float speed;
    Vector3 direction;
    float fadeTime;
    public AnimationClip criticalHitAnim;
    bool crit;
	
	// Update is called once per frame
	void Update () {
        if(!crit)
        {
            float translation = speed * Time.deltaTime;

            transform.Translate(direction * translation);
        }
	}

    public void Initilise(float speed, Vector3 direction, float fadeTime, bool crit)
    {
        this.speed = speed;
        this.direction = direction;
        this.fadeTime = fadeTime;
        this.crit = crit;

        if(crit)
        {
            GetComponent<Animator>().SetTrigger("Critical");
            StartCoroutine(Critical());
        }
        else
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator Critical()
    {
        yield return new WaitForSeconds(criticalHitAnim.length);

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float startAlpha = GetComponent<Text>().color.a;

        float rate = 1.0f / fadeTime;
        float progress = 0.0f;

        while(progress < 1.0)
        {
            Color tmpColor = GetComponent<Text>().color;

            GetComponent<Text>().color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(startAlpha, 0, progress));

            progress += rate * Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }
}
