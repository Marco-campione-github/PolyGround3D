using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
     * La struttura della cartella Resources in modo tale che tutto funzioni:
     * Resources/
     *      GameObjects/
     *          0_object
     *          1_objec
     *          ...
     *      Renders/
     *          0_object_render
     *          1_object_render
     *          ...
     *      Materials/
     *          mat1
     *          mat2
     *          ...
     *      ...
     * */
public static class ResourceLoader
{
    public static Object[] getGameObjects()
    {
        return Resources.LoadAll("GameObjects", typeof(GameObject));
    }

    //UNUSED
    public static Object[] getRenders()
    {
        return Resources.LoadAll("Renders", typeof(Sprite));
    }

    public static Object[] getMaterials()
    {
        return Resources.LoadAll("Materials", typeof(Material));
    }

    public static Sprite getRenderByName(string name)
    {
        return Resources.Load<Sprite>("Renders/" + name + "_render");
    }

    public static Material getOtherMaterialsByName(string name)
    {
        return Resources.Load<Material>("OtherMaterials/" + name);
    }

}
