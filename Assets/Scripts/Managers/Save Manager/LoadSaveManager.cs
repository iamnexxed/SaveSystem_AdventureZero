using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Security.Cryptography;

public class LoadSaveManager : MonoBehaviour
{

	// Save game data
	[XmlRoot("GameData")]
	public class GameStateData
	{
		public struct DataTransform
		{
			public float posX;
			public float posY;
			public float posZ;

			public float rotX;
			public float rotY;
			public float rotZ;

			public float scaleX;
			public float scaleY;
			public float scaleZ;
		}
			
		// Data for enemy
		public class DataEnemy
		{
			//Enemy Transform Data
			public DataTransform posRotScale;

			//Enemy ID
			public int enemyID;

			//Health
			public int health;
		}
			
		// Data for player
		public class DataPlayer
		{
            //Transform Data
            public DataTransform posRotScale;

			//Collected cash
			public float collectedCash;

			//Has Collected Gun 01?
			public bool collectedWeapon;

            //Health
            public int health;

			// Lives
			public int lives;
        }

		// Instance variables
		public List<DataEnemy> enemies = new List<DataEnemy>();
		public DataPlayer player = new DataPlayer();


	}

	// Game data to save/load
	public GameStateData gameState = new GameStateData();

	// Encryption
	// Reference link : https://stackoverflow.com/questions/965042/c-sharp-serializing-deserializing-a-des-encrypted-file-from-a-stream

	// Generate Key
	DESCryptoServiceProvider key = new DESCryptoServiceProvider();

	// Saves game data to XML file
	public void Save(string fileName = "GameData.xml")
	{
		// Clear existing enemy data
		// gameState.enemies.Clear();

		// Save game data
		// XmlSerializer serializer = new XmlSerializer(typeof(GameStateData));
		// FileStream stream = new FileStream(fileName, FileMode.Create);
		// serializer.Serialize(stream, gameState);

		// Encrypt the stream
		
		EncryptAndSerialize<GameStateData>(fileName, gameState, key);


		/*stream.Flush();
		stream.Dispose();
		stream.Close();*/
	}

	// Load game data from XML file
	public void Load(string fileName = "GameData.xml")
	{
		// XmlSerializer serializer = new XmlSerializer(typeof(GameStateData));
		// FileStream stream = new FileStream(fileName, FileMode.Open);

		// Decrypt the stream
		gameState = DecryptAndDeserialize<GameStateData>(fileName, key);


		// gameState = serializer.Deserialize(stream) as GameStateData;

		// stream.Flush();
		// stream.Dispose();
		// stream.Close();

	}

	
	public static void EncryptAndSerialize<T>(string filename, T obj, SymmetricAlgorithm key)
	{
		using (FileStream fs = File.Open(filename, FileMode.Create))
		{
			using (CryptoStream cs = new CryptoStream(fs, key.CreateEncryptor(), CryptoStreamMode.Write))
			{
				XmlSerializer xmlser = new XmlSerializer(typeof(T));
				xmlser.Serialize(cs, obj);
			}
		}
	}
	public static T DecryptAndDeserialize<T>(string filename, SymmetricAlgorithm key)
	{
		using (FileStream fs = File.Open(filename, FileMode.Open))
		{
			using (CryptoStream cs = new CryptoStream(fs, key.CreateDecryptor(), CryptoStreamMode.Read))
			{
				XmlSerializer xmlser = new XmlSerializer(typeof(T));
				return (T)xmlser.Deserialize(cs);
			}
		}
	}
}