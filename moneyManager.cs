using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class moneyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text floatingMoneyText;
	[SerializeField]private TMP_Text floatingPropMoneyText;
    [SerializeField] public float moneyMultiplier;
	[SerializeField] private float money;

	[Space(20)]
	[Header("Bool")]
	public bool isDrifting;
	public bool isProp;
	private bool isMoneyPaid;
	[Space(20)]
	private float _time = 0;
	private float _gainedMoney = 0;
	[Space(20)]
	[Header("Multipliers")]
	[SerializeField] private scriptableCarManager[] multipliers;

	public static moneyManager Instance;
	private void Awake()
	{
		Instance = this;
		money = 0;
	}
	private void Start()
	{
		isMoneyPaid = false;
		floatingMoneyText.text = Mathf.RoundToInt(money).ToString();
		moneyMultiplier = multipliers[GameManager.instance.ýndex].carIndex * 10;
		
	}
	

	private void moneyMaker()
	{
		money += moneyMultiplier * Time.deltaTime *2;
		floatingMoneyText.text = "+"+Mathf.RoundToInt(money).ToSafeString();
	}

	public void PropMoneyMaker()
	{
		_gainedMoney += 1;
		floatingPropMoneyText.text = "+"+ Mathf.RoundToInt(_gainedMoney).ToString();
		_time = 0;
	}

	private void checkPropMoney()
	{
		if(_time <= 3)
		{
			_time += Time.deltaTime;
		}
		else
		{
			floatingPropMoneyText.text = "";
			isProp = true;
		}
	}
	

	private void Update()
	{


		checkPropMoney();
		propMoneyPaid();

		if (!isDrifting)
		{
			floatingMoneyText.text = "";
			returnMoney();
			return;
		}

		if(isDrifting)
		{
			floatingMoneyText.GetComponent<Animator>().SetTrigger("moneyTrigger");
			isMoneyPaid = true;
			moneyMaker();
		}
		
	}
	


	private void propMoneyPaid()
	{
		if (!isProp)
		{
			return;
		}
		float current_money = GameManager.instance.money;
		current_money += _gainedMoney;
		GameManager.instance.money = current_money;

		GameManager.instance.moneyText.text = Mathf.RoundToInt(current_money).ToString();
		_gainedMoney = 0;
		isProp = false;
	}
	
	private void returnMoney()
	{
		if (!isMoneyPaid)
		{
			money = 0;
			return;
		}
		float current_money = GameManager.instance.money;
		current_money += money;
		GameManager.instance.money = current_money;

		GameManager.instance.moneyText.text = Mathf.RoundToInt(current_money).ToString();
		isMoneyPaid = false;
	}



}
