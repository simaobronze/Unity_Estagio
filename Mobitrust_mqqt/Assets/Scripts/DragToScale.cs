using UnityEngine;

public class DragToScale : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private float scaleSpeed = 0.1f;

    // Armazena a posição do handler no frame anterior para detectar o movimento
    private Vector3 previousPosition;
    // Define se o handler está sendo arrastado
    private bool isDragging = false;
    // Calcula e mantém o offset inicial entre o handler e o targetObject
    private Vector3 initialOffset;

    private void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object não foi atribuído.");
            return;
        }
        // Calcula o offset inicial a partir das posições atuais
        initialOffset = transform.position - targetObject.transform.position;
    }

    private void Update()
    {
        // Exemplo de detecção de input VR para arraste
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (!isDragging)
            {
                isDragging = true;
                previousPosition = transform.position;
            }
            else
            {
                Vector3 currentPosition = transform.position;
                Vector3 delta = currentPosition - previousPosition;

                // Se o movimento for para a direita e para baixo, aumenta a escala
                if (delta.x > 0 && delta.y < 0)
                {
                    targetObject.transform.localScale += Vector3.one * scaleSpeed;
                    Debug.Log("Aumentando escala.");
                }
                // Se o movimento for para a esquerda e para cima, diminui a escala
                else if (delta.x < 0 && delta.y > 0)
                {
                    targetObject.transform.localScale -= Vector3.one * scaleSpeed;
                    Debug.Log("Diminuindo escala.");
                }

                previousPosition = currentPosition;
            }
        }
        else
        {
            // Quando deixar de arrastar, reseta a flag
            isDragging = false;
        }
    }

    private void LateUpdate()
    {
        // Se o handler não estiver sendo arrastado, garante que ele fique próximo ao target
        if (!isDragging && targetObject != null)
        {
            transform.position = targetObject.transform.position + initialOffset;
        }
    }
}
