using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMoneyScript : MonoBehaviour
{
    [SerializeField] private int money = 1000;
    public TextMeshProUGUI moneyText;

    private void Start()
    {
        moneyText.text = money.ToString();
    }

    public int getMoney()
    {
        return money;
    }

    public void changeMoney(int change)
    {
        money += change;
        moneyText.text = money.ToString();
    }
}
