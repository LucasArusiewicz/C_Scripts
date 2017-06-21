/*

-=-=- Requisitos -=-=-

1- Definir Tag da Camera para "MainCamera".
2- Definir Tag do Objeto pricipal do jogador para "Player".
3- Criar um Objeto chamado "Centro" na raiz do Objedo do jogador.
4- Ajustar Objeto Centro conforme Objeto do Jogadora para ficar no foco da camera.

*/

using UnityEngine;

public class CameraPrincipal : MonoBehaviour
{

    //Limites para angulo de visão da camera
    private const float ANGULO_MIN_Y = 5.0f;
    private const float ANGULO_MAX_Y = 50.0f;

    //Limites para zoom
    private const float ZOOM_MIN = 0.6f;
    private const float ZOOM_MAX = 5.0f;

    private Transform player;
    private Transform camTransform;

    private float distancia = 2.0f;
    private float atualX = 0.0f;
    private float atualY = 10.0f;
    private float sensibilidadeX = 4.0f;
    private float sensibilidadeY = 1.0f;

    [Range(ZOOM_MIN, ZOOM_MAX)][SerializeField]float zoomPadrao = 2;

    private Vector3 centro;

    //Busta objetos ao iniciar
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        camTransform = Camera.main.transform;
    }

    private void Update()
    {
        centro = player.FindChild("Centro").position;

        //Quando aperta o scroll do Mouse é precionado ajusta zoom para medida padrão
        if (Input.GetMouseButton(2))
        {
            distancia = Mathf.Lerp(distancia, zoomPadrao, 0.2f);
        }


        //Verifica scrollWheel do Mouse e realiza ZoomOut
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            distancia -= 0.2f;
        }

        //Verifica scrollWheel do Mouse e realiza ZoomIn
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            distancia += 0.2f;
        }

        //Ajusta zoom para faixa limite
        distancia = Mathf.Clamp(distancia, ZOOM_MIN, ZOOM_MAX);

        //Quando o botão esquerdo do Mouse for precionado e movimentado, altera angulo da camera
        if (Input.GetMouseButton(1))
        {
            atualX += Input.GetAxis("Mouse X") * sensibilidadeX;
            atualY += -Input.GetAxis("Mouse Y") * sensibilidadeY;

            //Ajusta angulo para faixa limite
            atualY = Mathf.Clamp(atualY, ANGULO_MIN_Y, ANGULO_MAX_Y);
        }
    }

    //Após realizar operações atualiza posição da cemera
    private void LateUpdate()
    {
        AtualizaCamera();
    }

    private void AtualizaCamera()
    {
        Vector3 dir = new Vector3(0, 0, -distancia);

        //Define rotação da camera
        Quaternion rotacao = Quaternion.Euler(atualY, atualX, 0);

        //Calcula posição nos eixo X Y Z
        camTransform.position = player.position + rotacao * dir;

        //Aponta camera para o centro do objeto
        camTransform.LookAt(centro);
    }
}
