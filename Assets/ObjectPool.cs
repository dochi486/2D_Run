
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPool : MonoBehaviour
{

    static ObjectPool instance;
    public List<PoolItemInfo> poolList = new List<PoolItemInfo>();

    [System.Serializable]
    public class PoolItemInfo
    {
        public string name;
        public List<GameObject> items = new List<GameObject>();
        public Transform parent;

        public PoolItemInfo(GameObject original, int initCount, Transform transform)
        {
            this.parent = transform;
            name = original.name;

            for (int i = 0; i < initCount; i++)
            {
                var newItem = Object.Instantiate(original);
                newItem.name = original.name;
                InsertPoolItem(newItem);
            }
        }
        public void Push(GameObject newItem)
        {
            InsertPoolItem(newItem);
        }

        private void InsertPoolItem(GameObject newItem)
        {
            newItem.SetActive(false);
            newItem.transform.parent = parent;
            items.Add(newItem);

            if (items.Find(x => x == newItem) == null)
                items.Add(newItem);
        }

        public GameObject Pop(GameObject original)
        {
            GameObject result = null;
 
            if (items.Count == 0)
            {
                result = Object.Instantiate(original); //유니티에서 기본적으로 제공하는 오브젝트 생성 코드
                result.name = original.name; //오브젝트 이름 뒤에 (clone)이 붙지 않도록 하는 것..
                                             //이름이 같아야 오브젝트 풀에 들어가는 구조이기 때문에 이름을 무조건 같게 지정해주는 것.
                result.SetActive(true);
                return result;
            }
            result = items[0];
            items.Remove(result);
            result.transform.parent = null;
            result.SetActive(true);
            //result.transform.position = original.transform.position;
            //result.transform.rotation = original.transform.rotation;
            result.transform.SetPositionAndRotation(original.transform.position, original.transform.rotation);
            return result; //리턴하게 되면 아이템리스트가 풀에 있는 게 아니기 때문에 리무브..
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public static GameObject Instantiate(GameObject go) //static으로 한 이유: 
    {
        return instance.InstantiateGo(go);
    }
    private GameObject InstantiateGo(GameObject original)
    {
        PoolItemInfo find = poolList.Find(x => x.name == original.name);
        if(find == null)
        {
            find = RegistPool(original);
        }
        return find.Pop(original);
    }

    public int initCount = 5;

    private PoolItemInfo RegistPool(GameObject original)
    {
        PoolItemInfo newPoolItem = new PoolItemInfo(original, initCount, transform);
        poolList.Add(newPoolItem);
        return newPoolItem;
    }

    public static void Destroy(GameObject obj, float t)
    {
        instance.DestroyGo(obj, t);
    }

    private void DestroyGo(Object obj, float t)
    {
        StartCoroutine(DestroyGoCo( (GameObject)obj,  t));
    }

    private IEnumerator DestroyGoCo(GameObject original, float t)
    {
        yield return new WaitForSeconds(t);
        PoolItemInfo find = poolList.Find(x => x.name == original.name);
        if (find != null)
            find.Push(original);
        //find?.Push(original); 위에꺼랑 똑같은 코드
        //poolList.Find(x => x.name == original.name)?.Push(original); 똑같은 코드2

    }
}

