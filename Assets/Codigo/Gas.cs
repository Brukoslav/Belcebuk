using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Gas : MonoBehaviour
{
    public float tiempoParaCrecer = 2f; // Tiempo en segundos para alcanzar el tamaño máximo
    public float tiempoParaAchicar = 2f; // Tiempo en segundos para volver al tamaño mínimo
    public Vector3 tamanoMaximo = new Vector3(1, 1, 1); // Define el tamaño máximo de la esfera
    public float pausaEntreCiclos = 10f; // Tiempo de pausa entre ciclos de crecer y achicar
    public Todo todoSc;
    public float precioReducirGas;
    void Start() {
        todoSc = GameObject.FindGameObjectWithTag("GameController").GetComponent<Todo>();
        // Iniciar la esfera con tamaño cero
        transform.localScale = Vector3.zero;
        precioReducirGas = 100;
        // Iniciar la corutina para crecer, achicar y pausar
        StartCoroutine(CrecerYAchicarConPausa());
        ActualizarBotonSize();
    }

    IEnumerator CrecerYAchicarConPausa() {
        while (true) // Crea un bucle infinito para repetir el proceso
        {
            // Crecer
            yield return CambiarTamaño(transform.localScale, tamanoMaximo, tiempoParaCrecer);

            EliminarMoscasEnRadio();
           
            // Achicar
            yield return CambiarTamaño(tamanoMaximo, Vector3.zero, tiempoParaAchicar);

            // Pausar antes de repetir el ciclo
            yield return new WaitForSeconds(pausaEntreCiclos);
            transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(0, 10f), Random.Range(-10f, 10f));
            // Incrementa tamaño máximo
            tamanoMaximo = tamanoMaximo * 1.02f;
            ActualizarBotonSize();
        }
    }

    public void EliminarMoscasEnRadio() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, tamanoMaximo.x);
        foreach (var hitCollider in hitColliders) {
            if (hitCollider.gameObject.transform.tag == "Mosca" && Random.value >= todoSc.inmunidad) {
                todoSc.moscasMuertas++;
                todoSc.moscasVivas++;
                Destroy(hitCollider.gameObject);
            }
        }
    }

    public void ActualizarBotonSize() {
        todoSc.botonSizeInsecticida.text = "Reduce insecticide size: " + Mathf.Round(tamanoMaximo.x*10)/10 + "\n-"+ precioReducirGas+" food";
    }

    IEnumerator CambiarTamaño(Vector3 desde, Vector3 hasta, float duracion) {
        float tiempoInicio = Time.time;
        while (Time.time < tiempoInicio + duracion) {
            float t = (Time.time - tiempoInicio) / duracion;
            transform.localScale = Vector3.Lerp(desde, hasta, t);
            yield return null;
        }

        // Asegurar que el tamaño final sea exactamente el esperado
        transform.localScale = hasta;
    }
}
