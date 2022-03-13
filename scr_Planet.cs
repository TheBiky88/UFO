using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Planet : MonoBehaviour
{
    [SerializeField] private List<Material> planetMaterials = new List<Material>();
    private MeshRenderer mr;
    // Start is called before the first frame update
    void Start()
    {
        mr = gameObject.GetComponent<MeshRenderer>();

        int mat = Random.Range(0, planetMaterials.Count);

        mr.material = planetMaterials[mat];
    }
}
