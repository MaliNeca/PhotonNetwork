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
    public List<GameObject> playerNumbersList = new List<GameObject>();
    public GameObject playerNumbers; 
    public GameObject playerOneNumbers;
    public GameObject playerTwoNumbers;
    public GameObject playerThreeNumbers;
    public GameObject playerFourNumbers;
    public GameObject playerFiveNumbers;
    public GameObject playerSixNumbers;
    public GameObject playerSevenNumbers;
    public GameObject playerEightNumbers;
    public GameObject playerNineNumbers;
    public GameObject playerTenNumbers;
    public List<GameObject> allPlayersView = new List<GameObject>(10);

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

        /*  foreach (PhotonView pv in ListOfDragingObjects)
          {
              pv.gameObject.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
              pv.gameObject.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
          }*/

        //scale numbers items in player view

        

        if (PhotonNetwork.IsMasterClient)
        {
            //set Sheet view position
           // setGraphic();

            //enable views for all students
           // setAlphaForStudentsView();

            //players name visible
            //setPlayersName();

            //client view set alpha
            playerNumbers.transform.GetComponent<CanvasGroup>().alpha = 0;

            //set button active
            swapButton.gameObject.SetActive(true);
        }
        scaleImages();
        activateCellsOnSheet();

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

    //position of students view on teacher scene
    private void setGraphic()
    {
        //sheet view 
        //sheet.transform.SetPositionAndRotation(new Vector3(-449, 374, 0), sheet.transform.rotation);
        sheet.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 800);
        sheet.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(140, 140);

        //players view
        //for p1
        playerOneNumbers.transform.SetPositionAndRotation(new Vector3(-809, 352, 0), playerOneNumbers.transform.rotation);
        playerOneNumbers.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        playerOneNumbers.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 100);

        //for p2
        playerTwoNumbers.transform.SetPositionAndRotation(new Vector3(-514, 352, 0), playerTwoNumbers.transform.rotation);
        playerTwoNumbers.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        playerTwoNumbers.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 100);

        //for p3
        playerThreeNumbers.transform.SetPositionAndRotation(new Vector3(-210, 352, 0), playerThreeNumbers.transform.rotation);
        playerThreeNumbers.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        playerThreeNumbers.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 100);

        //for p4
        playerFourNumbers.transform.SetPositionAndRotation(new Vector3(98, 352, 0), playerFourNumbers.transform.rotation);
        playerFourNumbers.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        playerFourNumbers.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 100);

        //for p5
        playerFiveNumbers.transform.SetPositionAndRotation(new Vector3(-809, -57, 0), playerFiveNumbers.transform.rotation);
        playerFiveNumbers.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        playerFiveNumbers.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 100);

        //for p6
        playerSixNumbers.transform.SetPositionAndRotation(new Vector3(-514, -57, 0), playerSixNumbers.transform.rotation);
        playerSixNumbers.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        playerSixNumbers.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 100);

        //for p7
        playerSevenNumbers.transform.SetPositionAndRotation(new Vector3(-210, -57, 0), playerSevenNumbers.transform.rotation);
        playerSevenNumbers.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        playerSevenNumbers.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 100);

        //for p8
        playerEightNumbers.transform.SetPositionAndRotation(new Vector3(-809, -392, 0), playerEightNumbers.transform.rotation);
        playerEightNumbers.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        playerEightNumbers.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 100);


        //for p9
        playerNineNumbers.transform.SetPositionAndRotation(new Vector3(-514, -392, 0), playerNineNumbers.transform.rotation);
        playerNineNumbers.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        playerNineNumbers.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 100);

        //for p10
        playerTenNumbers.transform.SetPositionAndRotation(new Vector3(-210, -392, 0), playerTenNumbers.transform.rotation);
        playerTenNumbers.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        playerTenNumbers.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100, 100);
    }

    private void scaleImages()
    {
        for (int i = 0; i < ListOfDragingObjects.Count; i++)
        {
            if (i < 10)
            {
                ListOfDragingObjects[i].transform.GetChild(0).transform.GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 0.7f);

            }
            else
            {
                ListOfDragingObjects[i].transform.GetChild(0).transform.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }
        }
    }

    //enable alpha of students view for teacher
    private void setAlphaForStudentsView()
    {
        for (int i = 0; i < (PhotonNetwork.CurrentRoom.MaxPlayers - 1); i++)
        {
            allPlayersView[i].transform.GetComponent<CanvasGroup>().alpha = 1;

        }
    }

    private void setPlayersName()
    {
        for (int i = 0; i < (PhotonNetwork.CurrentRoom.MaxPlayers - 1); i++)
        {
            playersName.gameObject.transform.GetChild(i).gameObject.SetActive(true);
            //i+2 bcs player id start with 1 and master id is 1
            playersName.gameObject.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.CurrentRoom.GetPlayer(i + 2) != null ? PhotonNetwork.CurrentRoom.GetPlayer(i + 2).NickName : "";
        }
    }

    private void activateCellsOnSheet()
    {
        int maxCells = (PhotonNetwork.CurrentRoom.MaxPlayers - 1) * 4;
        for (int i = 0; i < maxCells; i++)
        {
            sheet.transform.GetChild(i).gameObject.SetActive(true);

        }
        switch (PhotonNetwork.CurrentRoom.MaxPlayers - 1)
        {
            case 5:
                //change size of fields
                sheet.transform.GetComponent<GridLayoutGroup>().constraintCount = 4;
                sheet.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(140, 140);
                break;
            case 6:
                //change size of fields
                sheet.transform.GetComponent<GridLayoutGroup>().constraintCount = 4;
                sheet.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(120, 120);
                break;
            case 7:
                //change size of fields
                sheet.transform.GetComponent<GridLayoutGroup>().constraintCount = 4;
                sheet.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(110, 110);
                break;
            case 8:
                //change size of fields
                sheet.transform.GetComponent<GridLayoutGroup>().constraintCount = 4;
                sheet.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(90, 90);
                break;
            case 9:
                //change size of fields
                sheet.transform.GetComponent<GridLayoutGroup>().constraintCount = 5;
                sheet.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(90, 90);
                break;
            case 10:
                //change size of fields
                sheet.transform.GetComponent<GridLayoutGroup>().constraintCount = 5;
                sheet.transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(90, 90);
                break;
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

    //players call to setup their views
    public void SetActiveList(List<int> numbers, int team)
    {

        if (team > 0 && team < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            for (int i = 0; i < 4; i++)
            {
                //enable component for this player
                playerNumbers.GetComponent<CanvasGroup>().alpha = 0;
                allPlayersView[team - 1].GetComponent<CanvasGroup>().alpha = 1;
                allPlayersView[team - 1].GetComponent<CanvasGroup>().interactable = true;
                allPlayersView[team - 1].GetComponent<CanvasGroup>().blocksRaycasts = true;

                int index = i + ((team - 1) * 4);
                Vector3 newVector = ListOfDragingObjects[numbers[index]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                GameObject ga = ListOfDragingObjects[numbers[index]].gameObject;
                ga.transform.GetComponent<DragAndDropItem>().dragDisabled = false;
                ga.transform.SetParent(allPlayersView[team - 1].gameObject.transform.GetChild(i));
                ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);


                //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                ListOfDragingObjects[numbers[index]].transform.parent.gameObject.SetActive(true);
                ListOfDragingObjects[numbers[index]].transform.GetChild(0).gameObject.SetActive(true);
                ListOfDragingObjects[numbers[index]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                ListOfDragingObjects[numbers[index]].GetComponent<Image>().enabled = true;
                ListOfDragingObjects[numbers[index]].transform.GetComponentInParent<Transform>().SetAsFirstSibling();
            }

        }
        else if (team == 0)
        {
            Debug.Log("MASTER");
        }
        else
        {
            Debug.Log("Wrong team id");
        }
    }

    //master call to setup views for all players
    public void setClientViews()
    {
        numbersSet = false;
        //enable first all clients items
        foreach (PhotonView GO in ListOfDragingObjects)
        {
            GO.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }

        //for all players
        for (int teamID = 1; teamID < PhotonNetwork.CurrentRoom.MaxPlayers; teamID++)
        {
            for (int i = 0; i < 4; i++)
            {
                int index = i + (teamID - 1) * 4;
                Vector3 newVector = ListOfDragingObjects[allNumbers[index]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                //new
                ListOfDragingObjects[allNumbers[index]].transform.parent.gameObject.SetActive(true);

                GameObject ga = ListOfDragingObjects[allNumbers[index]].gameObject;
                ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
                ga.transform.SetParent(allPlayersView[teamID - 1].gameObject.transform.GetChild(i));
                ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            }
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
        foreach (PhotonView GO in ListOfDragingObjects)
        {
            GO.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
