using UnityEngine;
using System.Collections;

public class BasicEnemy : Enemy, IMelee {

    public float attackCD = 2f;
	
	// Update is called once per frame
	void Update () {
        
        if (Vector2.Distance(transform.position, player.transform.position) < 5)
        {
            MeleeAttack();
        }
        else if(Vector2.Distance(transform.position, player.transform.position) < 10)
        {

        } 
        

	}

    public void MeleeAttack()
    {

    }
}
