using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using ZincFramework.Serialization;
using ZincFramework.Serialization.Events;
using ZincFramework.Binary.Excel;
using ZincFramework.Binary.Serialization;



namespace ZincFramework.DialogueSystem.TextData
{
	public class DialogueData : IExcelData
	{
		IEnumerable IExcelData.Collection => DialogueInfos;
		public Dictionary<int, DialogueInfo> DialogueInfos { get;  } = new ();
	}



	[ZincSerializable(200000)]
	public class DialogueInfo : ISerializable
	{


		static DialogueInfo()
		{
			_codeMap = new (11)
			{
				{nameof(TextId), 1},
				{nameof(CharacterName), 2},
				{nameof(VisibleStates), 3},
				{nameof(DialogueText), 4},
				{nameof(NextTextId), 5},
				{nameof(ChoiceTexts), 6},
				{nameof(ConditionExpressions), 7},
				{nameof(EventExpression), 8},
				{nameof(EffectNames), 9},
				{nameof(BackgroundName), 10},
				{nameof(Postion), 11},
			};
			
			
			PropertyInfo[] propertyInfos = typeof(DialogueInfo).GetProperties();
			PropertyInfo propertyInfo;
			int code;
			_setMap = new (11);
			
			propertyInfo = propertyInfos[0];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, int>(SerializationUtility.GetSetAction<DialogueInfo, int>(propertyInfo)));
			
			propertyInfo = propertyInfos[1];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, string>(SerializationUtility.GetSetAction<DialogueInfo, string>(propertyInfo)));
			
			propertyInfo = propertyInfos[2];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, VisibleState[]>(SerializationUtility.GetSetAction<DialogueInfo, VisibleState[]>(propertyInfo)));
			
			propertyInfo = propertyInfos[3];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, string>(SerializationUtility.GetSetAction<DialogueInfo, string>(propertyInfo)));
			
			propertyInfo = propertyInfos[4];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, int[]>(SerializationUtility.GetSetAction<DialogueInfo, int[]>(propertyInfo)));
			
			propertyInfo = propertyInfos[5];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, string[]>(SerializationUtility.GetSetAction<DialogueInfo, string[]>(propertyInfo)));
			
			propertyInfo = propertyInfos[6];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, string[]>(SerializationUtility.GetSetAction<DialogueInfo, string[]>(propertyInfo)));
			
			propertyInfo = propertyInfos[7];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, string[]>(SerializationUtility.GetSetAction<DialogueInfo, string[]>(propertyInfo)));
			
			propertyInfo = propertyInfos[8];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, string[]>(SerializationUtility.GetSetAction<DialogueInfo, string[]>(propertyInfo)));
			
			propertyInfo = propertyInfos[9];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, string>(SerializationUtility.GetSetAction<DialogueInfo, string>(propertyInfo)));
			
			propertyInfo = propertyInfos[10];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, UnityEngine.Vector2>(SerializationUtility.GetSetAction<DialogueInfo, UnityEngine.Vector2>(propertyInfo)));
			
		}

		private readonly static Dictionary<string ,int> _codeMap;
		private readonly static Dictionary<int ,SetActionBase> _setMap;


		[BinaryOrdinal(1)]
		public int TextId { get; set; }

		[BinaryOrdinal(2)]
		public string CharacterName { get; set; }

		[BinaryOrdinal(3)]
		public ZincFramework.DialogueSystem.TextData.VisibleState[] VisibleStates { get; set; }

		[BinaryOrdinal(4)]
		public string DialogueText { get; set; }

		[BinaryOrdinal(5)]
		public int[] NextTextId { get; set; }

		[BinaryOrdinal(6)]
		public string[] ChoiceTexts { get; set; }

		[BinaryOrdinal(7)]
		public string[] ConditionExpressions { get; set; }

		[BinaryOrdinal(8)]
		public string[] EventExpression { get; set; }

		[BinaryOrdinal(9)]
		public string[] EffectNames { get; set; }

		[BinaryOrdinal(10)]
		public string BackgroundName { get; set; }

		[BinaryOrdinal(11)]
		public UnityEngine.Vector2 Postion { get; set; }

		public void Write(ByteWriter byteWriter, SerializerOption serializerOption)
		{
			byteWriter.WriteInt32(_codeMap[nameof(TextId)]);
			byteWriter.WriteInt32(TextId);
			
			byteWriter.WriteInt32(_codeMap[nameof(CharacterName)]);
			byteWriter.WriteString(CharacterName);
			
			if(VisibleStates != null)
			{
				byteWriter.WriteInt32(_codeMap[nameof(VisibleStates)]);
				byteWriter.WriteArray(VisibleStates);
			}
			else
			{
				byteWriter.WriteInt32(int.MinValue);
			}
			
			byteWriter.WriteInt32(_codeMap[nameof(DialogueText)]);
			byteWriter.WriteString(DialogueText);
			
			if(NextTextId != null)
			{
				byteWriter.WriteInt32(_codeMap[nameof(NextTextId)]);
				byteWriter.WriteArray(NextTextId);
			}
			else
			{
				byteWriter.WriteInt32(int.MinValue);
			}
			
			if(ChoiceTexts != null)
			{
				byteWriter.WriteInt32(_codeMap[nameof(ChoiceTexts)]);
				byteWriter.WriteInt32(ChoiceTexts.Length);
				for(int i = 0;i < ChoiceTexts.Length; i++)
				{
					byteWriter.WriteString(ChoiceTexts[i]);
			
				}
			}
			else
			{
				byteWriter.WriteInt32(int.MinValue);
			}
			
			if(ConditionExpressions != null)
			{
				byteWriter.WriteInt32(_codeMap[nameof(ConditionExpressions)]);
				byteWriter.WriteInt32(ConditionExpressions.Length);
				for(int i = 0;i < ConditionExpressions.Length; i++)
				{
					byteWriter.WriteString(ConditionExpressions[i]);
			
				}
			}
			else
			{
				byteWriter.WriteInt32(int.MinValue);
			}
			
			if(EventExpression != null)
			{
				byteWriter.WriteInt32(_codeMap[nameof(EventExpression)]);
				byteWriter.WriteInt32(EventExpression.Length);
				for(int i = 0;i < EventExpression.Length; i++)
				{
					byteWriter.WriteString(EventExpression[i]);
			
				}
			}
			else
			{
				byteWriter.WriteInt32(int.MinValue);
			}
			
			if(EffectNames != null)
			{
				byteWriter.WriteInt32(_codeMap[nameof(EffectNames)]);
				byteWriter.WriteInt32(EffectNames.Length);
				for(int i = 0;i < EffectNames.Length; i++)
				{
					byteWriter.WriteString(EffectNames[i]);
			
				}
			}
			else
			{
				byteWriter.WriteInt32(int.MinValue);
			}
			
			byteWriter.WriteInt32(_codeMap[nameof(BackgroundName)]);
			byteWriter.WriteString(BackgroundName);
			
			byteWriter.WriteInt32(_codeMap[nameof(Postion)]);
			byteWriter.WriteBlittable(Postion);
			
		}
		public void Read(ref ByteReader byteReader, SerializerOption serializerOption)
		{
			int code;

			code = byteReader.ReadInt32();
			(_setMap[code] as SetAction<DialogueInfo, int>).Invoke(this, byteReader.ReadInt32());
			
			code = byteReader.ReadInt32();
			(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, byteReader.ReadString());
			
			if(code != int.MinValue)
			{
				code = byteReader.ReadInt32();
				(_setMap[code] as SetAction<DialogueInfo, VisibleState[]>).Invoke(this, byteReader.ReadArray<VisibleState>());
			}
			
			code = byteReader.ReadInt32();
			(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, byteReader.ReadString());
			
			if(code != int.MinValue)
			{
				code = byteReader.ReadInt32();
				(_setMap[code] as SetAction<DialogueInfo, int[]>).Invoke(this, byteReader.ReadArray<int>());
			}
			
			if(code != int.MinValue)
			{
				code = byteReader.ReadInt32();
				int count = byteReader.ReadInt32();
				string[] tempArray = count == 0 ? Array.Empty<string>() : new string[count];
				for(int i = 0;i < tempArray.Length; i++)
				{
					tempArray[i] = byteReader.ReadString();
			
				}
				(_setMap[code] as SetAction<DialogueInfo, string[]>).Invoke(this, tempArray);
			}
			
			if(code != int.MinValue)
			{
				code = byteReader.ReadInt32();
				int count = byteReader.ReadInt32();
				string[] tempArray = count == 0 ? Array.Empty<string>() : new string[count];
				for(int i = 0;i < tempArray.Length; i++)
				{
					tempArray[i] = byteReader.ReadString();
			
				}
				(_setMap[code] as SetAction<DialogueInfo, string[]>).Invoke(this, tempArray);
			}
			
			if(code != int.MinValue)
			{
				code = byteReader.ReadInt32();
				int count = byteReader.ReadInt32();
				string[] tempArray = count == 0 ? Array.Empty<string>() : new string[count];
				for(int i = 0;i < tempArray.Length; i++)
				{
					tempArray[i] = byteReader.ReadString();
			
				}
				(_setMap[code] as SetAction<DialogueInfo, string[]>).Invoke(this, tempArray);
			}
			
			if(code != int.MinValue)
			{
				code = byteReader.ReadInt32();
				int count = byteReader.ReadInt32();
				string[] tempArray = count == 0 ? Array.Empty<string>() : new string[count];
				for(int i = 0;i < tempArray.Length; i++)
				{
					tempArray[i] = byteReader.ReadString();
			
				}
				(_setMap[code] as SetAction<DialogueInfo, string[]>).Invoke(this, tempArray);
			}
			
			code = byteReader.ReadInt32();
			(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, byteReader.ReadString());
			
			code = byteReader.ReadInt32();
			(_setMap[code] as SetAction<DialogueInfo, UnityEngine.Vector2>).Invoke(this, byteReader.ReadBlittable<UnityEngine.Vector2>());
			
		}
	}
}
