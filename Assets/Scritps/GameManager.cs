using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    Player player;
    public override void Awake()
    {
        MakeSingleton(false);   
    }

    public override void Start()
    {
        base.Start();

        if(!PlayerPrefs.HasKey(PrefConst.COIN_KEY))
            Pref.Coins = 10000;

        ActivePlayer();

        GUIManager.Ins.UpdateCoins();
    } 

    public void ActivePlayer()
    {
        if (player)
            Destroy(player.gameObject);

        var newPlayerPb = ShopManager.Ins.items[Pref.CurPlayerId].playerPb;

        if (newPlayerPb)
            player = Instantiate(newPlayerPb, Vector3.zero, Quaternion.identity);
    }
}
