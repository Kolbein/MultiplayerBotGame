using Unity.Netcode;
using UnityEngine;

public class TeamPlayer : NetworkBehaviour
{
    [SerializeField] private Renderer teamColourRenderer;
    [SerializeField] private Color[] teamColours;

    private NetworkVariable<int> _teamIndex = new(0);

    public override void OnNetworkSpawn()
    {
        teamColourRenderer.material.SetColor("_BaseColor", teamColours[_teamIndex.Value]);
    }

    [ServerRpc]
    public void SetTeamServerRpc(int newTeamIndex)
    {
        if (newTeamIndex > 3) return;

        _teamIndex.Value = newTeamIndex;
    }

    private void OnEnable()
    {
        _teamIndex.OnValueChanged += OnTeamChanged;
    }

    private void OnDisable()
    {
        _teamIndex.OnValueChanged -= OnTeamChanged;
    }

    private void OnTeamChanged(int oldTeamIndex, int newTeamIndex)
    {
        if (!IsClient) return;

        teamColourRenderer.material.SetColor("_BaseColor", teamColours[newTeamIndex]);
    }
}