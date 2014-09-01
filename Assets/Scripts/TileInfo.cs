using UnityEngine;
using System.Collections;

public class TileInfo : MonoBehaviour 
{
    public enum TileType
    {
        NORMAL,
        BONUS_GUARANTEE_CANDY,
        BONUS_CANDY_SNATCHER,
        BONUS_GLUCOSE_PILL,
        PENALTY_WITCH_SPELL,
        PENALTY_FROZEN_FINGERS,
        PENALTY_BROKEN_DICE,
        CHEST,
        GATE
    }

    public int id;
    public int short_cut_id;
    public int nextId;
    public TileType type;
    public int currentOrietation;
    public int direction = 1; //Default is facing to left
    public bool isShortCut = false;
    public int shortCutAngle = 0;

    public int branchOutIndex = 0;
    public int mergeIndex = 0;

    public int sugarAward = 10;
    public int bonusCoinsAward = 50;

    public bool isChestAvailable = false;

    private void Start()
    {
        if(type == TileType.CHEST)
        {
            isChestAvailable = true;
        }
    }

    public void switchOrientation()
    {

    }

    public int getRandomCandy()
    {
        //1 - Green candy
        //2 - Red candy
        //3 - Blue candy

        return UnityEngine.Random.Range(1, 3);
    }

    public void chestPicked()
    {
        isChestAvailable = false;
    }

    public void respawnChest()
    {
        isChestAvailable = true;
    }
}