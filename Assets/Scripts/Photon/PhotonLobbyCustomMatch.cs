using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
public class PhotonLobbyCustomMatch : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    //lobby instance
    public static PhotonLobbyCustomMatch lobby;

    //button for start game
    public string roomName;
    public string roomCode;
    public int roomSize;
    public GameObject roomListingPrefab;
    public Transform roomsPanel;

    public List<RoomInfo> roomListings;

    //roomCode for access
    private static System.Random random = new System.Random();
    private int roomCodeLength = 5;

    //room settings
    private static int minRoomSize = 5;
    private static int maxRoomSize = 11;



    private void Awake()
    {
        //creates the singleton, lives withing the Main menu scene.
        lobby = this;
    }

    // Use this for initialization
    void Start()
    {
        //Connect to Master photon server
        PhotonNetwork.ConnectUsingSettings();
        roomListings = new List<RoomInfo>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Connect to Master server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has conntected to the Photon master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("Player has disconntected from the Photon master server");
    }
    //changes on available room on lobby
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        RemoveRoomListings(roomList);
    }

    static System.Predicate<RoomInfo> ByName(string whichName)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == whichName;
        };
    }

    public void RemoveRoomListings(List<RoomInfo> roomList)
    {
        int tempIndex;
        foreach (RoomInfo room in roomList)
        {
            if (roomListings != null)
            {
                tempIndex = roomListings.FindIndex(ByName(room.Name));
            }
            else
            {
                tempIndex = -1;
            }

            if (tempIndex != -1)
            {
                roomListings.RemoveAt(tempIndex);
                Destroy(roomsPanel.GetChild(tempIndex).gameObject);
            }
            else
            {
                roomListings.Add(room);
                ListRoom(room);
            }
        }
    }

    //display Room in room panel
    void ListRoom(RoomInfo newRoom)
    {
        if (newRoom.IsOpen && newRoom.IsVisible)
        {
            GameObject tempListing = Instantiate(roomListingPrefab, roomsPanel);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();

            tempButton.roomName = newRoom.Name.Substring(roomCodeLength);
            tempButton.roomSize = newRoom.MaxPlayers;
            tempButton.SetRoom();
        }
    }

    public GameObject RoomCodeText;
    public GameObject StartGameText;
    //create new Room
    public void CreateRoom()
    {
        Debug.Log("Trying to create a new room");
        //check default room size
        if (roomSize < minRoomSize || roomSize > maxRoomSize)
        {
            Debug.LogWarning("Room size must be between 4 and 10 players including teacher");
            return;
        }
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize};
        /*ExitGames.Client.Photon.Hashtable t = new ExitGames.Client.Photon.Hashtable();
        t.Add("playerName", "NE");
        roomOps.CustomRoomProperties = t;*/
        RoomText.text = "Send this code to participants. Select then Ctrl-c to copy.";
        WaitingRoom.SetActive(false);
        RoomCodeText.SetActive(true);
        StartGameText.SetActive(true);
        PhotonNetwork.CreateRoom(roomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must be a room with the same name");
    }

    //when user change room name
    public void OnRoomNameChanged(string nameIn)
    {
        //generate room Code
        roomName = RandomString(roomCodeLength);
        roomName = roomName + nameIn;
    }

    //when user change size
    public void OnRoomSizeChanged(string sizeIn)
    {
        //only number of clients add teacher
        int temp = int.Parse(sizeIn);
        temp++;
        roomSize = temp;
    }

    //when user click join lobby, disabled for now
    public void JoinLobbyOnClick()
    {   

        if (!PhotonNetwork.InLobby && PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinLobby();
        }
    }
    public TextMeshProUGUI RoomText;
    public GameObject WaitingRoom;
    //join room with code
    public void JoinRoomOnClick()
    {
        if (roomCode.Length != 0)
        {
            RoomText.text = "Please wait organiser to start the game.";
            WaitingRoom.SetActive(true);
            RoomCodeText.SetActive(false);
            StartGameText.SetActive(false);
            PhotonNetwork.JoinRoom(roomCode);
        }
    }

    //generate room code
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    //set Room code on user input
    public void OnRoomCodeChanged(string nameIn)
    {
        roomCode = nameIn;
    }

    public void OnPlayerNameChanged(string nameIn)
    {
        PhotonNetwork.NickName = nameIn;
    }

    public void OnTeacherNameChanged(string nameIn)
    {
        PhotonNetwork.NickName = nameIn;
    }

    public void connectToMaster()
    {
        //Connect to Master photon server
        PhotonNetwork.ConnectUsingSettings();
        roomListings = new List<RoomInfo>();
    }

    public void disconnectFromMaster()
    {
        //Disconnection current player
        PhotonNetwork.Disconnect();
    }

}


