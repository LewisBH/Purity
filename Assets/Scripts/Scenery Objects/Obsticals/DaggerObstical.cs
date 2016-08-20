using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class DaggerObstical : Obstical {

    Attack playerAttackScript;

	// Use this for initialization
	public override void Start () {
        base.Start();

        playerAttackScript = Player.Instance.GetComponent<Attack>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "playerWeapon")
        {
            if(col.GetComponent<Dagger>() != null)
            {
                if (playerAttackScript.secondaryAttack)
                {
                    TransitionState();
                }
            }
        }
    }
}
