using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	
	[Header("Data Name")]
	[SerializeField] private string dataName;
	[Space(10)]
	[Header("Money & Index")]
	public float money;
	[SerializeField] public int �ndex;
	[Space(10)]
	[Header("Text")]
	public TMP_Text moneyText;

	[Space(20)]
	public GameObject _car;
	[SerializeField] private GameObject _wheel;
	[SerializeField] private GameObject _arrows;
	public string wheelSTR;
	public bool carShiftBool;

	[Space(10)]
	[SerializeField] private Transform carLocation;
	[SerializeField] private List<GameObject> cars = new List<GameObject>();

	[Space(20)]
	public float AbsoulteCarSpeed;

	private bool[] _cars;
	public static GameManager instance;
	private void Awake()
	{
		Time.timeScale = 1.0f;
		instance = this;
		getIndexOfCar();
		wheelSTR = PlayerPrefs.GetString("enabled", "arrow");
	}

	#region wheelController

	private void changeController(string key)
	{
		switch(key)
		{
			case "wheel":
				{
					changer_wheel();
					break;
				}
			case "arrow":
				{
					changer_arrow();
					break;
				}
		}
	}
	private void FixedUpdate()
	{
		if(_car != null)
		AbsoulteCarSpeed = _car.GetComponent<CarController>().getCarspeedForDamage();

		
	}
	public bool getShiftMethod()
	{
		if (_car == null)
			return false;
		
			carShiftBool = _car.GetComponent<CarController>().returnShift();
			return carShiftBool;
		
		
	}
	private void changer_wheel()
	{
		_wheel.SetActive(true);
		_arrows.SetActive(false);

		_car.gameObject.GetComponent<CarController>().arrowBool = false;
		_car.gameObject.GetComponent<CarController>().wheelBool = true;
	}
	private void changer_arrow()
	{
		_arrows.SetActive(true);
		_wheel.SetActive(false);

		_car.gameObject.GetComponent<CarController>().arrowBool = true;
		_car.gameObject.GetComponent<CarController>().wheelBool = false;
	}
	public void changeType_wheel()
	{
		
		PlayerPrefs.SetString("enabled", "wheel");
		changeController("wheel");
	}
	public void changeType_arrow()
	{
		PlayerPrefs.SetString("enabled", "arrow");
		changeController("arrow");
	}
	#endregion

	#region car_data
	private void getIndexOfCar()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath + dataName, FileMode.Open);
		carSave cs = (carSave)bf.Deserialize(file);
		Debug.Log(" Data received");

		money = cs.money;
		�ndex = cs.selectedCarIndex;
		moneyText.text = Mathf.RoundToInt(money).ToString();
		_cars = cs.AmountOfCars;
		
		file.Close();

		placeTheCar();
	}



	private void placeTheCar()
	{

		cars[�ndex].SetActive(true);
		cars[�ndex].tag = "Player";
		_car = cars[�ndex];
		cars[�ndex].GetComponent<CarController>().enabled = true;
		cars[�ndex].GetComponent<CarController>().useUI = true;

		cars[�ndex].GetComponent<CarController>().shiftButton.onClick.AddListener(() => 
		cars[�ndex].GetComponent<CarController>().shiftMethod());

		cars[�ndex].transform.SetParent(carLocation, true);
		cars[�ndex].transform.position = carLocation.position;

		string _str = PlayerPrefs.GetString("enabled", "arrow");
		changeController(_str);
		
	}

	public void saveChanges()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + dataName);

		 carSave newCs = new carSave();
		 newCs.money = money;
		 newCs.selectedCarIndex = �ndex;
		 newCs.AmountOfCars = _cars;
		 bf.Serialize(file, newCs);
		 file.Close();
		
	}
	#endregion

}
