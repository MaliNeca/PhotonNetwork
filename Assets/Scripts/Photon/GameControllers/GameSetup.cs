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
    public GameObject[] playerCells;
    public List<PhotonView> ListOfDragingObjects = new List<PhotonView>();
    public List<int> allNumbers = new List<int>();

    private void OnEnable()
    {
        if(GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }

    public void DisconnectPlayer()
    {
        if(PhotonPlayer.player != null)
        {
            Destroy(PhotonPlayer.player.gameObject);

        }
        Destroy(PhotonRoomCustomMatch.room.gameObject);
        
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
        else if(nextPlayerTeam == 2)
        {
            nextPlayerTeam = 3;
        }
        else if (nextPlayerTeam == 3)
        {
            nextPlayerTeam = 4;
        }
    }

    public void SetList(List<int> numbers)
    {
        this.allNumbers = numbers;
    }

    public void SetActiveList(List<int> numbers, int team)
    {
        //allNumbers = numbers;
        switch (team)
        {
            case 1:
                for (int i = 0; i < 4; i++)
                {
                    Debug.LogWarning(ListOfDragingObjects[i].ViewID);

                    //ListOfDragingObjects[numbers[i]].gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                    ListOfDragingObjects[numbers[i]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i]].transform.GetComponentInParent<Transform>().SetAsFirstSibling();
                }
                break;
            case 2:
                for (int i = 0; i < 4; i++)
                {
                    Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    ListOfDragingObjects[numbers[i + 4]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i +4]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                    ListOfDragingObjects[numbers[i + 4]].GetComponent<Image>().enabled = true;
                    //ListOfDragingObjects[numbers[i+4]].gameObject.SetActive(true);
                }
                break;
            case 3:
                for (int i = 0; i < 4; i++)
                {
                    Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    //ListOfDragingObjects[numbers[i + 8]].gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 8]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i+8]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 8]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                }
                break;
            case 4:
                for (int i = 0; i < 4; i++)
                {
                    Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    //ListOfDragingObjects[numbers[i + 12]].gameObject.SetActive(true);

                    ListOfDragingObjects[numbers[i + 12]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i+12]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 12]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                }
                break;
        }
        

    }

    public void OnSwapClicked()
    {
        foreach(PhotonView GO in ListOfDragingObjects)
        {
            GO.gameObject.transform.GetChild(0).gameObject.SetActive(true);

        }
        /*foreach(DragAndDropItem item in FindObjectsOfType<DragAndDropItem>())
        {
            
        }*/
    }
}
