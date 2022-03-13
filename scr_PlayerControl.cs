using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scr_PlayerControl : MonoBehaviour
{
    [SerializeField] private KeyCode m_RightKey;
    [SerializeField] private KeyCode m_LeftKey;
    [SerializeField] private KeyCode m_FireKey;
    [SerializeField] private KeyCode m_ItemKey;
    [SerializeField] private Transform m_Planet;
    [SerializeField] private string m_InputAxis;
    [SerializeField] private GameObject m_Projectile;
    [SerializeField] private GameObject m_ShipMesh;
    [SerializeField] private float FireTimer = 0;
    [SerializeField] private string _Weapon;
    [SerializeField] private GameObject _HealthBar;
    [SerializeField] private Image _CanvasWeapon;
    [SerializeField] private GameObject _CanvasMovement;
    [SerializeField] private GameObject _CanvasFire;
    [SerializeField] private GameObject _CanvasGroup;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private string[] weaponsList = new string[3];
    [SerializeField] private GameObject[] _ProjectileArray = new GameObject[3];
    [SerializeField] private GameObject _GameController;
    [SerializeField] private bool isDead = false;
    [SerializeField] private Color colour;

    [SerializeField] private List<Sprite> weaponSprites = new List<Sprite>();

    [SerializeField] private int PositionRace = 4;
    [SerializeField] private int lap = 0;
    [SerializeField] private int checkpoint = 0;
    [SerializeField] private bool finished = false;

    [SerializeField] private TextMeshProUGUI textPos;
    [SerializeField] private TextMeshProUGUI textLap;

    private float FireMaxTimer;
    private float m_Horizontal = 0;
    [SerializeField] private float m_ShipSpeed = 10f;
    public float m_MaxShipSpeed = 50f;
    private float _ProjectileSpeed = 0f;
    private float _ProjectileLifeTime = 0f;


    public float shipHealth = 100;
    public float shipProjectileDamage = 7.5f;
    private float m_turnRotation = 90f;
    private Quaternion m_Rot;

    private float _NormalZRotation = 90;

    // Start is called before the first frame update
    void Start()
    {
        m_Rot = m_ShipMesh.transform.rotation;
        int RandomN = Random.Range(0, 3);
        _Weapon = weaponsList[RandomN];
        _CanvasWeapon.sprite = weaponSprites[RandomN];

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (textLap != null)
        {
            string text = "Lap " + lap + "-3";
            textLap.text = text;
        }

        _HealthBar.GetComponent<Slider>().value = shipHealth;

        bool fire = Input.GetKey(m_FireKey);

        bool left = Input.GetKey(m_LeftKey);
        bool right = Input.GetKey(m_RightKey);

        if (shipHealth <= 0)
        {
            gameObject.GetComponent<scr_Explosion>().Explode();
            Deactivate();
        }

        if (!finished)
        {
            if (right)
            {
                if (m_turnRotation > -50f + _NormalZRotation)
                {
                    m_ShipMesh.transform.Rotate(0, 0, -3f);
                    m_turnRotation -= 3f;
                }
            }
            else if (left)
            {
                if (m_turnRotation < 50f + _NormalZRotation)
                {
                    m_ShipMesh.transform.Rotate(0, 0, 3f);
                    m_turnRotation += 3f;
                }
            }
            else
            {
                if (m_ShipMesh.transform.rotation.z != _NormalZRotation)
                {
                    if (m_turnRotation > _NormalZRotation)
                    {
                        m_ShipMesh.transform.Rotate(0, 0, -2);
                        m_turnRotation -= 2f;
                    }

                    else if (m_turnRotation < _NormalZRotation)
                    {
                        m_ShipMesh.transform.Rotate(0, 0, 2);
                        m_turnRotation += 2f;
                    }
                }
            }

            if (FireTimer == 0)
            {
                if (fire)
                {
                    if (_Weapon == "Rocket")
                    {
                        FireMaxTimer = 0.8f;
                        shipProjectileDamage = 10f;
                        m_Projectile = _ProjectileArray[0];
                        _ProjectileSpeed = 100f;
                        _ProjectileLifeTime = 8f;
                    }
                    else if (_Weapon == "Gatling")
                    {
                        FireMaxTimer = 0.1f;
                        shipProjectileDamage = 1f;
                        m_Projectile = _ProjectileArray[1];
                        _ProjectileSpeed = 180f;
                        _ProjectileLifeTime = 4f;
                    }
                    else if (_Weapon == "Cannon")
                    {
                        FireMaxTimer = 0.4f;
                        shipProjectileDamage = 8f;
                        m_Projectile = _ProjectileArray[2];
                        _ProjectileSpeed = 140f;
                        _ProjectileLifeTime = 6f;
                    }

                    FireTimer = FireMaxTimer;
                    GameObject projectile = Instantiate(m_Projectile, transform.position, transform.rotation);
                    projectile.GetComponent<scr_Projectile>().Instantiation(m_Planet, gameObject, shipProjectileDamage, _ProjectileSpeed, _ProjectileLifeTime);
                    projectile.GetComponent<Light>().color = colour;
                }
            }
            else
            {
                FireTimer -= Time.deltaTime;

                if (FireTimer < 0)
                {
                    FireTimer = 0;
                }
            }

            if (left || right)
            {
                m_Horizontal = Input.GetAxis(m_InputAxis);
            }

            if (m_ShipSpeed != m_MaxShipSpeed)
            {
                m_ShipSpeed += Time.deltaTime * 8f;

                if (m_ShipSpeed > m_MaxShipSpeed)
                {
                    m_ShipSpeed = m_MaxShipSpeed;
                }
            }

            else
            {
                if (m_ShipSpeed > 0)
                {
                    m_ShipSpeed -= Time.deltaTime * 16f;
                }

                else
                {
                    m_ShipSpeed = 0;
                }
            }

            transform.Rotate(0, 0, m_Horizontal * 1.6f);

            m_Horizontal = Mathf.Lerp(m_Horizontal, 0, 0.05f);

            if (m_Planet != null)
            {
                transform.RotateAround(m_Planet.position, transform.right, m_ShipSpeed * Time.deltaTime);
            }
            else
            {
                transform.position += transform.up * m_ShipSpeed * 2 * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Gate" && checkpoint == 3)
        {
            if (lap == 3)
            {
                finished = true;
            }

            else
            {
                lap++;
                checkpoint = 0;
            }
        }   

        else if (other.tag == "CheckPoint")
        {
            checkpoint++;
        }
    }

    public void Damage(float damage)
    {
        shipHealth -= damage;
    }

    public void Deactivate()
    {
        m_ShipMesh.SetActive(false);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        _GameController.GetComponent<scr_GameControl>().RemoveOneFromAliveShips();
        isDead = true;
        this.enabled = false;
        _CanvasGroup.SetActive(false);
    }

    public void ChangeCamera(Rect rect)
    {
        PlayerCamera.rect = new Rect(rect);
    }

    public void SetFOV(int fov)
    {
        PlayerCamera.fieldOfView = fov;
    }

    public void ChangeCamera(bool b)
    {
        PlayerCamera.enabled = b;
    }

    public void MoveCanvas(int x, int y)
    {
        _CanvasGroup.transform.position = new Vector3(x, y, 0);
    }

    public void SetPosition(int pos)
    {
        PositionRace = pos;

        string text;

        if (pos == 1)
        {
            text = "1st";
        }
        else if (pos == 2)
        {
            text = "2nd";
        }
        else if (pos == 3)
        {
            text = "3rd";
        }
        else
        {
            text = "4th";
        }

        textPos.text = text;
    }

    public int GetLap()
    {
        return lap;
    }
    public int GetPos()
    {
        return PositionRace;
    }

    public void Victory()
    {
        Deactivate();
    }
}
