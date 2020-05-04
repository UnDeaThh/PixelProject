using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    //GUARDAR LOS DATOS DEL PLAYER
    public static void SavePlayerData(PlayerController2 player, Inventory2 inventory,  PlayerAttack plAttack)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerdata.info";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player, inventory, plAttack);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    //CARGAR LOS DATOS DEL PLAYER
    public static PlayerData LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/playerdata.info";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("SaveFile not found in" + path);
            return null;
        }
    }

    //DATOS DEL SCENEMANAGER
    public static void SaveSceneData(ScenesManager SM)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/scenesdata.info";
        FileStream stream = new FileStream(path, FileMode.Create);

        ScenesData sceneData = new ScenesData(SM);
        formatter.Serialize(stream, sceneData);
        stream.Close();
    }

    public static ScenesData LoadSceneData()
    {
        string path = Application.persistentDataPath + "/scenesdata.info";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            ScenesData sceneData = formatter.Deserialize(stream) as ScenesData;
            stream.Close();

            return sceneData;
        }
        else
        {
            Debug.LogError("SaveFile not found in" + path);
            return null;
        }
    }
}
