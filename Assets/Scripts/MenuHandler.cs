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
        private int _cachedSettingsPopupCloseTrigger;

        private void Awake()
        {
            _settingsPopupOpenButton.onClick.AddListener(OpenSettingsPopup);
            _settingsPopupCloseButton.onClick.AddListener(CloseSettingsPopup);

            _cachedSettingsPopupCloseTrigger = Animator.StringToHash(_settingsPopupCloseTrigger);
        }

        private void OpenSettingsPopup()
        {
            _popupBackgroundBlur.ApplyBlur();
            _settingsPopup.gameObject.SetActive(true);
        }

        private void CloseSettingsPopup()
        {
            _setttionsPopupAnimator.SetTrigger(_cachedSettingsPopupCloseTrigger);
        }
    }
}