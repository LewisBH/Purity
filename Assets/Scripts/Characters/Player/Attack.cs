using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

    [SerializeField]
    private int curAttackSequence = 0;
    [SerializeField]
    private int maxAttackSequence;

    private PlayerController controller;

    public enum AttackType
    {
        Normal,
        Airborne,
        Secondary
    }

    public Weapon curWeapon;
    private Animation animator;
    private AnimationClip attackAnimation;
    private AnimationClip secondaryAttackAnimation;

    private bool reduceAttackTimer;
    private bool reduceSecondaryAttackTimer;
    private float timeToAttackAgain;
    private float timeToSecondaryAttack;

    private bool airborneAttacked;

    public bool secondaryAttack;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animation>();
        controller = GetComponent<PlayerController>();
	}

    public void Initialise()
    {
        attackAnimation = curWeapon.GetComponent<Weapon>().attackAnimations[curAttackSequence];
        secondaryAttackAnimation = curWeapon.GetComponent<Weapon>().secondaryAttackAnim;
        maxAttackSequence = curWeapon.maxComboAmount;
    }
	
	// Update is called once per frame
    void Update()
    {
        if (reduceAttackTimer)
        {
            timeToAttackAgain -= Time.deltaTime;// reduces the timer that decides whether you can attack again.

            if (timeToAttackAgain < -1)// if the timer reaches or goes below 0, then reset the timer and attack combo
            {
                reduceAttackTimer = false;

                timeToAttackAgain = AttackTime() + 0.3f;// this sets the timer to how ever long the current attack animation is + 0.3 seconds.

                ResetAttack();
            }
        }

        if(reduceSecondaryAttackTimer)
        {
            timeToSecondaryAttack -= Time.deltaTime;

            if(timeToSecondaryAttack < -1)
            {
                reduceSecondaryAttackTimer = false;

                timeToSecondaryAttack = SecondaryAttackTime() + 0.3f;

                secondaryAttack = false;
            }
        }

        if(controller.collisions.below)
        {
            airborneAttacked = false;
        }
    }

    public void AirborneAttack()
    {
        if(!airborneAttacked)
        {
            DoAttack(AttackType.Airborne);
            airborneAttacked = true;
        }
    }

    public void RegularAttack()
    {
        if (!reduceAttackTimer)// if you havent attacked in the current combo, start a new one.
        {
            reduceAttackTimer = true;

            DoAttack(AttackType.Normal);
        }
        else if (timeToAttackAgain < AttackTime() - (AttackTime() - 0.3f) || timeToAttackAgain > -1f && timeToAttackAgain < 0)// this checks to see if the timer is withing 0.3 seconds of the end of the current animation
        {
            timeToAttackAgain = AttackTime() + 0.3f;
            DoAttack(AttackType.Normal);
        }
    }

    public void SecondaryAttack()
    {
        if(reduceSecondaryAttackTimer)
        {
            reduceSecondaryAttackTimer = true;

            DoAttack(AttackType.Secondary);
        }
    }

    public void DoAttack(AttackType attackType)
    {
        if(!secondaryAttack)
        {
            if(curAttackSequence < maxAttackSequence)
            {
                attackAnimation = curWeapon.GetComponent<Weapon>().attackAnimations[curAttackSequence];
                animator.clip = attackAnimation;
                animator.Play(attackAnimation.name, PlayMode.StopAll);
                curAttackSequence++;
                if (curAttackSequence > maxAttackSequence - 1)
                {
                    curAttackSequence = 0;
                }
            }
        }
        else
        {
            attackAnimation = curWeapon.GetComponent<Weapon>().secondaryAttackAnim;
            animator.clip = attackAnimation;
            animator.Play(attackAnimation.name, PlayMode.StopAll);
            secondaryAttack = true;
        }
        
    }

    public void ChangeWeaponTo(Weapon weapon)
    {
        curWeapon = weapon;
        maxAttackSequence = curWeapon.maxComboAmount;
    }

    private float AttackTime()
    {
        return attackAnimation.length;
    }

    private float SecondaryAttackTime()
    {
        return secondaryAttackAnimation.length;
    }

    private void ResetAttack()
    {
        curAttackSequence = 0;
    }
}
