using UnityEngine;
using System.Collections;

public class LevelMaster : MonoBehaviour {

    public GameObject skillPointPrefab;

	// Use this for initialization
	void Start () {
        SaveLists.Instance.InitialiseSceneList();

        if(GameControl.Instance.isSceneBeingLoaded || GameControl.Instance.isSceneBeingTransitioned)
        {
            SavedItemList itemLocalList = SaveLists.Instance.GetItemListForScene();
            SavedUpgrades upgradeLocalList = SaveLists.Instance.GetUpgradeList();
            SavedEnemyList enemyLocalList = SaveLists.Instance.GetEnemyListForScene();
            SavedObsticalList obsticalLocalList = SaveLists.Instance.GetObsticalListForScene();

            Player.Instance.gameObject.transform.position = new Vector3(
                SaveLists.Instance.savedPlayerData.PosX,
                SaveLists.Instance.savedPlayerData.PosY,
                0);

            if(upgradeLocalList != null)
            {
                for (int i = 0; i < upgradeLocalList.SavedUpgradeSkills.Count; i++)
                {
                    if (GameControl.Instance.upgradeScripts[i].id == upgradeLocalList.SavedUpgradeSkills[i].ID)
                    {
                        GameControl.Instance.upgradeScripts[i].avaliable = upgradeLocalList.SavedUpgradeSkills[i].Avaliable;
                        GameControl.Instance.upgradeScripts[i].unlocked = upgradeLocalList.SavedUpgradeSkills[i].Unlocked;
                    }
                }
            }

            if (itemLocalList != null)
            {
                for (int i = 0; i < itemLocalList.SavedSkillPoints.Count; i++)
                {
                    GameObject spawnedSkillPoint = Instantiate(skillPointPrefab) as GameObject;
                    print(itemLocalList.SavedSkillPoints[i].PickedUp);
                    spawnedSkillPoint.GetComponent<SkillPointPickUpable>().hasBeenPickedUp = itemLocalList.SavedSkillPoints[i].PickedUp;
                }
                
            }

            if(enemyLocalList != null)
            {
                GameObject[] SceneBossList = GameObject.FindGameObjectsWithTag("Boss");

                for (int i = 0; i < SceneBossList.Length; i++)
                {
                    for (int j = 0; j < enemyLocalList.savedBosses.Count; j++)
                    {
                        if (enemyLocalList.savedBosses[i].name == SceneBossList[j].name)
                        {
                            Boss bossScript = SceneBossList[i].GetComponent<Boss>();
                            bossScript.isDead = enemyLocalList.savedBosses[j].isDead;

                            if (bossScript.isDead)
                            {
                                bossScript.obstical.TransitionState();
                            }
                        }
                    }
                }
            }

            if(obsticalLocalList != null)
            { 
                for (int i = 0; i < obsticalLocalList.savedObsticals.Count; i++)
                {
                    print(obsticalLocalList.savedObsticals[i].curState);
                    ObjectLists.Instance.obsticalList[i].GetComponent<Obstical>().curState = obsticalLocalList.savedObsticals[i].curState;
                }
            }
        }
	}
}