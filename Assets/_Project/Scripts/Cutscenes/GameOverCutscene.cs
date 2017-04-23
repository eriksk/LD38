using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCutscene : MonoBehaviour
{
    public Transform Rocket;
    public Transform Robot;

    public ParticleSystem RocketParticles;

    public Transform RocketPath;
    public Transform RocketCameraViewPosition;
    public Transform RobotCameraViewPosition;
    public Transform LiftOffCameraViewPosition;
    public AudioSource Ambience;

    public UnityEngine.UI.Image Overlay;

    public float RocketSpeed = 10f;
    public float Acceleration = 10f;
    public float RotationSpeed = 10f;

    public void Start()
    {
        StartCoroutine(RunCutscene());
    }

    private IEnumerator RunCutscene()
    {
        yield return new WaitForSeconds(2f);
        
        var camera = Camera.main.transform;

        // 1. Look at robot
        camera.transform.position = RobotCameraViewPosition.position;
        camera.transform.rotation = RobotCameraViewPosition.rotation;
        yield return new WaitForSeconds(4f);

        // 2. Go To liftoff cam pose
        camera.transform.position = LiftOffCameraViewPosition.position;
        camera.transform.rotation = LiftOffCameraViewPosition.rotation;
        yield return new WaitForSeconds(2f);

        // 3. Ignite engines
        RocketParticles.Play(); // Play sound...
        yield return new WaitForSeconds(3.34f);

        // 4. Lift Off
        var path = RocketPath.Children().ToArray();
        var currentIndex = 0;
        
        var currentSpeed = RocketSpeed;

        while(currentIndex < path.Length - 1)
        {
            var target = path[currentIndex];
            
            while(true)
            {   
                var distance = Vector3.Distance(Rocket.position, target.position);
                var direction = (target.position - Rocket.position).normalized;
                Rocket.position += direction * currentSpeed * Time.deltaTime;
                Rocket.rotation = Quaternion.RotateTowards(Rocket.rotation, Quaternion.LookRotation(direction, Vector3.up), RotationSpeed * Time.deltaTime);
                currentSpeed += Acceleration * Time.deltaTime;

                if(distance < 5f)
                    break;

                yield return new WaitForEndOfFrame();
            }
            currentIndex++;

            if(currentIndex == 2)
            {
                // 5. Rocket Path, attach camera to rocket
                Ambience.Stop();
                camera.parent = RocketCameraViewPosition;
                camera.transform.localPosition = Vector3.zero;
                camera.transform.localRotation = Quaternion.identity;
            }
            if(currentIndex == path.Length - 2)
            {
                // 6. Rocket Path, attach camera to rocket
                // reparent camera
                camera.parent = null;
                StartCoroutine(FadeOutScreenAndExitToMainMenu());
            }
        }

    }

    private IEnumerator FadeOutScreenAndExitToMainMenu()
    {
        var current = 0f;
        var duration = 3f;

        while(current <= duration)
        {
            current += Time.deltaTime;
            var alpha = current / duration;
            var color = Overlay.color;
            color.a = alpha;
            Overlay.color = color;
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("Menu");
    }
}