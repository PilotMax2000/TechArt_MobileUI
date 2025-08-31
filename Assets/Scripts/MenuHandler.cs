using System;
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
        
        private int _cachedSettingsPopupCloseTrigger;
        private int _cachedLevelCompletedTrigger;

        private void Awake()
        {
            _settingsPopupOpenButton.onClick.AddListener(OpenSettingsPopup);
            _settingsPopupCloseButton.onClick.AddListener(CloseSettingsPopup);
            _levelCompletedOpenButton.onClick.AddListener(OpenLevelCompletedMenu);
            _levelCompletedCloseButton.onClick.AddListener(CloseLevelCompletingMenu);

            _cachedSettingsPopupCloseTrigger = Animator.StringToHash(_settingsPopupCloseTrigger);
            _cachedLevelCompletedTrigger = Animator.StringToHash(_levelCompletedCloseTrigger);
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
        }

        private void CloseLevelCompletingMenu()
        {
            _levelCompletedAnimator.SetTrigger(_cachedLevelCompletedTrigger);
        }
    }
}