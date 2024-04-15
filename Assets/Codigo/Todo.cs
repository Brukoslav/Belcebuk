using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Todo : MonoBehaviour
{

    public GameObject moscaPrefab;
    public float morlacos;
    public float moscasVivas;
    public float moscasMuertas;
    public TMP_Text morlacosText;
    public TMP_Text errorText;
    public float precioMosca1;
    public TMP_Text botonInmunidad;
    public TMP_Text botonQueso;
    public TMP_Text botonSizeInsecticida;
    public float inmunidad;
    public Gas gasSc;
    // Start is called before the first frame update
    void Start()
    {
        inmunidad = 0;
        errorText.text = "Press 1 to summon a fly";
        moscasVivas = 0;
        moscasMuertas = 0;
        gasSc = GameObject.FindGameObjectWithTag("Gas").GetComponent<Gas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1)) {
            if (morlacos >= precioMosca1) {
                Instantiate(moscaPrefab, new Vector3(Random.Range(-4f, 4f), Random.Range(8f, 15f), Random.Range(-4f, 4f)), Quaternion.identity);
                morlacos -= precioMosca1;
                errorText.text = "";
                moscasVivas++;
            }
            else
            {
                errorText.text = "You don't have enough food to summon a fly";
            }
        }
        morlacosText.text = "Food: " + Formato(morlacos)+"\nLiving flies: " + moscasVivas + "\nDead flies: " + moscasMuertas;
    }

    public string Formato(float x) {
        if (x > 1000000000) {
            return Mathf.Round(x / 100000000) / 10 + "B";
        } else if (x>1000000) {
            return Mathf.Round(x / 100000) / 10 + "M";
        } else if(x>1000) {
            return Mathf.Round(x / 100) / 10 + "K";
        }
        return Mathf.Round(x)+"";
    }

    public void MejorarInmunidad() {
        float precioInmunidad = 10 + 1/(1-inmunidad);
        
        if (morlacos >= precioInmunidad) {
            morlacos -= precioInmunidad;
            inmunidad += 0.05f;
            precioInmunidad = 10 + 1 / (1 - inmunidad);
            botonInmunidad.text = "Immunity: " + inmunidad * 100 + "%\n-" + precioInmunidad + " food";
        } else {
            errorText.text = "You don't have enough food to improve inmmunity";
        }
    }

    public void ReducirSizeInsecticida() {
        if (morlacos >= gasSc.precioReducirGas) {
            gasSc.tamanoMaximo *= 0.98f;
            morlacos-=gasSc.precioReducirGas;
            gasSc.precioReducirGas = gasSc.precioReducirGas * 2;
            gasSc.ActualizarBotonSize();
        } else {
            errorText.text = "You don't have enough food to reduce insectice size";
        }
        
    }
}
