using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using System;

public class PhotonNetworkManager : MonoBehaviourPunCallbacks
    {
    public static PhotonNetworkManager Instance;
    public string gameVersion;

    public GameObject LobbyPanel;
    public GameObject DisconnectPanel;
    public Text PlayersReady;
    public Toggle SetReady;
    List<RoomInfo> createdRooms = new List<RoomInfo>();
    public GameObject PUNRoomContainer;
    public GameObject PUNRoomItem;


    public InputField PUNRoomName;
    public Dropdown PUNPlayerCount;
    //public GameObject CharacterPanel;

    public int PlayerReady = 0;
    public UnityEngine.Object SceneToLoad;

    private void Awake()
        {
        Instance = this;
        }

    void Start()
        {
        if (!PhotonNetwork.IsConnected)
            {
            //Set the App version before connecting
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
            // Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)
            PhotonNetwork.ConnectUsingSettings();
            }
        }

    public override void OnConnectedToMaster()
        {
        if (PhotonNetwork.NetworkingClient.State != ClientState.JoiningLobby) PhotonNetwork.JoinLobby(TypedLobby.Default);

        }

    public override void OnJoinedRoom()
        {
        Hashtable hash = new Hashtable();
        hash.Add("Ready", false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        base.OnJoinedRoom();
        }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
        //After this callback, update the room list

        createdRooms = roomList;
        object tmp_hn;
        if (PUNRoomContainer != null)
            {
            foreach (Transform Object in PUNRoomContainer.transform)
                {
                Destroy(Object.gameObject);
                }
            foreach (RoomInfo a in createdRooms)
                {
                if (a.PlayerCount < a.MaxPlayers)
                    {
                    GameObject go = (GameObject)Instantiate(PUNRoomItem);
                    go.transform.SetParent(PUNRoomContainer.transform);
                    go.GetComponent<RectTransform>().localPosition = new Vector3(go.GetComponent<RectTransform>().localPosition.x, go.GetComponent<RectTransform>().localPosition.y, 0);
                    go.transform.localScale = Vector3.one;
                    go.transform.localRotation = Quaternion.identity;
                    go.transform.GetChild(0).gameObject.GetComponent<Text>().text = a.Name; //Roomname
                    a.CustomProperties.TryGetValue("HostName", out tmp_hn);    //GameMode
                    go.transform.GetChild(1).gameObject.GetComponent<Text>().text = (string)tmp_hn;
                    go.transform.GetChild(2).gameObject.GetComponent<Text>().text = a.PlayerCount.ToString() + "/" + a.MaxPlayers;
                    }
                }

            }
        }
    public void JoinRoom(string LobbyName)
        {
        if (PhotonNetwork.IsConnectedAndReady)
            {
            PhotonNetwork.JoinRoom(LobbyName);
            LobbyPanel.SetActive(false);
            DisconnectPanel.SetActive(true);
            //CharacterPanel.SetActive(true);
            }
        }

    public override void OnJoinRandomFailed(short returnCode, string message)
        {
        Create();
        base.OnJoinRandomFailed(returnCode, message);
        }

    public void Create()
        {
        RoomOptions roomOptions = new RoomOptions
            {
            EmptyRoomTtl = 2000,
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = (byte)(PUNPlayerCount.value + 1),
            };
        roomOptions.CustomRoomProperties = new Hashtable();
        roomOptions.CustomRoomProperties.Add("HostName", PhotonNetwork.LocalPlayer.NickName);
        roomOptions.CustomRoomPropertiesForLobby = new string[1];
        roomOptions.CustomRoomPropertiesForLobby[0] = "HostName";


        if (PUNRoomName.text == "")
            {
            PUNRoomName.text = "I forgot to put a name";
            }
        PhotonNetwork.JoinOrCreateRoom(PUNRoomName.text, roomOptions, TypedLobby.Default);
        LobbyPanel.SetActive(false);
        DisconnectPanel.SetActive(true);
        //CharacterPanel.SetActive(true);
        Debug.Log("Created a new room " + PUNRoomName.text);
        }

    public void DisconnectRoom()
        {
        LobbyPanel.SetActive(true);
        DisconnectPanel.SetActive(false);
        //CharacterPanel.SetActive(false);
        SetReady.isOn = false;
        PhotonNetwork.LeaveRoom();
        }

    public void ReadyToRoom(GameObject Sender)
        {
        bool ready = (bool)PhotonNetwork.LocalPlayer.CustomProperties["Ready"];
        ready = Sender.GetComponent<Toggle>().isOn;
        Hashtable hash = new Hashtable();
        hash.Add("Ready", ready);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
        if (changedProps.ContainsKey("Ready"))
            {
            PlayerReady = 0;
            foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
                {
                if ((bool)player.Value.CustomProperties["Ready"] == true)
                    {
                    PlayerReady += 1;
                    }
                }

            PlayerReady = Mathf.Clamp(PlayerReady, 0, PhotonNetwork.CurrentRoom.MaxPlayers);
            PlayersReady.text = "PLAYERS READY: " + PlayerReady + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

            if (PlayerReady == PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                SceneManager.LoadSceneAsync(SceneToLoad.name);
                }
            } 
        }
    }
