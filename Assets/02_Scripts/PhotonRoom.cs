using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonRoom : MonoBehaviourPunCallbacks
    {

    public Text RoomName;
   public void ConnectToRoom()
        {
        PhotonNetworkManager.Instance.JoinRoom(RoomName.text);
        }
}
