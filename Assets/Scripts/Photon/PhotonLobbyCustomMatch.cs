using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private static System.Random random = new System.Random();
    private int roomCodeLength = 10; 
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

    //Connect to Master server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has conntected to the Photon master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000);

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

    //create new Room
    public void CreateRoom()
    {
        Debug.Log("Trying to create a new room");
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom(roomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must be a room with the same name");
        //CreateRoom();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //when user change room name
    public void OnRoomNameChanged(string nameIn)
    {

        roomName = RandomString(roomCodeLength);
        roomName = roomName + nameIn;
        //roomName = nameIn;
    }

    //when user change size
    public void OnRoomSizeChanged(string sizeIn)
    {
        roomSize = int.Parse(sizeIn);
    }

    //when user click join lobby
    public void JoinLobbyOnClick()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public void OnRoomCodeChanged(string nameIn)
    {

        roomCode = nameIn;
       
        //roomName = nameIn;
    }

    public void JoinRoomOnClick()
    {
        if(roomCode.Length != 0)
        {
            PhotonNetwork.JoinRoom(roomCode);
        }
    }
}


