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


    //master view
    public GameObject playerNumbers;
    public List<GameObject> playerNumbersList = new List<GameObject>();
    public GameObject playerOneNumbers;
    public GameObject playerTwoNumbers;
    public GameObject playerThreeNumbers;
    public GameObject playerFourNumbers;
    public bool numbersSet = false;
   
    public GameObject PV;

    public GameObject sheet;
    public Button swapButton;

    private void OnEnable()
    {
        if(GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }

    void Start()
    {
        foreach(PhotonView pv in ListOfDragingObjects)
        {
            pv.gameObject.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            pv.gameObject.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }

        if (PhotonNetwork.IsMasterClient)
        {

            //set Sheet view position
            sheet.transform.SetPositionAndRotation(new Vector3(0, 0, 0), sheet.transform.rotation);
            sheet.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 600);
            sheet.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(120, 120);

            //set objects active
            playerOneNumbers.gameObject.SetActive(true);
            playerTwoNumbers.gameObject.SetActive(true);
            playerThreeNumbers.gameObject.SetActive(true);
            playerFourNumbers.gameObject.SetActive(true);

            playerNumbers.transform.GetComponent<CanvasGroup>().alpha = 0;


            //set button active
            swapButton.gameObject.SetActive(true);

            /* //set Sheet view position
             sheet.transform.SetPositionAndRotation(new Vector3(0, 0, 0), sheet.transform.rotation);
             sheet.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 600);
             sheet.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(120, 120);




             



             //set players views enabled
             playerNumbers.transform.GetComponent<CanvasGroup>().alpha = 0;
             //  playerNumbers.transform.GetComponent<CanvasGroup>().interactable = false;
             playerOneNumbers.transform.GetComponent<CanvasGroup>().alpha = 1;
             playerTwoNumbers.transform.GetComponent<CanvasGroup>().alpha = 1;
             playerThreeNumbers.transform.GetComponent<CanvasGroup>().alpha = 1;
             playerFourNumbers.transform.GetComponent<CanvasGroup>().alpha = 1;*/


            //OnSwapClicked();
            

        }
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (numbersSet)
            {
                setClientViews();
            }
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
        this.numbersSet = true;
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
                    ListOfDragingObjects[numbers[i + 4]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
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
                    ListOfDragingObjects[numbers[i + 8]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 8]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                }
                break;
            case 4:
                for (int i = 0; i < 4; i++)
                {
                    Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    //ListOfDragingObjects[numbers[i + 12]].gameObject.SetActive(true);

                    ListOfDragingObjects[numbers[i + 12]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i + 12]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 12]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                }
                break;
        }
        

    }

    public void setClientViews()
    {
        numbersSet = false;
        foreach (PhotonView GO in ListOfDragingObjects)
        {
            GO.gameObject.transform.GetChild(0).gameObject.SetActive(true);

        }

        //set object for playerOne view
        for (int i = 0; i < 4; i++)
        {

            //object is not dragged 
            //position of dragging object is on playerList
            Vector3 newVector = ListOfDragingObjects[allNumbers[i]].gameObject.transform.GetComponent<RectTransform>().localPosition;

            GameObject ga = ListOfDragingObjects[allNumbers[i]].gameObject;
            ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
            ga.transform.SetParent(playerOneNumbers.gameObject.transform.GetChild(i));
            ga.transform.GetComponent<RectTransform>().localPosition = newVector;
            ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
           
            
        }

        //set object for playerTwo view
        for (int i = 0; i < 4; i++)
        {

            //object is not dragged 
            //position of dragging object is on playerList
            Vector3 newVector = ListOfDragingObjects[allNumbers[i+4]].gameObject.transform.GetComponent<RectTransform>().localPosition;

            GameObject ga = ListOfDragingObjects[allNumbers[i+4]].gameObject;
            ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
            ga.transform.SetParent(playerTwoNumbers.gameObject.transform.GetChild(i));
            ga.transform.GetComponent<RectTransform>().localPosition = newVector;
            ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
           
        }

        //set object for playerThree view
        for (int i = 0; i < 4; i++)
        {

            //object is not dragged 
            //position of dragging object is on playerList
            Vector3 newVector = ListOfDragingObjects[allNumbers[i + 8]].gameObject.transform.GetComponent<RectTransform>().localPosition;

            GameObject ga = ListOfDragingObjects[allNumbers[i + 8]].gameObject;
            ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
            ga.transform.SetParent(playerThreeNumbers.gameObject.transform.GetChild(i));
            ga.transform.GetComponent<RectTransform>().localPosition = newVector;
            ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

        }

        //set object for playerFour view
        for (int i = 0; i < 4; i++)
        {

            //object is not dragged 
            //position of dragging object is on playerList
            Vector3 newVector = ListOfDragingObjects[allNumbers[i + 12]].gameObject.transform.GetComponent<RectTransform>().localPosition;

            GameObject ga = ListOfDragingObjects[allNumbers[i + 12]].gameObject;
            ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
            ga.transform.SetParent(playerFourNumbers.gameObject.transform.GetChild(i));
            ga.transform.GetComponent<RectTransform>().localPosition = newVector;
            ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

        }
    }
    public void OnSwapClicked()
    {
        
      
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("Call rpc");

            PhotonPlayer.player.sendAll();
        }
        
    }

    
    public void setView()
    {
        Debug.LogWarning("RPC CALLED");

        foreach (PhotonView GO in ListOfDragingObjects)
        {
            GO.gameObject.transform.GetChild(0).gameObject.SetActive(true);

        }
    }
}
