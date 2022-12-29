using Unity.Netcode;
using UnityEngine;

public class TeamPicker : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey("1"))
            SelectTeam(0);
        if (Input.GetKey("2"))
            SelectTeam(1);
        if (Input.GetKey("3"))
            SelectTeam(2);
        if (Input.GetKey("4"))
            SelectTeam(3);
    }

    public void SelectTeam(int teamIndex)
    {
        if (!NetworkManager.Singleton.LocalClient.PlayerObject.TryGetComponent(out TeamPlayer teamPlayer))
            return;

        teamPlayer?.SetTeamServerRpc(teamIndex);
    }
}