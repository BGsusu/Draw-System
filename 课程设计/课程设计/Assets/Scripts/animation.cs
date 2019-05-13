using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
public class animation : MonoBehaviour
{
    [SerializeField] private Slider slider1;
    [SerializeField] private Slider slider2;
    [SerializeField] private Slider slider3;
    [SerializeField] private Slider slider4;
    [SerializeField] private InputField name;
    [SerializeField] private InputField slogen;
    [SerializeField] private Button draw;
    [SerializeField] private Image ani;

    public bool start_play;
    public bool comuni = false;
    public int result = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        start_play = this.transform.GetComponent<observer>().start_play;
        result = this.transform.GetComponent<observer>().result;
        if (start_play)
        {
            StartCoroutine("Show");
        }

    }

    private IEnumerator Show()
    {
        ani.transform.Rotate(Vector3.forward * 100);
        yield return new WaitForSeconds(3);
        switch (result)
        {
            case 0:
                ani.transform.localEulerAngles = new Vector3(0, 0, (float)158.42);
                break;
            case 1:
                ani.transform.localEulerAngles = new Vector3(0, 0, (float)15.2);
                break;
            case 2:
                ani.transform.localEulerAngles = new Vector3(0, 0, (float)104.4);
                break;
            case 3:
                ani.transform.localEulerAngles = new Vector3(0, 0, (float)207.9);
                break;
            case 4:
                ani.transform.localEulerAngles = new Vector3(0, 0, (float)290.9);
                break;
            default:
                break;
        }
        comuni = true;
        slider1.enabled = true;
        slider2.enabled = true;
        slider3.enabled = true;
        slider4.enabled = true;
        name.enabled = true;
        slogen.enabled = true;
        draw.enabled = true;
        StopAllCoroutines();
    }
}
