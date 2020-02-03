using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    //GUARDAR LOS DATOS DEL PLAYER
    public static void SavePlayerData(PlayerController2 player, Inventory2 inventory)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.info";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player, inventory);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    //CARGAR LOS DATOS DEL PLAYER
    public static PlayerData LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/player.info";
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
}
