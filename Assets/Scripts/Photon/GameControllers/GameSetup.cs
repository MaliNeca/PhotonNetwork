using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetup : MonoBehaviour
{

    public static GameSetup GS;
    public Text healthDisplay;

    public int nextPlayerTeam;
    public Transform[] spawnPointsTeamOne;
    public Transform[] spawnPointsTeamTwo;


    private void OnEnable()
    {
        if(GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }

    public void DisconnectPlayer()
    {
        Destroy(PhotonRoom.room.gameObject);

        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
       // PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.IsConnected) yield return null;
        // while(PhotonNetwork.InRoom) yield return null;
        SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
    }

    public void UpdateTeam()
    {
        if(nextPlayerTeam == 1)
        {
            nextPlayerTeam = 2;
        }
        else
        {
            nextPlayerTeam = 1;
        }
    }
}
