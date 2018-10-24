using UnityEngine;
using System.Collections;

namespace DigitalRuby.AnimatedLineRenderer
{
    public class DemoScriptDraw : MonoBehaviour
    {
        public AnimatedLineRenderer AnimatedLine;
        public AnimatedLineRenderer AnimatedLine2;
        private bool firing;
        private int currentFiring = 0;

        private bool stop = false;

        private void Start()
        {

        }



        public void DrawManual(Vector3 source, Vector3 target)
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
            GameObject lineaObject = new GameObject("x"+ Mathf.Sin(1 + t));
            lineaObject.transform.parent = AnimatedLine.transform;
            LineRenderer lineRenderer = lineaObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 3.00f;
            lineRenderer.endWidth = 3.00f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.widthMultiplier = 0.2f;
            lineRenderer.positionCount = lengthOfLineRenderer;
            lineRenderer.useWorldSpace = false;

            // A simple 2 color gradient with a fixed alpha of 1.0f.
            float alpha = 0.4f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
                );
            lineRenderer.colorGradient = gradient;


            lineRenderer.SetPosition(0, source);
            lineRenderer.SetPosition(1, target);



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

                    DrawManual(AnimatedLine.alLetras[i].posicion, AnimatedLine.alLetras[i].alLetrasRelaciones[h].posicion);
                }

            }
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

        public void OnButton()
        {
            stop = true;
            Debug.Log("Button was pressed!");
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
                else if (Input.GetMouseButtonUp(1))
                {


                    StartCoroutine(LateStart(1f));
                    //StopCoroutine("LateStart");
                    //AnimatedLine.ResetAfterSeconds(0.5f, null);
                }
            }
        }
    }
}