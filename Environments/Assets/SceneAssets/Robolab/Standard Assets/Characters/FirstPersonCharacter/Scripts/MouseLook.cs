using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson {
  [Serializable]
  public class MouseLook {
    public bool clampVerticalRotation = true;
    public bool lockCursor = true;
    Quaternion m_CameraTargetRot;

    Quaternion m_CharacterTargetRot;
    bool m_cursorIsLocked = true;
    public float MaximumX = 90F;
    public float MinimumX = -90F;
    public bool smooth;
    public float smoothTime = 5f;
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;

    public void Init(Transform character, Transform camera) {
      this.m_CharacterTargetRot = character.localRotation;
      this.m_CameraTargetRot = camera.localRotation;
    }

    public void LookRotation(Transform character, Transform camera) {
      var yRot = CrossPlatformInputManager.GetAxis(name : "Mouse X") * this.XSensitivity;
      var xRot = CrossPlatformInputManager.GetAxis(name : "Mouse Y") * this.YSensitivity;

      if (this.m_cursorIsLocked) {
        this.m_CharacterTargetRot *= Quaternion.Euler(
                                                      x : 0f,
                                                      y : yRot,
                                                      z : 0f);
        this.m_CameraTargetRot *= Quaternion.Euler(
                                                   x : -xRot,
                                                   y : 0f,
                                                   z : 0f);
      }

      if (this.clampVerticalRotation)
        this.m_CameraTargetRot = this.ClampRotationAroundXAxis(q : this.m_CameraTargetRot);

      if (this.smooth) {
        character.localRotation = Quaternion.Slerp(
                                                   a : character.localRotation,
                                                   b : this.m_CharacterTargetRot,
                                                   t : this.smoothTime * Time.deltaTime);
        camera.localRotation = Quaternion.Slerp(
                                                a : camera.localRotation,
                                                b : this.m_CameraTargetRot,
                                                t : this.smoothTime * Time.deltaTime);
      } else {
        character.localRotation = this.m_CharacterTargetRot;
        camera.localRotation = this.m_CameraTargetRot;
      }

      this.UpdateCursorLock();
    }

    public void SetCursorLock(bool value) {
      this.lockCursor = value;
      if (!this.lockCursor) {
        //we force unlock the cursor if the user disable the cursor locking helper
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
      }
    }

    public void UpdateCursorLock() {
      //if the user set "lockCursor" we check & properly lock the cursos
      if (this.lockCursor) this.InternalLockUpdate();
    }

    void InternalLockUpdate() {
      if (Input.GetKeyUp(key : KeyCode.Escape))
        this.m_cursorIsLocked = false;
      else if (Input.GetMouseButtonUp(button : 0))
        this.m_cursorIsLocked = true;

      if (this.m_cursorIsLocked) {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
      } else if (!this.m_cursorIsLocked) {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
      }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q) {
      q.x /= q.w;
      q.y /= q.w;
      q.z /= q.w;
      q.w = 1.0f;

      var angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(f : q.x);

      angleX = Mathf.Clamp(
                           value : angleX,
                           min : this.MinimumX,
                           max : this.MaximumX);

      q.x = Mathf.Tan(f : 0.5f * Mathf.Deg2Rad * angleX);

      return q;
    }
  }
}
