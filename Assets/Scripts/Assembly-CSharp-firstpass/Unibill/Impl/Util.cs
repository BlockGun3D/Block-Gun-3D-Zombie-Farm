using System.IO;

namespace Unibill.Impl
{
	public class Util
	{
		public static string ReadAllText(string path)
		{
			//Discarded unreachable code: IL_0013
			using (StreamReader streamReader = new StreamReader(path))
			{
				return streamReader.ReadToEnd();
			}
		}

		public static void WriteAllText(string path, string text)
		{
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				streamWriter.Write(text);
			}
		}
	}
}
