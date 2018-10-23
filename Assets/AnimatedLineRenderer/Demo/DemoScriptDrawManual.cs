using UnityEngine;
using System.Collections;

namespace DigitalRuby.AnimatedLineRenderer
{
    public class DemoScriptDrawManual : MonoBehaviour
    {
        public AnimatedLineRenderer AnimatedLine;

        private void Start()
        {

            StartCoroutine(LateStart(1f));
            
        }


        IEnumerator LateStart(float waitTime)
        {
            

            //Your Function You Want to Call
            Vector3 pos;
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(waitTime);
                pos = new Vector3(1, -10* i);


                AnimatedLine.Enqueue(pos);
            }

        }

        private void Update()
        {
            if (AnimatedLine == null)
            {
                return;
            }
            else if (Input.GetMouseButton(0))
            {

            }
            else if (Input.GetKey(KeyCode.R))
            {
                AnimatedLine.ResetAfterSeconds(0.5f, null);
            }
        }
    }
}