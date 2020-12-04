using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GameObject playerFiveNumbers;
    public GameObject playerSixNumbers;
    public GameObject playerSevenNumbers;

    //player names
    public GameObject playersName;

    public bool numbersSet = false;

    public GameObject PV;

    public GameObject sheet;
    public Button swapButton;

    private void OnEnable()
    {
        if (GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }

    void Start()
    {
        //set position of items in player view
        foreach (PhotonView pv in ListOfDragingObjects)
        {
            pv.gameObject.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            pv.gameObject.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            //set Sheet view position
            sheet.transform.SetPositionAndRotation(new Vector3(610, -190, 0), sheet.transform.rotation);
            sheet.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 700);
            sheet.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(150, 150);

            //set objects active
            switch (PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                //4 player + master
                case 5:
                    playerOneNumbers.gameObject.SetActive(true);
                    playerTwoNumbers.gameObject.SetActive(true);
                    playerThreeNumbers.gameObject.SetActive(true);
                    playerFourNumbers.gameObject.SetActive(true);
                    break;
                //5 player + master
                case 6:
                    playerOneNumbers.gameObject.SetActive(true);
                    playerTwoNumbers.gameObject.SetActive(true);
                    playerThreeNumbers.gameObject.SetActive(true);
                    playerFourNumbers.gameObject.SetActive(true);
                    playerFiveNumbers.gameObject.SetActive(true);
                    break;
                //6 player + master
                case 7:
                    playerOneNumbers.gameObject.SetActive(true);
                    playerTwoNumbers.gameObject.SetActive(true);
                    playerThreeNumbers.gameObject.SetActive(true);
                    playerFourNumbers.gameObject.SetActive(true);
                    playerFiveNumbers.gameObject.SetActive(true);
                    playerSixNumbers.gameObject.SetActive(true);
                    break;
                //7 player + master
                case 8:
                    playerOneNumbers.gameObject.SetActive(true);
                    playerTwoNumbers.gameObject.SetActive(true);
                    playerThreeNumbers.gameObject.SetActive(true);
                    playerFourNumbers.gameObject.SetActive(true);
                    playerFiveNumbers.gameObject.SetActive(true);
                    playerSixNumbers.gameObject.SetActive(true);
                    playerSevenNumbers.gameObject.SetActive(true);
                    break;

            }

            //players name
            for(int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers - 1; i++)
            {
                playersName.gameObject.transform.GetChild(i).gameObject.SetActive(true);
                //i+2 bcs player id start with 1 and master id is 1
                playersName.gameObject.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.CurrentRoom.GetPlayer(i + 2) != null ? PhotonNetwork.CurrentRoom.GetPlayer(i + 2).NickName : "";    
            }
           


            //client view set alpha
            playerNumbers.transform.GetComponent<CanvasGroup>().alpha = 0;

            //set button active
            swapButton.gameObject.SetActive(true);
        }

        //set new object active in corelation with max player
        switch (PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            case 6:
                //update list of draging objects
                for (int i = 0; i < 4; i++)
                {
                    ListOfDragingObjects[i + 16].gameObject.transform.parent.gameObject.SetActive(true);
                    //sheet objects
                    sheet.transform.GetChild(i + 16).gameObject.SetActive(true);
                }

                //change size of fields
                playerNumbers.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(120, 120);
                sheet.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(120, 120);
                break;
            case 7:
                //update list of draging objects
                for (int i = 0; i < 8; i++)
                {
                    ListOfDragingObjects[i + 16].gameObject.transform.parent.gameObject.SetActive(true);
                    //sheet objects
                    sheet.transform.GetChild(i + 16).gameObject.SetActive(true);
                }

                //change size of fields
                playerNumbers.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(120, 120);
                sheet.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(120, 120);
                break;
            case 8:
                //update list of draging objects
                for (int i = 0; i < 12; i++)
                {
                    ListOfDragingObjects[i + 16].gameObject.transform.parent.gameObject.SetActive(true);
                    //sheet objects
                    sheet.transform.GetChild(i + 16).gameObject.SetActive(true);
                }

                //change size of fields
                playerNumbers.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 100);
                sheet.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 100);
                break;
        }
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (numbersSet)
            {
                //set items on all client views
                setClientViews();
            }
        }
    }

    public void DisconnectPlayer()
    {
        if (PhotonPlayer.player != null)
        {
            Destroy(PhotonPlayer.player.gameObject);
        }
        Destroy(PhotonRoomCustomMatch.room.gameObject);
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected) yield return null;
        SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
    }

    public void UpdateTeam()
    {
        if (nextPlayerTeam == 1)
        {
            nextPlayerTeam = 2;
        }
        else if (nextPlayerTeam == 2)
        {
            nextPlayerTeam = 3;
        }
        else if (nextPlayerTeam == 3)
        {
            nextPlayerTeam = 4;
        }
    }

    //Photon player master set random numbers
    public void SetList(List<int> numbers)
    {
        this.allNumbers = numbers;
        this.numbersSet = true;
    }

    public void SetActiveList(List<int> numbers, int team)
    {
        switch (team)
        {
            //Player one objects
            case 1:
                for (int i = 0; i < 4; i++)
                {
                    //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    ListOfDragingObjects[numbers[i]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                    ListOfDragingObjects[numbers[i]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i]].transform.GetComponentInParent<Transform>().SetAsFirstSibling();
                }
                break;
            //Player two objects
            case 2:
                for (int i = 0; i < 4; i++)
                {
                    //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    ListOfDragingObjects[numbers[i + 4]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 4]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                    ListOfDragingObjects[numbers[i + 4]].GetComponent<Image>().enabled = true;
                }
                break;
            //Player three objects
            case 3:
                for (int i = 0; i < 4; i++)
                {
                    //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    ListOfDragingObjects[numbers[i + 8]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i + 8]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 8]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                }
                break;
            //Player four objects
            case 4:
                for (int i = 0; i < 4; i++)
                {
                    //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    ListOfDragingObjects[numbers[i + 12]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i + 12]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 12]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                }
                break;
            //Player five objects
            case 5:
                for (int i = 0; i < 4; i++)
                {
                    //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    ListOfDragingObjects[numbers[i + 16]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i + 16]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 16]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                }
                break;
            //Player six objects
            case 6:
                for (int i = 0; i < 4; i++)
                {
                    //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    ListOfDragingObjects[numbers[i + 20]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i + 20]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 20]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                }
                break;
            //Player seven objects
            case 7:
                for (int i = 0; i < 4; i++)
                {
                    //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    ListOfDragingObjects[numbers[i + 24]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i + 24]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 24]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                }
                break;
        }
    }

    public void setClientViews()
    {
        numbersSet = false;
        //enable first all clients items
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
            Vector3 newVector = ListOfDragingObjects[allNumbers[i + 4]].gameObject.transform.GetComponent<RectTransform>().localPosition;

            GameObject ga = ListOfDragingObjects[allNumbers[i + 4]].gameObject;
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

        //for other players
        switch (PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            case 8:
                //set object for playerSeven view
                for (int i = 0; i < 4; i++)
                {
                    //object is not dragged 
                    //position of dragging object is on playerList
                    Vector3 newVector = ListOfDragingObjects[allNumbers[i + 24]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    GameObject ga = ListOfDragingObjects[allNumbers[i + 24]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
                    ga.transform.SetParent(playerSevenNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }

                //set object for playerSix view
                for (int i = 0; i < 4; i++)
                {
                    //object is not dragged 
                    //position of dragging object is on playerList
                    Vector3 newVector = ListOfDragingObjects[allNumbers[i + 20]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    GameObject ga = ListOfDragingObjects[allNumbers[i + 20]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
                    ga.transform.SetParent(playerSixNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }

                //set object for playerFive view
                for (int i = 0; i < 4; i++)
                {
                    //object is not dragged 
                    //position of dragging object is on playerList
                    Vector3 newVector = ListOfDragingObjects[allNumbers[i + 16]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    GameObject ga = ListOfDragingObjects[allNumbers[i + 16]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
                    ga.transform.SetParent(playerFiveNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }
                break;
            case 7:
                //set object for playerSix view
                for (int i = 0; i < 4; i++)
                {
                    //object is not dragged 
                    //position of dragging object is on playerList
                    Vector3 newVector = ListOfDragingObjects[allNumbers[i + 20]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    GameObject ga = ListOfDragingObjects[allNumbers[i + 20]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
                    ga.transform.SetParent(playerSixNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }

                //set object for playerFive view
                for (int i = 0; i < 4; i++)
                {
                    //object is not dragged 
                    //position of dragging object is on playerList
                    Vector3 newVector = ListOfDragingObjects[allNumbers[i + 16]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    GameObject ga = ListOfDragingObjects[allNumbers[i + 16]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
                    ga.transform.SetParent(playerFiveNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }
                break;

            case 6:
                //set object for playerFive view
                for (int i = 0; i < 4; i++)
                {
                    //object is not dragged 
                    //position of dragging object is on playerList
                    Vector3 newVector = ListOfDragingObjects[allNumbers[i + 16]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    GameObject ga = ListOfDragingObjects[allNumbers[i + 16]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
                    ga.transform.SetParent(playerFiveNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }
                break;
        }
    }

    //master buttonSwap clicked
    public void OnSwapClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //Debug.LogWarning("Call rpc");
            PhotonPlayer.player.sendAll();
        }
    }

    //enable view for all clients
    public void setView()
    {
        //Debug.LogWarning("RPC CALLED");
        foreach (PhotonView GO in ListOfDragingObjects)
        {
            GO.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
