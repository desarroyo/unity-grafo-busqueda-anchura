using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace DigitalRuby.AnimatedLineRenderer
{
    [RequireComponent(typeof(LineRenderer))]
    public class AnimatedLineRenderer : MonoBehaviour
    {
        [Tooltip("Canvas")]
        public Canvas CurrentCanvas;

        [Tooltip("The minimum distance that must be in between line segments (0 for infinite). Attempts to make lines with distances smaller than this will fail.")]
        public float MinimumDistance = 0.0f;

        [Tooltip("Seconds that each new line segment should animate with")]
        public float SecondsPerLine = 0.1f;

        [Tooltip("Start color for the line renderer since Unity does not provider a getter for this")]
        public Color StartColor = Color.white;

        [Tooltip("End color for the line renderer since Unity does not provide a getter for this")]
        public Color EndColor = Color.white;

        [Tooltip("Start line width")]
        public float StartWidth = 2.0f;

        [Tooltip("End line width")]
        public float EndWidth = 2.0f;

        [Tooltip("Sort layer name")]
        public string SortLayerName = "Default";

        [Tooltip("Order in sort layer")]
        public int OrderInSortLayer = 1;


        public List<Letra> alLetras = null;
        private int sigLetra = 1;


        public struct Letra
        {
            public String letra;
            public Vector3 posicion;
            public TextMesh txtLetra;
            public TextMesh txtLetraPunto;
        }

        public struct QueueItem
        {
            public Vector3 Position;
            public float ElapsedSeconds;
            public float TotalSeconds;
            public float TotalSecondsInverse;
        }

        private LineRenderer lineRenderer;
        private readonly Queue<QueueItem> queue = new Queue<QueueItem>();
        private QueueItem prev;
        private QueueItem current;
        public QueueItem? lastQueued;
        private int index = -1;
        private float remainder;


        

            private void ProcessCurrent()
        {
            if (current.ElapsedSeconds == current.TotalSeconds)
            {
                if (queue.Count == 0)
                {                   


                    return;
                }
                else
                {


                    prev = current;
                    current = queue.Dequeue();
                    if (++index == 0)
                    {
                        lineRenderer.SetVertexCount(1);
                        StartPoint = current.Position;
                        current.ElapsedSeconds = current.TotalSeconds = current.TotalSecondsInverse = 0.0f;
                        lineRenderer.SetPosition(0, current.Position);

                        return;
                    }
                    else
                    {

                        lineRenderer.SetVertexCount(index + 1);
                    }

//                    crearPivote(index);
                }
            }

            float newElapsedSeconds = current.ElapsedSeconds + Time.deltaTime + remainder;
            if (newElapsedSeconds > current.TotalSeconds)
            {

                remainder = newElapsedSeconds - current.TotalSeconds;
                current.ElapsedSeconds = current.TotalSeconds;

            }
            else
            {

                remainder = 0.0f;
                current.ElapsedSeconds = newElapsedSeconds;



            }
            current.ElapsedSeconds = Mathf.Min(current.TotalSeconds, current.ElapsedSeconds + Time.deltaTime);
            float lerp = current.TotalSecondsInverse * current.ElapsedSeconds;
            EndPoint = Vector3.Lerp(prev.Position, current.Position, lerp);
            print(current.Position);

         




            lineRenderer.SetPosition(index, EndPoint);
        }

        private String Number2String(int number, bool isCaps)

        {

            Char c = (Char)((isCaps ? 65 : 97) + (number - 1));

            return c.ToString();

        }

        private void crearPivote(int indice, Vector3 p)
        {

            Letra l = new Letra();
            String letra = "v";
            if(indice > 0)
            {
                letra = Number2String(indice, false);
            }


            /* Dibujar Letra*/
            GameObject UItextGO = new GameObject(letra);
            UItextGO.transform.SetParent(this.transform);

            RectTransform trans = UItextGO.AddComponent<RectTransform>();
            trans.anchoredPosition = new Vector2(p.x - 0.1f, p.y + 0.3f);

            //Text text = UItextGO.AddComponent<Text>();

            // TextMesh textMesh = new TextMesh();
            TextMesh text = UItextGO.AddComponent<TextMesh>();


            text.text = letra;
            text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text.fontSize = 28;
            text.characterSize = 0.33f;
            text.anchor = TextAnchor.LowerRight;
            text.color = Color.black;

            l.txtLetra = text;

            /* Dibujar Punto*/
            UItextGO = new GameObject(letra+"-O");
            UItextGO.transform.SetParent(this.transform);

            trans = UItextGO.AddComponent<RectTransform>();
            trans.anchoredPosition = new Vector2(p.x, p.y);
            
            //Text text = UItextGO.AddComponent<Text>();

            // TextMesh textMesh = new TextMesh();
            text = UItextGO.AddComponent<TextMesh>();


            text.text = "O";
            text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text.fontSize = 28;
            text.characterSize = 0.33f;
            text.anchor = TextAnchor.MiddleCenter;
            text.fontStyle = FontStyle.Bold;
            text.color = Color.black;


            l.txtLetraPunto = text;

            l.letra = letra;
            l.posicion = p;
            alLetras.Add(l);


            l.txtLetra.color = Color.green;
            l.txtLetraPunto.color = Color.green;


        }

        private void Start()
        {
            alLetras = new List<Letra>();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetVertexCount(0);
        }

        private void Update()
        {
            ProcessCurrent();
            if (!Resetting)
            {
                lineRenderer.SetColors(StartColor, EndColor);
                lineRenderer.SetWidth(StartWidth, EndWidth);
                lineRenderer.sortingLayerName = SortLayerName;
                lineRenderer.sortingOrder = OrderInSortLayer;
            }
        }

        private IEnumerator ResetAfterSecondsInternal(float seconds, Action callback)
        {
            if (seconds <= 0.0f)
            {
                Reset();                
                if (callback != null)
                {
                    callback();
                }
                yield break;
            }

            float elapsedSeconds = 0.0f;
            float secondsInverse = 1.0f / seconds;
            Color c1 = new Color(StartColor.r, StartColor.g, StartColor.b, 1.0f);
            Color c2 = new Color(EndColor.r, EndColor.g, EndColor.b, 1.0f);

            while (elapsedSeconds < seconds)
            {
                float a = 1.0f - (secondsInverse * elapsedSeconds);
                elapsedSeconds += Time.deltaTime;
                c1.a = a;
                c2.a = a;
                lineRenderer.SetColors(c1, c2);
                yield return new WaitForSeconds(0.01f);
            }
            Reset();
            if (callback != null)
            {
                callback();
            }
        }

        /// <summary>
        /// Enqueue a line segment, using SecondsPerLine for the duration
        /// </summary>
        /// <param name="pos">Position of the line segment</param>
        /// <returns>True if enqueued, false if not</returns>
        public bool Enqueue(Vector3 pos)
        {
            return Enqueue(pos, SecondsPerLine);
        }

        /// <summary>
        /// Enqueue a line segment
        /// </summary>
        /// <param name="pos">Position of the line segment</param>
        /// <param name="duration">Duration the line segment should take to become the full length</param>
        /// <returns>True if enqueued, false if not</returns>
        public bool Enqueue(Vector3 pos, float duration)
        {
            bool cerrarGrafo = false;

            for (int i = 0; i < alLetras.Count; i++)
            {
                Letra l = alLetras[i];
                

                if (Math.Abs(l.posicion.x - (pos.x)) < 1 && Math.Abs(l.posicion.y - (pos.y)) < 1)
                {
                    print(l.posicion.x);
                    cerrarGrafo = true;
                }

                if(i < alLetras.Count-1)
                {
                    l.txtLetra.color = Color.black;
                    l.txtLetraPunto.color = Color.black;
                }
            }
            

            /*

            if (true)
            {
                return false;
            }
            */

            if (Resetting)
            {
                return false;
            }
            else if (MinimumDistance > 0.0f && lastQueued != null)
            {
                Vector3 prevPos = lastQueued.Value.Position;
                float distance = Vector3.Distance(prevPos, pos);

                if (distance < MinimumDistance)
                {
                    return false;
                }
                else
                {
                    // Debug.LogFormat("Distance between {0} and {1}: {2}", prevPos, pos, distance);
                }
            }

            float durationSeconds = Mathf.Max(0.0f, duration);
            QueueItem item = new QueueItem
            {
                Position = pos,
                TotalSecondsInverse = (durationSeconds == 0.0f ? 0.0f : 1.0f / durationSeconds),
                TotalSeconds = durationSeconds,
                ElapsedSeconds = 0.0f
            };
            queue.Enqueue(item);
            lastQueued = item;

            if (!cerrarGrafo) {
                crearPivote(sigLetra++, pos);
            }
            

            return true;
        }

        /// <summary>
        /// Reset the line renderer, setting everything back to defaults
        /// </summary>
        public void Reset()
        {
            index = -1;
            prev = current = new QueueItem();
            lastQueued = null;
            if (lineRenderer != null)
            {
                lineRenderer.SetVertexCount(0);
            }
            remainder = 0.0f;
            queue.Clear();
            Resetting = false;
            StartPoint = EndPoint = Vector3.zero;
        }

        /// <summary>
        /// Reset the line renderer, fading out smoothly over seconds
        /// </summary>
        /// <param name="seconds">Seconds to fade out</param>
        public void ResetAfterSeconds(float seconds)
        {
            ResetAfterSeconds(seconds, null, null);
        }

        /// <summary>
        /// Reset the line renderer, fading out smoothly over seconds
        /// </summary>
        /// <param name="seconds">Seconds to fade out</param>
        /// <param name="endPoint">Force the end point to a new value (optional)</param>
        public void ResetAfterSeconds(float seconds, Vector3? endPoint)
        {
            ResetAfterSeconds(seconds, endPoint, null);
        }

        /// <summary>
        /// Reset the line renderer, fading out smoothly over seconds
        /// </summary>
        /// <param name="seconds">Seconds to fade out</param>
        /// <param name="callback">Callback when the fade out is finished</param>
        public void ResetAfterSeconds(float seconds, Vector3? endPoint, Action callback)
        {
            Resetting = true;
            if (endPoint != null)
            {
                current.Position = endPoint.Value;
                if (index > 0)
                {
                    lineRenderer.SetPosition(index, endPoint.Value);
                }
            }
            StartCoroutine(ResetAfterSecondsInternal(seconds, callback));
        }

        /// <summary>
        /// The Unity Line Renderer
        /// </summary>
        public LineRenderer LineRenderer { get { return lineRenderer; } }

        /// <summary>
        /// The current line start point (point index 0)
        /// </summary>
        public Vector3 StartPoint { get; set; }

        /// <summary>
        /// The current line end point (point index n - 1)
        /// </summary>
        public Vector3 EndPoint { get; set; }

        /// <summary>
        /// Is the line renderer in the process of resetting?
        /// </summary>
        public bool Resetting { get; private set; }
    }
}