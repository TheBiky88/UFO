using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_PlayerShips : MonoBehaviour
{
    [SerializeField] private List<GameObject> PlayerShips = new List<GameObject>();
    [SerializeField] private List<MeshRenderer> PlayerMaterial = new List<MeshRenderer>();
    [SerializeField] private List<MeshFilter> PlayerFilter = new List<MeshFilter>();
    [SerializeField] private int PlayerCount = 0;


    [SerializeField] private List<Material> tempMaterial = new List<Material>();
    [SerializeField] private List<Mesh> tempMesh = new List<Mesh>();
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < PlayerShips.Count; i++)
        {
            tempMaterial.Add(null);
            tempMesh.Add(null);

            PlayerMaterial.Add(PlayerShips[i].GetComponent<MeshRenderer>());
            PlayerFilter.Add(PlayerShips[i].GetComponent<MeshFilter>());
        }

    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "SelectMenu")
        {
            Debug.Log("Updating");
            for (int i = 0; i < PlayerShips.Count; i++)
            {
                PlayerMaterial[i].material = PlayerShips[i].GetComponent<MeshRenderer>().material;
                PlayerFilter[i].mesh = PlayerShips[i].GetComponent<MeshFilter>().mesh;

                tempMaterial[i] = PlayerMaterial[i].material;
                tempMesh[i] = PlayerFilter[i].mesh;
            }
        }
    }

    public void LoadData(GameObject go)
    {
        Debug.Log("Loading Data");

        PlayerShips.Clear();
        PlayerShips.Add(GameObject.FindGameObjectWithTag("Player 1")); 
        PlayerShips.Add(GameObject.FindGameObjectWithTag("Player 2")); 
        PlayerShips.Add(GameObject.FindGameObjectWithTag("Player 3")); 
        PlayerShips.Add(GameObject.FindGameObjectWithTag("Player 4"));

        for (int i = 0; i < PlayerCount; i++)
        {
            PlayerShips[i].GetComponent<MeshRenderer>().material = tempMaterial[i];
            PlayerShips[i].GetComponent<MeshFilter>().mesh = tempMesh[i];
        }

        if (PlayerCount == 2)
        {
            PlayerShips[2].GetComponentInParent<scr_PlayerControl>().Deactivate();
            PlayerShips[3].GetComponentInParent<scr_PlayerControl>().Deactivate();
            PlayerShips[0].GetComponentInParent<scr_PlayerControl>().ChangeCamera(new Rect(0, 0.5f, 1, 0.5f));
            PlayerShips[1].GetComponentInParent<scr_PlayerControl>().ChangeCamera(new Rect(0, 0, 1, 0.5f));
            PlayerShips[0].GetComponentInParent<scr_PlayerControl>().SetFOV(70);
            PlayerShips[1].GetComponentInParent<scr_PlayerControl>().SetFOV(70);
            PlayerShips[0].GetComponentInParent<scr_PlayerControl>().MoveCanvas(480, 1060);
            PlayerShips[1].GetComponentInParent<scr_PlayerControl>().MoveCanvas(480, 540);
            PlayerShips[2].GetComponentInParent<scr_PlayerControl>().ChangeCamera(false);
            PlayerShips[3].GetComponentInParent<scr_PlayerControl>().ChangeCamera(false);

        }

        else if (PlayerCount == 3)
        {
            PlayerShips[3].GetComponentInParent<scr_PlayerControl>().Deactivate();
        }

        go.GetComponent<scr_GameControl>().SetAmount(PlayerCount);

        Destroy(gameObject);
    }

    public void SetPlayerCount(int count)
    {
        PlayerCount = count;
    }
}
