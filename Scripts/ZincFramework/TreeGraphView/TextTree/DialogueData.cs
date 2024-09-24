using ZincFramework.Binary;
using ZincFramework.Serialization;
using ZincFramework.Serialization.Events;
using ZincFramework.Serialization.Excel;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using GameSystem;


namespace ZincFramework.TreeGraphView.TextTree
{
	public class DialogueData : IExcelData
	{
		IEnumerable IExcelData.Collection => DialogueInfos;
		public Dictionary<int, DialogueInfo> DialogueInfos { get;  } = new ();
	}



	[BinarySerializable(140001)]
	public class DialogueInfo : ISerializable, IConvertable, IAppend
	{


		static DialogueInfo()
		{
			_codeMap = new (13)
			{
				{nameof(TextId), 1},
				{nameof(CharacterName), 2},
				{nameof(Differential), 3},
				{nameof(DialogueText), 4},
				{nameof(NextTextId), 5},
				{nameof(ChoiceTexts), 6},
				{nameof(ConditionExpressions), 7},
				{nameof(EventExpression), 8},
				{nameof(AnimationName), 9},
				{nameof(SoundName), 10},
				{nameof(MusicName), 11},
				{nameof(XPosition), 12},
				{nameof(Yposition), 13},
			};
			
			
			PropertyInfo[] propertyInfos = typeof(DialogueInfo).GetProperties();
			PropertyInfo propertyInfo;
			int code;
			_setMap = new (13);
			
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
			_setMap.Add(code, new SetAction<DialogueInfo, string[]>(SerializationUtility.GetSetAction<DialogueInfo, string[]>(propertyInfo)));
			
			propertyInfo = propertyInfos[7];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, string>(SerializationUtility.GetSetAction<DialogueInfo, string>(propertyInfo)));
			
			propertyInfo = propertyInfos[8];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, string>(SerializationUtility.GetSetAction<DialogueInfo, string>(propertyInfo)));
			
			propertyInfo = propertyInfos[9];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, string>(SerializationUtility.GetSetAction<DialogueInfo, string>(propertyInfo)));
			
			propertyInfo = propertyInfos[10];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, string>(SerializationUtility.GetSetAction<DialogueInfo, string>(propertyInfo)));
			
			propertyInfo = propertyInfos[11];
			code = _codeMap[propertyInfo.Name];
			_setMap.Add(code, new SetAction<DialogueInfo, float>(SerializationUtility.GetSetAction<DialogueInfo, float>(propertyInfo)));
			
			propertyInfo = propertyInfos[12];
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
		public string[] ConditionExpressions { get; set; }

		[BinaryOrdinal(8)]
		public string EventExpression { get; set; }

		[BinaryOrdinal(9)]
		public string AnimationName { get; set; }

		[BinaryOrdinal(10)]
		public string SoundName { get; set; }

		[BinaryOrdinal(11)]
		public string MusicName { get; set; }

		[BinaryOrdinal(12)]
		public float XPosition { get; set; }

		[BinaryOrdinal(13)]
		public float Yposition { get; set; }

		public byte[] Serialize()
		{
			byte[] bytes = new byte[GetBytesLength()];
			int nowIndex = 0;

			ByteAppender.AppendInt32(_codeMap[nameof(TextId)], bytes, ref nowIndex);
			ByteAppender.AppendInt32(TextId, bytes, ref nowIndex);
			
			if(CharacterName != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(CharacterName)], bytes, ref nowIndex);
				ByteAppender.AppendString(CharacterName, bytes, ref nowIndex);
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			ByteAppender.AppendInt32(_codeMap[nameof(Differential)], bytes, ref nowIndex);
			ByteAppender.AppendInt32(Differential, bytes, ref nowIndex);
			
			if(DialogueText != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(DialogueText)], bytes, ref nowIndex);
				ByteAppender.AppendString(DialogueText, bytes, ref nowIndex);
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			if(NextTextId != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(NextTextId)], bytes, ref nowIndex);
				ByteAppender.AppendInt16((short)NextTextId.Length, bytes, ref nowIndex);
				for(int i = 0;i < NextTextId.Length; i++)
				{
					ByteAppender.AppendInt32(NextTextId[i], bytes, ref nowIndex);
				}
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			if(ChoiceTexts != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(ChoiceTexts)], bytes, ref nowIndex);
				ByteAppender.AppendInt16((short)ChoiceTexts.Length, bytes, ref nowIndex);
				for(int i = 0;i < ChoiceTexts.Length; i++)
				{
					ByteAppender.AppendString(ChoiceTexts[i], bytes, ref nowIndex);
				}
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			if(ConditionExpressions != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(ConditionExpressions)], bytes, ref nowIndex);
				ByteAppender.AppendInt16((short)ConditionExpressions.Length, bytes, ref nowIndex);
				for(int i = 0;i < ConditionExpressions.Length; i++)
				{
					ByteAppender.AppendString(ConditionExpressions[i], bytes, ref nowIndex);
				}
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			if(EventExpression != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(EventExpression)], bytes, ref nowIndex);
				ByteAppender.AppendString(EventExpression, bytes, ref nowIndex);
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			if(AnimationName != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(AnimationName)], bytes, ref nowIndex);
				ByteAppender.AppendString(AnimationName, bytes, ref nowIndex);
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			if(SoundName != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(SoundName)], bytes, ref nowIndex);
				ByteAppender.AppendString(SoundName, bytes, ref nowIndex);
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			if(MusicName != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(MusicName)], bytes, ref nowIndex);
				ByteAppender.AppendString(MusicName, bytes, ref nowIndex);
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			ByteAppender.AppendInt32(_codeMap[nameof(XPosition)], bytes, ref nowIndex);
			ByteAppender.AppendFloat(XPosition, bytes, ref nowIndex);
			
			ByteAppender.AppendInt32(_codeMap[nameof(Yposition)], bytes, ref nowIndex);
			ByteAppender.AppendFloat(Yposition, bytes, ref nowIndex);
			
			return bytes;
		}
		public void Deserialize(byte[] bytes)
		{
			int nowIndex = 0;
			int code;

			short count;
			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			(_setMap[code] as SetAction<DialogueInfo, int>).Invoke(this, ByteConverter.ToInt32(bytes, ref nowIndex));
			
			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, ByteConverter.ToString(bytes, ref nowIndex));
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			(_setMap[code] as SetAction<DialogueInfo, int>).Invoke(this, ByteConverter.ToInt32(bytes, ref nowIndex));
			
			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, ByteConverter.ToString(bytes, ref nowIndex));
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				count = ByteConverter.ToInt16(bytes, ref nowIndex);
				int[] tempArray = new int[count];
				for(int i = 0;i < tempArray.Length; i++)
				{
					tempArray[i] = ByteConverter.ToInt32(bytes, ref nowIndex);
				}
				(_setMap[code] as SetAction<DialogueInfo, int[]>).Invoke(this, tempArray);
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				count = ByteConverter.ToInt16(bytes, ref nowIndex);
				string[] tempArray = new string[count];
				for(int i = 0;i < tempArray.Length; i++)
				{
					tempArray[i] = ByteConverter.ToString(bytes, ref nowIndex);
				}
				(_setMap[code] as SetAction<DialogueInfo, string[]>).Invoke(this, tempArray);
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				count = ByteConverter.ToInt16(bytes, ref nowIndex);
				string[] tempArray = new string[count];
				for(int i = 0;i < tempArray.Length; i++)
				{
					tempArray[i] = ByteConverter.ToString(bytes, ref nowIndex);
				}
				(_setMap[code] as SetAction<DialogueInfo, string[]>).Invoke(this, tempArray);
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, ByteConverter.ToString(bytes, ref nowIndex));
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, ByteConverter.ToString(bytes, ref nowIndex));
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, ByteConverter.ToString(bytes, ref nowIndex));
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, ByteConverter.ToString(bytes, ref nowIndex));
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			(_setMap[code] as SetAction<DialogueInfo, float>).Invoke(this, ByteConverter.ToFloat(bytes, ref nowIndex));
			
			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			(_setMap[code] as SetAction<DialogueInfo, float>).Invoke(this, ByteConverter.ToFloat(bytes, ref nowIndex));
			
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
				bytesLength += ByteUtility.GetStringLength(EventExpression);
			}

			bytesLength += 4;
			if(AnimationName != null)
			{
				bytesLength += ByteUtility.GetStringLength(AnimationName);
			}

			bytesLength += 4;
			if(SoundName != null)
			{
				bytesLength += ByteUtility.GetStringLength(SoundName);
			}

			bytesLength += 4;
			if(MusicName != null)
			{
				bytesLength += ByteUtility.GetStringLength(MusicName);
			}

			bytesLength += 4;
			bytesLength += 4;
			
			bytesLength += 4;
			bytesLength += 4;
			
			return bytesLength;

		}
		public void Append(byte[] bytes, ref int nowIndex)
		{
			ByteAppender.AppendInt32(_codeMap[nameof(TextId)], bytes, ref nowIndex);
			ByteAppender.AppendInt32(TextId, bytes, ref nowIndex);
			
			if(CharacterName != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(CharacterName)], bytes, ref nowIndex);
				ByteAppender.AppendString(CharacterName, bytes, ref nowIndex);
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			ByteAppender.AppendInt32(_codeMap[nameof(Differential)], bytes, ref nowIndex);
			ByteAppender.AppendInt32(Differential, bytes, ref nowIndex);
			
			if(DialogueText != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(DialogueText)], bytes, ref nowIndex);
				ByteAppender.AppendString(DialogueText, bytes, ref nowIndex);
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			if(NextTextId != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(NextTextId)], bytes, ref nowIndex);
				ByteAppender.AppendInt16((short)NextTextId.Length, bytes, ref nowIndex);
				for(int i = 0;i < NextTextId.Length; i++)
				{
					ByteAppender.AppendInt32(NextTextId[i], bytes, ref nowIndex);
				}
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			if(ChoiceTexts != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(ChoiceTexts)], bytes, ref nowIndex);
				ByteAppender.AppendInt16((short)ChoiceTexts.Length, bytes, ref nowIndex);
				for(int i = 0;i < ChoiceTexts.Length; i++)
				{
					ByteAppender.AppendString(ChoiceTexts[i], bytes, ref nowIndex);
				}
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			if(ConditionExpressions != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(ConditionExpressions)], bytes, ref nowIndex);
				ByteAppender.AppendInt16((short)ConditionExpressions.Length, bytes, ref nowIndex);
				for(int i = 0;i < ConditionExpressions.Length; i++)
				{
					ByteAppender.AppendString(ConditionExpressions[i], bytes, ref nowIndex);
				}
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			if(EventExpression != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(EventExpression)], bytes, ref nowIndex);
				ByteAppender.AppendString(EventExpression, bytes, ref nowIndex);
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			if(AnimationName != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(AnimationName)], bytes, ref nowIndex);
				ByteAppender.AppendString(AnimationName, bytes, ref nowIndex);
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			if(SoundName != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(SoundName)], bytes, ref nowIndex);
				ByteAppender.AppendString(SoundName, bytes, ref nowIndex);
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			if(MusicName != null)
			{
				ByteAppender.AppendInt32(_codeMap[nameof(MusicName)], bytes, ref nowIndex);
				ByteAppender.AppendString(MusicName, bytes, ref nowIndex);
			}
			else
			{
				ByteAppender.AppendInt32(int.MinValue, bytes, ref nowIndex);
			}

			ByteAppender.AppendInt32(_codeMap[nameof(XPosition)], bytes, ref nowIndex);
			ByteAppender.AppendFloat(XPosition, bytes, ref nowIndex);
			
			ByteAppender.AppendInt32(_codeMap[nameof(Yposition)], bytes, ref nowIndex);
			ByteAppender.AppendFloat(Yposition, bytes, ref nowIndex);
			
		}
		public void Convert(byte[] bytes, ref int nowIndex)
		{
			int code;

			short count;
			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			(_setMap[code] as SetAction<DialogueInfo, int>).Invoke(this, ByteConverter.ToInt32(bytes, ref nowIndex));
			
			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, ByteConverter.ToString(bytes, ref nowIndex));
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			(_setMap[code] as SetAction<DialogueInfo, int>).Invoke(this, ByteConverter.ToInt32(bytes, ref nowIndex));
			
			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, ByteConverter.ToString(bytes, ref nowIndex));
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				count = ByteConverter.ToInt16(bytes, ref nowIndex);
				int[] tempArray = new int[count];
				for(int i = 0;i < tempArray.Length; i++)
				{
					tempArray[i] = ByteConverter.ToInt32(bytes, ref nowIndex);
				}
				(_setMap[code] as SetAction<DialogueInfo, int[]>).Invoke(this, tempArray);
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				count = ByteConverter.ToInt16(bytes, ref nowIndex);
				string[] tempArray = new string[count];
				for(int i = 0;i < tempArray.Length; i++)
				{
					tempArray[i] = ByteConverter.ToString(bytes, ref nowIndex);
				}
				(_setMap[code] as SetAction<DialogueInfo, string[]>).Invoke(this, tempArray);
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				count = ByteConverter.ToInt16(bytes, ref nowIndex);
				string[] tempArray = new string[count];
				for(int i = 0;i < tempArray.Length; i++)
				{
					tempArray[i] = ByteConverter.ToString(bytes, ref nowIndex);
				}
				(_setMap[code] as SetAction<DialogueInfo, string[]>).Invoke(this, tempArray);
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, ByteConverter.ToString(bytes, ref nowIndex));
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, ByteConverter.ToString(bytes, ref nowIndex));
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, ByteConverter.ToString(bytes, ref nowIndex));
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			if(code != int.MinValue)
			{
				(_setMap[code] as SetAction<DialogueInfo, string>).Invoke(this, ByteConverter.ToString(bytes, ref nowIndex));
			}

			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			(_setMap[code] as SetAction<DialogueInfo, float>).Invoke(this, ByteConverter.ToFloat(bytes, ref nowIndex));
			
			code = ByteConverter.ToInt32(bytes, ref nowIndex);
			(_setMap[code] as SetAction<DialogueInfo, float>).Invoke(this, ByteConverter.ToFloat(bytes, ref nowIndex));
			
		}
	}
}
