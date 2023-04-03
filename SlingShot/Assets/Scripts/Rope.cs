using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rope : MonoBehaviour
{
    GameManager gameManager;
    Projection prj;
    LineRenderer LineRenderer;
    public Camera mainCamera;
    float throwPower;
    public bool canShot;

    [Header("-------Transforms")]

    [SerializeField] Transform throwRotation;
    [SerializeField] Transform baseTransform;
    Transform mainTransform;

    [Header("-------GameObjects")]
    public GameObject manobje;
    GameObject man;
    [SerializeField] GameObject rubberObject;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        prj = GetComponent<Projection>();
        LineRenderer = GetComponent<LineRenderer>();
        LineRenderer.enabled = false;
        canShot = false;
    }

  
    void Update()
    {

        if (Input.mousePosition.y < 600 && canShot && gameManager.shotCounter !=0 && gameManager.rate !=100)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (mainTransform == null)
                {
                    man = Instantiate(manobje, throwRotation.position, Quaternion.identity);
                    mainTransform = manobje.transform;
                }
                LineRenderer.enabled = true;
            }
            if (Input.GetMouseButton(0) && mainTransform)
            {
                Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(baseTransform.position).z);
                Vector3 wp = mainCamera.ScreenToWorldPoint(pos);

                mainTransform.position = new Vector3(wp.x, 3.5f, wp.z*1.3f);
                man.transform.position = new Vector3(mainTransform.position.x, mainTransform.position.y, mainTransform.position.z + .5f);

                throwPower = Vector3.Distance(mainTransform.position, baseTransform.position) * 3500;

                rubberObject.transform.position = Vector3.MoveTowards(rubberObject.transform.position, mainTransform.position, 20 * Time.deltaTime);

                throwRotation.transform.LookAt(rubberObject.transform, Vector3.forward);

                Transform projectionRotation = mainTransform;
                projectionRotation.rotation = throwRotation.rotation;
                float projectionRate = throwPower/ 1.15f;

                prj.SimulateTrajectory(manobje, man.transform.position, -throwRotation.forward * projectionRate);

            }
            else if (Input.GetMouseButtonUp(0))
            {

                LineRenderer.enabled = false;
                if (mainTransform)
                {
                    man.GetComponent<Rigidbody>().isKinematic = false;
                    man.GetComponent<Rigidbody>().AddForce(-throwRotation.forward * throwPower);
                    mainTransform = null;
                    ShotCounter();
                    StartCoroutine(shotTime());
                }
            }
        }
        else
        {
            if (man != null && man.GetComponent<Rigidbody>().velocity.magnitude == 0)
            {
                man.transform.position = new Vector3(rubberObject.transform.position.x, rubberObject.transform.position.y, rubberObject.transform.position.z + .5f);
            }
            LineRenderer.enabled = false;
            ResetRubberPosition();
        }

    }

   void ShotCounter()
    {
        gameManager.shotCounter--;
        if(gameManager.shotCounter == 0)
        {
            gameManager.counterText.text = "No More Shot";
            StartCoroutine(gameControl());
        }
        else
        {
            gameManager.counterText.text = gameManager.shotCounter + " Shot Left";
        }
    }
    IEnumerator gameControl()
    {
        yield return new WaitForSeconds(5);
        if(gameManager.rate != 100)
        {
            gameManager.GameOver(false, gameManager.rate);
        }
    }
    IEnumerator shotTime()
    {
        canShot = false;
        yield return new WaitForSeconds(1f);
        canShot = true;
    }
    public IEnumerator startDelay()
    {
        yield return new WaitForSeconds(.5f);
        canShot = true;
    }
    void ResetRubberPosition()
    {
        rubberObject.transform.position = Vector3.Lerp(rubberObject.transform.position, baseTransform.position, 10 * Time.deltaTime);
    }
}
