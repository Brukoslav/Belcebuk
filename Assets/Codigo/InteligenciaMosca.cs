using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InteligenciaMosca : MonoBehaviour
{
    private List<GameObject> comida;
    public Vector3 direccion;
    public float velocidad;
    public float cambioDireccionTiempo; //Cada cuanto tiempo cambia de direccion
    public float atraccion;     //A que punto la mosca es mas atraida por la comida que por lo aleatorio
    public float vecesCambiadas;
    public float cuandoBuscaComida;
    private bool llego; //Si la mosca alcanzo una fuente de comida
    public Todo todoSc;
    public 

    // Start is called before the first frame update
    void Start()
    {
        todoSc = GameObject.FindGameObjectWithTag("GameController").GetComponent<Todo>();
        comida = GameObject.FindGameObjectsWithTag("Comida").ToList();
        vecesCambiadas = 0;
        cuandoBuscaComida = 10;
        llego = false;
        StartCoroutine(CambiarDireccionPeriodicamente());
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!llego) {
            transform.Translate(direccion * velocidad * Time.deltaTime);
            if (transform.position.y < 0.5f) {
                transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            }
        } else {
            todoSc.morlacos += 0.01f;
        }
        
    }

    public Vector3 Direccion() {
        // La dirección es una componente aleatoria más una componente en dirección a la comida más cercana
        Vector3 random = Random.onUnitSphere;
        Vector3 direccionComida = FindClosestPosition(comida, transform.position);
        direccionComida = direccionComida - transform.position;
        if(direccionComida.magnitude < 1f) {
            llego = true;
            return new Vector3(0, 0, 0);
        }
        direccionComida = direccionComida.normalized;
        // La mosca solo se acerca a la comida luego de haber cambiado cuandoBuscaComida veces de posición
        vecesCambiadas++;
        Vector3 direccion = new Vector3(0, 0, 0);
        if (vecesCambiadas > 2*cuandoBuscaComida) {
            // Se acerca
            direccion = direccionComida;
        } else if (vecesCambiadas > cuandoBuscaComida) {
            // Se acerca
            direccion = ((1 - atraccion) * random + direccionComida * atraccion);
        } else {
            // da vueltas en el cielo
            direccion = new Vector3(random.x, 0, random.z);
            direccion = direccion.normalized;
        }

        return direccion;
    }

    IEnumerator CambiarDireccionPeriodicamente() {
        while (true) // Bucle infinito para seguir cambiando de dirección
        {
            // Genera una nueva dirección aleatoria
            // Aquí podrías definir cualquier lógica para la nueva dirección dependiendo de tus necesidades
            direccion = Direccion();

            // Espera durante el tiempo especificado antes de cambiar de dirección nuevamente
            yield return new WaitForSeconds(cambioDireccionTiempo*Random.Range(0.8f,1.2f));
        }
    }

    public Vector3 FindClosestPosition(List<GameObject> objetos, Vector3 posicion) {
        // Comprueba si la lista está vacía
        if (objetos.Count == 0) return Vector3.zero; // Retorna Vector3.zero si la lista está vacía

        // Inicializa el objeto más cercano
        GameObject closestObject = objetos[0];
        float closestDistance = Vector3.Distance(posicion, closestObject.transform.position);

        // Itera a través del resto de objetos para encontrar el más cercano
        foreach (GameObject obj in objetos) {
            float currentDistance = Vector3.Distance(posicion, obj.transform.position);
            if (currentDistance < closestDistance) {
                closestDistance = currentDistance;
                closestObject = obj;
            }
        }

        // Retorna la posición del objeto más cercano
        return closestObject.transform.position;
    }

}
