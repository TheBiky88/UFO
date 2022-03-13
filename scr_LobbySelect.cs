using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_LobbySelect : MonoBehaviour
{
    [SerializeField] private GameObject Ship;
    [SerializeField] private KeyCode m_RightKey;
    [SerializeField] private KeyCode m_LeftKey;
    [SerializeField] private KeyCode m_FireKey;
    [SerializeField] private KeyCode m_ItemKey;
        
    [SerializeField] private List<Mesh> ShipMeshesList = new List<Mesh>();
    [SerializeField] private List<Material> ShipMeshOneMaterialsList = new List<Material>();
    [SerializeField] private List<Material> ShipMeshTwoMaterialsList = new List<Material>();
    [SerializeField] public int material { get; private set; } = 0;
    [SerializeField] public int fakeMat;
    [SerializeField] public int mesh { get; private set; } = 0;

    [SerializeField] private List<GameObject> OtherShips = new List<GameObject>();
    [SerializeField] private int OtherActiveShips; // 1 - 3

    private MeshFilter mf;
    private MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {

        while(OtherShips[0].GetComponent<scr_LobbySelect>().material == material
                || OtherShips[1].GetComponent<scr_LobbySelect>().material == material
                || OtherShips[2].GetComponent<scr_LobbySelect>().material == material)//creditos a Aaron Blok
        {
            material = Random.Range(0, 8);
        }

        mf = Ship.GetComponent<MeshFilter>();
        mr = Ship.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, 20f * Time.deltaTime, 0);

        ChangeShipAndShipColour();
        fakeMat = material;
        Material mat = null;

        if (mesh == 0)
        {
            mat = ShipMeshOneMaterialsList[material];
        }
        else if (mesh == 1)
        {
            mat = ShipMeshTwoMaterialsList[material];
        }

        mf.mesh = ShipMeshesList[mesh];
        mr.material = mat;
    }


    private void ChangeShipAndShipColour()
    {
        bool left2 = Input.GetKeyDown(m_FireKey);
        bool right2 = Input.GetKeyDown(m_ItemKey);
        bool left = Input.GetKeyDown(m_LeftKey);
        bool right = Input.GetKeyDown(m_RightKey);

        if (right)
        {
            material += 1;

            if (material > ShipMeshOneMaterialsList.Count - 1)
            {
                material = 0;
            }

            while (OtherShips[0].GetComponent<scr_LobbySelect>().material == material
                || OtherShips[1].GetComponent<scr_LobbySelect>().material == material
                || OtherShips[2].GetComponent<scr_LobbySelect>().material == material) //creditos a Aaron Blok
            {
                material += 1;

                if (material > ShipMeshOneMaterialsList.Count - 1)
                {
                    material = 0;
                }
            }
        }

        else if (left)
        {
            material -= 1;

            if (material < 0)
            {
                material = ShipMeshOneMaterialsList.Count - 1;
            }

            while (OtherShips[0].GetComponent<scr_LobbySelect>().material == material
                || OtherShips[1].GetComponent<scr_LobbySelect>().material == material
                || OtherShips[2].GetComponent<scr_LobbySelect>().material == material) //creditos a Aaron Blok
            {
                material -= 1;

                if (material < 0)
                {
                    material = ShipMeshOneMaterialsList.Count - 1;
                }
            }
        }

        if (right2)
        {
            mesh += 1;
            if (mesh > ShipMeshesList.Count - 1)
            {
                mesh = 0;
            }
        }

        else if (left2)
        {
            mesh -= 1;
            if (mesh < 0)
            {
                mesh = ShipMeshesList.Count - 1;
            }
        }
    }

    public void ActiveShipsCount(int amount)
    {
        OtherActiveShips = amount;
    }
}
