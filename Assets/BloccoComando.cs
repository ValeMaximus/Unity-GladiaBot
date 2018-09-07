using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloccoComando : MonoBehaviour {

    #region Variabili
    [HideInInspector] public GameObject[] blocchi = null;
    //public Transform puntoDiAggancio;
    //public Transform puntoDiPartenza;

    [Header("Prima scelta")]
    //la parte dove si chiede se il blocco sara if o azione
    [SerializeField] private GameObject primaSelezione;    
    //le due parti dei comandi
    [SerializeField] private GameObject bloccoIF;
    [SerializeField] private GameObject bloccoAzz;

    #region Variabili Scelta IF

    [Header("IF: Scelta chi")]
    //blocco if
    [SerializeField] private Dropdown selezioneChi;        
    [SerializeField] private GameObject parteMe;
    [SerializeField] private GameObject parteNemico;

    [Header("Me: Scelta cosa")]
    //se scegli me -> bisogna scegliere cosa: vita/scudo
    [SerializeField] private Dropdown parteMe_cosa;        
    [SerializeField] private GameObject parteMe_parteVita; 
    [SerializeField] private GameObject parteMe_parteScudo;

    [Header("Nemico: Scelta cosa")]
    [SerializeField] private Dropdown parteNemico_cosa;    
    [SerializeField] private GameObject parteNemico_parteVita; 
    [SerializeField] private GameObject parteNemico_parteScudo;
    [SerializeField] private GameObject parteNemico_parteDistanza;

    #endregion

    #region Scelta Azione

    [SerializeField] private Dropdown azioneQuale;
    [SerializeField] private GameObject parteMovimento;

    #endregion

    #region Variabili per l'esterno

    [HideInInspector] public bool primaScelta = false; //se IF:false o AZIONE:true;
    [HideInInspector] public bool aChi = false; // IF:(se a me o nemico)
    [HideInInspector] public short cosa = 0; //IF:[ME:(vita:0/scudo:1) ... NEMICO(vita:0/scudo:1/distanza:3)]
                                           //AZ:[rgen.vita:0 / rgn.scudo:1 / movimento:2]
    [HideInInspector] public short valore_drp = 25; //IF:{ME:[vari valori] / NEMICO:[vari valori]}
    //[HideInInspector] public int aNemico_cosa = 0;
    //[HideInInspector] public int aNemico_valore = 0;

    //public int cosa { get { Debug.Log("...cosa get" + cosa_); return cosa_; } set { cosa_ = value; Debug.Log("...cosa set" + value); } }
    //public int valore_drp { get {Debug.Log("...valore get:" + valore_drp_); return valore_drp_; } set { valore_drp_ = value; Debug.Log("...valore set:" + value); } }

    #endregion

    #endregion

    //decidere quale strada prendere: if / azione
    #region Prima scelta

    public void SceltaIf_bottone()
    {
        bloccoIF.SetActive(true);
        bloccoAzz.SetActive(false);

        //disattivare la scelta appena fatta
        primaSelezione.SetActive(false);
        primaScelta = false;
    }

    public void SceltaAzione_bottone()
    {
        bloccoAzz.SetActive(true);
        bloccoIF.SetActive(false);

        //disattivare la scelta appena fatta
        primaSelezione.SetActive(false);
        primaScelta = true;
    }

    #endregion

    //funzioni per la scelta IF
    #region Scelta IF

    //per il dropdown d sceta chi
    public void A_Chi_dropDown(int valore)
    {
        switch(valore)
        {
            case 0: //parte me attiva
                aChi = false;
                parteMe.SetActive(true);
                parteNemico.SetActive(false);
                break;
            case 1: //parte nemico attiva
                aChi = true;
                parteMe.SetActive(false);
                parteNemico.SetActive(true);
                break;
        }
    }

    //funzioni per la scelta IF->ME
    #region Scelta IF ME

    public void ParteMe_ACosa_DropDown(int valore)
    {
        //valore puo essere... vita:0 / scudo:1
        switch(valore)
        {
            case 0:
                cosa = 0;
                parteMe_parteVita.SetActive(true);
                parteMe_parteScudo.SetActive(false);
                valore_drp = 25;//inizializzazzione
                break;
            case 1:
                cosa = 1;
                parteMe_parteVita.SetActive(false);
                parteMe_parteScudo.SetActive(true);
                valore_drp = 0;//inizializzazzione
                break;
        }
    }
    

    #endregion

    //funzioni per la scelta IF->NEMICO
    #region Scelta IF NEMICO

    public void ParteNemico_aCosa_DropDown(int value)
    {
        //se e: vita:0 / scudo:1 / distanza:2
        switch(value)
        {
            case 0:
                cosa = 0;
                parteNemico_parteVita.SetActive(true);
                parteNemico_parteScudo.SetActive(false);
                parteNemico_parteDistanza.SetActive(false);
                break;
            case 1:
                cosa = 1;
                parteNemico_parteVita.SetActive(false);
                parteNemico_parteScudo.SetActive(true);
                parteNemico_parteDistanza.SetActive(false);
                valore_drp = 0; //perche cosi lo setto subito
                break;
            case 2:
                cosa = 2;
                parteNemico_parteVita.SetActive(false);
                parteNemico_parteScudo.SetActive(false);
                parteNemico_parteDistanza.SetActive(true);
                valore_drp = 2; //idem set
                break;
        }
    }

    #endregion

    #endregion

    #region Scelta Azione

    public void QualeAzione_Dropdown(int valore)
    {
        //AZ:[rgen.vita:0 / rgn.scudo:1 / movimento:2]
        switch(valore)
        {
            case 0:
                //vita rigenerazione
                cosa = 0;
                parteMovimento.SetActive(false);
                break;
            case 1:
                //scudo rigenerazione
                cosa = 1;
                parteMovimento.SetActive(false);
                break;
            case 2:
                //movimento
                cosa = 2;
                parteMovimento.SetActive(true);
                valore_drp = 0; //inizializzazione per "avanti"
                break;
            case 3:
                //sparo
                cosa = 3;
                parteMovimento.SetActive(false);
                break;
        }
    }

    public void CheTipoDiMovimento(int value)
    {
        //avanti:0 / indietro:1
        valore_drp = (short)value;
    }

    #endregion

    #region Scelta IF ME COSA GENERICO

    public void ParteMe_Vita(int valore)
    {
        switch (valore)
        {
            case 0:
                valore_drp = 25;
                break;
            case 1:
                valore_drp = 50;
                break;
            case 2:
                valore_drp = 75;
                break;
            case 3:
                valore_drp = 100;
                break;
        }
    }

    public void ParteMe_Scudo(int valore)
    {
        Debug.Log(valore);
        switch (valore)
        {
            case 0:
                valore_drp = 0;
                break;
            case 1:
                valore_drp = 25;
                break;
            case 2:
                valore_drp = 50;
                break;
            case 3:
                valore_drp = 75;
                break;
            case 4:
                valore_drp = 100;
                break;
        }
    }

    public void ParteNemico_Distanza(int valore)
    {
        switch(valore)
        {
            case 0:
                valore_drp = 2;
                break;
            case 1:
                valore_drp = 4;
                break;
            case 2:
                valore_drp = 6;
                break;
            case 3:
                valore_drp = 8;
                break;
            case 4:
                valore_drp = 9;
                break;
        }
    }

    #endregion

    public void CreaNuovoBlocco()
    {
        GameObject[] new_blocci = new GameObject[blocchi.Length + 1];
        for (int i = 0; i < blocchi.Length; i++)
            new_blocci[i] = blocchi[i];

        new_blocci[blocchi.Length] = Instantiate(StartPanel.bloccoComando);
        new_blocci[blocchi.Length].transform.SetParent(transform, false);

        blocchi = new_blocci;

        //blocchi[blocchi.Length - 1].GetComponent<LineRenderer>().SetPosition(0, Camera.main.ScreenToWorldPoint(puntoDiPartenza.position));
        StartCoroutine(MovimentoBlocco());
    }

    private IEnumerator MovimentoBlocco()
    {
        while (!Input.GetMouseButton(0))
        {
            blocchi[blocchi.Length - 1].transform.position = 
                Input.mousePosition - Vector3.up * 100; 
    //        blocchi[blocchi.Length - 1].GetComponent<LineRenderer>().SetPosition(1,
    //            blocchi[blocchi.Length - 1].GetComponent<BloccoComando>().puntoDiAggancio.position);
    //            //Camera.main.ScreenToWorldPoint(blocchi[blocchi.Length - 1].GetComponent<BloccoComando>().puntoDiAggancio.position));
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
