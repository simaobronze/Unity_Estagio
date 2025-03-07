using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class LuchaController : MonoBehaviour
{
    // Velocidade de movimento do NPC
    public float velocidade = 3.0f;
    // Raio dentro do qual o NPC irá se movimentar
    public float raioMovimento = 10.0f;
    // Tempo de espera após alcançar o destino antes de escolher um novo
    public float tempoEspera = 2.0f;
    // Velocidade de rotação do NPC
    public float speedRotation = 5.0f;

    private Vector3 destino;

    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        // Inicia a corrotina de movimento
        StartCoroutine(MovimentoAleatorio());
    }

    IEnumerator MovimentoAleatorio()
    {
        while (true)
        {
            // Gera uma direção aleatória dentro de uma esfera e zera o eixo Y para manter no plano horizontal
            Vector3 direcaoAleatoria = Random.insideUnitSphere * raioMovimento;
            direcaoAleatoria.y = 0;
            // Define o destino como a posição atual mais a direção aleatória
            destino = transform.position + direcaoAleatoria;

            // Enquanto o NPC não estiver próximo do destino
            while (Vector3.Distance(transform.position, destino) > 0.1f)
            {
                // Calcula a direção para o destino
                Vector3 direcao = (destino - transform.position).normalized;

                // Se a direção for válida, calcula a rotação alvo e aplica uma rotação suave
                if (direcao != Vector3.zero)
                {
                    Quaternion rotacaoAlvo = Quaternion.LookRotation(direcao);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, speedRotation * Time.deltaTime);
                }

                _animator.SetFloat("Speed", direcao.magnitude);

                // Move o NPC em direção ao destino
                transform.position = Vector3.MoveTowards(transform.position, destino, velocidade * Time.deltaTime);
                yield return null;
            }

            // Aguarda um tempo antes de definir um novo destino
            yield return new WaitForSeconds(tempoEspera);
        }
    }
}
