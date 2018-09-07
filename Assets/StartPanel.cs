using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : MonoBehaviour {

    [HideInInspector] public GameObject[] blocchiComando = null;
    [SerializeField] private GameObject blocco_comando;
    public static GameObject bloccoComando;
    public static SetIstruzioni setIstruzioni;

    // Use this for initialization
    void Start()
    {
        bloccoComando = blocco_comando;
        setIstruzioni = new SetIstruzioni(true, true, 1, 1) { iniz = true };
    }

    public void PulsanteStart()
    {
        Compila(blocchiComando, setIstruzioni);
        AttivaOggetti();
        gameObject.SetActive(false);
    }

    public void Compila(GameObject[] comand, SetIstruzioni setInst)
    {
        //if(comand.Length > 1)
        //{
        //    for(int i = 0; i < comand.Length; i++)
        //        for(int j = 1; j < comand.Length; j++)
        //            if(comand[i].transform.position.x > comand[j].transform.position.x)
        //            {
        //                GameObject mom = comand[i];
        //                comand[i] = comand[j];
        //                comand[j] = mom;
        //            }
        //}

        foreach(GameObject ogg in comand)
        {
            BloccoComando blo = ogg.GetComponent<BloccoComando>();
            setInst.AggiungiIstruzione(new SetIstruzioni(blo.primaScelta, blo.aChi, blo.cosa, blo.valore_drp));
            Debug.Log("----------" + blo.valore_drp);

            if (blo.blocchi != null)
                Compila(blo.blocchi, setInst.istruzioni[setInst.istruzioni.Length - 1]);
        }

        //for(int i = 0; i < comand.Length; i++)
        //{
        //    BloccoComando blo = comand[i].GetComponent<BloccoComando>();
        //}
    }

    private void AttivaOggetti()
    {
        BotAI[] botAIs = FindObjectsOfType<BotAI>();
        Nemico[] nemicos = FindObjectsOfType<Nemico>();

        for (int i = 0; i < botAIs.Length; i++)
            botAIs[i].enabled = true;
        for (int i = 0; i < nemicos.Length; i++)
            nemicos[i].enabled = true;
    }

    public void CreaNuovoBlocco()
    {
        GameObject[] new_blocci = new GameObject[blocchiComando.Length + 1];
        for (int i = 0; i < blocchiComando.Length; i++)
            new_blocci[i] = blocchiComando[i];

        new_blocci[blocchiComando.Length] = Instantiate(bloccoComando);
        new_blocci[blocchiComando.Length].transform.SetParent(transform, false);

        blocchiComando = new_blocci;
        //blocchiComando[blocchiComando.Length - 1].GetComponent<LineRenderer>()
        //    .SetPosition(0, Input.mousePosition);

        StartCoroutine(MovimentoBlocco());
    }

    private IEnumerator MovimentoBlocco()
    {
        while(!Input.GetMouseButton(0))
        {
            blocchiComando[blocchiComando.Length - 1]
                .transform.position = Input.mousePosition - Vector3.up * 100;
            //blocchiComando[blocchiComando.Length - 1].GetComponent<LineRenderer>()
            //    .SetPosition(1, Input.mousePosition);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

}

public class SetIstruzioni
{
    public SetIstruzioni(bool _if_azione, bool _me_nemico, short _cosa, short _valore)
    {
        if_azione = _if_azione;
        me_nemico = _me_nemico;
        cosa = _cosa;
        valore = _valore;

        n_instruzioni = 0;
        istruzioni = null;

        //Debug.Log(if_azione + " " + me_nemico + " " + cosa + " " + valore);
    }

    public void AggiungiIstruzione(SetIstruzioni inst)
    {
        SetIstruzioni[] new_set = new SetIstruzioni[n_instruzioni + 1];
        for (short i = 0; i < n_instruzioni; i++)
            new_set[i] = istruzioni[i];
        new_set[n_instruzioni] = inst;
        istruzioni = new_set;
        n_instruzioni++;
    }

    public bool iniz = false;

    public bool if_azione; // if:false / azione:true
    public bool me_nemico; // me:false / nemico:true

    public short cosa; // IF[ME(vita:0/scudo:1) ... NEMICO(vita:0/scudo:1/distanza:2)]
                     // azione(rigen.vita:0/rigen.scudo:1/movimento:2)
    public short valore; //vedi dropdown

    public short n_instruzioni;
    public SetIstruzioni[] istruzioni;
}