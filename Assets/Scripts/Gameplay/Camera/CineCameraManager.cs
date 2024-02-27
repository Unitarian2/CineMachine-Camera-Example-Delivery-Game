using Cinemachine;
using DG.Tweening;

using UnityEngine;

public class CineCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera steadyFarCamera;
    [SerializeField] private CinemachineVirtualCamera[] dollyCameras;

    Tween currentDollyAnim;
   
    public void ToggleCameraTransition()
    {
        if (CinemachineCore.Instance.IsLive(steadyFarCamera))
        {     
            
            StartDollies();
        }
        else
        {
            StopDollyProcess();
            
        }
    }

    public void StartDollies()
    {
        steadyFarCamera.enabled = false;
        DisableDollies();
        SingleDollyProcess(0);
    }

    void SingleDollyProcess(int index)
    {
        if (index >= dollyCameras.Length) index = 0;//Liste bittiyse baþa dönüyoruz
        if (index != 0) dollyCameras[index - 1].enabled = false;// Bir önceki dolly kapatýyoruz.

        dollyCameras[index].enabled = true;//Dolly'yi açtýk.

        var trackedDolly = dollyCameras[index].GetCinemachineComponent<CinemachineTrackedDolly>();//Dolly Body eriþtik.
        var dollyData = dollyCameras[index].GetComponent<DollyDefinition>();//Ease ve anim datasýný alýyoruz.

        SingleDollyAnim(trackedDolly, index, dollyData);
        
    }

    void SingleDollyAnim(CinemachineTrackedDolly trackedDolly, int currentDollyIndex, DollyDefinition dollyData)
    {
        trackedDolly.m_PathPosition = 0;//Dolly'yi baþlangýç position'a alýyoruz.
        
        currentDollyAnim = DOTween.To(() => trackedDolly.m_PathPosition, x => trackedDolly.m_PathPosition = x, 1f, dollyData.animDuration).SetEase(dollyData.easeType).OnComplete(() =>
        {
            Debug.Log("Anim Completed for "+ currentDollyIndex);
            
            SingleDollyProcess(currentDollyIndex + 1);
        });
    }

    public void StopDollyProcess()
    {
        currentDollyAnim.Kill();
        foreach (var dolly in dollyCameras)
        {
            dolly.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = 0;
        }

        DisableDollies();
        steadyFarCamera.enabled = true;
    }

    void DisableDollies()
    {
        foreach (var dolly in dollyCameras)
        {
            dolly.enabled = false;
        }
    }


}
