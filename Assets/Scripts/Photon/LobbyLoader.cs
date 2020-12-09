using UnityEngine;
using TMPro;

public class LobbyLoader : MonoBehaviour
{
    public static LobbyLoader lobbyLoader;

    //different user
    public GameObject playerType;
    public GameObject teacherLobby;
    public GameObject playerLobby;
    public TMP_InputField TeacherInputField;
    public string TeacherInputFieldPassword;
    public TMP_InputField PlayerInputField;
    public string PlayerInputFieldPassword;
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
        if (TeacherInputField.text == TeacherInputFieldPassword)
        {
            type = 1;
            setLobbyByClient(type);
        }
    }

    public void PlayerButtonPressed()
    {
        //player client
        if (PlayerInputField.text == PlayerInputFieldPassword)
        {
            type = 2;
            setLobbyByClient(type);
        }
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
