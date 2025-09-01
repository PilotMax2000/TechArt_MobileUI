using System;
using System.Collections;
using TechArtProject.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace TechArtProject
{
    public class MenuHandler : MonoBehaviour
    {
        [Header("Options Popup")]
        [SerializeField] private PopupBackgroundBlur _popupBackgroundBlur;
        [SerializeField] private Button _settingsPopupOpenButton;
        [SerializeField] private Button _settingsPopupCloseButton;
        [SerializeField] private GameObject _settingsPopup;
        [SerializeField] private Animator _setttionsPopupAnimator;
        [SerializeField] private string _settingsPopupCloseTrigger;
        
        [Header("Level Completed")]
        [SerializeField] private Button _levelCompletedOpenButton;
        [SerializeField] private Button _levelCompletedCloseButton;
        [SerializeField] private Animator _levelCompletedAnimator;
        [SerializeField] private GameObject _levelCompletedMenu;
        [SerializeField] private string _levelCompletedCloseTrigger;

        [Header("Bottom Bar Panel")] 
        [SerializeField] private Animator _bottomBarAnimator;
        [SerializeField] private string _bottomBarHideTrigger;
        [SerializeField] private string _bottomBarShowTrigger;
        [SerializeField] private float _levelCompletedWaitingTime = 1f;
        
        private int _cachedSettingsPopupCloseTrigger;
        private int _cachedLevelCompletedTrigger;
        private int _cachedBottomBarHideTrigger;
        private int _cachedBottomBarShowTrigger;

        private void Awake()
        {
            _settingsPopupOpenButton.onClick.AddListener(OpenSettingsPopup);
            _settingsPopupCloseButton.onClick.AddListener(CloseSettingsPopup);
            _levelCompletedOpenButton.onClick.AddListener(OpenLevelCompletedMenu);
            _levelCompletedCloseButton.onClick.AddListener(CloseLevelCompletingMenu);

            _cachedSettingsPopupCloseTrigger = Animator.StringToHash(_settingsPopupCloseTrigger);
            _cachedLevelCompletedTrigger = Animator.StringToHash(_levelCompletedCloseTrigger);
            _cachedBottomBarHideTrigger = Animator.StringToHash(_bottomBarHideTrigger);
            _cachedBottomBarShowTrigger = Animator.StringToHash(_bottomBarShowTrigger);
        }

        private void OpenSettingsPopup()
        {
            _popupBackgroundBlur.ApplyBlur();
            _settingsPopup.SetActive(true);
        }

        private void CloseSettingsPopup() => 
            _setttionsPopupAnimator.SetTrigger(_cachedSettingsPopupCloseTrigger);

        private void OpenLevelCompletedMenu()
        {
            _levelCompletedMenu.SetActive(true);
            _bottomBarAnimator.SetTrigger(_cachedBottomBarHideTrigger);
        }

        private void CloseLevelCompletingMenu()
        {
            _levelCompletedAnimator.SetTrigger(_cachedLevelCompletedTrigger);
            StartCoroutine(WaitAndDo(_levelCompletedWaitingTime, () => _bottomBarAnimator.SetTrigger(_cachedBottomBarShowTrigger)));
        }

        private IEnumerator WaitAndDo(float waitingTime, Action action)
        {
            yield return new WaitForSeconds(waitingTime);
            action?.Invoke();
        }
    }
}