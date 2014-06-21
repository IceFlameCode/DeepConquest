using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Net : MonoBehaviour {

	// server stuff
	public List<Account> Accounts;

	// client stuff
	public Account CurrentAccount;
	public int CurrTicket;
	public int TotalCities = 0;

	HUD hud;
	
	void Start () {
		if (S.ClientBuild) {
			CurrentAccount = new Account(GetComponent<GameData>(), false);
			CurrentAccount.Name = PlayerPrefs.GetString("UserName", "");
			Network.Connect("127.0.0.1", 25001);
		} else {
			Accounts = new List<Account>();
		}
		hud = GetComponent<HUD>();
	} // end of Start()

	void Update () {
	}

	public void SendRegisterRequest () {
		CurrTicket = Random.Range(0, 10000);
		networkView.RPC("RegisterAccount", RPCMode.Server, CurrentAccount.Name, CurrentAccount.Password, CurrentAccount.Email, CurrTicket);
	}

	// called on the server
	[RPC]
	void RegisterAccount (string name, string password, string email, int ticket) {
		print("Register attempt with name " + name + ", password " + password);

		if (!S.NotEmpty(name) || !S.NotEmpty(password)) {
			networkView.RPC("RegisterFail", RPCMode.Others, ticket);
			return;
		}
		Account a = new Account(hud.GameStats);
		a.Name = name;
		a.Password = password;
		a.Email = email;
		for (int i = 0; i < Accounts.Count; i++) {
			if (Accounts[i].Name == name) {
				networkView.RPC("RegisterFail", RPCMode.Others, ticket);
				return;
			}
		}
		Accounts.Add(a);
		networkView.RPC("RegisterSuccess", RPCMode.Others, ticket);
	}

	[RPC]
	void RegisterSuccess (int ticket) {
		if (CurrTicket == ticket) {
			hud.RegisterSuccessful = true;
			hud.WrongPassword = false;
		}
	}

	[RPC]
	void RegisterFail (int ticket) {
		if (CurrTicket == ticket) {
			hud.RegisterFailed = true;
			hud.WrongPassword = false;
		}
	}

	public void SendLoginRequest() {
		CurrTicket = Random.Range(0, 10000);
		networkView.RPC("AttemptLogin", RPCMode.Server, CurrentAccount.Name, CurrentAccount.Password, CurrTicket);
	}

	// called on the server by clients that want to log in
	[RPC]
	void AttemptLogin (string name, string password, int loginTicket) {
		print("Login attempt with name " + name + ", password " + password);
		
		for (int i = 0; i < Accounts.Count; i++) {
			if (Accounts[i].Name == name) {
				if (Accounts[i].Password == password) {
					networkView.RPC("LoginSuccess", RPCMode.Others, loginTicket);
					SendCities(loginTicket, i);
					return;
				}
				networkView.RPC("LoginFail", RPCMode.Others, loginTicket);
				return;
			}
		}
		networkView.RPC("LoginFail", RPCMode.Others, loginTicket);
	}

	// this is called on clients, and only the client with the same login ticket responds
	[RPC]
	void LoginSuccess (int loginTicket) {
		if (CurrTicket == loginTicket) {
			hud.Mode = HudMode.CityView;
			PlayerPrefs.SetString("UserName", CurrentAccount.Name);
		}
	}

	// this is called on all clients, and only the one with the same login ticket responds
	[RPC]
	void LoginFail (int loginTicket) {
		if (CurrTicket == loginTicket) {
			hud.LoggingIn = false;
			hud.WrongPassword = true;
		}
	}
	
	void SendCities (int ticket, int userID) {
		print ("Sending cities to user " + userID.ToString());
		
		networkView.RPC("SetCityCount", RPCMode.Others, ticket, Accounts[userID].Cities.Count);
		for (int i = 0; i < Accounts[userID].Cities.Count; i++) {
			networkView.RPC("RecieveCity", RPCMode.Others, ticket, Accounts[userID].Cities[i].Name, Accounts[userID].Cities[i].Encode(),
			                Accounts[userID].Cities[i].ResourcesInside[(int)ResourceType.Energy],
			                Accounts[userID].Cities[i].ResourcesInside[(int)ResourceType.Food],
			                Accounts[userID].Cities[i].ResourcesInside[(int)ResourceType.Iron],
			                Accounts[userID].Cities[i].ResourcesInside[(int)ResourceType.Silicon],
			                Accounts[userID].Cities[i].ResourcesInside[(int)ResourceType.Population],
			                Accounts[userID].Cities[i].ResourcesInside[(int)ResourceType.Happiness],
			                Accounts[userID].Cities[i].ResourcesInside[(int)ResourceType.Money]);
		}
	}

	[RPC]
	void SetCityCount (int ticket, int count) {
		if (CurrTicket == ticket) {
			TotalCities = count;
		}
	}

	// called on all clients, only the one which needs that info should respond
	[RPC]
	void RecieveCity (int ticket, string cityName, string code,
	                  float energy, float food, float iron, 
	                  float silicon, float population, float happiness,
	                  float money) {

		if (CurrTicket == ticket) {
			City c = new City(hud.GameStats);
			c.Name = cityName;
			c.Decode(code);

			c.ResourcesInside[(int)ResourceType.Energy] = energy;
			c.ResourcesInside[(int)ResourceType.Food] = food;
			c.ResourcesInside[(int)ResourceType.Iron] = iron;
			c.ResourcesInside[(int)ResourceType.Silicon] = silicon;
			c.ResourcesInside[(int)ResourceType.Population] = population;
			c.ResourcesInside[(int)ResourceType.Happiness] = happiness;
			c.ResourcesInside[(int)ResourceType.Money] = money;

			CurrentAccount.Cities.Add(c);
			if (CurrentAccount.Cities.Count == TotalCities) {
				hud.AllCitiesLoaded = true;
			}
		}
	}

	// called by the client to tell the server that we changed something in the currently viewed city
	public void BroadcastResourceChange (float energyGain, float foodGain, float ironGain,
	                             		float siliconGain, float populationGain, float happinessGain,
	                             		float moneyGain) {
		networkView.RPC("UpdateCityResourcesOnServer", RPCMode.Server,
		                CurrentAccount.Name, hud.CurrCityID, CurrentAccount.Cities[hud.CurrCityID].Encode(),
		                energyGain, foodGain, ironGain,
		                siliconGain, populationGain, happinessGain,
		                moneyGain);
	}

	// called on the server to tell it our client changed something
	[RPC]
	void UpdateCityResourcesOnServer (string accountName, int cityID, string cityCode,
	                         float energyGain, float foodGain, float ironGain,
	                         float siliconGain, float populationGain, float happinessGain,
	                         float moneyGain) {
		for (int i = 0; i < Accounts.Count; i++) {
			if (Accounts[i].Name == accountName) {
				Accounts[i].Cities[cityID].Decode(cityCode);
				Accounts[i].Cities[cityID].ResourcesInside[(int)ResourceType.Energy] += energyGain;
				Accounts[i].Cities[cityID].ResourcesInside[(int)ResourceType.Food] += foodGain;
				Accounts[i].Cities[cityID].ResourcesInside[(int)ResourceType.Iron] += ironGain;
				Accounts[i].Cities[cityID].ResourcesInside[(int)ResourceType.Silicon] += siliconGain;
				Accounts[i].Cities[cityID].ResourcesInside[(int)ResourceType.Population] += populationGain;
				Accounts[i].Cities[cityID].ResourcesInside[(int)ResourceType.Happiness] += happinessGain;
				for (int j = 0; j < Accounts[i].Cities.Count; i++) {
					Accounts[i].Cities[j].ResourcesInside[(int)ResourceType.Money] += moneyGain; // money is the same in all cities
				}
			}
		}
	} // end of UpdateCityResourcesOnServer()

	public void BroadcastUpgrade() {
		networkView.RPC("ServerGetUpgrade", RPCMode.Server, CurrentAccount.Name, hud.CurrCityID, hud.CurrBuilding.x, hud.CurrBuilding.y);
	}

	[RPC]
	void ServerGetUpgrade (string AccountName, int cityID, int x, int y) {
		for (int i = 0; i < Accounts.Count; i++) {
			if (Accounts[i].Name == AccountName) {
				Accounts[i] = DoUpgrade(Accounts[i], cityID, x, y);
			}
		}
	}

	public Account DoUpgrade (Account acc, int cityID, int x, int y) { // coords of building
		for (int i = 0; i < (int)ResourceType.Count; i++) {
			if ((ResourceType)i == ResourceType.Money) {
				for (int j = 0; j < acc.Cities.Count; j++) {
					acc.Cities[j].ResourcesInside[(int)ResourceType.Money] -= hud.GameStats.GetBuildCost(acc.Cities[cityID].Buildings[x, y].type, ResourceType.Money, acc.Cities[cityID].Buildings[x, y].Level);
				}
			} else if ((ResourceType)i == ResourceType.Population) {
				acc.Cities[cityID].ResourcesInside[(int)ResourceType.Population] += acc.Cities[cityID].Buildings[x, y].WorkersAtWork;
				acc.Cities[cityID].Buildings[x, y].WorkersAtWork = Mathf.CeilToInt(hud.GameStats.GetBuildCost(acc.Cities[cityID].Buildings[x, y].type, ResourceType.Population, acc.Cities[cityID].Buildings[x, y].Level));
				acc.Cities[cityID].ResourcesInside[(int)ResourceType.Population] -= hud.GameStats.GetBuildCost(acc.Cities[cityID].Buildings[x, y].type, ResourceType.Population, acc.Cities[cityID].Buildings[x, y].Level);
				acc.Cities[cityID].Buildings[x, y].isBeingBuilt = true;
				acc.Cities[cityID].Buildings[x, y].BuildingFinishMoment = Time.time + hud.GameStats.GetBuildTime(acc.Cities[cityID].Buildings[x, y].type, acc.Cities[cityID].Buildings[x, y].Level);
			} else {
				acc.Cities[cityID].ResourcesInside[i] -= hud.GameStats.GetBuildCost(acc.Cities[cityID].Buildings[x, y].type, (ResourceType)i, acc.Cities[cityID].Buildings[x, y].Level);
			}
		}
		acc.Cities[cityID].Buildings[x, y].Level++;
		return acc;
	}

	public void BroadcastBuildFromEmpty(BuildingType buildingType) {
		networkView.RPC("ServerBuildFromEmpty", RPCMode.Server, CurrentAccount.Name, hud.CurrCityID, hud.CurrBuilding.x, hud.CurrBuilding.y, (int)buildingType);
	}
	
	[RPC]
	void ServerBuildFromEmpty (string AccountName, int cityID, int x, int y, int bType) {
		for (int i = 0; i < Accounts.Count; i++) {
			if (Accounts[i].Name == AccountName) {
				Accounts[i] = BuildFromEmpty(Accounts[i], cityID, x, y, (BuildingType)bType);
			}
		}
	}

	public Account BuildFromEmpty (Account acc, int cityID, int x, int y, BuildingType buildType) {
		acc.Cities[cityID].Buildings[x, y].type = buildType;
		return DoUpgrade(acc, cityID, x, y);
	}

	public void AssignWorkersAtWork() {
		networkView.RPC("ServerAssignWorkersAtWork", RPCMode.Server, CurrentAccount.Name, hud.CurrCityID, hud.CurrBuilding.x, hud.CurrBuilding.y, hud.CurrBuilding.WorkersAtWork);
	}

	[RPC]
	void ServerAssignWorkersAtWork (string accName, int cityID, int x, int y, int workersToAssign) {
		for (int i = 0; i < Accounts.Count; i++) {
			if (Accounts[i].Name == accName) {
				Accounts[i].Cities[cityID].Buildings[x, y].WorkersAtWork = workersToAssign;
			}
		}
	}

	public void BroadcastCityNameChange() {
		networkView.RPC("RecieveCityNameChange", RPCMode.Server, CurrentAccount.Name, hud.CurrCityID, CurrentAccount.Cities[hud.CurrCityID].Name);
	}

	[RPC]
	void RecieveCityNameChange (string accName, int cityID, string cityName) {
		for (int i = 0; i < Accounts.Count; i++) {
			if (Accounts[i].Name == accName) {
				Accounts[i].Cities[cityID].Name = cityName;
			}
		}
	}
}
