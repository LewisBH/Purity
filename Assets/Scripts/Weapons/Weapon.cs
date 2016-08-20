using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public int maxComboAmount;
    public int curComboAmount;
    public int damage;
    public int spCost;
    public AnimationClip[] attackAnimations;
    public AnimationClip secondaryAttackAnim;

	// Use this for initialization
    void Start ()
    {

    }
}
