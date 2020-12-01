using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

public class PhotonPlayer : MonoBehaviour
{
    public PhotonView PV;
    public static GameObject myAvatar;
    public static PhotonPlayer player;

    public int myTeam;

    //random list
    public int[] myRandomNumbers = new int[4];
    public List<int> AllRandomNumbers = new List<int>();
    private static System.Random rng = new System.Random();
    public string myData;
    private BinaryFormatter bf = new BinaryFormatter();
    public Text eno;
    public Text ajde;

    public bool getList = false;
    public bool getTeam = false;
    public bool founded = false;


    void Awake()
    {
        PhotonPlayer.player = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
       
        if (PhotonNetwork.IsMasterClient)
        {
            if (PV.IsMine)
                {
                      
                    //randomize list
                    while (true)
                    {
                        int num = rng.Next(0, 16);
                        if (!AllRandomNumbers.Contains(num))
                        {
                            AllRandomNumbers.Add(num);
                        }
                        if (AllRandomNumbers.Count == 16)
                        {
                            break;
                        }
                    }

                    string tako = "LISTA NA MASTERU: ";
                    foreach (int i in AllRandomNumbers)
                    {
                        tako += " " + i.ToString();
                        
                    // listOfObjects[i].GetComponentInChildren<TextMesh>().text = i.ToString();
                    }
                    Debug.LogWarning(tako);

               
                    GameSetup.GS.SetActiveList(AllRandomNumbers,0);
                    GameSetup.GS.SetList(AllRandomNumbers);
                        var o = new MemoryStream(); //Create something to hold the data

                         //Create a formatter
                        bf.Serialize(o, AllRandomNumbers); //Save the list
                        var data = Convert.ToBase64String(o.GetBuffer()); //Convert the data to a string
                       // PV.RPC("RPC_GetList", RpcTarget.OthersBuffered, data);
                      /*  myTeam = GameSetup.GS.nextPlayerTeam;
                        GameSetup.GS.UpdateTeam();*/

            }
            
        }

        if (PV.IsMine)
        {
            PV.RPC("RPC_GetTeam", RpcTarget.MasterClient);
            
        }
        
    }
    public PhotonView getPV()
    {
        return this.PV;
    }

    public void comeON()
    {
        if(PV.IsMine)
        Debug.LogWarning("TEAM " + myTeam + "LIST COUNT" + AllRandomNumbers.Count );

        /*if (PV.IsMine)
        {
            switch (myTeam)
            {
                case 1:
                    {
                        
                            Debug.LogWarning("P1 COUNT " + AllRandomNumbers.Count);

                            string s = " ";
                            for (int i = 0; i < myRandomNumbers.Length; i++)
                            {
                                myRandomNumbers[i] = AllRandomNumbers[i];
                                s += " " + myRandomNumbers[i];
                            }
                            Debug.LogWarning("Player 1 Numbers: " + s + "COUNT " + AllRandomNumbers.Count);

                        
                        break;
                    }
                case 2:
                    {
                        
                            Debug.LogWarning("P2 COUNT " + AllRandomNumbers.Count);

                            string s = " ";
                            for (int i = 0; i < myRandomNumbers.Length; i++)
                            {
                                myRandomNumbers[i] = AllRandomNumbers[i + 4];
                                s += " " + myRandomNumbers[i];

                            }
                            Debug.LogWarning("Player 2 Numbers: " + s + "COUNT " + myRandomNumbers.Length);
                        
                        break;
                    }
                case 3:
                    {
                        
                            Debug.LogWarning("P3 COUNT " + AllRandomNumbers.Count);

                            string s = " ";
                            for (int i = 0; i < myRandomNumbers.Length; i++)
                            {
                                myRandomNumbers[i] = AllRandomNumbers[i + 8];
                                s += " " + myRandomNumbers[i];
                            }
                            Debug.LogWarning("Player 3 Numbers: " + s + "COUNT " + AllRandomNumbers.Count);
                        
                        break;
                    }

                case 4:
                    {
                        
                            Debug.LogWarning("P4 COUNT " + AllRandomNumbers.Count);

                            string s = " ";
                            for (int i = 0; i < myRandomNumbers.Length; i++)
                            {
                                myRandomNumbers[i] = AllRandomNumbers[i];
                            }
                            Debug.LogWarning("Player 4 Numbers: " + s + "COUNT " + AllRandomNumbers.Count);
                        
                        break;
                    }
                default: break;
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    void RPC_GetTeam()
    {
        myTeam = GameSetup.GS.nextPlayerTeam;
        GameSetup.GS.UpdateTeam();
        Debug.LogWarning("MASTER GET TEAM" + myTeam.ToString());
        PV.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);
        if (PV.IsMine)
        {
            var o = new MemoryStream(); //Create something to hold the data

            //Create a formatter
            bf.Serialize(o, AllRandomNumbers); //Save the list
            var data = Convert.ToBase64String(o.GetBuffer());
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
        //Create a formatter 
        //var bf = new BinaryFormatter();   
        // Reading it back in
        var ins = new MemoryStream(Convert.FromBase64String(data)); //Create an input stream from the string

        //Read back the data
        List<int> x = (List<int>)bf.Deserialize(ins);
        AllRandomNumbers = x;
           
            GameSetup.GS.SetActiveList(AllRandomNumbers, PhotonRoomCustomMatch.room.myNumberInRoom-1);
            //GameSetup.GS.SetActiveList(AllRandomNumbers, myTeam);

            getList = true;
        }
    }
}
