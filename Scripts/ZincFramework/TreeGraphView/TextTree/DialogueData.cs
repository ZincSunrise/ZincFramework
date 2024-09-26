using GameSystem;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using ZincFramework.Binary;
using ZincFramework.Serialization;
using ZincFramework.Serialization.Events;
using ZincFramework.Binary.Excel;
using ZincFramework.Binary.Serialization;


namespace ZincFramework.TreeGraphView.TextTree
{
	public class DialogueData : IExcelData
	{
		IEnumerable IExcelData.Collection => DialogueInfos;
		public Dictionary<int, DialogueInfo> DialogueInfos { get;  } = new ();
	}



	[ZincSerializable(140001)]
	public class DialogueInfo : ISerializable
	{


		static DialogueInfo()
		{
			_codeMap = new (12)
			{
				{nameof(TextId), 1},
				{nameof(CharacterName), 2},
				{nameof(Differential), 3},
				{nameof(DialogueText), 4},
				{nameof(NextTextId), 5},
				{nameof(ChoiceTexts), 6},
				{nameof(NextTextTreeName), 7},
				{nameof(ConditionExpressions), 8},
				{nameof(EventExpression), 9},
				{nameof(EffectName), 10},
				{nameof(XPosition), 11},
				{nameof(Yposition), 12},
			};
			
			
			PropertyInfo[] propertyInfos = typeof(DialogueInfo).GetProperties();
			PropertyInfo propertyInfo;
			int code;
			_setMap = new (12);
			
			propertyInfo = propertyInfos[0];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, int>(SerializationUtility.GetSetAction<DialogueInfo, int>(propertyInfo)));
			
			propertyInfo = propertyInfos[1];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, string>(SerializationUtility.GetSetAction<DialogueInfo, string>(propertyInfo)));
			
			propertyInfo = propertyInfos[2];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, int>(SerializationUtility.GetSetAction<DialogueInfo, int>(propertyInfo)));
			
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
			_setMap.Add(code, new SetAction<DialogueInfo, string>(SerializationUtility.GetSetAction<DialogueInfo, string>(propertyInfo)));
			
			propertyInfo = propertyInfos[7];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, string[]>(SerializationUtility.GetSetAction<DialogueInfo, string[]>(propertyInfo)));
			
			propertyInfo = propertyInfos[8];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, string[]>(SerializationUtility.GetSetAction<DialogueInfo, string[]>(propertyInfo)));
			
			propertyInfo = propertyInfos[9];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, string[]>(SerializationUtility.GetSetAction<DialogueInfo, string[]>(propertyInfo)));
			
			propertyInfo = propertyInfos[10];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, float>(SerializationUtility.GetSetAction<DialogueInfo, float>(propertyInfo)));
			
			propertyInfo = propertyInfos[11];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, float>(SerializationUtility.GetSetAction<DialogueInfo, float>(propertyInfo)));
			
		}

		private readonly static Dictionary<string ,int> _codeMap;
		private readonly static Dictionary<int ,SetActionBase> _setMap;


		[BinaryOrdinal(1)]
		public int TextId { get; set; }

		[BinaryOrdinal(2)]
		public string CharacterName { get; set; }

		[BinaryOrdinal(3)]
		public int Differential { get; set; }

		[BinaryOrdinal(4)]
		public string DialogueText { get; set; }

		[BinaryOrdinal(5)]
		public int[] NextTextId { get; set; }

		[BinaryOrdinal(6)]
		public string[] ChoiceTexts { get; set; }

		[BinaryOrdinal(7)]
		public string NextTextTreeName { get; set; }

		[BinaryOrdinal(8)]
		public string[] ConditionExpressions { get; set; }

		[BinaryOrdinal(9)]
		public string[] EventExpression { get; set; }

		[BinaryOrdinal(10)]
		public string[] EffectName { get; set; }

		[BinaryOrdinal(11)]
		public float XPosition { get; set; }

		[BinaryOrdinal(12)]
		public float Yposition { get; set; }

		public void Write(ByteWriter byteWriter)
		{
			byteWriter.WriteInt32(_codeMap[nameof(TextId)]);
			byteWriter.WriteInt32(TextId);
			
			if(CharacterName != null)
			{
				byteWriter.WriteInt32(_codeMap[nameof(CharacterName)]);
				byteWriter.WriteString(CharacterName);
			}
			else
			{
				byteWriter.WriteInt32(int.MinValue);
			}

			byteWriter.WriteInt32(_codeMap[nameof(Differential)]);
			byteWriter.WriteInt32(Differential);
			
			if(DialogueText != null)
			{
				byteWriter.WriteInt32(_codeMap[nameof(DialogueText)]);
				byteWriter.WriteString(DialogueText);
			}
			else
			{
				byteWriter.WriteInt32(int.MinValue);
			}

			if(NextTextId != null)
			{
				byteWriter.WriteInt32(_codeMap[nameof(NextTextId)]);
				byteWriter.WriteInt32(NextTextId.Length);
				for(int i = 0;i < NextTextId.Length; i++)
				{
					byteWriter.WriteInt32(NextTextId[i]);
				}
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

			if(NextTextTreeName != null)
			{
				byteWriter.WriteInt32(_codeMap[nameof(NextTextTreeName)]);
				byteWriter.WriteString(NextTextTreeName);
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

			if(EffectName != null)
			{
				byteWriter.WriteInt32(_codeMap[nameof(EffectName)]);
				byteWriter.WriteInt32(EffectName.Length);
				for(int i = 0;i < EffectName.Length; i++)
				{
					byteWriter.WriteString(EffectName[i]);
				}
			}
			else
			{
				byteWriter.WriteInt32(int.MinValue);
			}

			byteWriter.WriteInt32(_codeMap[nameof(XPosition)]);
			byteWriter.WriteSingle(XPosition);
			
			byteWriter.WriteInt32(_codeMap[nameof(Yposition)]);
			byteWriter.WriteSingle(Yposition);
			
		}
		public void Read(ref ByteReader byteReader)
		{
			int code;

			int count;
			code = byteReader.ReadInt32();
			(_setMap[code] as SetAction<DialogueInfo, int>).Invoke(this, byteReader.ReadInt32());
			
			code = byteReader.ReadInt32();
			if(code != int.MinValue)
			{
				(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, byteReader.ReadString());
			}

			code = byteReader.ReadInt32();
			(_setMap[code] as SetAction<DialogueInfo, int>).Invoke(this, byteReader.ReadInt32());
			
			code = byteReader.ReadInt32();
			if(code != int.MinValue)
			{
				(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, byteReader.ReadString());
			}

			code = byteReader.ReadInt32();
			if(code != int.MinValue)
			{
				count = byteReader.ReadInt32();
				int[] tempArray = new int[count];
				for(int i = 0;i < tempArray.Length; i++)
				{
					tempArray[i] = byteReader.ReadInt32();
				}
				(_setMap[code] as SetAction<DialogueInfo, int[]>).Invoke(this, tempArray);
			}

			code = byteReader.ReadInt32();
			if(code != int.MinValue)
			{
				count = byteReader.ReadInt32();
				string[] tempArray = new string[count];
				for(int i = 0;i < tempArray.Length; i++)
				{
					tempArray[i] = byteReader.ReadString();
				}
				(_setMap[code] as SetAction<DialogueInfo, string[]>).Invoke(this, tempArray);
			}

			code = byteReader.ReadInt32();
			if(code != int.MinValue)
			{
				(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, byteReader.ReadString());
			}

			code = byteReader.ReadInt32();
			if(code != int.MinValue)
			{
				count = byteReader.ReadInt32();
				string[] tempArray = new string[count];
				for(int i = 0;i < tempArray.Length; i++)
				{
					tempArray[i] = byteReader.ReadString();
				}
				(_setMap[code] as SetAction<DialogueInfo, string[]>).Invoke(this, tempArray);
			}

			code = byteReader.ReadInt32();
			if(code != int.MinValue)
			{
				count = byteReader.ReadInt32();
				string[] tempArray = new string[count];
				for(int i = 0;i < tempArray.Length; i++)
				{
					tempArray[i] = byteReader.ReadString();
				}
				(_setMap[code] as SetAction<DialogueInfo, string[]>).Invoke(this, tempArray);
			}

			code = byteReader.ReadInt32();
			if(code != int.MinValue)
			{
				count = byteReader.ReadInt32();
				string[] tempArray = new string[count];
				for(int i = 0;i < tempArray.Length; i++)
				{
					tempArray[i] = byteReader.ReadString();
				}
				(_setMap[code] as SetAction<DialogueInfo, string[]>).Invoke(this, tempArray);
			}

			code = byteReader.ReadInt32();
			(_setMap[code] as SetAction<DialogueInfo, float>).Invoke(this, byteReader.ReadSingle());
			
			code = byteReader.ReadInt32();
			(_setMap[code] as SetAction<DialogueInfo, float>).Invoke(this, byteReader.ReadSingle());
			
		}

		public int GetBytesLength()
		{
			int bytesLength = 0;

			
			bytesLength += 4;
			bytesLength += 4;
			
			bytesLength += 4;
			if(CharacterName != null)
			{
				bytesLength += ByteUtility.GetStringLength(CharacterName);
			}

			bytesLength += 4;
			bytesLength += 4;
			
			bytesLength += 4;
			if(DialogueText != null)
			{
				bytesLength += ByteUtility.GetStringLength(DialogueText);
			}

			bytesLength += 4;
			if(NextTextId != null)
			{
				bytesLength += 2;
				bytesLength += NextTextId.Length * 4;
			}

			bytesLength += 4;
			if(ChoiceTexts != null)
			{
				bytesLength += 2;
				for(int i = 0;i < ChoiceTexts.Length; i++)
				{
				bytesLength += ByteUtility.GetStringLength(ChoiceTexts[i]);
				}
			}

			bytesLength += 4;
			if(NextTextTreeName != null)
			{
				bytesLength += ByteUtility.GetStringLength(NextTextTreeName);
			}

			bytesLength += 4;
			if(ConditionExpressions != null)
			{
				bytesLength += 2;
				for(int i = 0;i < ConditionExpressions.Length; i++)
				{
				bytesLength += ByteUtility.GetStringLength(ConditionExpressions[i]);
				}
			}

			bytesLength += 4;
			if(EventExpression != null)
			{
				bytesLength += 2;
				for(int i = 0;i < EventExpression.Length; i++)
				{
				bytesLength += ByteUtility.GetStringLength(EventExpression[i]);
				}
			}

			bytesLength += 4;
			if(EffectName != null)
			{
				bytesLength += 2;
				for(int i = 0;i < EffectName.Length; i++)
				{
				bytesLength += ByteUtility.GetStringLength(EffectName[i]);
				}
			}

			bytesLength += 4;
			bytesLength += 4;
			
			bytesLength += 4;
			bytesLength += 4;
			
			return bytesLength;

		}
	}
}
