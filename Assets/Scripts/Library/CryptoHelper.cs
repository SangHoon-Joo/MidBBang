using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Krafton.SP2.X1.Lib
{
	public class CryptoHelper
	{
		static public byte[] EncryptBytesToBytes(byte[] plainTextByte, string key)
		{
			if (plainTextByte == null || plainTextByte.Length <= 0)
				return null;
			if (key == null || key.Length <= 0)
				return null;

			byte[] salt = Encoding.ASCII.GetBytes(key.Length.ToString());
			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(key, salt);

			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			rijndaelManaged.KeySize = 256;
			rijndaelManaged.BlockSize = 128;
			rijndaelManaged.Mode = CipherMode.CBC;
			rijndaelManaged.Padding = PaddingMode.PKCS7;
			rijndaelManaged.Key = passwordDeriveBytes.GetBytes(32);
			rijndaelManaged.IV = passwordDeriveBytes.GetBytes(16);
				
			byte[] result = null;

			ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor();
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
				{
					cryptoStream.Write(plainTextByte, 0, plainTextByte.Length);
				}
				
				result = memoryStream.ToArray();
			}

			return result;
		}

		static public byte[] DecryptBytesFromBytes(byte[] cipherTextByte, string key)
		{
			if (cipherTextByte == null || cipherTextByte.Length <= 0)
				return null;
			if (key == null || key.Length <= 0)
				return null;

			byte[] salt = Encoding.ASCII.GetBytes(key.Length.ToString());
			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(key, salt);

			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			rijndaelManaged.KeySize = 256;
			rijndaelManaged.BlockSize = 128;
			rijndaelManaged.Mode = CipherMode.CBC;
			rijndaelManaged.Padding = PaddingMode.PKCS7;
			rijndaelManaged.Key = passwordDeriveBytes.GetBytes(32);
			rijndaelManaged.IV = passwordDeriveBytes.GetBytes(16);

			byte[] result = null;

			ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor();
			using (MemoryStream memoryStream = new MemoryStream())
			{
				try
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
					{
						cryptoStream.Write(cipherTextByte, 0, cipherTextByte.Length);
					}

					result = memoryStream.ToArray();
				}
				catch
				{
				}

			}

			return result;
		}

		static public string EncryptStringToString(string plainTextString, string key)
		{
			if (plainTextString == null || plainTextString.Length <= 0)
				return null;
			if (key == null || key.Length <= 0)
				return null;

			byte[] plainTextByte = Encoding.UTF8.GetBytes(plainTextString);

			byte[] result = EncryptBytesToBytes(plainTextByte, key);
			if (result == null || result.Length <= 0)
				return null;
				
			return Convert.ToBase64String(result);
		}

		static public string DecryptStringFromString(string cipherTextString, string key)
		{
			if (cipherTextString == null || cipherTextString.Length <= 0)
				return null;
			if (key == null || key.Length <= 0)
				return null;

			byte[] cipherTextByte = Convert.FromBase64String(cipherTextString);

			byte[] result = DecryptBytesFromBytes(cipherTextByte, key);
			if (result == null || result.Length <= 0)
				return null;

			return Encoding.UTF8.GetString(result);
		}
	}
}
