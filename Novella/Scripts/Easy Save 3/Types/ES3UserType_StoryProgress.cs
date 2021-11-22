using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("isQLinesFromPreviousActAdded", "heroBodyKey", "heroHairKey", "heroDressKey", "isElixirOn", "bedroom", "partydress", "storyId", "actId", "lastPurchasedAct", "recordId", "items", "qlines", "qlinesActCurrent", "qlinesActLast", "paidQlines", "paidItems", "reputation", "lastActReputation", "globalReputationBeforeAct", "heroDressKeyBeforeAct", "heroHairKeyBeforeAct")]
	public class ES3UserType_StoryProgress : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_StoryProgress() : base(typeof(Scripts.Serializables.User.StoryProgress)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (Scripts.Serializables.User.StoryProgress)obj;
			
			writer.WriteProperty("isQLinesFromPreviousActAdded", instance.isQLinesFromPreviousActAdded, ES3Type_bool.Instance);
			writer.WriteProperty("heroBodyKey", instance.heroBodyKey, ES3Type_string.Instance);
			writer.WriteProperty("heroHairKey", instance.heroHairKey, ES3Type_string.Instance);
			writer.WriteProperty("heroDressKey", instance.heroDressKey, ES3Type_string.Instance);
			writer.WriteProperty("isElixirOn", instance.isElixirOn, ES3Type_bool.Instance);
			writer.WriteProperty("bedroom", instance.bedroom, ES3Type_string.Instance);
			writer.WriteProperty("partydress", instance.partydress, ES3Type_string.Instance);
			writer.WriteProperty("storyId", instance.storyId, ES3Type_string.Instance);
			writer.WriteProperty("actId", instance.actId, ES3Type_int.Instance);
			writer.WriteProperty("lastPurchasedAct", instance.lastPurchasedAct, ES3Type_int.Instance);
			writer.WriteProperty("recordId", instance.recordId, ES3Type_int.Instance);
			writer.WriteProperty("items", instance.items);
			writer.WriteProperty("qlines", instance.qlines);
			writer.WriteProperty("qlinesActCurrent", instance.qlinesActCurrent);
			writer.WriteProperty("qlinesActLast", instance.qlinesActLast);			
			writer.WriteProperty("paidQlines", instance.paidQlines);
			writer.WriteProperty("paidItems", instance.paidItems);
			writer.WriteProperty("reputation", instance.reputation);
			writer.WriteProperty("lastActReputation", instance.lastActReputation);
			writer.WriteProperty("globalReputationBeforeAct", instance.globalReputationBeforeAct, ES3Type_int.Instance);
			writer.WriteProperty("heroDressKeyBeforeAct", instance.heroDressKeyBeforeAct, ES3Type_string.Instance);
			writer.WriteProperty("heroHairKeyBeforeAct", instance.heroHairKeyBeforeAct, ES3Type_string.Instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (Scripts.Serializables.User.StoryProgress)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "isQLinesFromPreviousActAdded":
						instance.isQLinesFromPreviousActAdded = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "heroBodyKey":
						instance.heroBodyKey = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "heroHairKey":
						instance.heroHairKey = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "heroDressKey":
						instance.heroDressKey = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "isElixirOn":
						instance.isElixirOn = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "bedroom":
						instance.bedroom = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "partydress":
						instance.partydress = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "storyId":
						instance.storyId = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "actId":
						instance.actId = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "lastPurchasedAct":
						instance.lastPurchasedAct = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;					
					case "recordId":
						instance.recordId = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "items":
						instance.items = reader.Read<System.Collections.Generic.List<System.String>>();
						break;
					case "qlines":
						instance.qlines = reader.Read<System.Collections.Generic.List<System.Collections.Generic.List<System.String>>>();
						break;
					case "qlinesActCurrent":
						instance.qlinesActCurrent = reader.Read<System.Collections.Generic.List<System.String>>();
						break;
					case "qlinesActLast":
						instance.qlinesActLast = reader.Read<System.Collections.Generic.List<System.String>>();
						break;						
					case "paidQlines":
						instance.paidQlines = reader.Read<System.Collections.Generic.List<System.String>>();
						break;
					case "paidItems":
						instance.paidItems = reader.Read<System.Collections.Generic.List<System.String>>();
						break;
					case "reputation":
						instance.reputation = reader.Read<System.Collections.Generic.Dictionary<System.String, System.Int32>>();
						break;
					case "lastActReputation":
						instance.lastActReputation = reader.Read<System.Collections.Generic.Dictionary<System.String, System.Int32>>();
						break;
					case "globalReputationBeforeAct":
						instance.globalReputationBeforeAct = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "heroDressKeyBeforeAct":
						instance.heroDressKeyBeforeAct = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "heroHairKeyBeforeAct":
						instance.heroHairKeyBeforeAct = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new Scripts.Serializables.User.StoryProgress("0");
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_StoryProgressArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_StoryProgressArray() : base(typeof(Scripts.Serializables.User.StoryProgress[]), ES3UserType_StoryProgress.Instance)
		{
			Instance = this;
		}
	}
}