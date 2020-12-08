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
        //set position of items in player view
        foreach (PhotonView pv in ListOfDragingObjects)
        {
            pv.gameObject.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            pv.gameObject.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            //set Sheet view position
            setGraphic();

            //enable views for all students
            setAlphaForStudentsView();

            //players name visible
            setPlayersName();

            //client view set alpha
            playerNumbers.transform.GetComponent<CanvasGroup>().alpha = 0;

            //set button active
            swapButton.gameObject.SetActive(true);
        }

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
        sheet.transform.SetPositionAndRotation(new Vector3(-449, 374, 0), sheet.transform.rotation);
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

    public void SetActiveList(List<int> numbers, int team)
    {
        switch (team)
        {
            case 0:
                Debug.Log("MASTER");
                break;
            //Player one objects
            case 1:
                for (int i = 0; i < 4; i++)
                {
                    
                    playerNumbers.GetComponent<CanvasGroup>().alpha = 0;
                    playerOneNumbers.GetComponent<CanvasGroup>().alpha = 1;
                    playerOneNumbers.GetComponent<CanvasGroup>().interactable = true;
                    playerOneNumbers.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    Vector3 newVector = ListOfDragingObjects[numbers[i]].gameObject.transform.GetComponent<RectTransform>().localPosition;
                    
                    GameObject ga = ListOfDragingObjects[numbers[i]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = false;
                    ga.transform.SetParent(playerOneNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);


                    //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    ListOfDragingObjects[numbers[i]].transform.parent.gameObject.SetActive(true);
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
                    
                    playerNumbers.GetComponent<CanvasGroup>().alpha = 0;
                    playerTwoNumbers.GetComponent<CanvasGroup>().alpha = 1;
                    playerTwoNumbers.GetComponent<CanvasGroup>().interactable = true;
                    playerTwoNumbers.GetComponent<CanvasGroup>().blocksRaycasts = true;

                    Vector3 newVector = ListOfDragingObjects[numbers[i + 4]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    GameObject ga = ListOfDragingObjects[numbers[i + 4]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = false;
                    ga.transform.SetParent(playerTwoNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);


                    //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    //ListOfDragingObjects[numbers[i + 4]].transform.parent.gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 4]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 4]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                    ListOfDragingObjects[numbers[i + 4]].GetComponent<Image>().enabled = true;
                }
                break;
            //Player three objects
            case 3:
                for (int i = 0; i < 4; i++)
                {

                    playerNumbers.GetComponent<CanvasGroup>().alpha = 0;
                    playerThreeNumbers.GetComponent<CanvasGroup>().alpha = 1;
                    playerThreeNumbers.GetComponent<CanvasGroup>().interactable = true;
                    playerThreeNumbers.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    Vector3 newVector = ListOfDragingObjects[numbers[i + 8]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    GameObject ga = ListOfDragingObjects[numbers[i + 8]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = false;
                    ga.transform.SetParent(playerThreeNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);


                    //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    ListOfDragingObjects[numbers[i + 8]].transform.parent.gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 8]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i + 8]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 8]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                }
                break;
            //Player four objects
            case 4:
                for (int i = 0; i < 4; i++)
                {
                    playerNumbers.GetComponent<CanvasGroup>().alpha = 0;
                    playerFourNumbers.GetComponent<CanvasGroup>().alpha = 1;
                    playerFourNumbers.GetComponent<CanvasGroup>().interactable = true;
                    playerFourNumbers.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    Vector3 newVector = ListOfDragingObjects[numbers[i + 12]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    GameObject ga = ListOfDragingObjects[numbers[i + 12]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = false;
                    ga.transform.SetParent(playerFourNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);


                    //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    ListOfDragingObjects[numbers[i + 12]].transform.parent.gameObject.SetActive(true);

                    ListOfDragingObjects[numbers[i + 12]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i + 12]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 12]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                }
                break;
            //Player five objects
            case 5:
                for (int i = 0; i < 4; i++)
                {
                    playerNumbers.GetComponent<CanvasGroup>().alpha = 0;
                    playerFiveNumbers.GetComponent<CanvasGroup>().alpha = 1;
                    playerFiveNumbers.GetComponent<CanvasGroup>().interactable = true;
                    playerFiveNumbers.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    Vector3 newVector = ListOfDragingObjects[numbers[i + 16]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    GameObject ga = ListOfDragingObjects[numbers[i + 16]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = false;
                    ga.transform.SetParent(playerFiveNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

                    //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    ListOfDragingObjects[numbers[i + 16]].transform.parent.gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 16]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i + 16]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 16]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                }
                break;
            //Player six objects
            case 6:
                for (int i = 0; i < 4; i++)
                {
                    playerNumbers.GetComponent<CanvasGroup>().alpha = 0;
                    playerSixNumbers.GetComponent<CanvasGroup>().alpha = 1;
                    playerSixNumbers.GetComponent<CanvasGroup>().interactable = true;
                    playerSixNumbers.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    Vector3 newVector = ListOfDragingObjects[numbers[i + 20]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    GameObject ga = ListOfDragingObjects[numbers[i + 20]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = false;
                    ga.transform.SetParent(playerSixNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);


                    //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    ListOfDragingObjects[numbers[i + 20]].transform.parent.gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 20]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i + 20]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 20]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                }
                break;
            //Player seven objects
            case 7:
                for (int i = 0; i < 4; i++)
                {
                    playerNumbers.GetComponent<CanvasGroup>().alpha = 0;
                    playerSevenNumbers.GetComponent<CanvasGroup>().alpha = 1;
                    playerSevenNumbers.GetComponent<CanvasGroup>().interactable = true;
                    playerSevenNumbers.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    Vector3 newVector = ListOfDragingObjects[numbers[i + 24]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    GameObject ga = ListOfDragingObjects[numbers[i + 24]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = false;
                    ga.transform.SetParent(playerSevenNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);


                    //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    ListOfDragingObjects[numbers[i + 24]].transform.parent.gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 24]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i + 24]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 24]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                }
                break;

            //Player eight objects
            case 8:
                for (int i = 0; i < 4; i++)
                {
                    playerNumbers.GetComponent<CanvasGroup>().alpha = 0;
                    playerEightNumbers.GetComponent<CanvasGroup>().alpha = 1;
                    playerEightNumbers.GetComponent<CanvasGroup>().interactable = true;
                    playerEightNumbers.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    Vector3 newVector = ListOfDragingObjects[numbers[i + 28]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    GameObject ga = ListOfDragingObjects[numbers[i + 28]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = false;
                    ga.transform.SetParent(playerEightNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);


                    //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    ListOfDragingObjects[numbers[i + 28]].transform.parent.gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 28]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i + 28]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 28]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                }
                break;

            //Player nine objects
            case 9:
                for (int i = 0; i < 4; i++)
                {
                    playerNumbers.GetComponent<CanvasGroup>().alpha = 0;
                    playerNineNumbers.GetComponent<CanvasGroup>().alpha = 1;
                    playerNineNumbers.GetComponent<CanvasGroup>().interactable = true;
                    playerNineNumbers.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    Vector3 newVector = ListOfDragingObjects[numbers[i + 32]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    GameObject ga = ListOfDragingObjects[numbers[i + 32]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = false;
                    ga.transform.SetParent(playerNineNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);


                    //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    ListOfDragingObjects[numbers[i + 32]].transform.parent.gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 32]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i + 32]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 32]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                }
                break;

            //Player ten objects
            case 10:
                for (int i = 0; i < 4; i++)
                {
                    playerNumbers.GetComponent<CanvasGroup>().alpha = 0;
                    playerTenNumbers.GetComponent<CanvasGroup>().alpha = 1;
                    playerTenNumbers.GetComponent<CanvasGroup>().interactable = true;
                    playerTenNumbers.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    Vector3 newVector = ListOfDragingObjects[numbers[i + 36]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    GameObject ga = ListOfDragingObjects[numbers[i + 36]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = false;
                    ga.transform.SetParent(playerTenNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);


                    //Debug.LogWarning(ListOfDragingObjects[i].ViewID);
                    ListOfDragingObjects[numbers[i + 36]].transform.parent.gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 36]].GetComponent<Image>().enabled = true;
                    ListOfDragingObjects[numbers[i + 36]].transform.GetChild(0).gameObject.SetActive(true);
                    ListOfDragingObjects[numbers[i + 36]].transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
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

        /*//for other players
        int maxCells = (PhotonNetwork.CurrentRoom.MaxPlayers - 1) * 4;
        for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers - 1; i++)
        {
            int t = i*4; 
            for (int j = 0; j < 4; j++)
            {
                Vector3 newVector = ListOfDragingObjects[allNumbers[t]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                //new
                ListOfDragingObjects[allNumbers[j]].transform.parent.gameObject.SetActive(true);


                GameObject ga = ListOfDragingObjects[allNumbers[j]].gameObject;
                ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
                ga.transform.SetParent(allPlayersView[i].gameObject.transform.GetChild(j));
                ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            }
            

        }*/


        //set object for playerOne view
        for (int i = 0; i < 4; i++)
        {
            //object is not dragged 
            //position of dragging object is on playerList
            Vector3 newVector = ListOfDragingObjects[allNumbers[i]].gameObject.transform.GetComponent<RectTransform>().localPosition;

            //new
            ListOfDragingObjects[allNumbers[i]].transform.parent.gameObject.SetActive(true);


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

            ListOfDragingObjects[allNumbers[i + 4]].transform.parent.gameObject.SetActive(true);


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

            ListOfDragingObjects[allNumbers[i + 8]].transform.parent.gameObject.SetActive(true);


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

            ListOfDragingObjects[allNumbers[i + 12]].transform.parent.gameObject.SetActive(true);


            GameObject ga = ListOfDragingObjects[allNumbers[i + 12]].gameObject;
            ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
            ga.transform.SetParent(playerFourNumbers.gameObject.transform.GetChild(i));
            ga.transform.GetComponent<RectTransform>().localPosition = newVector;
            ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }



        //for other players
        switch (PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            case 11:
                //set object for playerTen view
                for (int i = 0; i < 4; i++)
                {
                    //object is not dragged 
                    //position of dragging object is on playerList
                    Vector3 newVector = ListOfDragingObjects[allNumbers[i + 36]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    ListOfDragingObjects[allNumbers[i + 36]].transform.parent.gameObject.SetActive(true);

                    GameObject ga = ListOfDragingObjects[allNumbers[i + 36]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
                    ga.transform.SetParent(playerTenNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }


                //set object for playerNine view
                for (int i = 0; i < 4; i++)
                {
                    //object is not dragged 
                    //position of dragging object is on playerList
                    Vector3 newVector = ListOfDragingObjects[allNumbers[i + 32]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    ListOfDragingObjects[allNumbers[i + 32]].transform.parent.gameObject.SetActive(true);

                    GameObject ga = ListOfDragingObjects[allNumbers[i + 32]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
                    ga.transform.SetParent(playerNineNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }

                //set object for playerEight view
                for (int i = 0; i < 4; i++)
                {
                    //object is not dragged 
                    //position of dragging object is on playerList
                    Vector3 newVector = ListOfDragingObjects[allNumbers[i + 28]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    ListOfDragingObjects[allNumbers[i + 28]].transform.parent.gameObject.SetActive(true);

                    GameObject ga = ListOfDragingObjects[allNumbers[i + 28]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
                    ga.transform.SetParent(playerEightNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }


                //set object for playerSeven view
                for (int i = 0; i < 4; i++)
                {
                    //object is not dragged 
                    //position of dragging object is on playerList
                    Vector3 newVector = ListOfDragingObjects[allNumbers[i + 24]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    ListOfDragingObjects[allNumbers[i + 24]].transform.parent.gameObject.SetActive(true);

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

                    ListOfDragingObjects[allNumbers[i + 24]].transform.parent.gameObject.SetActive(true);


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

                    ListOfDragingObjects[allNumbers[i + 24]].transform.parent.gameObject.SetActive(true);


                    GameObject ga = ListOfDragingObjects[allNumbers[i + 16]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
                    ga.transform.SetParent(playerFiveNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }
                break;


            case 10:
                //set object for playerNine view
                for (int i = 0; i < 4; i++)
                {
                    //object is not dragged 
                    //position of dragging object is on playerList
                    Vector3 newVector = ListOfDragingObjects[allNumbers[i + 32]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    ListOfDragingObjects[allNumbers[i + 32]].transform.parent.gameObject.SetActive(true);

                    GameObject ga = ListOfDragingObjects[allNumbers[i + 32]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
                    ga.transform.SetParent(playerNineNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }

                //set object for playerEight view
                for (int i = 0; i < 4; i++)
                {
                    //object is not dragged 
                    //position of dragging object is on playerList
                    Vector3 newVector = ListOfDragingObjects[allNumbers[i + 28]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    ListOfDragingObjects[allNumbers[i + 28]].transform.parent.gameObject.SetActive(true);

                    GameObject ga = ListOfDragingObjects[allNumbers[i + 28]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
                    ga.transform.SetParent(playerEightNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }


                //set object for playerSeven view
                for (int i = 0; i < 4; i++)
                {
                    //object is not dragged 
                    //position of dragging object is on playerList
                    Vector3 newVector = ListOfDragingObjects[allNumbers[i + 24]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    ListOfDragingObjects[allNumbers[i + 24]].transform.parent.gameObject.SetActive(true);

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

                    ListOfDragingObjects[allNumbers[i + 24]].transform.parent.gameObject.SetActive(true);


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

                    ListOfDragingObjects[allNumbers[i + 24]].transform.parent.gameObject.SetActive(true);


                    GameObject ga = ListOfDragingObjects[allNumbers[i + 16]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
                    ga.transform.SetParent(playerFiveNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }
                break;

            case 9:
                //set object for playerEight view
                for (int i = 0; i < 4; i++)
                {
                    //object is not dragged 
                    //position of dragging object is on playerList
                    Vector3 newVector = ListOfDragingObjects[allNumbers[i + 28]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    ListOfDragingObjects[allNumbers[i + 28]].transform.parent.gameObject.SetActive(true);

                    GameObject ga = ListOfDragingObjects[allNumbers[i + 28]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
                    ga.transform.SetParent(playerEightNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }


                //set object for playerSeven view
                for (int i = 0; i < 4; i++)
                {
                    //object is not dragged 
                    //position of dragging object is on playerList
                    Vector3 newVector = ListOfDragingObjects[allNumbers[i + 24]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    ListOfDragingObjects[allNumbers[i + 24]].transform.parent.gameObject.SetActive(true);

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

                    ListOfDragingObjects[allNumbers[i + 24]].transform.parent.gameObject.SetActive(true);


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

                    ListOfDragingObjects[allNumbers[i + 24]].transform.parent.gameObject.SetActive(true);


                    GameObject ga = ListOfDragingObjects[allNumbers[i + 16]].gameObject;
                    ga.transform.GetComponent<DragAndDropItem>().dragDisabled = true;
                    ga.transform.SetParent(playerFiveNumbers.gameObject.transform.GetChild(i));
                    ga.transform.GetComponent<RectTransform>().localPosition = newVector;
                    ga.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                }
                break;
            case 8:
                //set object for playerSeven view
                for (int i = 0; i < 4; i++)
                {
                    //object is not dragged 
                    //position of dragging object is on playerList
                    Vector3 newVector = ListOfDragingObjects[allNumbers[i + 24]].gameObject.transform.GetComponent<RectTransform>().localPosition;

                    ListOfDragingObjects[allNumbers[i + 24]].transform.parent.gameObject.SetActive(true);

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

                    ListOfDragingObjects[allNumbers[i + 24]].transform.parent.gameObject.SetActive(true);


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

                    ListOfDragingObjects[allNumbers[i + 24]].transform.parent.gameObject.SetActive(true);


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

                    ListOfDragingObjects[allNumbers[i + 24]].transform.parent.gameObject.SetActive(true);


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

                    ListOfDragingObjects[allNumbers[i + 24]].transform.parent.gameObject.SetActive(true);


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

                    ListOfDragingObjects[allNumbers[i + 24]].transform.parent.gameObject.SetActive(true);


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
            // playerNumbers.GetComponent<CanvasGroup>().alpha = 1;
            //GO.transform.parent.gameObject.SetActive(true);
            GO.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
