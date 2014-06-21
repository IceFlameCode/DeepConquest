using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Account {

	public string Name;
	public string Password;
	public string Email;

	public List<City> Cities;

	public Account (GameData gameData, bool createStartCity = true) {
		Name = "";
		Password = "";
		Email = "";
		Cities = new List<City>();
		if (createStartCity)
			Cities.Add(new City(gameData));
	}
}
