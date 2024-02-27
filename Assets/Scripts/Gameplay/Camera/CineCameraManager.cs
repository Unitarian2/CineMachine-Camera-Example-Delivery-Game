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
        if (index >= dollyCameras.Length) index = 0;//Liste bittiyse ba�a d�n�yoruz
        if (index != 0) dollyCameras[index - 1].enabled = false;// Bir �nceki dolly kapat�yoruz.

        dollyCameras[index].enabled = true;//Dolly'yi a�t�k.

        var trackedDolly = dollyCameras[index].GetCinemachineComponent<CinemachineTrackedDolly>();//Dolly Body eri�tik.
        var dollyData = dollyCameras[index].GetComponent<DollyDefinition>();//Ease ve anim datas�n� al�yoruz.

        SingleDollyAnim(trackedDolly, index, dollyData);
        
    }

    void SingleDollyAnim(CinemachineTrackedDolly trackedDolly, int currentDollyIndex, DollyDefinition dollyData)
    {
        trackedDolly.m_PathPosition = 0;//Dolly'yi ba�lang�� position'a al�yoruz.
        
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
