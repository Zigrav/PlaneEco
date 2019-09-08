using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MakePNGFromRenderTexture : MonoBehaviour
{
    [SerializeField]
    private new Camera camera = null;

    [SerializeField]
    private string file_name = "";

    [ContextMenu("Save")]
    public void MakeSquarePngFromOurVirtualThingy()
    {
        int sqr = 512;

        camera.aspect = 1.0f;
        // recall that the height is now the "actual" size from now on

        RenderTexture tempRT = new RenderTexture(sqr, sqr, 24);
        tempRT.antiAliasing = 8;

        camera.targetTexture = tempRT;
        camera.Render();

        RenderTexture.active = tempRT;
        Texture2D virtualPhoto =
            new Texture2D(sqr, sqr, TextureFormat.RGBA32, false);
        // false, meaning no need for mipmaps
        virtualPhoto.ReadPixels(new Rect(0, 0, sqr, sqr), 0, 0);

        RenderTexture.active = null; //can help avoid errors 
        camera.targetTexture = null;
        // consider ... Destroy(tempRT);

        byte[] bytes;
        bytes = virtualPhoto.EncodeToPNG();

        System.IO.File.WriteAllBytes(GetFilePath(), bytes);
        // virtualCam.SetActive(false); ... no great need for this.
    }

    private string GetFilePath()
    {
        return Application.persistentDataPath + "/" + file_name + ".png";
    }
}