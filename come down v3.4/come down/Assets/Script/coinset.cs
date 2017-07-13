using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
public class coinset : MonoBehaviour {

    public Text Coin;
    private int num;

	// Use this for initialization
	void Start () {
        Coin = GameObject.Find("Canvas/coin").GetComponent<Text>();
       // string str = File.ReadAllText(@"C:\Users\yyr\Desktop\comdown3.4\coin.txt",Encoding.ASCII);
        //int.TryParse(str, out num);
        //Coin.text = " " + num;
	}
	
    public void Add(int tmp)
    {
        num += tmp;
        //string str2 = num.ToString();
        //File.WriteAllText(@"C:\Users\yyr\Desktop\comdown3.4\coin.txt", str2);
    }

    public void Show()
    {
        Coin.text = "Coins: " + num;
    }

	// Update is called once per frame
	void Update () {
        Show();
	}
}
