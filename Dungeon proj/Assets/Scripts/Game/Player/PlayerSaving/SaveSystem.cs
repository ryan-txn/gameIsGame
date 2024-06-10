using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(CoinController playerCoins, HealthController playerHealth)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savefile.sigma";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData
        {
            coins = playerCoins.coinAmt,
            health = playerHealth.currentHealthNum
        };

        Debug.Log("Saved coins are " + data.coins);
        Debug.Log("Saved health is " + data.health);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/savefile.sigma";
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
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
