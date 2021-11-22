using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using Scripts.Managers;
using Scripts.Serializables.Story;
using DG.Tweening.Plugins;
using Firebase.Firestore;
using JetBrains.Annotations;
using UnityEngine;

namespace Scripts.Serializables.User
{
    [Serializable]
    public class StoryProgress
    {        
        private PlayerItemBase PlayerItemBase => GameManager.Instance.playerInformation.playerItemBase;
        
        public bool isQLinesFromPreviousActAdded;
        public bool IsHeroCreated => heroBodyKey != null;
        
        public string heroBodyKey;
        public BodyDescription HeroBody => PlayerItemBase.GetBodyByKey(heroBodyKey);
        
        public string heroHairKey;
        public ItemDescription HeroHair => PlayerItemBase.GetHairByKey(heroHairKey);
        
        public string heroDressKey;
        public ItemDescription HeroDress => PlayerItemBase.GetDressByKey(heroDressKey);
        public ItemDescription Mask => PlayerItemBase.GetDressByKey("mask");
        
        public bool isElixirOn;
        public string bedroom;
        public string partydress;
        public string storyId;
        public int actId;
        public int lastPurchasedAct = -1;
        public int recordId;
        public List<string> items;
        public List<List<string>> qlines;
        public List<string> qlinesActCurrent;
        public List<string> qlinesActLast;
        public List<string> paidQlines;
        public List<string> paidItems;
        public Dictionary<string, int> reputation;
        public Dictionary<string, int> lastActReputation;
        public int globalReputationBeforeAct;
        public string heroDressKeyBeforeAct;
        public string heroHairKeyBeforeAct;
        private readonly char[] _separator = {';'};       
        
        internal List<string> actQlines
        {
            get
            {
                if (qlines == null)
                {
                    isQLinesFromPreviousActAdded = false;
                    qlines = new List<List<string>>();
                }
                while (qlines.Count <= actId) qlines.Add(new List<string>());
                var newQLines = qlines[actId];
                if (actId <= 0) return newQLines;
                if (isQLinesFromPreviousActAdded) return newQLines;
                newQLines.AddRange(qlines[actId -1]);
                isQLinesFromPreviousActAdded = true;
                return newQLines;
            }
        }
        public StoryProgress(string id)
        {
            storyId = id;
            items = new List<string>();
            qlines = new List<List<string>>();
            paidItems = new List<string>();
            paidQlines = new List<string>();
            reputation = new Dictionary<string, int>();
            lastActReputation = new Dictionary<string, int>();
            reputation.Add("reputation", 0);// чтобы не делать проверку на наличия ключа глобальной репутации
        }

        public void CleanQline()
        {
            qlines[actId] = new List<string>();
        }

        public string GetCurrentChainIndex(Record record)
        {
            string[] splitLines = SplitQline(record.qline[0]);
            return splitLines[1];
        }

        internal bool CanReadRecord(Record currentRecord)
        {
            
            if (currentRecord.qline == null || currentRecord.qline.Length == 0)
            {
                Debug.LogWarning($"Record {currentRecord.row} Qline is null or empty. Can Read");
                return true;
            }

            for (int i = 0; i < currentRecord.qline.Length; i++)
            {        
                string qline = currentRecord.qline[i];
                // In case we are already going through some qline
                int userQlineIdx = IndexOfQlineInProgress(qline);
                if (userQlineIdx != -1)
                {
                    Debug.Log("<color=red>Index found in Progress</color>");
                    SetQlineAtIndex(userQlineIdx, qline);
                    return true;
                }

                // In case we just chose an option IndexOfQlineInProgress will return -1; So we have to search for it differently
                userQlineIdx = actQlines.FindIndex(q => qline.StartsWith(q));
                if (userQlineIdx != -1)
                {
                    Debug.Log("<color=green>Index found in actQlines</color>");
                    string[] recordQlineArray = SplitQline(qline);
                    string[] userQlineArray = SplitQline(actQlines[userQlineIdx]);
                    var _condition = currentRecord.condition;
                    if(!_condition.Equals(new Condition()))
                    {
                        if (recordQlineArray.Length - userQlineArray.Length == 2)
                        {
                            if(!reputation.ContainsKey(_condition.target))// при скрипах в дебаг окне, если репутацию не получил ранее
                            {
                                reputation.Add(_condition.target, 0);
                            }

                            if(_condition.operand == ">")// >=
                            {                                
                                if(reputation[_condition.target] >= _condition.value)
                                {
                                    SetQlineAtIndex(userQlineIdx, qline);
                                    return true; 
                                }
                                return false;
                            }
                            else // <
                            {
                                if(reputation[_condition.target] < _condition.value)
                                {
                                    SetQlineAtIndex(userQlineIdx, qline);
                                    return true;    
                                }
                                return false; 
                            }
                            
                        }    
                    }
                    if (recordQlineArray.Length - userQlineArray.Length == 1)
                    {
                        SetQlineAtIndex(userQlineIdx, qline);
                        return true;
                    }
                    // Если нижний блок будет где-либо что-то ломать,
                    // то нужно добавить в StoryScreen bool поле, где если false
                    // принимает true и запускает не CanReadRecord(), а новый метод в StoryProgress
                    // bool CanReadRecordOnLoadStory() - метод что будет запускаться лишь 1 раз и имеет проверку с нижнего блока
                    // А если вернет false, то запускать уже CanReadRecord()
                    if (recordQlineArray.Length - userQlineArray.Length == 0)//Artem: seems to work good
                    {                        
                        return true;
                    }
                }
                if (SplitQline(qline)?.Length == 1)
                {
                    actQlines.Add(qline);
                    return true;
                }

                //Debug.LogWarning("qline: " + qline);
            }

            //Debug.LogWarning("Cannot read this record");
            return false;
        }


        private void SetQlineAtIndex(int i, string qline)
        {
            if (i != -1)
                actQlines[i] = qline;
            else
                actQlines.Add(qline);
        }

        private string[] SplitQline(string qline)
        {
            return qline.Split(new []{';'}, StringSplitOptions.RemoveEmptyEntries);
        }

        private int IndexOfQlineInProgress(string qline)
        {
            string[] recordQlineArray = SplitQline(qline);
            // E.G. recordQline was 13;2; so now prevQline will be 13; 
            string prevQline = string.Join(";", recordQlineArray.Take(recordQlineArray.Length - 1));
            // user Qline saved to story progress 
            int userQlineIdx = actQlines.FindIndex(q => q.StartsWith(prevQline));


            if (userQlineIdx != -1)
            {
                string userQline = actQlines[userQlineIdx];
                string[] userQlineArray = SplitQline(userQline);
                if (recordQlineArray.Length != userQlineArray.Length)
                    return -1;

                var recordToCompare = recordQlineArray.Take(recordQlineArray.Length - 2);
                var userToCompare = userQlineArray.Take(userQlineArray.Length - 2);
                Debug.LogWarning($"Record Last Element: {recordQlineArray[recordQlineArray.Length-1]} - User Last Element: {userQlineArray[recordQlineArray.Length-1]}");
                // If all elements are the same besides the last one and the last one is bigger by 1;

                if (recordToCompare.SequenceEqual(userToCompare))
                {
                    if (int.TryParse(recordQlineArray.Last(), out int recordQlineArrayLast))
                    {
                        if (int.TryParse(userQlineArray.Last(), out int userQlineArrayLast))
                        {
                            if (recordQlineArrayLast - 1 == userQlineArrayLast)
                            {
                                return userQlineIdx;
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Couldnt Parse user Qline:" + userQlineArray.Last());
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Couldnt Parse record Qline:" + recordQlineArray.Last());
                    }
                }
            }

            return -1;
        }


        internal bool SelectOption(int i, Record currentRecord)
        {
            if (currentRecord.options[i] != null && currentRecord.options[i].qline != null)
            {
                if(currentRecord.options[i].price > 0)
                {
                    string optionQline = string.Join("", currentRecord.options[i].qline);
                    if(!paidQlines.Contains(optionQline))
                    {
                        //paidQlines.Add(currentRecord.options[i].qline);
                        paidQlines.Add(optionQline); 
                    }   
                }
                string qline = currentRecord.options[i].qline[0];
                int userQlineIdx = actQlines.FindIndex(q => qline.StartsWith(q));
                if (userQlineIdx != -1)
                {
                    string[] recordQlineArray = SplitQline(qline);
                    string[] userQlineArray = SplitQline(actQlines[userQlineIdx]);

                    SetQlineAtIndex(recordQlineArray.Length - userQlineArray.Length == 1 ? userQlineIdx : -1, qline);
                }
                else
                {
                    SetQlineAtIndex(IndexOfQlineInProgress(qline), qline);
                }
                //Artem 
                if(currentRecord.options[i].effect != null && currentRecord.options[i].points != 0)
                {
                    if(reputation.ContainsKey(currentRecord.options[i].effect))
                    {
                        reputation[currentRecord.options[i].effect] += currentRecord.options[i].points;
                    }
                    else
                    {
                        reputation.Add(currentRecord.options[i].effect, currentRecord.options[i].points);
                    }
                }
                

                return true;
            }

            return false;
        }
    }


}