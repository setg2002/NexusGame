using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
    public static void SavePlayer(PlayerCharacterController player, MetroidCameraController cameraController, GameObject[] TextLogs, ItemSave[] items, Boundary[] rooms, GameObject minimap)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/savedata.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player, cameraController, TextLogs, items, rooms, minimap);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/savedata.dat";

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            //Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveAchievements(AchievementManager achievementManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/achievementdata.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        AchievementData achievementData = new AchievementData(achievementManager);

        formatter.Serialize(stream, achievementData);
        stream.Close();
    }

    public static AchievementData LoadAchievements()
    {
        string path = Application.persistentDataPath + "/achievementdata.dat";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            AchievementData achievementData = formatter.Deserialize(stream) as AchievementData;
            stream.Close();

            return achievementData;
        }
        else
        {
            //Debug.LogError("Achievement file not found in " + path);
            return null;
        }
    }


}
