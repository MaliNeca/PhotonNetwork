using Photon.Pun;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class PhotonPlayer : MonoBehaviour
{
    public PhotonView PV;
    public static GameObject myAvatar;
    public static PhotonPlayer player;

    public int myTeam;

    //random list
    //public int[] myRandomNumbers = new int[4];
    public List<int> AllRandomNumbers = new List<int>();

    //create Random generator and binary formatter
    private static System.Random rng = new System.Random();
    public string myData;
    private BinaryFormatter bf = new BinaryFormatter();

    public bool getList = false;
    public bool getTeam = false;
    public bool founded = false;
    public bool randomList;

    private static int playerViewsCounter = 2;


    void Awake()
    {
        PhotonPlayer.player = this;
       
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        //Debug.LogWarning("counter " + playerViewsCounter);
        if (PhotonNetwork.IsMasterClient)
        {
            if (PV.IsMine)
            {
                //randomize list
                //Debug.Log("max player in room" + PhotonNetwork.CurrentRoom.MaxPlayers);
              /*  while (true)
                {
                    int num = rng.Next(0, (PhotonNetwork.CurrentRoom.MaxPlayers - 1) * playerViewsCounter);
                    if (!AllRandomNumbers.Contains(num))
                    {
                        AllRandomNumbers.Add(num);
                    }
                    if (AllRandomNumbers.Count == ((PhotonNetwork.CurrentRoom.MaxPlayers - 1) * playerViewsCounter))
                    {
                        break;
                    }
                }*/

                AllRandomNumbers = randomizeList(randomList);
                //send gameSetup list of numbers
                GameSetup.GS.SetActiveList(AllRandomNumbers, 0);
                GameSetup.GS.SetList(AllRandomNumbers);

                //Create something to hold the data
                var o = new MemoryStream();
                //Save the list
                bf.Serialize(o, AllRandomNumbers);
                //Convert the data to a string
                var data = Convert.ToBase64String(o.GetBuffer());

                //PV.RPC("RPC_GetList", RpcTarget.OthersBuffered, data);
            }
        }
        //call master client to get team
        if (PV.IsMine)
        {
            PV.RPC("RPC_GetTeam", RpcTarget.MasterClient);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private List<int> randomizeList(bool random)
    {
        List<int> tempList = new List<int>();
        if (random)
        {
            while (true)
            {
                int num = rng.Next(0, (PhotonNetwork.CurrentRoom.MaxPlayers - 1) * playerViewsCounter);
                if (!tempList.Contains(num))
                {
                    tempList.Add(num);
                }
                if (tempList.Count == ((PhotonNetwork.CurrentRoom.MaxPlayers - 1) * playerViewsCounter))
                {
                    return tempList;
                }
            }
        }
        else
        {
            for(int i = 0; i < (PhotonNetwork.CurrentRoom.MaxPlayers - 1) * playerViewsCounter; i++)
            {
                tempList.Add(i);
            }
           
            return tempList;
        }
    }

    //call rpc to update view on all clients
    public void sendAll()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //Debug.LogWarning("Send all recive");
            PV.RPC("RPC_GameSetupSetVisible", RpcTarget.Others);
        }
    }

    [PunRPC]
    void RPC_GetTeam()
    {
        myTeam = GameSetup.GS.nextPlayerTeam;
        GameSetup.GS.UpdateTeam();

        //Debug.LogWarning("MASTER GET TEAM" + myTeam.ToString());

        PV.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);
        if (PV.IsMine)
        {
            //Create something to hold the data
            var o = new MemoryStream();
            //Save the list
            bf.Serialize(o, AllRandomNumbers);
            var data = Convert.ToBase64String(o.GetBuffer());
            //send list to other players
            PV.RPC("RPC_GetList", RpcTarget.OthersBuffered, data);
        }
    }

    [PunRPC]
    void RPC_SentTeam(int whichTeam)
    {
        //set Player team
        myTeam = whichTeam;
    }

    [PunRPC]
    void RPC_GetList(string data)
    {
        if (AllRandomNumbers.Count == 0)
        {
            //Create an input stream from the string
            var ins = new MemoryStream(Convert.FromBase64String(data));

            //Read back the data
            List<int> x = (List<int>)bf.Deserialize(ins);

            //set AllRandomNumbers
            AllRandomNumbers = x;

            //set List for Players
            GameSetup.GS.SetActiveList(AllRandomNumbers, PhotonRoomCustomMatch.room.myNumberInRoom - 1);

            //GameSetup.GS.SetActiveList(AllRandomNumbers, myTeam);
            getList = true;
        }
    }

    [PunRPC]
    public void RPC_GameSetupSetVisible()
    {
        //Debug.LogWarning("rpc Send all recive");
        GameSetup.GS.setView();
    }
}
