using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

struct info
{
    string id;
    int num;

    public int Num { get => num; set => num = value; }
    public string Id { get => id; set => id = value; }
};

public class observer : MonoBehaviour
{
    info reward1, reward2, reward3, reward4;
    [SerializeField] private Slider slider1;
    [SerializeField] private Slider slider2;
    [SerializeField] private Slider slider3;
    [SerializeField] private Slider slider4;
    [SerializeField] private InputField name;
    [SerializeField] private InputField slogen;
    [SerializeField] private Button draw;
    public bool isworking;
   public int sum = 0;
    public string path = "F:\\课程设计\\课程设计\\";
    string person_now = null;
    //不同奖项的概率,保证有50%没奖，奖品按照个数从50%里面算。
    private double percent1 = 0;
    private double percent2 = 0;
    private double percent3 = 0;
    private double percent4 = 0;

    public int result = 0;
    private float m_timer = 0;
    public bool start_play = false;
    // Start is called before the first frame update
    void Start()
    {
        FileStream cFile = new FileStream(@"" + path+"抽奖结果.txt", FileMode.OpenOrCreate);
        StreamWriter c_sw = new StreamWriter(cFile);
        c_sw.Write("本轮抽奖结果如下：\n");
        c_sw.Close();
        c_sw.Dispose();

        reward1.Id = "瓦格拉尔";
        reward2.Id = "弑骨者伯恩凯勒"; 
        reward3.Id = "蛛魔之耀";
        reward4.Id = "施坦维本-星之愤怒";

        reward1.Num = (int)slider1.value;
        reward2.Num = (int)slider2.value;
        reward3.Num = (int)slider3.value;
        reward4.Num = (int)slider4.value;

        sum = reward1.Num + reward2.Num + reward3.Num + reward4.Num;
        percent1 = (double)reward1.Num / (2 * sum);
        percent2 = (double)reward2.Num / (2 * sum);
        percent3 = (double)reward3.Num / (2 * sum);
        percent4 = (double)reward4.Num / (2 * sum);
        Debug.Log(sum);
        Debug.Log(percent1);
        Debug.Log(percent2);
        Debug.Log(percent3);
        Debug.Log(percent4);

        isworking = false;

        /*Debug.Log(reward1.Num);
        Debug.Log(reward2.Num);
        Debug.Log(reward3.Num);
        Debug.Log(reward4.Num);*/
    }

    // Update is called once per frame
    void Update()
    {

        //实现脚本间通信
        if(this.transform.GetComponent<animation>().comuni)
        {
            slider1.value = reward1.Num;
            slider2.value = reward2.Num;
            slider3.value = reward3.Num;
            slider4.value = reward4.Num;
            this.transform.GetComponent<animation>().comuni = false;
            start_play = false;
            this.transform.GetComponent<AudioSource>().Stop();
        }

        if (isworking)
        {
            if (person_now == null)
            {
                person_now = "匿名用户";
            }
            sum = reward1.Num + reward2.Num + reward3.Num + reward4.Num;
            percent1 = (double)reward1.Num / (2 * sum);
            percent2 = (double)reward2.Num / (2 * sum);
            percent3 = (double)reward3.Num / (2 * sum);
            percent4 = (double)reward4.Num / (2 * sum);
            

            slider1.enabled = false;
            slider2.enabled = false;
            slider3.enabled = false;
            slider4.enabled = false;
            name.enabled = false;
            slogen.enabled = false;
            draw.enabled = false;

            //一般而言，轮盘式的抽奖系统抽奖不是赠完为止，对于已经被抽空的奖项而言，
            //任然可能再次抽到该奖，对于这种情况而言，更一般的考虑是作为谢谢惠顾处理
             int maxrange = 2 * sum;
             result=Random.Range(1, maxrange+1);
            Debug.Log(maxrange);
            Debug.Log(result);
            if (result <= sum) result = 0;
            else if (result <= sum + reward1.Num) result = 1;
            else if (result <= sum + reward1.Num + reward2.Num) result = 2;
            else if (result <= sum + reward1.Num + reward2.Num + reward3.Num) result = 3;
            else if (result <= maxrange) result = 4;
            else
            {
                Debug.Log("抽奖错误！！！");
            }
             bool receive=changePercent(result);

            if (result==0)
            {
                FileStream cFile = new FileStream(@"" + path + "抽奖结果.txt", FileMode.Append);
                StreamWriter c_sw = new StreamWriter(cFile);
                c_sw.Write("本次抽奖结果：抱歉，"+person_now+"没中奖!\n");
                c_sw.Close();
                c_sw.Dispose();
            }
            else if(receive)
            {
                FileStream cFile = new FileStream(@"" + path + "抽奖结果.txt", FileMode.Append);
                StreamWriter c_sw = new StreamWriter(cFile);
                c_sw.Write("本次抽奖结果：恭喜！" + person_now + "在" + System.DateTime.Now + "抽到了"+result+"号奖品！"+"\n");
                c_sw.Close();
                c_sw.Dispose();
            }
            else
            {
                FileStream cFile = new FileStream(@"" + path + "抽奖结果.txt", FileMode.Append);
                StreamWriter c_sw = new StreamWriter(cFile);
                c_sw.Write("本次抽奖结果：抱歉！" + person_now + "在" + System.DateTime.Now + "抽到的" + result + "号奖品被人抽光了！" + "\n");
                c_sw.Close();
                c_sw.Dispose();
            }

            isworking = false;
        }
    }

    public void clearwords_name()
    {
        //GameObject inputfield = GameObject.Find("InputField");
        //GameObject placeholder = inputfield.transform.FindChild("Placeholder").gameObject;
        GameObject placeholder = GameObject.Find("Placeholder");
        placeholder.GetComponent<Text>().text = null;
    }

    public void clearwords_slogen()
    {
        //GameObject inputfield = GameObject.Find("InputField");
        //GameObject placeholder = inputfield.transform.FindChild("Placeholder").gameObject;
        GameObject placeholder = GameObject.Find("Placeholder1");
        placeholder.GetComponent<Text>().text = null;
    }

    public void getname_name()
    {
        GameObject txt = GameObject.Find("Inputname");
        Debug.Log(txt.GetComponent<InputField>().text);
        person_now = txt.GetComponent<InputField>().text;
        txt.GetComponent<InputField>().text = null;
    }

    public void Enter1()
    {
        GameObject text_f = GameObject.Find("奖品1文本").gameObject;
        text_f.transform.position = Input.mousePosition;
        GameObject text_s = GameObject.Find("奖品1文本1").gameObject;
        text_f.GetComponent<Text>().text=text_s.GetComponent<Text>().text= @"名称：" + reward1.Id + "\n" + "剩余人数：" + reward1.Num + "\n"+"中奖概率为："+percent1+"\n";
    }

    public void Enter2()
    {
        GameObject text_f = GameObject.Find("奖品2文本").gameObject;
        text_f.transform.position = Input.mousePosition;
        GameObject text_s = GameObject.Find("奖品2文本2").gameObject;
        text_f.GetComponent<Text>().text = text_s.GetComponent<Text>().text = @"名称：" + reward2.Id + "\n" + "剩余人数：" + reward2.Num + "\n" + "中奖概率为：" + percent2 + "\n";
    }

    public void Enter3()
    {
        GameObject text_f = GameObject.Find("奖品3文本").gameObject;
        text_f.transform.position = Input.mousePosition;
        GameObject text_s = GameObject.Find("奖品3文本3").gameObject;
        text_f.GetComponent<Text>().text = text_s.GetComponent<Text>().text = @"名称：" + reward3.Id + "\n" + "剩余人数：" + reward3.Num + "\n" + "中奖概率为：" + percent3 + "\n";
    }

    public void Enter4()
    {
            GameObject text_f = GameObject.Find("奖品4文本").gameObject;
            text_f.transform.position = Input.mousePosition;
            GameObject text_s = GameObject.Find("奖品4文本4").gameObject;
            text_f.GetComponent<Text>().text = text_s.GetComponent<Text>().text = @"名称：" + reward4.Id + "\n" + "剩余人数：" + reward4.Num + "\n" + "中奖概率为：" + percent4 + "\n";
    }

    public void Exst1()
    {
        GameObject text_f = GameObject.Find("奖品1文本").gameObject;
        text_f.transform.position = new Vector3(484, -92, 0);
    }

    public void Exst2()
    {
        GameObject text_f = GameObject.Find("奖品2文本").gameObject;
        text_f.transform.position = new Vector3(484, -92, 0);
    }

    public void Exst3()
    {
        GameObject text_f = GameObject.Find("奖品3文本").gameObject;
        text_f.transform.position = new Vector3(484, -92, 0);
    }

    public void Exst4()
    {
        GameObject text_f = GameObject.Find("奖品4文本").gameObject;
        text_f.transform.position = new Vector3(484, -92, 0);
    }


    public void click_on_draw()
    {
        isworking = true;
        start_play = true;
        this.transform.GetComponent<AudioSource>().Play();
    }

    public bool changePercent(int result)
    {
        switch(result)
        {
            case 0:
                Debug.Log("很遗憾没中！！");
                return false;
            case 1:
                if(reward1.Num>0)
                {
                    reward1.Num--;
                    sum--;
                    Debug.Log("恭喜获得奖品1，目前奖品1剩余为：" + reward1.Num);
                    return true;
                }
                else
                {
                    Debug.Log("很遗憾，奖品1被抢光了！");
                    return false;
                }
            case 2:
                if (reward2.Num > 0)
                {
                    reward2.Num--;
                    sum--;
                    Debug.Log("恭喜获得奖品2，目前奖品2剩余为：" + reward2.Num);
                    return true;
                }
                else
                {
                    Debug.Log("很遗憾，奖品2被抢光了！");
                    return false;
                }
            case 3:
                if (reward3.Num > 0)
                {
                    reward3.Num--;
                    sum--;
                    Debug.Log("恭喜获得奖品3，目前奖品3剩余为：" + reward3.Num);
                    return true;
                }
                else
                {
                    Debug.Log("很遗憾，奖品3被抢光了！");
                    return false;
                }
            case 4:
                if (reward4.Num > 0)
                {
                    reward4.Num--;
                    sum--;
                    Debug.Log("恭喜获得奖品4，目前奖品4剩余为：" + reward4.Num);
                    return true;
                }
                else
                {
                    Debug.Log("很遗憾，奖品4被抢光了！");
                    return false;
                }
            default:
                return false;
        }
    }

    public void change_slide1()
    {
        reward1.Num =(int ) slider1.value;

        sum = reward1.Num + reward2.Num + reward3.Num + reward4.Num;
        percent1 = (double)reward1.Num / (2 * sum);
        percent2 = (double)reward2.Num / (2 * sum);
        percent3 = (double)reward3.Num / (2 * sum);
        percent4 = (double)reward4.Num / (2 * sum);


        Debug.Log(reward1.Num);
    }

    public void change_slide2()
    {
        reward2.Num = (int)slider2.value;

        sum = reward1.Num + reward2.Num + reward3.Num + reward4.Num;
        percent1 = (double)reward1.Num / (2 * sum);
        percent2 = (double)reward2.Num / (2 * sum);
        percent3 = (double)reward3.Num / (2 * sum);
        percent4 = (double)reward4.Num / (2 * sum);

        Debug.Log(reward2.Num);
    }

    public void change_slide3()
    {
        reward3.Num = (int)slider3.value;

        sum = reward1.Num + reward2.Num + reward3.Num + reward4.Num;
        percent1 = (double)reward1.Num / (2 * sum);
        percent2 = (double)reward2.Num / (2 * sum);
        percent3 = (double)reward3.Num / (2 * sum);
        percent4 = (double)reward4.Num / (2 * sum);

        Debug.Log(reward3.Num);
    }

    public void change_slide4()
    {
        reward4.Num = (int)slider4.value;

        sum = reward1.Num + reward2.Num + reward3.Num + reward4.Num;
        percent1 = (double)reward1.Num / (2 * sum);
        percent2 = (double)reward2.Num / (2 * sum);
        percent3 = (double)reward3.Num / (2 * sum);
        percent4 = (double)reward4.Num / (2 * sum);

        Debug.Log(reward4.Num);
    }

    public void openfile()
    {
        System.Diagnostics.Process.Start(path + "抽奖结果.txt");
    }
}
