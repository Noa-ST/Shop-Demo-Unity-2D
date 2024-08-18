using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDialog : Dialog
{
    public Transform gridRoot;
    public ShopItemUi itemUIPb;

    public override void Show(bool isShow)
    {
        base.Show(isShow);
        UpdateUI();
    }

    public override void Close()
    {
        base.Close();
    }

    private void UpdateUI()
    {
        var items = ShopManager.Ins.items;

        if (items == null || items.Length <= 0 || !gridRoot || !itemUIPb) return;

        ClearChilds();

        for (int i = 0; i < items.Length; i++)
        {
            int idx = i;

            var item = items[i];

            if (item != null)
            {
                var itemUIClone = Instantiate(itemUIPb, Vector3.zero, Quaternion.identity);

                itemUIClone.transform.SetParent(gridRoot);

                itemUIClone.transform.localPosition = Vector3.zero;

                itemUIClone.transform.localScale = Vector3.one;

                itemUIClone.UpdateUI(item, idx);

                if(itemUIClone.bth)
                {
                    itemUIClone.bth.onClick.RemoveAllListeners();
                    itemUIClone.bth.onClick.AddListener(() => ItemEvent(item, idx));
                }
            }
        }
    }

    void ItemEvent(ShopItem item, int shopItemId)
    {
        if (item == null) return;

        bool isInlocked = Pref.GetBool(PrefConst.PLAYER_PEFIX + shopItemId);


        if (isInlocked)
        {
            if (shopItemId == Pref.CurPlayerId) return;

            Pref.CurPlayerId = shopItemId;

            GameManager.Ins.ActivePlayer();

            UpdateUI();
        }
        else
        {
            if(Pref.Coins >= item.price)
            {
                Pref.Coins -= item.price;

                Pref.SetBool(PrefConst.PLAYER_PEFIX + shopItemId, true);

                Pref.CurPlayerId = shopItemId;

                GUIManager.Ins.UpdateCoins();

                GameManager.Ins.ActivePlayer();

                UpdateUI();
            } else
            {
                Debug.Log("Ban ko du tien");
            }
        }
    }

    public void ClearChilds()
    {
        if (!gridRoot || gridRoot.childCount <= 0) return;

        for (int i = 0; i < gridRoot.childCount; i++)
        {
            var child = gridRoot.GetChild(i);

            if (child)
                Destroy(child.gameObject);
        }
    }
}
