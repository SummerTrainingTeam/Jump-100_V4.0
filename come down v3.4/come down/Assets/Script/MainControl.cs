using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Text;
public class MainControl : MonoBehaviour
{
    public AudioSource putong;
    public AudioSource ci;
    public AudioSource tanhuang;
    public AudioSource suilie;
    public AudioSource Bomb;
    public ArrayList boards;
    public ArrayList bullet;
    public ArrayList bulletmovdirection;
    public ArrayList bomb;
    public ArrayList bombtime;
    private int frame = 0;
    float aimovspeed = 2.0f;
    float elfmovspeed = 2.0f;
    private Vector3 movvec = new Vector3(0, 1, 0);
    float boardsUpSpeed = 2.0f;//屏幕刷新频率
    float speed = 5.0f;//人物活动频率
    const float MAXHEIGHT = 5.5f, MINHEIGHT = -5.5f;//客户区底端与顶端
    float currentHeight;//最上砖块的上沿
    private textset blood;//人物血量
    private scoreset score;//分数
    private coinset coin;
    public SpriteSheet sheet;
    int count = 0;
    //动画
    private GameObject normal_brick;
    private GameObject break_brick;
    private GameObject slide_left_brick;
    private GameObject slide_right_brick;
    private GameObject hurt_brick;
    private GameObject spring_brick;

    private GameObject break_brick_anim;
    private GameObject spring_brick_anim;
    private GameObject anima;
    private GameObject elf;
    private GameObject bullet_explore_anim;

	// Gold Coins
	private GameObject GC;
	private GameObject RealGC;
	private bool isGoldAway;

    int[] pre;
    string[] str;

    private void Awake()
    {
        sheet = GetComponent<SpriteSheet>();
        sheet.AddAnim("move_death", 13, 1, true);
        sheet.AddAnim("move_left", 16, 1, true);
        sheet.AddAnim("move_right", 16, 1, true);

    }

    //private int HP = 100;
    void Start()
    {
        blood = GameObject.Find("Canvas/blood").GetComponent<textset>();
        score = GameObject.Find("Canvas/score").GetComponent<scoreset>();
        coin = GameObject.Find("Canvas/coin").GetComponent<coinset>();

        normal_brick = (GameObject)Resources.Load("Prefab/normal_brick");
        break_brick = (GameObject)Resources.Load("Prefab/break_brick");
        slide_left_brick = (GameObject)Resources.Load("Prefab/left_brick");
        slide_right_brick = (GameObject)Resources.Load("Prefab/right_brick");
        hurt_brick = (GameObject)Resources.Load("Prefab/hurt_brick");
        spring_brick = (GameObject)Resources.Load("Prefab/spring_brick");


        break_brick_anim = (GameObject)Resources.Load("Prefab/break_brick_anim");

        bullet_explore_anim = (GameObject)Resources.Load("Prefab/green_bullet_anim");

		// Gold Coins
		GC = (GameObject)Resources.Load("Prefab/GoldCoin");
		isGoldAway = false;
		RealGC = Instantiate (GC) as GameObject;
		RealGC.transform.localScale = new Vector3 (0.55f, 0.55f, 0.8f);
		RealGC.name = "GoldCoin";

        GameObject AI_robot = (GameObject)Resources.Load("Prefab/monster");
        GameObject airobot=Instantiate(AI_robot);
        airobot.transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
        airobot.transform.position = new Vector3(6.0f, 0, 0);
        airobot.name = "AI";

        boards = new ArrayList();
        bullet = new ArrayList();
        bulletmovdirection = new ArrayList();
        bomb = new ArrayList();
        bombtime = new ArrayList();
        currentHeight = MAXHEIGHT;
        AddBoards(1);

        /*str = File.ReadAllLines(@"C:\Users\yyr\Desktop\comdown3.4\score.txt", Encoding.ASCII);
        pre = new int[10];
        for (int i = 0; i < 10; i++)
            int.TryParse(str[i], out pre[i]);*/

    }
    GameObject Generateobj(int id)
    {

        int randomid = Random.Range(1, 12);
        GameObject obj = Instantiate(normal_brick);
        if (obj != null) Destroy(obj);
        switch (randomid)
        {
            case 1: { obj = Instantiate(spring_brick); obj.name = "cube6"; break; } //弹
            case 2: { obj = Instantiate(hurt_brick); obj.name = "cube2"; break; } //刺 
            case 3: { obj = Instantiate(slide_left_brick); obj.name = "cube3"; break; } //传 
            case 4: { obj = Instantiate(slide_right_brick); obj.name = "cube4"; break; } //传
            case 5: { obj = Instantiate(break_brick); obj.name = "cube5"; break; } //碎
            case 6: { obj = Instantiate(slide_left_brick); obj.name = "cube3"; break; };
            case 7: { obj = Instantiate(slide_right_brick); obj.name = "cube4"; break; };

            default: {obj = Instantiate(normal_brick); obj.name = "cube1"; break; } 
        }

        float x;

        x = id == 1 ? Random.Range(-6.0f, -1.5f) : Random.Range(1.5f, 6.0f);

        //obj.transform.localScale = new Vector3(3.5f, 1.5f, 1.0f);
        obj.transform.position = new Vector3(x, currentHeight, 0);

        boards.Add(obj);

		// Gold Coins Appears Here
		//print(isGoldAway?"True" : "False");
		if (isGoldAway) {
			isGoldAway = false;
			RealGC.transform.position = new Vector3(x, -2.0f, 0);
		}

        return obj;
    }
    void AddBoards(int id)
    {
        while (currentHeight > MINHEIGHT)
        {
            score.add();
            boardsUpSpeed += 0.1f;
            int rid = id==1? 2:(int)Random.Range(0, 3);
            if (rid==0) Generateobj(1);
            else if (rid==1) Generateobj(2);
            else { Generateobj(1); Generateobj(2); }
            currentHeight -= 2.5f;
        }
    }
    private void TerminateFunction()
    {
        // 间隔一段时间
        /*int rankscore = 10;
        if (score.score > pre[9])
        {
            for (int i = 9; i >= 0 && score.score > pre[i]; i--)
                rankscore = i;
            for (int i = 9; i >= rankscore+1; i--)
                pre[i] = pre[i - 1];
            pre[rankscore] = score.score;

            for (int i = 0; i < 10; i++)
                str[i] = pre[i].ToString();
            File.WriteAllLines(@"C:\Users\yyr\Desktop\comdown3.4\score.txt", str, Encoding.ASCII);
        }*/
        // 跳转
        SceneManager.LoadScene("End");
    }
    // Update is called once per frame
    void Update()
    {
        frame++;
        ++count;
        if (count % 15 == 0) Destroy(anima);
		if (transform.position.y < -5.5f || transform.position.y > 5.5f || blood.num <= 0)
        {
            //结尾动画
            //GameObject.Find("ObjectName").GetComponent<MainControl>().enabled = false;
            TerminateFunction();
        }


        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            sheet.Play("move_right");

            transform.Translate(Vector3.right * speed * Time.deltaTime*2.0f);

        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            sheet.Play("move_left");
            transform.Translate(Vector3.left * speed * Time.deltaTime*2.0f);
        }
        BoardsUp();
        blood.Show();
		coin.Show ();
        //monster ai
        GameObject ai = GameObject.Find("AI");
        if (frame % 480 == 0) ai.transform.position = new Vector3(-ai.transform.position.x, 0, 0);
        if (frame % 60 == 0) movvec = -movvec;
        ai.transform.Translate(movvec * aimovspeed * Time.deltaTime);

        Vector3 mainposition = (Vector3)transform.position;
        Vector3 aiposition = (Vector3)ai.transform.position;
        Vector3 movdirection = mainposition - aiposition;
        if (frame % 120 == 0)
        {
            GameObject bul_anim = (GameObject)Resources.Load("Prefab/green_bullet");
            GameObject bul = Instantiate(bul_anim);
            bul.transform.localScale = new Vector3(1.50f, 1.50f, 1.0f);

            bul.transform.position = (Vector3)aiposition;
            bul.name = "bullet";

            bullet.Add(bul);
            Vector3 bulletmovvector = (Vector3)movdirection;
            bulletmovdirection.Add((Vector3)bulletmovvector);
        }
        for (int i = 0; i < bulletmovdirection.Count;)
        {
            GameObject obj = (GameObject)bullet[i];
            Vector3 vec = (Vector3)bulletmovdirection[i];
            obj.transform.Translate(vec * Time.deltaTime);
            if (obj.transform.position.x < -7.0f || obj.transform.position.x > 7.0f || obj.transform.position.y > 5.5f || obj.transform.position.y < -5.5f || obj.name=="nonactive")
            {
                bullet.Remove(obj);
                Destroy(obj);
                bulletmovdirection.Remove(vec);
            }
            else i++;

        }
        //bullet ai

        for (int i = 0; i < bomb.Count;)
        {
            GameObject obj = (GameObject)bomb[i];

            int preframe = (int)bombtime[i];
    
            if (frame-preframe==20)
            {
                bomb.Remove(obj);
                Destroy(obj);
                bombtime.Remove(preframe);

            }
            else i++;

        }

        //elf ai
        if (frame == 300)
        {
            GameObject elf_now = (GameObject)Resources.Load("Prefab/elf");
            elf = Instantiate(elf_now);
            elf.transform.localScale = new Vector3(0.8f, 0.8f, 0.1f);
            elf.transform.position = new Vector3(0, 0, 0);
            elf.GetComponent<Renderer>().material.mainTexture = (Texture)Resources.Load("elf");
       
        }
        if (frame > 300)
        {
          
            Vector3 elfposition = (Vector3)elf.transform.position;
            Vector3 elfdirection = mainposition - elfposition;
            elf.transform.Translate(elfdirection * elfmovspeed * Time.deltaTime);
            if (frame % 60 == 0) blood.add2();
        }


 
    }
    void BoardsUp()
    {
        currentHeight += boardsUpSpeed * Time.deltaTime;
        for (int i = 0; i < boards.Count;)
        {

            GameObject obj = (GameObject)boards[i];

            obj.transform.Translate(new Vector3(0, 1, 0) * boardsUpSpeed * Time.deltaTime);
            if (obj.transform.position.y > MAXHEIGHT)
            {
                boards.Remove(obj);
                Destroy(obj);
            }
            else
            {
                i++;
            }
        }
		// Time For Gold Coin Away
		if(RealGC.transform.position.y > MAXHEIGHT){
			isGoldAway = true;
			RealGC.transform.position = new Vector3 (100.0f, 100.0f, 0);
		}
        AddBoards(2);

    }

    void OnCollisionEnter(Collision thing)
    {

        var name = thing.collider.name;
        //Debug.Log("Thing is " + name);
        if (name == "cube2")
        {
            ci.Play();
            blood.add();
        }
        else if (name == "cube3")
        {
            putong.Play();
            //Destroy(thing.collider);

        }
        else if (name == "cube4")
        {
            putong.Play();
        }
        else if (name == "cube5")
        {

            /*Vector3 vex_1 = thing.transform.position;
            Vector3 vex_2 = thing.other.gameObject.transform.position;
            Vector3 vex_3 = thing.other.gameObject.GetComponent<Renderer>().bounds.size;*/

            Debug.Log(name);
            ContactPoint vPoint = thing.contacts[0];//获取第一个碰撞点

            Quaternion quate = Quaternion.FromToRotation(Vector3.up, new Vector3(0, 1, 0));//碰撞点法线

            count = 0;
            suilie.Play();
            anima = Instantiate(break_brick_anim, vPoint.point, quate) as GameObject;


            boards.Remove(thing.gameObject);
            Destroy(thing.gameObject);

        }
        else if (name == "cube6")
        {
            transform.Translate(new Vector3(0, 12, 0) * speed * Time.deltaTime);

            tanhuang.Play();
          /*  Debug.Log(name);
            ContactPoint vPoint = thing.contacts[0];//获取第一个碰撞点

            Quaternion quate = Quaternion.FromToRotation(Vector3.up, new Vector3(0, 1, 0));

            count = 0;

            anima = Instantiate(spring_brick_anim, vPoint.point, quate) as GameObject;

            boards.Remove(thing.gameObject);
            Destroy(thing.gameObject);
            */
        }
        else if (name == "cube1")
        {
            putong.Play();
        }
        else if (name=="bullet")
        {
            blood.add();
            Bomb.Play();
                      
            GameObject bul_explore = Instantiate(bullet_explore_anim);
            bul_explore.transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);

            ContactPoint vPoint = thing.contacts[0];//获取第一个碰撞点

            Quaternion quate = Quaternion.FromToRotation(Vector3.up, vPoint.normal);//碰撞点法线
            GameObject animation = Instantiate(bul_explore, vPoint.point, quate) as GameObject;
            bomb.Add(animation);
            bombtime.Add(frame);
            thing.gameObject.name = "nonactive";
            Destroy(bul_explore);
        }

		// Collision on GoldCoins
		else if(name == "GoldCoin"){
			isGoldAway = true;
			// Go to an inaccessible place
			RealGC.transform.position = new Vector3 (100.0f, 100.0f, 0);
			// Coin add one
			coin.Add(1);
		}

    }

    void OnCollisionExit(Collision thing)
    {
        var name = thing.collider.name;
        //Debug.Log("Thing is " + name);
        if (name == "cube2")
        {

        }
        else if (name == "cube3")
        {
            //transform.Translate(Vector3.left * speed * Time.deltaTime * 0.3f);
        }
        else if (name == "cube4")
        {
            //transform.Translate(Vector3.right * speed * Time.deltaTime * 0.3f);
        }
        else if (name == "cube5")
        {
            //
        }
        else if(name == "cube6")
        {
            //thing.gameObject.SetActive(true);
        }
    }

    void OnCollisionStay(Collision thing)
    {
        var name = thing.collider.name;
        //Debug.Log("Thing is " + name);
        if (name == "cube2")
        {

        }
        else if (name == "cube3")
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime * 0.3f);
        }
        else if (name == "cube4")
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime * 0.3f);
        }
        else if (name == "cube5")
        {
            //
        }
    }

}
