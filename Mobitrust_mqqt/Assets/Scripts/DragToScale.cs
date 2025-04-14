using UnityEngine;

public class DragToScaleOVR : MonoBehaviour
{
    // Objeto que será escalado (atribua via Inspetor)
    [SerializeField] private Transform objetoASerEscalado;

    // Sensibilidade que define o quanto o movimento do controlador influencia a escala
    [SerializeField] private float scaleSensitivity = 0.005f;

    // Escala mínima para o objeto que será escalado, evitando escalas menores que o desejado
    [SerializeField] private float minScale = 0.1f;

    // Componente que permite o grab via OVR
    private OVRGrabbable ovrGrabbable;

    // Flag que indica se o scaling já iniciou
    private bool isScaling = false;

    // Posição do controlador no momento inicial do grab
    private Vector3 initialControllerPos;

    // Escala inicial do objeto que será escalado no início do grab
    private Vector3 initialScale;

    // Armazena a posição e rotação iniciais do handle (quadrado) para que este não se mova
    private Vector3 initialHandlePos;
    private Quaternion initialHandleRot;

    void Awake()
    {
        // Obtém o componente OVRGrabbable deste objeto
        ovrGrabbable = GetComponent<OVRGrabbable>();
        if (ovrGrabbable == null)
        {
            Debug.LogError("OVRGrabbable não foi encontrado! Adicione-o ao handle (quadrado).");
        }
        // Salva a posição e rotação iniciais do handle
        initialHandlePos = transform.position;
        initialHandleRot = transform.rotation;
    }

    void Update()
    {
        // Se o objeto não estiver mais sendo agarrado, reseta o scaling e reposiciona o handle
        if (!ovrGrabbable.isGrabbed)
        {
            isScaling = false;
            transform.position = initialHandlePos;
            transform.rotation = initialHandleRot;
            return;
        }

        // Verifica se existe um controlador que está agarrando o objeto via OVR
        if (ovrGrabbable.isGrabbed && ovrGrabbable.grabbedBy != null)
        {
            var grabber = ovrGrabbable.grabbedBy;

            if (!isScaling)
            {
                isScaling = true;
                // Registra a posição do controlador quando o grab inicia
                initialControllerPos = grabber.transform.position;
                // Registra a escala atual do objeto que será escalado
                if (objetoASerEscalado != null)
                {
                    initialScale = objetoASerEscalado.localScale;
                }
                else
                {
                    Debug.LogError("Objeto a ser escalado não foi atribuído!");
                }
            }
            else
            {
                // Calcula a variação do movimento do controlador (usando o eixo X neste exemplo)
                Vector3 currentControllerPos = grabber.transform.position;
                Vector3 delta = currentControllerPos - initialControllerPos;
                float scaleDelta = delta.x * scaleSensitivity;

                if (objetoASerEscalado != null)
                {
                    // Calcula a nova escala somando a variação (para todos os eixos, de forma uniforme)
                    Vector3 newScale = initialScale + new Vector3(scaleDelta, scaleDelta, scaleDelta);
                    // Garante que a escala não seja menor que o valor mínimo estabelecido
                    newScale.x = Mathf.Max(newScale.x, minScale);
                    newScale.y = Mathf.Max(newScale.y, minScale);
                    newScale.z = Mathf.Max(newScale.z, minScale);

                    objetoASerEscalado.localScale = newScale;
                }
            }
        }

        // Redefine a posição e a rotação do handle para que ele não se desloque mesmo sendo agarrado
        transform.position = initialHandlePos;
        transform.rotation = initialHandleRot;
    }
}
