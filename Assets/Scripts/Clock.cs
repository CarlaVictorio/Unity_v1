using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Clock : MonoBehaviour
{
    [Tooltip("tiemo inicial en segundos")]
    public int tiempoInicial;


    [Tooltip("escala del tiempo del reloj")]


    [Range(-10.0f,10.0f)]
    public float escalaTiempo = 1;

    private Text timeText;
    private float tiempoDelFrameConTimeScale = 0f;
    private float tiempoAMostrarEnSegundos = 0f;
    private float escalaDeTiempoAlPausar, escalaDeTiempoInicial;
    private bool pausado = false;

    // Start is called before the first frame update
    void Start()
    {
        escalaDeTiempoInicial = escalaTiempo;
        timeText = GameObject.Find("TimeText").GetComponent<Text>();

        tiempoAMostrarEnSegundos = tiempoInicial;
        actualizarReloj(tiempoInicial);
    }

    // Update is called once per frame
    void Update()
    {
        //tiempo de cada frame considerando la escala de tiempo
        tiempoDelFrameConTimeScale = Time.deltaTime*escalaTiempo;
        //acumula tiempo transcurrido para mostrar en el reloj
        tiempoAMostrarEnSegundos -= tiempoDelFrameConTimeScale;
        actualizarReloj(tiempoAMostrarEnSegundos);
        CheckIfGameOver(tiempoAMostrarEnSegundos);
    }

    public void actualizarReloj (float tiempoEnSegundos){
        int minutos = 0;
        int segundos = 0;
        string textoDelReloj;

        if (tiempoEnSegundos < 0)//para asegurar que el tiempo no es negativo
        {
            tiempoEnSegundos = 0;
        } else if (tiempoEnSegundos <= 10){
            timeText.color = Color.red;
        } 

        //Calcular minutos y segundos
        minutos = (int)tiempoEnSegundos/60;
        segundos = (int)tiempoEnSegundos%60;

        //Crear la cadena de caracteres con dos digitos separados por dos puntos
        textoDelReloj = minutos.ToString("00") + ":" + segundos.ToString("00");
        timeText.text = textoDelReloj;
    }

    private void CheckIfGameOver(float tiempoEnSegundos)
    {
        //Check if food point total is less than or equal to zero.
        if (tiempoEnSegundos <= 1)
        {
            //Call the GameOver function of GameManager.
            GameManager.instance.GameOver(0);
        }
    }
}
