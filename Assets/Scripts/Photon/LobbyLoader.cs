using UnityEngine;


public class LobbyLoader : MonoBehaviour
{
    public static LobbyLoader lobbyLoader;

    //different user
    public GameObject playerType;
    public GameObject teacherLobby;
    public GameObject playerLobby;
    //1 - teacher, 2 - player
    public int type = 0;

    private void Awake()
    {
        //creates the singleton, lives withing the Main menu scene.
        lobbyLoader = this;
    }

    public void TeacherButtonPressed()
    {
        //master client
        type = 1;
        setLobbyByClient(type);
    }

    public void PlayerButtonPressed()
    {
        //player client
        type = 2;
        setLobbyByClient(type);
    }
    private void setLobbyByClient(int clientType)
    {
        playerType.SetActive(false);
        switch (clientType)
        {
            //master
            case 1:
                teacherLobby.SetActive(true);
                break;
            //player
            case 2:
                playerLobby.SetActive(true);
                //show list of rooms
                PhotonLobbyCustomMatch.lobby.JoinLobbyOnClick();
                break;
            default:
                Debug.Log("Client type is not selected");
                return;

        }
       
    }

    public void backButtonPressed()
    {
        //PhotonLobbyCustomMatch.lobby.disconnectFromMaster();
        playerLobby.SetActive(false);
        teacherLobby.SetActive(false);
        playerType.SetActive(true);
    }
}
