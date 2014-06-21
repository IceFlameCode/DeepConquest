using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// pretty much the main program
public class HUD : MonoBehaviour {

	public Net MainNet;
	public GameData GameStats;

	public Font SciFiFont;
	public Texture2D Logo;
	public HudMode Mode = HudMode.Login;

	public bool AllCitiesLoaded = false;
	public bool LoggingIn = false;
	public bool WrongPassword = false;
	public bool RegisterSuccessful = false;
	public bool RegisterFailed = false;
	public int CurrCityID = 0;
	public bool CurrCityLoaded = false;

	public bool ShowBuildingWindow = false;
	public bool ShowAdvancedStatsWindow = false;
	public bool ShowFromScratchWindow = false;
	public bool ShowCityStatsWindow = false;
	public Building CurrBuilding;
	public Rect BuildingWindowRect = new Rect(0f, 100f, 600f, 400f);
	public Rect AdvancedStatsWindowRect = new Rect(600f, 100f, 600f, 400f);
	public Rect FromScratchWindowRect = new Rect(600f, 100f, 600f, 400f);
	public Rect CityStatsWindowRect = new Rect(300f, 100f, 600f, 400f);

	public float TempWorkers = 0f;
	public float NextWorkerUpdate = 0f;
	public float NextResourceUpdate = 0f;
	public float NextCityNameUpdate = 0f;



	void Start () {
		MainNet = GetComponent<Net>();
		GameStats = GetComponent<GameData>();
		DontDestroyOnLoad(transform.gameObject);
	}



	void Update () {
		if (CurrCityLoaded) {
			// that means we're the client, too
			if (MainNet.CurrentAccount.Cities[CurrCityID].BuildingSelected) {
				if (Input.GetMouseButtonDown(0)) {
					ShowBuildingWindow = true;
					CurrBuilding = MainNet.CurrentAccount.Cities[CurrCityID].SelectedBuilding;
					TempWorkers = MainNet.CurrentAccount.Cities[CurrCityID].ResourcesInside[(int)ResourceType.Population] + CurrBuilding.WorkersAtWork;
				}
			}
			MainNet.CurrentAccount.Cities[CurrCityID].UpdateBuildings();
			if (NextResourceUpdate <= Time.time) {
				float moneyGain = 0f;
				for (int i = 0; i < MainNet.CurrentAccount.Cities.Count; i++) {
					moneyGain += MainNet.CurrentAccount.Cities[i].UpdateResources();
				}
				for (int i = 0; i < MainNet.CurrentAccount.Cities.Count; i++) {
					MainNet.CurrentAccount.Cities[i].ResourcesInside[(int)ResourceType.Money] += moneyGain;
				}
				NextResourceUpdate = Time.time + S.ResourceUpdateTime;
			}
			if (ShowBuildingWindow) {
				if (Time.time >= NextWorkerUpdate) {
					MainNet.AssignWorkersAtWork();
					NextWorkerUpdate = Time.time + 1f;
				}
			}
			if (ShowCityStatsWindow) {
				if (Time.time >= NextCityNameUpdate) {
					MainNet.BroadcastCityNameChange();
					NextCityNameUpdate = Time.time + 1f;
				}
			}
		}
		if (S.ClientBuild == false) {
			for (int i = 0; i < MainNet.Accounts.Count; i++) {
				for (int j = 0; j < MainNet.Accounts[i].Cities.Count; j++) {
					MainNet.Accounts[i].Cities[j].UpdateBuildings();
				}
			}
			if (NextResourceUpdate <= Time.time) {
				for (int i = 0; i < MainNet.Accounts.Count; i++) {
					float moneyGain = 0f;
					for (int j = 0; j < MainNet.Accounts[i].Cities.Count; j++) {
						moneyGain += MainNet.Accounts[i].Cities[j].UpdateResources();
					}
					for (int j = 0; j < MainNet.Accounts[i].Cities.Count; j++) {
						MainNet.Accounts[i].Cities[j].ResourcesInside[(int)ResourceType.Money] += moneyGain;
					}
				}
				NextResourceUpdate = Time.time + S.ResourceUpdateTime;
			}
		}
	}



	// GUI stuff
	void OnGUI () {
		if (S.ClientBuild) {
			
			float midHeight = Screen.height * 0.5f;
			float midWidth = Screen.width * 0.5f;
			float textHeight = 30f;
			stylizeGUI();

			if (Network.peerType == NetworkPeerType.Client) {
					
				switch(Mode) {
				case HudMode.Login:
					GUI.color = Color.white;
					GUI.DrawTexture(new Rect(Screen.width - 512f, 0f, 512f, 174f), Logo);
					GUI.Box(new Rect(midWidth - 200, midHeight, 120, textHeight), "Account name:");
					MainNet.CurrentAccount.Name = GUI.TextArea (new Rect(midWidth - 80, midHeight, 280, textHeight), MainNet.CurrentAccount.Name);
	
					GUI.Box(new Rect(midWidth - 200, midHeight + textHeight, 120, textHeight), "Password:");
					MainNet.CurrentAccount.Password = GUI.PasswordField (new Rect(midWidth - 80, midHeight + textHeight, 280, textHeight), MainNet.CurrentAccount.Password, '#');
						
					if (GUI.Button(new Rect(midWidth - 200, midHeight + textHeight * 2f, 400, textHeight), "Log in")) {
						MainNet.SendLoginRequest();
						LoggingIn = true;
					}
						
					if (GUI.Button(new Rect(midWidth - 200, midHeight + textHeight * 3f, 400, textHeight), "Register new account")) {
						MainNet.SendRegisterRequest();
					}
						
					if (LoggingIn) {
						GUI.color = S.FlashyColor;
						GUI.Box(new Rect(midWidth - 200, midHeight + textHeight * 4f, 400, textHeight), "Logging in, please wait...");
						GUI.color = Color.white;
					} else if (WrongPassword) {
						GUI.color = S.FlashyColor;
						GUI.Box(new Rect(midWidth - 200, midHeight + textHeight * 4f, 400, textHeight), "Wrong username/password combination!");
						GUI.color = Color.white;
					} else if (RegisterSuccessful) {
						GUI.color = S.FlashyColor;
						GUI.Box(new Rect(midWidth - 200, midHeight + textHeight * 4f, 400, textHeight), "Account registered successfully!");
						GUI.color = Color.white;
					} else if (RegisterFailed) {
						GUI.color = S.FlashyColor;
						GUI.Box(new Rect(midWidth - 200, midHeight + textHeight * 4f, 400, textHeight), "Pick a different name!");
						GUI.color = Color.white;
					}
					break;
				case HudMode.CityView:
					if (AllCitiesLoaded) {
						if (CurrCityLoaded) {

							int currWindowID = 0;
							if (ShowBuildingWindow) {
								BuildingWindowRect = GUI.Window(currWindowID, BuildingWindowRect, displayBuildingWindow, "");
								currWindowID++;
							}
							if (ShowAdvancedStatsWindow) {
								AdvancedStatsWindowRect = GUI.Window(currWindowID, AdvancedStatsWindowRect, displayAdvancedStatsWindow, "");
								currWindowID++;
							}
							if (ShowFromScratchWindow) {
								FromScratchWindowRect = GUI.Window(currWindowID, FromScratchWindowRect, displayFromScratchWindow, "");
								currWindowID++;
							}
							if (ShowCityStatsWindow) {
								CityStatsWindowRect = GUI.Window(currWindowID, CityStatsWindowRect, displayCityStatsWindow, "");
								currWindowID++;
							}

							navBar();

						} else { // current city not loaded
							Application.LoadLevel("CityLevel");
							CurrCityLoaded = true;
						}
					} else { // some cities still need to be loaded by the net
						GUI.Box(new Rect(0f, midHeight, Screen.width, textHeight), "Please wait while all your cities are loaded...");
					}
					break;
				default:
					print ("Warning: unhandled HudMode == " + Mode.ToString());
					break;
				}
			} else { // not connected yet, but we're meant to be the client
				GUI.Box(new Rect(0f, midHeight, Screen.width, 30f), "Attempting to connect to server...");
			}
		} else { // server build
			if (Network.peerType == NetworkPeerType.Server) {
				GUI.Box(new Rect (0f, 0f, 300f, 30f), "Server online!");
			} else { // server not set up yet
				Network.InitializeServer(50, 25001, true);
			}
		}
	} // end of OnGUI()



	Vector2 buildingWindowScrollPos = new Vector2(0f, 0f);
	void displayBuildingWindow (int windowID) {
		GUILayout.BeginArea(new Rect(5f, 5f, BuildingWindowRect.width - 10f, BuildingWindowRect.height - 10f));

		GUILayout.BeginHorizontal();
		GUILayout.Box(GameStats.BuildingStats[(int)CurrBuilding.type].Name);
		if (GUILayout.Button("X", GUILayout.MaxWidth(25f))) {
			ShowBuildingWindow = false;
			ShowAdvancedStatsWindow = false;
			ShowFromScratchWindow = false;
			MainNet.AssignWorkersAtWork();
		}
		GUILayout.EndHorizontal();

		buildingWindowScrollPos = GUILayout.BeginScrollView(buildingWindowScrollPos);

		GUILayout.BeginHorizontal();
		GUILayout.Box(GameStats.BuildingStats[(int)CurrBuilding.type].Description);
		GUILayout.EndHorizontal();

		if (CurrBuilding.type == BuildingType.EmptySpace) {
			for (int i = 0; i < (int)BuildingType.Count; i++) {
				if ((BuildingType)i != BuildingType.EmptySpace) {
					GUILayout.BeginHorizontal();
					bool canBuild = true;
					for (int j = 0; j < (int)ResourceType.Count; j++) {
						if (GameStats.GetBuildCost((BuildingType)i, (ResourceType)j, 0) > MainNet.CurrentAccount.Cities[CurrCityID].ResourcesInside[j]) {
							canBuild = false;
							break;
						}
					}
					if (canBuild) {
						if (GUILayout.Button("Build " + GameStats.BuildingStats[i].Name)) {
							GUI.color = Color.white;
							MainNet.CurrentAccount = MainNet.BuildFromEmpty(MainNet.CurrentAccount, CurrCityID, CurrBuilding.x, CurrBuilding.y, (BuildingType)i);
							MainNet.BroadcastBuildFromEmpty((BuildingType)i);
							Application.LoadLevel("CityLevel");
						}
					} else {
						// can't build
						GUI.color = Color.red;
						GUILayout.Box("Can't build " + GameStats.BuildingStats[i].Name);
					}
					GUI.color = Color.white;
					if (GUILayout.Button("More info", GUILayout.MaxWidth(100f))) {
						ShowFromScratchWindow = true;
						fromScratchType = (BuildingType)i;
					}
					GUILayout.EndHorizontal();
				}
			}
		} else {
			if (CurrBuilding.isBeingBuilt) {
				GUILayout.BeginHorizontal();
				GUILayout.Box("Upgrading from level " + (CurrBuilding.Level - 1).ToString() + " to level " + CurrBuilding.Level.ToString() + ", " +
				              (Mathf.CeilToInt(CurrBuilding.BuildingFinishMoment - Time.time)).ToString() + " seconds remaining");
				GUILayout.EndHorizontal();
			} else {
				GUILayout.BeginHorizontal();
				GUILayout.Box("Level: " + CurrBuilding.Level.ToString());

				bool canBuild = true;
				for (int i = 0; i < (int)ResourceType.Count; i++) {
					if (GameStats.GetBuildCost(CurrBuilding.type, (ResourceType)i, CurrBuilding.Level) > MainNet.CurrentAccount.Cities[CurrCityID].ResourcesInside[i]) {
						canBuild = false;
						break;
					}
				}

				if (canBuild) {
					if (GUILayout.Button("Level up!")) {
						MainNet.CurrentAccount = MainNet.DoUpgrade(MainNet.CurrentAccount, CurrCityID, CurrBuilding.x, CurrBuilding.y);
						MainNet.BroadcastUpgrade();
					}
					GUILayout.Box(GameStats.BuildingStats[(int)CurrBuilding.type].LevelUpText);
				} else {
					GUILayout.Box("Can't level up. See statistics for details.");
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUI.color = Color.white;
				GUILayout.Box(capitalize(GameStats.BuildingStats[(int)CurrBuilding.type].WorkerName) + "s " + GameStats.BuildingStats[(int)CurrBuilding.type].WorkerFunction + ": " + CurrBuilding.WorkersAtWork.ToString(),
				              GUILayout.MaxWidth(200f));
				CurrBuilding.WorkersAtWork = (int)Mathf.Round(GUILayout.HorizontalSlider(CurrBuilding.WorkersAtWork, 0f,
				                                                                         GameStats.GetProductivity(CurrBuilding.type, ResourceType.Population, CurrBuilding.Level)));
				CurrBuilding.WorkersAtWork = (CurrBuilding.WorkersAtWork > 0) ? CurrBuilding.WorkersAtWork : 0;
				MainNet.CurrentAccount.Cities[CurrCityID].ResourcesInside[(int)ResourceType.Population] = TempWorkers - CurrBuilding.WorkersAtWork;
				GUILayout.EndHorizontal();
			}

			GUILayout.BeginHorizontal();
			GUI.color = Color.white;
			if (GUILayout.Button("Show statistics window")) {
				ShowAdvancedStatsWindow = true;
			}
			GUILayout.EndHorizontal();
		}

		GUILayout.EndScrollView();

		GUILayout.EndArea();
		GUI.DragWindow();
	} // end of displayBuildingWindow()



	Vector2 statsWindowScrollPos = new Vector2(0f, 0f);
	void displayAdvancedStatsWindow (int windowID) {
		GUILayout.BeginArea(new Rect(5f, 5f, AdvancedStatsWindowRect.width - 10f, AdvancedStatsWindowRect.height - 10f));

		GUILayout.BeginHorizontal();
		GUILayout.Box("Statistics for " + GameStats.BuildingStats[(int)CurrBuilding.type].Name);
		if (GUILayout.Button("X", GUILayout.MaxWidth(25f))) {
			ShowAdvancedStatsWindow = false;
		}
		GUILayout.EndHorizontal();

		statsWindowScrollPos = GUILayout.BeginScrollView(statsWindowScrollPos);

		GUILayout.BeginHorizontal();
		GUI.color = Color.green;
		GUILayout.Box("Level up info:");
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUI.color = Color.yellow;
		GUILayout.Box("Build time: " + GameStats.GetBuildTime(CurrBuilding.type, CurrBuilding.Level) + " seconds");
		GUILayout.EndHorizontal();

		for (int i = 0; i < (int)ResourceType.Count; i++) {
			GUILayout.BeginHorizontal();
			
			if (GameStats.GetBuildCost(CurrBuilding.type, (ResourceType)i, CurrBuilding.Level) > MainNet.CurrentAccount.Cities[CurrCityID].ResourcesInside[i]) {
				GUI.color = Color.red;
			} else {
				GUI.color = Color.white;
			}
			if ((ResourceType)i == ResourceType.Population)
				GUILayout.Box("Workers required to build: " + (Mathf.CeilToInt(GameStats.GetBuildCost(CurrBuilding.type, (ResourceType)i, CurrBuilding.Level))).ToString());
			else if (GameStats.GetBuildCost(CurrBuilding.type, (ResourceType)i, CurrBuilding.Level) != 0f)
				GUILayout.Box(GameStats.ResourceNames[i] + " cost: " + (GameStats.GetBuildCost(CurrBuilding.type, (ResourceType)i, CurrBuilding.Level)).ToString());
			
			GUILayout.EndHorizontal();
		}

		GUILayout.BeginHorizontal();
		GUI.color = Color.cyan;
		GUILayout.Box("Currently " + GameStats.BuildingStats[(int)CurrBuilding.type].WorkerFunction + " " + GameStats.BuildingStats[(int)CurrBuilding.type].WorkerName + "s: " +
		              CurrBuilding.WorkersAtWork.ToString() +
		              " out of " + (Mathf.Round(GameStats.GetProductivity(CurrBuilding.type, ResourceType.Population, CurrBuilding.Level))).ToString() +
		              " " + GameStats.BuildingStats[(int)CurrBuilding.type].WorkerName + "s possible to be " + GameStats.BuildingStats[(int)CurrBuilding.type].WorkerFunction);
		GUI.color = Color.white;
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUI.color = Color.green;
		GUILayout.Box("Hourly resource balance:");
		GUI.color = Color.white;
		GUILayout.EndHorizontal();

		for (int i = 0; i < (int)ResourceType.Count; i++) {
			if ((ResourceType)i != ResourceType.Population) {
				if (GameStats.GetProductivity(CurrBuilding.type, (ResourceType)i, CurrBuilding.Level) != 0f) {
					GUILayout.BeginHorizontal();

					GUILayout.Box(GameStats.ResourceNames[i] + ": " + GameStats.GetProductivity(CurrBuilding.type, (ResourceType)i, CurrBuilding.Level).ToString() +
					              " per " + GameStats.BuildingStats[(int)CurrBuilding.type].WorkerName + " " + GameStats.BuildingStats[(int)CurrBuilding.type].WorkerFunction +
					              ", giving a total of " + (GameStats.GetProductivity(CurrBuilding.type, (ResourceType)i, CurrBuilding.Level) * CurrBuilding.WorkersAtWork).ToString());

					GUILayout.EndHorizontal();
				}
			}
		}

		GUILayout.BeginHorizontal();
		GUI.color = Color.magenta;
		GUILayout.Box("On the next level:");
		GUI.color = Color.white;
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUI.color = Color.cyan;
		GUILayout.Box((Mathf.Round(GameStats.GetProductivity(CurrBuilding.type, ResourceType.Population, CurrBuilding.Level + 1))).ToString() +
		              " " + GameStats.BuildingStats[(int)CurrBuilding.type].WorkerName + "s possible to be " + GameStats.BuildingStats[(int)CurrBuilding.type].WorkerFunction);
		GUI.color = Color.white;
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUI.color = Color.green;
		GUILayout.Box("Hourly resource balance:");
		GUI.color = Color.white;
		GUILayout.EndHorizontal();
		
		for (int i = 0; i < (int)ResourceType.Count; i++) {
			if ((ResourceType)i != ResourceType.Population) {
				if (GameStats.GetProductivity(CurrBuilding.type, (ResourceType)i, CurrBuilding.Level + 1) != 0f) {
					GUILayout.BeginHorizontal();
					
					GUILayout.Box(GameStats.ResourceNames[i] + ": " + GameStats.GetProductivity(CurrBuilding.type, (ResourceType)i, CurrBuilding.Level + 1).ToString() +
					              " per " + GameStats.BuildingStats[(int)CurrBuilding.type].WorkerName + " " + GameStats.BuildingStats[(int)CurrBuilding.type].WorkerFunction);
					
					GUILayout.EndHorizontal();
				}
			}
		}

		GUILayout.EndScrollView();

		GUILayout.EndArea();
		GUI.DragWindow();
	} // end of displayAdvancedStatsWindow()



	Vector2 fromScratchWindowScrollPos = new Vector2(0f, 0f);
	BuildingType fromScratchType = BuildingType.Barracks;
	void displayFromScratchWindow (int windowID) {
		GUILayout.BeginArea(new Rect(5f, 5f, FromScratchWindowRect.width - 10f, FromScratchWindowRect.height - 10f));
		
		GUILayout.BeginHorizontal();
		GUILayout.Box("Build info for " + GameStats.BuildingStats[(int)fromScratchType].Name);
		if (GUILayout.Button("X", GUILayout.MaxWidth(25f))) {
			ShowFromScratchWindow = false;
		}
		GUILayout.EndHorizontal();
		
		fromScratchWindowScrollPos = GUILayout.BeginScrollView(fromScratchWindowScrollPos);

		GUILayout.BeginHorizontal();
		GUI.color = Color.white;
		GUILayout.Box(GameStats.BuildingStats[(int)fromScratchType].Description);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUI.color = Color.yellow;
		GUILayout.Box("Build time: " + GameStats.GetBuildTime(fromScratchType, CurrBuilding.Level) + " seconds");
		GUILayout.EndHorizontal();
		
		for (int i = 0; i < (int)ResourceType.Count; i++) {
			GUILayout.BeginHorizontal();
			
			if (GameStats.GetBuildCost(fromScratchType, (ResourceType)i, CurrBuilding.Level) > MainNet.CurrentAccount.Cities[CurrCityID].ResourcesInside[i]) {
				GUI.color = Color.red;
			} else {
				GUI.color = Color.white;
			}
			if ((ResourceType)i == ResourceType.Population)
				GUILayout.Box("Workers required to build: " + (Mathf.CeilToInt(GameStats.GetBuildCost(fromScratchType, (ResourceType)i, CurrBuilding.Level))).ToString());
			else if (GameStats.GetBuildCost(fromScratchType, (ResourceType)i, CurrBuilding.Level) != 0f)
				GUILayout.Box(GameStats.ResourceNames[i] + " cost: " + (GameStats.GetBuildCost(fromScratchType, (ResourceType)i, CurrBuilding.Level)).ToString());
			
			GUILayout.EndHorizontal();
		}
		
		GUILayout.BeginHorizontal();
		GUI.color = Color.magenta;
		GUILayout.Box("When built - at level 1:");
		GUI.color = Color.white;
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUI.color = Color.cyan;
		GUILayout.Box((Mathf.Round(GameStats.GetProductivity(fromScratchType, ResourceType.Population, CurrBuilding.Level + 1))).ToString() +
		              " " + GameStats.BuildingStats[(int)fromScratchType].WorkerName + "s possible to be " + GameStats.BuildingStats[(int)fromScratchType].WorkerFunction);
		GUI.color = Color.white;
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUI.color = Color.green;
		GUILayout.Box("Hourly resource balance:");
		GUI.color = Color.white;
		GUILayout.EndHorizontal();
		
		for (int i = 0; i < (int)ResourceType.Count; i++) {
			if ((ResourceType)i != ResourceType.Population) {
				if (GameStats.GetProductivity(fromScratchType, (ResourceType)i, CurrBuilding.Level + 1) != 0f) {
					GUILayout.BeginHorizontal();
					
					GUILayout.Box(GameStats.ResourceNames[i] + ": " + GameStats.GetProductivity(fromScratchType, (ResourceType)i, CurrBuilding.Level + 1).ToString() +
					              " per " + GameStats.BuildingStats[(int)fromScratchType].WorkerName + " " + GameStats.BuildingStats[(int)fromScratchType].WorkerFunction);
					
					GUILayout.EndHorizontal();
				}
			}
		}
		
		GUILayout.EndScrollView();
		
		GUILayout.EndArea();
		GUI.DragWindow();
	} // end of displayFromScratchWindow()



	Vector2 cityStatsWindowScrollPos = new Vector2(0f, 0f);
	void displayCityStatsWindow (int windowID) {
		GUILayout.BeginArea(new Rect(5f, 5f, CityStatsWindowRect.width - 10f, CityStatsWindowRect.height - 10f));

		GUILayout.BeginHorizontal();
		GUILayout.Box("City statistics");
		if (GUILayout.Button("X", GUILayout.MaxWidth(25f))) {
			ShowCityStatsWindow = false;
			MainNet.BroadcastCityNameChange();
		}
		GUILayout.EndHorizontal();

		cityStatsWindowScrollPos = GUILayout.BeginScrollView(cityStatsWindowScrollPos);

		GUILayout.BeginHorizontal();
		GUILayout.Box("City name: ", GUILayout.MaxWidth(100f));
		MainNet.CurrentAccount.Cities[CurrCityID].Name = GUILayout.TextArea(MainNet.CurrentAccount.Cities[CurrCityID].Name);
		GUILayout.EndHorizontal();

		GUILayout.EndScrollView();

		GUILayout.EndArea();
		GUI.DragWindow();
	}



	void navBar() {
		float textHeight = 30f;

		if (GUI.Button(new Rect(0f, 0f, Screen.width / ((float)ResourceType.Count + 1f), textHeight), MainNet.CurrentAccount.Cities[CurrCityID].Name)) {
			ShowCityStatsWindow = true;
		}

		for (int i = 0; i < (int)ResourceType.Count; i++) {
			if ((ResourceType)i == ResourceType.Population)
				GUI.Box(new Rect(Screen.width / ((float)ResourceType.Count + 1f) * (i + 1f), 0f, Screen.width / ((float)ResourceType.Count + 1f), textHeight),
				        GameStats.ResourceNames[i] + ": " + ((int)MainNet.CurrentAccount.Cities[CurrCityID].ResourcesInside[i]).ToString());
			else
				GUI.Box(new Rect(Screen.width / ((float)ResourceType.Count + 1f) * (i + 1f), 0f, Screen.width / ((float)ResourceType.Count + 1f), textHeight),
			        	GameStats.ResourceNames[i] + ": " + (MainNet.CurrentAccount.Cities[CurrCityID].ResourcesInside[i]).ToString());
		}
	}



	void stylizeGUI() {
		GUI.skin.font = SciFiFont;
		GUI.skin.box.font = SciFiFont;
		GUI.skin.textArea.font = SciFiFont;
	}



	string capitalize (string s) {
		string t =  "" + (char)(s[0] + 'A' - 'a');
		t += s.Substring(1);
		return t;
	}



	void OnLevelWasLoaded (int level) {
		if (level == 1)
			MainNet.CurrentAccount.Cities[CurrCityID].Build();
	}
}
