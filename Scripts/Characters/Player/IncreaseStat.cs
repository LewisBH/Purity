using UnityEngine;
using System.Collections;

public class IncreaseStat : MonoBehaviour {

    [SerializeField]
    Dagger dagger;
    [SerializeField]
    Hammer hammer;
    [SerializeField]
    Sword sword;
    [SerializeField]
    RodOfLight rodOfLight;
    [SerializeField]
    Player player;

    void Start()
    {
        dagger = Dagger.Instance;
        hammer = Hammer.Instance;
        sword = Sword.Instance;
        rodOfLight = RodOfLight.Instance;
    }

	public void IncreaseDaggerCombo()
    {
        dagger.curComboAmount++;
        Debug.Log("Dagger combo amount is now: " + dagger.curComboAmount);
    }

    public void IncreaseHammerCombo()
    {
        hammer.curComboAmount++;
        Debug.Log("Hammer combo amount is now: " + hammer.curComboAmount);
    }

    public void IncreaseSwordCombo()
    {
        sword.curComboAmount++;
        Debug.Log("Sword combo amount is now: " + sword.curComboAmount);
    }

    public void IncreaseRodOfLightCombo()
    {
        rodOfLight.curComboAmount++;
        Debug.Log("Rod Of Light combo amount is now: " + rodOfLight.curComboAmount);
    }

    public void IncreaseMaxHP(int Amount)
    {
        player.HP.MaxVal += Amount;
        Debug.Log(" the players max HP is now: " + player.HP.MaxVal);
    }

    public void IncreaseMaxSp(int Amount)
    {
        player.SP.MaxVal += Amount;
        Debug.Log(" the players max SP is now: " + player.SP.MaxVal);
    }
}
