using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    public Rigidbody bulletPrefabs;
    public GameObject cursor;
    public LayerMask layer;
    public Transform shootingPoint;
    public LineRenderer visualizeLine;
    public int lineSegment;
    public GameObject AiPlayer;
    public string textValue;
    public Text textElementVelHori;
    public Text textElementVelVert;
    public Text textElementTime;
    public Text textElementForceHorizontal;
    public Text textElementForceVertical;
    public Slider sliderTime;
    public Slider sliderMass;
    public Text textElementTimeIndicator;
    public Text textElementMassIndicator;
    public Text textElementDistanceIndicator;
    public Text textElementAngleIndicator;

    [SerializeField]
    private float timeTaken;
    [SerializeField]
    private float mass;

    private Camera cam;
    public Camera aicamera;
    private float timetoWait =0;
    float X;
    float y;
    // Start is called before the first frame update
    void Start()
    {
        //initial mass
        mass = 2f;
        //slidre mass
        sliderMass.minValue = 1f;
        sliderMass.maxValue = 10f;
        sliderMass.wholeNumbers = true;
        sliderMass.value = mass;
        //slider Timer
        sliderTime.minValue = 2f;
        sliderTime.maxValue = 10f;
        sliderTime.wholeNumbers = true;
       
        cam = Camera.main;
        visualizeLine.positionCount = lineSegment;
        timeTaken = 2f;
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        textElementTime.text ="Time to Reach Target: "+ timeTaken.ToString()+" seconds.";
      
        ProjectileLaunch();
        sliderTime.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        sliderMass.onValueChanged.AddListener(delegate { ValueChangeCheckMass(); });
        PlayerPrefs.SetFloat("Time:", timeTaken);
    }

    Vector3 CalculateVelocity(Vector3 target,Vector3 origin,float time)
    {
        // define distance x and y first
        Vector3 distance = target - origin;
        //assign distance from target to origin in xz plane
        Vector3 distanceXZ = distance;
        // distance at y set to 0 ( horizontal distance )
        distanceXZ.y = 0f;

        //float representing distance
        // kinematics Final distance Y is set to Sy
        float Sy = distance.y;
        X = distance.x;
        y = distance.y;

        // final distance X and Z is set 
        float Sxz = distanceXZ.magnitude;

        // initial velocity x and z
        // in horizontal force so there is no gravity. so Sxz = initial Velocity(Vxz) *time;
        float Vxz = Sxz / time;

        // initial velocity y p Vy = Sy/t + 1/2*|g|*t;
        // vertical force, so gravity is applied to this
        // gravity is negative here

        float Vy = Sy / time +0.5f * Mathf.Abs(Physics.gravity.y)*time;

        Vector3 result = distanceXZ.normalized; // direction of vector normalized
        
        // direction multiplied with the velocity 
        result *= Vxz;
        result.y = Vy;
        textElementDistanceIndicator.text = "Distance: " + Mathf.Sqrt(
                                                      Mathf.Pow(target.x - origin.x, 2) +
                                                      Mathf.Pow(target.y - origin.y, 2) + 
                                                      Mathf.Pow(target.z - origin.z, 2));
        textElementForceHorizontal.text = "ForceHorizontal:" + bulletPrefabs.mass * (Vxz/time);
        textElementForceVertical.text = "ForceVertical:" + bulletPrefabs.mass * (Vy/time);
        textElementVelHori.text ="Initial Velocity Horizontal: "+ Vxz.ToString();
        textElementVelVert.text = "Initial Velocity Vertical:" + Vy.ToString();
        return result;
    }

    void ProjectileLaunch()
    {
        // cast ray from cam to mouse 
        // gets position of mouse
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
     
        RaycastHit hitinfo;
        cursor.transform.position = Input.mousePosition;
     
        if (Physics.Raycast(camRay,out hitinfo,100f,layer))
        {
            cursor.SetActive(true);
            cursor.transform.position = hitinfo.point + Vector3.up * 0.01f;

            Vector3 Vo = CalculateVelocity(hitinfo.point, shootingPoint.position, timeTaken);

           
            TraceLine(Vo);
            transform.rotation = Quaternion.LookRotation(Vo); // cannon look at the start angle 
            Vector3 dist = cursor.transform.position - transform.position;

            //shooting angle calculation
            Vector3 xis = (new Vector3(hitinfo.point.x,0f,0f)-new Vector3(transform.position.x,0f,0f));
            float angle = Mathf.Acos(Vector3.Dot(transform.forward, xis) / (transform.forward.magnitude * xis.magnitude));
            textElementAngleIndicator.text = "Angle: " + angle * (180 / Mathf.PI) + " Degrees";

            timetoWait += 10 * Time.deltaTime;
            if (Input.GetMouseButtonDown(0) && timetoWait >1f)
            {
                timetoWait = 0;
                Rigidbody obj = Instantiate(bulletPrefabs, shootingPoint.position, Quaternion.identity);
                obj.velocity = Vo;
            }
        }
        else
        {
            cursor.SetActive(false);
        }
    }

    Vector3 calculatePosInTime(Vector3 Vo,float time)
    {
        Vector3 Vxz = Vo;
        Vxz.y = 0f;

        // to locate the position of projectile in time we use motion equation
     
        Vector3 result = shootingPoint.position + Vo * time;
        float Sy = (-0.5f*Mathf.Abs(Physics.gravity.y)*(time*time))+(Vo.y * time) + shootingPoint.position.y;
        result.y = Sy;
        return result;
    }

    void TraceLine(Vector3 Vo)
    {
        for(int i =0;i<lineSegment;++i)
        {
            Vector3 pos = calculatePosInTime(Vo, timeTaken * i / (float)lineSegment);
            visualizeLine.SetPosition(i, pos);
        }
    }

    void MovePlayer(Vector3 location)
    {
        AiPlayer.transform.position = location;
    }

    void showText()
    {

    }

    public void ValueChangeCheck()
    {
        timeTaken = sliderTime.value;
        textElementTimeIndicator.text = timeTaken.ToString() + "Seconds";
    }

    public void ValueChangeCheckMass()
    {
        bulletPrefabs.mass = sliderMass.value;
        textElementMassIndicator.text = sliderMass.value.ToString() + "kg";
    }

}
