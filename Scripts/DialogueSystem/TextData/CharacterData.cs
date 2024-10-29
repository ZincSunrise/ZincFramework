using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using ZincFramework.Serialization;
using ZincFramework.Serialization.Events;
using ZincFramework.Binary.Excel;
using ZincFramework.Binary.Serialization;


namespace ZincFramework.DialogueSystem.TextData
{
	public class CharacterData : IExcelData
	{
		IEnumerable IExcelData.Collection => CharacterInfos;
		public Dictionary<int, CharacterInfo> CharacterInfos { get;  } = new ();
	}



	[ZincSerializable(200001)]
	public class CharacterInfo : ISerializable
	{


		static CharacterInfo()
		{
			_codeMap = new (4)
			{
				{nameof(CharaceterId), 1},
				{nameof(CharacterName), 2},
				{nameof(AllDifferential), 3},
				{nameof(LocalNameKey), 4},
			};
			
			
			PropertyInfo[] propertyInfos = typeof(CharacterInfo).GetProperties();
			PropertyInfo propertyInfo;
			int code;
			_setMap = new (4);
			
			propertyInfo = propertyInfos[0];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<CharacterInfo, int>(SerializationUtility.GetSetAction<CharacterInfo, int>(propertyInfo)));
			
			propertyInfo = propertyInfos[1];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<CharacterInfo, string>(SerializationUtility.GetSetAction<CharacterInfo, string>(propertyInfo)));
			
			propertyInfo = propertyInfos[2];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<CharacterInfo, int[]>(SerializationUtility.GetSetAction<CharacterInfo, int[]>(propertyInfo)));
			
			propertyInfo = propertyInfos[3];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<CharacterInfo, string>(SerializationUtility.GetSetAction<CharacterInfo, string>(propertyInfo)));
			
		}

		private readonly static Dictionary<string ,int> _codeMap;
		private readonly static Dictionary<int ,SetActionBase> _setMap;


		[BinaryOrdinal(1)]
		public int CharaceterId { get; set; }

		[BinaryOrdinal(2)]
		public string CharacterName { get; set; }

		[BinaryOrdinal(3)]
		public int[] AllDifferential { get; set; }

		[BinaryOrdinal(4)]
		public string LocalNameKey { get; set; }

		public void Write(ByteWriter byteWriter, SerializerOption serializerOption)
		{
			byteWriter.WriteInt32(_codeMap[nameof(CharaceterId)]);
			byteWriter.WriteInt32(CharaceterId);
			
			byteWriter.WriteInt32(_codeMap[nameof(CharacterName)]);
			byteWriter.WriteString(CharacterName);
			
			if(AllDifferential != null)
			{
				byteWriter.WriteInt32(_codeMap[nameof(AllDifferential)]);
				byteWriter.WriteArray(AllDifferential);
			}
			else
			{
				byteWriter.WriteInt32(int.MinValue);
			}
			
			byteWriter.WriteInt32(_codeMap[nameof(LocalNameKey)]);
			byteWriter.WriteString(LocalNameKey);
			
		}
		public void Read(ref ByteReader byteReader, SerializerOption serializerOption)
		{
			int code;

			code = byteReader.ReadInt32();
			(_setMap[code] as SetAction<CharacterInfo, int>).Invoke(this, byteReader.ReadInt32());
			
			code = byteReader.ReadInt32();
			(_setMap[code] as SetAction<CharacterInfo, string>).Invoke(this, byteReader.ReadString());
			
			if(code != int.MinValue)
			{
				code = byteReader.ReadInt32();
				(_setMap[code] as SetAction<CharacterInfo, int[]>).Invoke(this, byteReader.ReadArray<int>());
			}
			
			code = byteReader.ReadInt32();
			(_setMap[code] as SetAction<CharacterInfo, string>).Invoke(this, byteReader.ReadString());
			
		}
	}
}
