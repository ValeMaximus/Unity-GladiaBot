using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAI : MonoBehaviour {

    [SerializeField] float vita = 100f;
    [SerializeField] float vita_iniziale = 100f;
    [SerializeField] float vita_rigen = 5f;
    [SerializeField] float scudo = 100f;
    [SerializeField] float scudo_iniziale = 100f;
    [SerializeField] float scudo_rigen = 5f;
    [SerializeField] float danni = 20f; //da implementare
    [SerializeField] float velocita = 2f;

	// Use this for initialization
	void Start () {
        this.enabled = false;
	}

    Nemico nemico_piu_vicino = null;
    float distanza = Mathf.Infinity;

	// Update is called once per frame
	void Update () {
        Debug.Log("Uptade BotAI");

        distanza = Mathf.Infinity;
        foreach(Nemico enemy in FindObjectsOfType<Nemico>())
            if((enemy.transform.position - transform.position).magnitude < distanza)
            {
                distanza = (enemy.transform.position - transform.position).magnitude;
                nemico_piu_vicino = enemy;
            }

        transform.LookAt(nemico_piu_vicino.transform.position); //guardare sempre il nemico piu vicino

       // Debug.Log(StartPanel.setIstruzioni.istruzioni[0].if_azione + " - " + StartPanel.setIstruzioni.istruzioni[0].istruzioni[0].if_azione);

        if(StartPanel.setIstruzioni.n_instruzioni > 0)
            Intelligenza(StartPanel.setIstruzioni.istruzioni);
    }

    //con i commenti con ricorsione  
    private bool Intelligenza(SetIstruzioni[] setIstruzionis)
    {
        foreach (SetIstruzioni istruzione in setIstruzionis)
        {
            bool condizioneAvverata = false;
            string debug = "";
            if (istruzione.if_azione)
            {
                //azione
                debug += "è un' azione :";
                switch (istruzione.cosa)
                {
                    case 0:
                        //rigen. vita
                        debug += " rigen. vita";
                        Rigeneazione(ref vita, vita_iniziale, vita_rigen);
                        break;
                    case 1:
                        //rigen. scudo
                        debug += " rigen. scudo";
                        Rigeneazione(ref scudo, scudo_iniziale, scudo_rigen);
                        break;
                    case 2:
                        //movimento
                        debug += " movimento";
                        Movimento(istruzione.valore); //delego alla funzione Movimento() la scelta della dirazione
                        break;
                    case 3:
                        //spara
                        debug += " spara";
                        nemico_piu_vicino.PrendiDanno(danni); //delego alla funzione passiva del nemico
                        break;
                }
                Debug.Log(debug);
                return true; //se è un' azione ritorna true per segnalare che per questo frame l'azione è fatto
            }
            else
            {
                //condizione
                debug += "è una condizione:";
                if (istruzione.me_nemico)
                {
                    //nemico
                    debug += " se il nemico:";

                    switch (istruzione.cosa)
                    {
                        case 0:
                            //vita
                            if (istruzione.valore == 100)
                            {
                                debug += " ha la vita al massimo";
                                if (nemico_piu_vicino.Vita > 99.99f)
                                {
                                    debug += " (true) ciao";
                                    condizioneAvverata = true;
                                }
                            }
                            else
                            {
                                debug += " ha la vita sotto il " + istruzione.valore;
                                if (nemico_piu_vicino.Vita < istruzione.valore)
                                {
                                    debug += " (true)";
                                    condizioneAvverata = true;
                                }
                            }
                            break;
                        case 1:
                            //scudo
                            if (istruzione.valore == 100)
                            {
                                debug += " ha lo scudo al massimo";
                                if (nemico_piu_vicino.Scudo > 99.99f)
                                {
                                    debug += " (true)";
                                    condizioneAvverata = true;
                                }
                            }
                            else
                            {
                                debug += " ha lo scudo sotto il " + istruzione.valore;
                                if (nemico_piu_vicino.Scudo <= istruzione.valore)
                                {
                                    debug += "(true)";
                                    condizioneAvverata = true;
                                }
                            }
                            break;
                        case 2:
                            //distanza
                            if (istruzione.valore == 9)
                            {
                                debug += " è piu distante di 8 metri";
                                if (distanza > 8)
                                {
                                    debug += "(true)";
                                    condizioneAvverata = true;
                                }
                            }
                            else
                            {
                                debug += " è meno distante di " + istruzione.valore;
                                if (distanza < istruzione.valore)
                                {
                                    debug += " (true)";
                                    condizioneAvverata = true;
                                }
                            }
                            break;
                    }
                }
                else
                {
                    //me
                    debug += " se io:";
                    switch (istruzione.cosa)
                    {
                        case 0:
                            //vita
                            if (istruzione.valore == 100)
                            {
                                debug += " ho la vita al massimo";
                                if (vita > 99.99f)
                                {
                                    debug += " (true)";
                                    condizioneAvverata = true;
                                }
                            }
                            else
                            {
                                debug += " ho la vita sotto il " + istruzione.valore;
                                if (vita < istruzione.valore)
                                {
                                    debug += " (true)";
                                    condizioneAvverata = true;
                                }
                            }
                            break;
                        case 1:
                            //scudo
                            if (istruzione.valore == 100)
                            {
                                debug += " ha lo scudo al massimo";
                                if (scudo > 99.99f)
                                {
                                    debug += " (true)";
                                    condizioneAvverata = true;
                                }
                            }
                            else
                            {
                                debug += " ha lo scudo sotto il " + istruzione.valore;
                                if (scudo <= istruzione.valore)
                                {
                                    debug += "(true)";
                                    condizioneAvverata = true;
                                }
                            }
                            break;
                    }
                }
            }
            Debug.Log(debug);
            if(condizioneAvverata)
            {
                if(istruzione.istruzioni != null && Intelligenza(istruzione.istruzioni))
                {
                    //se arriviamo qua significa che questo ramo è riuscito a concludere un'azione
                    return true;
                }
                //altrimenti si continua a vedere gli altri rami successivi (paralleli) a questo
            }
        }
        return false;
    }

    void Movimento(int valore)
    {
        switch(valore)
        {
            case 0:
                //avanti
                transform.Translate(Vector3.forward * Time.deltaTime * velocita, Space.Self);
                break;
            case 1:
                //indietro
                transform.Translate(Vector3.back * Time.deltaTime * velocita, Space.Self);
                break;
        }
    }

    void Rigeneazione(ref float parte, float max, float vel)
    {
        //è applicabile sia alla rigenerazione scudo che della vita
        if(parte < max)
        {
            parte += Time.deltaTime * vel;
            if (parte > max)
                parte = max;
        }
    }

    //con i commenti senza ricorsione
    /*
    //private void Intelligenza(SetIstruzioni[] setIstruzionis)
    //{
    //    foreach(SetIstruzioni istruzione in setIstruzionis)
    //    {
    //        string debug = "";
    //        if(istruzione.if_azione)
    //        {
    //            //azione
    //            debug += "è un' azione :";
    //            switch(istruzione.cosa)
    //            {
    //                case 0:
    //                    //rigen. vita
    //                    debug += " rigen. vita";
    //                    break;
    //                case 1:
    //                    //rigen. scudo
    //                    debug += " rigen. scudo";
    //                    break;
    //                case 2:
    //                    //movimento
    //                    debug += " movimento";
    //                    break;
    //            }
    //        }
    //        else
    //        {
    //            //condizione
    //            debug += "è una condizione:";
    //            if (istruzione.me_nemico)
    //            {
    //                //nemico
    //                debug += " se il nemico:";

    //                switch(istruzione.cosa)
    //                {
    //                    case 0:
    //                        //vita
    //                        if(istruzione.valore == 100)
    //                        {
    //                            debug += " ha la vita al massimo";
    //                            if(nemico_piu_vicino.vita > 99.99f)
    //                            {
    //                                debug += " (true)";
    //                            }
    //                        }
    //                        else
    //                        {
    //                            debug += " ha la vita sotto il " + istruzione.valore;
    //                            if(nemico_piu_vicino.vita < istruzione.valore)
    //                            {
    //                                debug += " (true)";
    //                            }
    //                        }
    //                        break;
    //                    case 1:
    //                        //scudo
    //                        if(istruzione.valore == 100)
    //                        {
    //                            debug += " ha lo scudo al massimo";
    //                            if (nemico_piu_vicino.scudo > 99.99f)
    //                            {
    //                                debug += " (true)";
    //                            }
    //                        }
    //                        else
    //                        {
    //                            debug += " ha lo scudo sotto il " + istruzione.valore;
    //                            if(nemico_piu_vicino.scudo <= istruzione.valore)
    //                            {
    //                                debug += "(true)";
    //                            }
    //                        }
    //                        break;
    //                    case 2:
    //                        //distanza
    //                        if(istruzione.valore == 9)
    //                        {
    //                            debug += " è piu distante di 8 metri";
    //                            if(distanza > 8)
    //                            {
    //                                debug += "(true)";
    //                            }
    //                        }
    //                        else
    //                        {
    //                            debug += " è meno distante di " + istruzione.valore;
    //                            if(distanza < istruzione.valore)
    //                            {
    //                                debug += " (true)";
    //                            }
    //                        }
    //                        break;
    //                }
    //            }
    //            else
    //            {
    //                //me
    //                debug += " se io:";
    //                switch(istruzione.cosa)
    //                {
    //                    case 0:
    //                        //vita
    //                        if (istruzione.valore == 100)
    //                        {
    //                            debug += " ho la vita al massimo";
    //                            if (vita > 99.99f)
    //                            {
    //                                debug += " (true)";
    //                            }
    //                        }
    //                        else
    //                        {
    //                            debug += " ho la vita sotto il " + istruzione.valore;
    //                            if (vita < istruzione.valore)
    //                            {
    //                                debug += " (true)";
    //                            }
    //                        }
    //                        break;
    //                    case 1:
    //                        //scudo
    //                        if (istruzione.valore == 100)
    //                        {
    //                            debug += " ha lo scudo al massimo";
    //                            if (scudo > 99.99f)
    //                            {
    //                                debug += " (true)";
    //                            }
    //                        }
    //                        else
    //                        {
    //                            debug += " ha lo scudo sotto il " + istruzione.valore;
    //                            if (scudo <= istruzione.valore)
    //                            {
    //                                debug += "(true)";
    //                            }
    //                        }
    //                        break;
    //                }
    //            }
    //        }
    //        Debug.Log(debug);
    //    }
    //}
    */

}
