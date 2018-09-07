using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nemico : MonoBehaviour {

    [SerializeField] private float vita = 100f;
    [SerializeField] private float vita_iniziale = 100f;
    [SerializeField] private float scudo = 100f;
    [SerializeField] private float scudo_iniziale = 100f;

    public float Vita { get { return vita; } }
    public float Scudo { get { return scudo; } }

	// Use this for initialization
	void Start () {
        this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PrendiDanno(float colpo)
    {
        vita -= colpo * Time.deltaTime;
        if(vita <= 0)
        {
            //se è morto
            gameObject.SetActive(false);
        }
    }
}
