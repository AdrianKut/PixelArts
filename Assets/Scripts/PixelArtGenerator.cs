using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PixelArtGenerator : MonoBehaviour
{
    [Header("Pixel fragment")]
    public GameObject prefabToSpawn;
    public float zSpawnPosition = -9f;
    public int numberOfFragments;
    private List<int> fragmentsCounterLists = new List<int>();

    void Awake()
    {
        Application.targetFrameRate = 999;
        numberOfFragments = transform.childCount;
        System.Random rnd = new System.Random();

        fragmentsCounterLists.AddRange(Enumerable.Range(0, numberOfFragments));
        fragmentsCounterLists = fragmentsCounterLists.OrderBy(x => rnd.Next()).ToList<int>();

        for (int i = 0; i < numberOfFragments; i++)
            transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;

        progressSlider.maxValue = numberOfFragments;
    }

    [Range(1, 100)]
    public float gameSpeed = 10f;
    public bool isStarted;
    public Button buttonRestart;
    public void OnClickedStart(Button btn)
    {
        isStarted = true;
        btn.interactable = false;
        Time.timeScale = gameSpeed;

        buttonRestart.interactable = true;
    }

    [Header("Timer")]
    [Range(0.01f, 1f)]
    public float spawnDelay = 0.5f;
    private float spawnTimer;

    private void Start()
    {
        spawnTimer = spawnDelay;
    }

    private void Update()
    {
        if (isStarted)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f && numberOfFragments >= 0)
            {
                spawnTimer = spawnDelay;
                Invoke("Generate",0f);
            }
        }
    }

    public Slider progressSlider;
    public TextMeshProUGUI textProgressSlider;

    private GameObject currentChild;
    private void Generate()
    {

        if (fragmentsCounterLists.Count != 0 && numberOfFragments >= 0)
        {
            numberOfFragments--;
            currentChild = transform.GetChild(fragmentsCounterLists[0]).gameObject;
            fragmentsCounterLists.RemoveAt(0);

            var tempGameObject = Instantiate(prefabToSpawn, new Vector3(currentChild.transform.position.x, currentChild.transform.position.y, currentChild.transform.position.z + zSpawnPosition),
                Quaternion.identity) as GameObject;

            //var tempGameObject = Instantiate(prefabToSpawn, UnityEngine.Random.onUnitSphere * 15, Quaternion.identity) as GameObject;
            tempGameObject.GetComponent<MoveToPosition>().targetPos = currentChild.GetComponent<Transform>();
            tempGameObject.GetComponent<Renderer>().material.color = currentChild.GetComponent<Renderer>().material.color;
            tempGameObject.name = $"Cube({numberOfFragments})";

            StartCoroutine(ValueChangedProgressBar());

            tempGameObject.GetComponent<MoveToPosition>().OnDesiredPositionEvent.AddListener(SpawnParticle);
        }
    }

    [Header("Particle Drop Effect")]
    public GameObject particleSystem;
    private void SpawnParticle()
    {
        var obj = Instantiate(particleSystem, new Vector3(currentChild.transform.position.x, currentChild.transform.position.y, currentChild.transform.position.z -1f), particleSystem.transform.rotation) as GameObject;
        obj.GetComponent<ParticleSystemRenderer>().material.color = currentChild.GetComponent<Renderer>().material.color;
        obj.GetComponent<ParticleSystem>().Play();

        Destroy(obj, 2.5f);
    }

    private IEnumerator ValueChangedProgressBar()
    {
        float values = 4f;
        do
        {
            progressSlider.value += 0.2f;
            values -= 1f;
            yield return new WaitForSeconds(0.12f);
            textProgressSlider.SetText($"{Mathf.Clamp(progressSlider.value * 2, 0, 100):F0}%");
        } while (values >= 0);
    }

    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);


}
