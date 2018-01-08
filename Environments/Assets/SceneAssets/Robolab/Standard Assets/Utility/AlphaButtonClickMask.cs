using UnityEngine;
using UnityEngine.UI;

public class AlphaButtonClickMask : MonoBehaviour,
                                    ICanvasRaycastFilter {
  protected Image _image;

  public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera) {
    Vector2 localPoint;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                                                            rect : this._image.rectTransform,
                                                            screenPoint : sp,
                                                            cam : eventCamera,
                                                            localPoint : out localPoint);

    var pivot = this._image.rectTransform.pivot;
    var normalizedLocal = new Vector2(
                                      x : pivot.x + localPoint.x / this._image.rectTransform.rect.width,
                                      y : pivot.y + localPoint.y / this._image.rectTransform.rect.height);
    var uv = new Vector2(
                         x : this._image.sprite.rect.x + normalizedLocal.x * this._image.sprite.rect.width,
                         y : this._image.sprite.rect.y + normalizedLocal.y * this._image.sprite.rect.height);

    uv.x /= this._image.sprite.texture.width;
    uv.y /= this._image.sprite.texture.height;

    //uv are inversed, as 0,0 or the rect transform seem to be upper right, then going negativ toward lower left...
    var c = this._image.sprite.texture.GetPixelBilinear(
                                                        u : uv.x,
                                                        v : uv.y);

    return c.a > 0.1f;
  }

  public void Start() {
    this._image = this.GetComponent<Image>();

    var tex = this._image.sprite.texture;

    var isInvalid = false;
    if (tex != null)
      try {
        tex.GetPixels32();
      } catch (UnityException e) {
        Debug.LogError(message : e.Message);
        isInvalid = true;
      }
    else
      isInvalid = true;

    if (isInvalid) Debug.LogError(message : "This script need an Image with a readbale Texture2D to work.");
  }
}
