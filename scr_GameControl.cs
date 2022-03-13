using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scr_GameControl : MonoBehaviour
{
    [SerializeField] private KeyCode _PauseKey;
    [SerializeField] private KeyCode _ReturnKey;
    [SerializeField] private GameObject pausedText;
    private bool paused = false;        
    [SerializeField] private int aliveShips = 0;
    [SerializeField] private string mode = "";

    [SerializeField] private GameObject[] ships;
    [SerializeField] private int[] shipRacePositions;
    [SerializeField] private float[] shipAngles;
    private float timer = 10f;
    [SerializeField] private GameObject victoryScreen;
    private bool victory;
    private int amount = 4;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<scr_PlayerShips>().LoadData(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == "Race") // unused
        {
            shipAngles = new float[amount];
            for (int i = 0; i < shipAngles.Length; i++)
            {
                float MyPositionX = transform.position.x;
                float MyPositionZ = transform.position.z;
                float TargetPositionX = ships[i].transform.position.x;
                float TargetPositionZ = ships[i].transform.position.z;
                float degree = FindDegree(MyPositionX - TargetPositionX, MyPositionZ - TargetPositionZ);

                shipAngles[i] = degree;
            }

            float[] angles = new float[ships.Length];
            for (int i = 0; i < shipAngles.Length; i++)
            {
                angles[i] = shipAngles[i];
            }

            Array.Sort(angles);
            Array.Reverse(angles);

            for (int t = 0; t < shipAngles.Length; t++)
            {
                for (int u = 0; u < shipAngles.Length; u++)
                {
                    if (u != t)
                    {
                        if (ships[t].GetComponent<scr_PlayerControl>().GetLap() == ships[u].GetComponent<scr_PlayerControl>().GetLap())
                        {
                            for (int i = 0; i < shipAngles.Length; i++)
                            {
                                for (int pos = 0; pos < shipAngles.Length; pos++)
                                {
                                    if (shipAngles[i] == angles[pos])
                                    {
                                        shipRacePositions[i] = pos + 1;
                                        ships[i].GetComponent<scr_PlayerControl>().SetPosition(pos + 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(_PauseKey))
        {
            paused = !paused;
            pausedText.gameObject.SetActive(paused);
            if (paused) PauseGame();
            else if (!paused) ContinueGame();
        }

        if ((paused && Input.GetKeyDown(_ReturnKey)))
        {
            paused = false;
            ContinueGame();
            SceneManager.LoadScene("SelectMenu");
        }

        if (aliveShips == 1)
        {
            if (!victory)
            {
                for (int i = 0; i < shipAngles.Length; i++)
                {
                    ships[i].GetComponent<scr_PlayerControl>().Victory();
                }
                victory = true;
                victoryScreen.SetActive(true);
            }

            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }

            else
            {
                SceneManager.LoadScene("SelectMenu");
            }
        }
    }

    private static float FindDegree(float x, float y)
    {
        float value = (float)((System.Math.Atan2(x, y) / System.Math.PI) * 180f);
        if (value < 0) value += 360f;
        return value;
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ContinueGame()
    {
        Time.timeScale = 1;
    }

    public void RemoveOneFromAliveShips()
    {
        aliveShips--;
    }

    public void SetAmount(int amount)
    {
        this.amount = amount;
    }
}
