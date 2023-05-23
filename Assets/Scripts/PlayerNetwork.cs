using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class PlayerNetwork : NetworkBehaviour
{
    private float speed = 10f; //velocidad del jugador
    public List<Color> colorequipo1 = new List<Color>();//lista colores equipo rojo
    public List<Color> colorequipo2 = new List<Color>();//lista colores equipo azul
    public NetworkVariable<Color> ColorEquipo = new NetworkVariable<Color>(Color.white); //variable que indica el color que tiene el jugador



    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            ChangePositionCenter();
        }
    }
    private void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.M))
        {
            ChangePositionCenter();
        }

        this.transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed * Time.deltaTime;
        ZonaServerRpc();
    }


    [ClientRpc]
    public void ChangePositionCenterClientRpc()
    {

        ChangePositionCenter();

    }

    public void ChangePositionCenter()
    {
        this.transform.position = new Vector3(Random.Range(-20f, 20f), 1f, Random.Range(-70f, 70f));
    }

    [ServerRpc]
    public void ZonaServerRpc()
    {
        if (this.transform.position.x < -20f)
        {
            if (ColorEquipo.Value == Color.white)
            {
                ColorEquipo.Value = colorequipo1[Random.Range(0, colorequipo1.Count)];
            }
        }
        else if (this.transform.position.x < 20f)
        {
            ColorEquipo.Value = Color.white;
        }
        else
        {
            if (ColorEquipo.Value == Color.white)
            {
                ColorEquipo.Value = colorequipo2[Random.Range(0, colorequipo2.Count)];
            }
        }
        ChangeColorClientRpc();
    }

    [ClientRpc]
    void ChangeColorClientRpc()
    {
        GetComponent<MeshRenderer>().material.color = ColorEquipo.Value;
    }

}

