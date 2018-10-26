﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace DigitalRuby.AnimatedLineRenderer
{
    public class DemoScriptDraw : MonoBehaviour
    {
        public AnimatedLineRenderer AnimatedLine;
        public AnimatedLineRenderer AnimatedLine2;
        private bool firing;
        private int currentFiring = 0;
        public int currentPaso = 0;
        public int currentPasoArbol = 0;
        public int currentLetra = 0;
        public GameObject areaArbol;
        public GameObject areaCola;
        public GameObject titulo;
        public List<AnimatedLineRenderer.Letra> alLetrasArbol = null;
        public List<AnimatedLineRenderer.Letra> alLetrasCola = null;
        public List<AnimatedLineRenderer.Letra> alLetrasColaTache = null;
        public List<AnimatedLineRenderer.Letra> alCola = null;
        private int currentCola = 0;

        private bool stop = false;

        Color colorAzul = new Color();
        Color colorVerde = new Color();
        Color colorDarkGray = new Color();
        float posIni = -2f;

        Vector3 posIniArbol = new Vector3(0f, 0f);
        Vector3 posIniCola = new Vector3(0f, 0f);


        private void Start()
        {

            ColorUtility.TryParseHtmlString("#009688", out colorVerde);
            ColorUtility.TryParseHtmlString("#0000B4", out colorAzul);
            ColorUtility.TryParseHtmlString("#455A64", out colorDarkGray);

            alLetrasArbol = new List<AnimatedLineRenderer.Letra>();
            alLetrasCola = new List<AnimatedLineRenderer.Letra>();
            alLetrasColaTache = new List<AnimatedLineRenderer.Letra>();
            alCola = new List<AnimatedLineRenderer.Letra>();


        }



        public void DrawManual(AnimatedLineRenderer.Letra letra, AnimatedLineRenderer.Letra letraRelacion, Vector3 source, Vector3 target)
        {

            LineRenderer line;
            float startWidth = 0.00f;
            float endWidth = 0.00f;
            Color c1 = Color.green;
            Color c2 = Color.green;
           // c1 = new Color(c1.r, c1.g, c1.b, 0.3f);
            // c2 = new Color(c2.r, c2.g, c2.b, 0.3f);

            int lengthOfLineRenderer = 2;

            float t = Time.time;
            GameObject lineaObject = new GameObject("_"+ letra.letra+""+letraRelacion.letra);
            lineaObject.transform.parent = AnimatedLine.lineaDinamica.transform;
            LineRenderer lineRenderer = lineaObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 2.00f;
            lineRenderer.endWidth = 2.00f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.widthMultiplier = 0.2f;
            lineRenderer.positionCount = lengthOfLineRenderer;
            lineRenderer.useWorldSpace = false;
            lineRenderer.transform.localScale = new Vector3(1.0F, 1.0F, 0);
            lineRenderer.sortingOrder = -10;

            // A simple 2 color gradient with a fixed alpha of 1.0f.
            float alpha = 0.6f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
                );
            lineRenderer.colorGradient = gradient;


            lineRenderer.SetPosition(0, source);
            lineRenderer.SetPosition(1, target);


            letra.alLineas.Add(lineRenderer);



            //LineRenderer lineRenderer = GetComponent<LineRenderer>();
            /*
            for (int i = 0; i < lengthOfLineRenderer; i++)
            {
                //StartCoroutine(LateStartLine(0.3f*i, lineRenderer, i, t, source, target));
                //StopCoroutine("LateStartLine");

            }
            */
        }

        /*
        IEnumerator LateStartLine(float waitTime, LineRenderer lineRenderer, int i, float t, Vector3 source, Vector3 target)
        {
            yield return new WaitForSeconds(waitTime);
            print("ok");
            print(i);
            lineRenderer.SetPosition(i, source, target);
            
        }
        */
            IEnumerator LateStart(float waitTime)
        {

            
            //Your Function You Want to Call
            yield return new WaitForSeconds(waitTime);

            for (int i = 0; i < AnimatedLine.alLetras.Count; i++)
            {
                

                print("-> " + AnimatedLine.alLetras[i].letra);

                for (int h = 0; h < AnimatedLine.alLetras[i].alLetrasRelaciones.Count; h++)
                {
                    print("     ->" + AnimatedLine.alLetras[i].alLetrasRelaciones[h].letra);

                    DrawManual(AnimatedLine.alLetras[i], AnimatedLine.alLetras[i].alLetrasRelaciones[h], AnimatedLine.alLetras[i].posicion, AnimatedLine.alLetras[i].alLetrasRelaciones[h].posicion);
                }

            }
            
            AnimatedLine.lineaDinamica.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));
            AnimatedLine.lineaDinamica.transform.position = new Vector3(1.2f , -2.0f , 0);


            //DrawManual();

            /*
            for (int i = 0; i < AnimatedLine.alLetras.Count; i++)
            {

                //AnimatedLine2.Enqueue(posIni);
                AnimatedLine2.Enqueue(AnimatedLine.alLetras[i].posicion);
                //currentFiring = i;
                //firing = false;
                //Fire(posIni, AnimatedLine.alLetras[i].posicion, currentFiring);
                
                
                AnimatedLine2.EndPoint = new Vector3(10, -10);
                AnimatedLine2.StartPoint = new Vector3(10, -10);

                AnimatedLineRenderer.QueueItem current = new AnimatedLineRenderer.QueueItem();

                current.Position = new Vector3(10, -10);

                yield return new WaitForSeconds(waitTime);
                pos = AnimatedLine.alLetras[i].posicion;

                AnimatedLine2.lastQueued = current;
                AnimatedLine2.Enqueue(pos);

                
            }
        */

        }

        static int SortByTxtLetra(AnimatedLineRenderer.Letra l1, AnimatedLineRenderer.Letra l2)
        {
            return l1.letra.CompareTo(l2.letra);
        }


        IEnumerator muestraLineasLetra(AnimatedLineRenderer.Letra letra, int current)
        {
            print(Time.time);

            letra.alLetrasRelaciones.Sort(SortByTxtLetra);

            for (int h = 0; h < letra.alLineas.Count; h++)
            {
                print("     ->" + letra.alLetrasRelaciones[h].letra);

                letra.txtLetraPunto.color = Color.green;
                yield return new WaitForSeconds(0.6f);
                letra.alLineas[h].enabled = true;

                /* Dibujar Letra*/
                GameObject UItextGO = new GameObject("y" + h);
                UItextGO.transform.SetParent(this.transform);

                RectTransform trans = UItextGO.AddComponent<RectTransform>();
                trans.anchoredPosition = new Vector2(-16.5f + (current * 1f), posIni - ((h + 1) * 1f));

                //Text text = UItextGO.AddComponent<Text>();

                // TextMesh textMesh = new TextMesh();
                TextMesh text = UItextGO.AddComponent<TextMesh>();


                text.text = letra.alLetrasRelaciones[h].letra;
                text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                text.fontSize = 20;
                text.characterSize = 0.33f;
                text.anchor = TextAnchor.LowerRight;
                text.color = colorDarkGray;

            }


            print(Time.time);
        }


        IEnumerator muestraLineasArbol(AnimatedLineRenderer.Letra letra, int current)
        {
            print(Time.time);
            print(letra.letra);
            yield return new WaitForSeconds(0.6f);

            if (current == 0)
            {

                alLetrasCola[current].txtLetra.text = letra.letra;
                alCola.Add(letra);
                


                /* Dibujar Letra*/
                GameObject UItextGO = new GameObject("a_" + letra.letra);
                UItextGO.transform.SetParent(areaArbol.transform);

                RectTransform trans = UItextGO.AddComponent<RectTransform>();
                trans.anchoredPosition = posIniArbol;

                //Text text = UItextGO.AddComponent<Text>();

                // TextMesh textMesh = new TextMesh();
                TextMesh text = UItextGO.AddComponent<TextMesh>();


                text.text = letra.letra;
                text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                text.fontSize = 20;
                text.characterSize = 0.33f;
                text.anchor = TextAnchor.LowerRight;
                text.color = colorDarkGray;


                AnimatedLineRenderer.Letra l = new AnimatedLineRenderer.Letra();

                l.letra = letra.letra;
                l.txtLetra = text;
                l.posicion = text.transform.position;

                alLetrasArbol.Add(l);
            }

            else
            {

                float separacionX = 2.2f;
                float separacionY = 2.0f;

                for (int h = 0; h < letra.alLetrasRelaciones.Count; h++)
                {
                    print("     ->" + letra.alLetrasRelaciones[h].letra);

                    yield return new WaitForSeconds(0.6f);

                    if (alCola.Contains(letra.alLetrasRelaciones[h]))
                    {
                        continue;
                    }
                        

                    /* Dibujar Letra*/
                    GameObject UItextGO = new GameObject("y" + h);
                    UItextGO.transform.SetParent(areaArbol.transform);

                    RectTransform trans = UItextGO.AddComponent<RectTransform>();
                    trans.anchoredPosition = new Vector2((-(letra.alLetrasRelaciones.Count * separacionX) * 0.5f) + (h * separacionX) + (separacionX * 0.5f), posIniArbol.y - (current * 0.5f) - separacionY);

                    //Text text = UItextGO.AddComponent<Text>();

                    // TextMesh textMesh = new TextMesh();
                    TextMesh text = UItextGO.AddComponent<TextMesh>();


                    text.text = letra.alLetrasRelaciones[h].letra;
                    text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                    text.fontSize = 20;
                    text.characterSize = 0.33f;
                    text.anchor = TextAnchor.LowerRight;
                    text.color = colorDarkGray;

                    AnimatedLineRenderer.Letra l = new AnimatedLineRenderer.Letra();

                    l.letra = letra.letra;
                    l.txtLetra = text;
                    l.posicion = text.transform.position;

                    alLetrasArbol.Add(l);

                    alLetrasCola[alCola.Count].txtLetra.text = letra.alLetrasRelaciones[h].letra;
                    alCola.Add(letra.alLetrasRelaciones[h]);

                }
                alLetrasCola[currentCola].txtLetraPunto.text = "X";
                currentCola++;

            }
            print(Time.time);
        }

        public void OnButtonArbol()
        {

            try
            {
                if(currentPasoArbol == 0)
                {
                    StartCoroutine(muestraLineasArbol(AnimatedLine.alLetras[currentPasoArbol], currentPasoArbol));
                }
                else
                {
                    StartCoroutine(muestraLineasArbol(alCola[currentCola], currentPasoArbol));
                }
                

                currentPasoArbol++;
            }
            catch (Exception ex) { }
            
        }

            public void OnButtonLetra()
        {
            limpiarSeleccionLinea();
            try
            {
                StartCoroutine(muestraLineasLetra(AnimatedLine.alLetras[currentLetra], currentLetra));

                currentLetra++;
            }
            catch (Exception ex) {

                contruyeCola();
            }
        }

        private void contruyeCola()
        {

            for (int h = 0; h < AnimatedLine.alLetras.Count; h++)
            {
                print("     ->" + AnimatedLine.alLetras[h].letra);

                ////yield return new WaitForSeconds(0.6f);
                //letra.alLineas[h].enabled = true;

                /* Dibujar Letra*/
                GameObject UItextGO = new GameObject("cola_" + h);
                UItextGO.transform.SetParent(areaCola.transform);

                RectTransform trans = UItextGO.AddComponent<RectTransform>();
                trans.anchoredPosition = new Vector2(posIniCola.x + (h * 1f), posIniCola.y);

                //Text text = UItextGO.AddComponent<Text>();

                // TextMesh textMesh = new TextMesh();
                TextMesh text = UItextGO.AddComponent<TextMesh>();


                text.text = "_";
                text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                text.fontSize = 20;
                text.characterSize = 0.33f;
                text.anchor = TextAnchor.MiddleCenter;
                text.color = Color.black;


                UItextGO = new GameObject("colaT_" + h);
                UItextGO.transform.SetParent(areaCola.transform);

                trans = UItextGO.AddComponent<RectTransform>();
                trans.anchoredPosition = new Vector2(posIniCola.x + (h * 1f), posIniCola.y);

                //Text text = UItextGO.AddComponent<Text>();

                // TextMesh textMesh = new TextMesh();
                TextMesh text2 = UItextGO.AddComponent<TextMesh>();


                text2.text = "";
                text2.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                text2.fontSize = 28;
                text2.characterSize = 0.33f;
                text2.anchor = TextAnchor.MiddleCenter;
                Color cda = colorAzul;
                cda.a = 0.4f;
                text2.color = cda;


                AnimatedLineRenderer.Letra l = new AnimatedLineRenderer.Letra();

                l.letra = ""+h;
                l.txtLetra = text;
                l.txtLetraPunto = text2;
                l.posicion = text.transform.position;

                alLetrasCola.Add(l);



                UItextGO = new GameObject("colaCUADRO_" + h);
                UItextGO.transform.SetParent(areaCola.transform);

                trans = UItextGO.AddComponent<RectTransform>();
                trans.anchoredPosition = new Vector2(posIniCola.x + (h * 1f), posIniCola.y + 0.22f);

                //Text text = UItextGO.AddComponent<Text>();

                // TextMesh textMesh = new TextMesh();
                text2 = UItextGO.AddComponent<TextMesh>();


                text2.text = "□";
                text2.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                text2.fontSize = 70;
                text2.characterSize = 0.33f;
                text2.anchor = TextAnchor.MiddleCenter;
                text2.color = colorAzul;

            }

           // alLetrasCola.Add();
        }

        public void OnButton()
        {

            if (!stop)
            {
                stop = true;
                titulo.SetActive(false);
                
            }
            
            Debug.Log("Button was pressed!");

            switch (currentPaso) {

                case 0:
                    /* Dibujar Letra Inicio*/
                    GameObject UItextGO = new GameObject("inicio");
                    UItextGO.transform.SetParent(AnimatedLine.transform);

                    RectTransform trans = UItextGO.AddComponent<RectTransform>();
                    trans.anchoredPosition = new Vector3(AnimatedLine.alLetras[0].posicion.x, AnimatedLine.alLetras[0].posicion.y + 2f, 100); // AnimatedLine.alLetras[0].posicion;

                    //Text text = UItextGO.AddComponent<Text>();

                    // TextMesh textMesh = new TextMesh();
                    TextMesh text = UItextGO.AddComponent<TextMesh>();


                    text.text = "Inicio\n   ↓";
                    text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                    text.fontSize = 20;
                    text.characterSize = 0.33f;
                    text.anchor = TextAnchor.LowerRight;
                    text.color = Color.blue;



                    /***/
                    for (int i = 0; i < AnimatedLine.alLetras.Count; i++)
                    {
                        print("-> " + AnimatedLine.alLetras[i].letra);

                        for (int h = 0; h < AnimatedLine.alLetras[i].alLetrasRelaciones.Count; h++)
                        {
                            print("     ->" + AnimatedLine.alLetras[i].alLetrasRelaciones[h].letra);

                            DrawManual(AnimatedLine.alLetras[i], AnimatedLine.alLetras[i].alLetrasRelaciones[h], AnimatedLine.alLetras[i].posicion, AnimatedLine.alLetras[i].alLetrasRelaciones[h].posicion);
                        }

                    }


                    limpiarSeleccionLinea();



//                    StartCoroutine(muestraLineasLetra(AnimatedLine.alLetras[0]));
                    




                    /****/




                    

                    for (int i = 0; i < AnimatedLine.alTxtLetras.Count; i++)
                    {
                        AnimatedLine.alTxtLetras[i].transform.localScale = new Vector3(1.5F, 1.5F, 0);
                        AnimatedLine.alTxtLetrasPunto[i].transform.localScale = new Vector3(1.5F, 1.5F, 0);

                        if (i == 0)
                        {
                            AnimatedLine.alTxtLetrasPunto[i].GetComponent<TextMesh>().color = colorVerde;
                        }
                        else
                        {
                            AnimatedLine.alTxtLetrasPunto[i].GetComponent<TextMesh>().color = colorAzul;
                        }

                    }



                    AnimatedLine.transform.localScale = new Vector3(0.5F, 0.5F, 0);
                    AnimatedLine.StartWidth = 0.15f;
                    AnimatedLine.EndWidth = 0.15f;

                    AnimatedLine.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));
                    AnimatedLine.transform.position = new Vector3(1.2f + AnimatedLine.transform.position.x - ((AnimatedLine.minX) * 0.5f), -2.0f + AnimatedLine.transform.position.y - ((AnimatedLine.maxY) * 0.5f), 0);


                    print(AnimatedLine.maxX - AnimatedLine.minX);
                    print(AnimatedLine.maxY - AnimatedLine.minY);
                    print(AnimatedLine.minX);
                    print(AnimatedLine.maxY);
                    print(AnimatedLine.minY);




                    



                    for (int i = 0; i < AnimatedLine.alLetras.Count; i++)
                    {

                        /* Dibujar Letra*/
                        UItextGO = new GameObject("x" + i);
                        UItextGO.transform.SetParent(this.transform);

                        trans = UItextGO.AddComponent<RectTransform>();
                        trans.anchoredPosition = new Vector2(-16.5f + (i * 1f), posIni + 0f);

                        //Text text = UItextGO.AddComponent<Text>();

                        // TextMesh textMesh = new TextMesh();
                        text = UItextGO.AddComponent<TextMesh>();


                        text.text = AnimatedLine.alLetras[i].letra;
                        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                        text.fontSize = 20;
                        text.characterSize = 0.33f;
                        text.anchor = TextAnchor.LowerRight;
                        text.color = Color.black;


                        //for (int j = 0; j < AnimatedLine.alLetras[i].alLetrasRelaciones.Count; j++)
                        //{
                        //    /* Dibujar Letra*/
                        //    UItextGO = new GameObject("y" + j);
                        //    UItextGO.transform.SetParent(this.transform);

                        //    trans = UItextGO.AddComponent<RectTransform>();
                        //    trans.anchoredPosition = new Vector2(-16.5f + (i * 1f), posIni - ((j + 1) * 1f));

                        //    //Text text = UItextGO.AddComponent<Text>();

                        //    // TextMesh textMesh = new TextMesh();
                        //    text = UItextGO.AddComponent<TextMesh>();


                        //    text.text = AnimatedLine.alLetras[i].alLetrasRelaciones[j].letra;
                        //    text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                        //    text.fontSize = 20;
                        //    text.characterSize = 0.33f;
                        //    text.anchor = TextAnchor.LowerRight;
                        //    text.color = Color.grey;
                        //}

                    }

                    break;

                case 1:

                    break;
            }


            
            

            //alTxtLetras.Add(UItextGO);
            currentPaso++;

        }

        private void limpiarSeleccionLinea()
        {
            for (int i = 0; i < AnimatedLine.alLetras.Count; i++)
            {
                print("-> " + AnimatedLine.alLetras[i].letra);

                AnimatedLine.alLetras[i].txtLetraPunto.color = colorAzul;

                for (int h = 0; h < AnimatedLine.alLetras[i].alLineas.Count; h++)
                {
                    print("     ->" + AnimatedLine.alLetras[i].alLetrasRelaciones[h].letra);

                    AnimatedLine.alLetras[i].alLineas[h].enabled = false;
                }

            }
        }

        public bool Fire(Vector3 source, Vector3 target, int cF)
        {
            if (firing)
            {
                return false;
            }

            firing = true;

            AnimatedLine2.Enqueue(source);
            AnimatedLine2.Enqueue(target);
            return true;
        }

        private void Update()
        {
            if (!stop)
            {
                if (AnimatedLine == null)
                {
                    return;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Vector3 pos = Input.mousePosition;

                    pos = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, AnimatedLine.transform.position.z));

                    /*Omitir Area del Boton Terminar*/
                    if( !(pos.x > 10.72 && pos.y < -7.82))
                        AnimatedLine.Enqueue(pos);
                }
                else if (Input.GetKey(KeyCode.R))
                {
                    AnimatedLine.ResetAfterSeconds(0.5f, null);
                }
                
            }

            if (Input.GetMouseButtonUp(1))
            {


                StartCoroutine(LateStart(1f));
                //StopCoroutine("LateStart");
                //AnimatedLine.ResetAfterSeconds(0.5f, null);
            }
        }
    }
}