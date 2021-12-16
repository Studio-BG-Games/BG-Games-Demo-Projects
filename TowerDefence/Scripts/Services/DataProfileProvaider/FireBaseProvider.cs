using System;
using System.Linq;
using System.Threading.Tasks;
using DefaultNamespace.Ad;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Gameplay.Builds;
using Gameplay.HubObject.Data;
using Gameplay.Localizators;
using Interface;
using Newtonsoft.Json;
using Plugins.HabObject;
using UnityEngine;

namespace DefaultNamespace.Infrastructure.Data
{
    public class FireBaseProvider : SaveDataProvider, IActionFireBase
    {
        private FirebaseAuth _firebaseAuth;
        private FirebaseUser _firebaseUser;
        private DataSnapshot _snapshotUser;
        private DataSnapshot _snapshotSettingAp;

        public event Action Connected;
        public event Action FailedConnected;

        public void Connect()
        {
            //Ненавижу работу с сетью! ХОЧУ ДЕЛАТЬ РОГАЛИКИ! Одиночные! Сюжетные, дай боже!
            //
            //Коль ты сюда залез, значит что то не работает. Раскрывай коменты с Debug и смотри пошагово, где и что ломается.
            //
            CheckUser();
            FirebaseAuth.DefaultInstance.StateChanged += OnAuthStateChange;
        }

        private void CheckUser()
        {
            if (FirebaseAuth.DefaultInstance.CurrentUser != null)
            {
                _firebaseAuth = FirebaseAuth.DefaultInstance;
                _firebaseUser = _firebaseAuth.CurrentUser;
                GetRefRootUserId(_firebaseUser);
            }
            else
            {
                _firebaseAuth = FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance);
                _firebaseAuth.SignInAnonymouslyAsync().ContinueWithOnMainThread(x =>
                {
                    _firebaseUser = x.Result;
                    if (x.IsFaulted || x.IsCanceled)
                    {
                        //Debug.Log("Fail auto");
                        FailedConnected?.Invoke();
                        return;
                    }
                    //Debug.Log("Succes auto");
                    GetRefRootUserId(_firebaseUser);
                });
            }
        }

        private void GetRefRootUserId(FirebaseUser user)
        {
            var referenceUsers = FirebaseDatabase.DefaultInstance.GetReference($"users/{user.UserId}");
            var referenceSettingAp = FirebaseDatabase.DefaultInstance.GetReference("SettingApp");

            referenceSettingAp.ChildChanged += (x, y) => _snapshotSettingAp = y.Snapshot;
            referenceSettingAp.ValueChanged += (x, y) => _snapshotSettingAp = y.Snapshot;
            referenceSettingAp.GetValueAsync().ContinueWithOnMainThread(t =>
            {
                _snapshotSettingAp = t.Result;
                //Debug.Log($"ref has get by {user.UserId}");
                referenceUsers.ChildChanged += OnUserChildChange;
                referenceUsers.ValueChanged += OnValueChange;
                referenceUsers.GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    _snapshotUser = task.Result;
                    Connected?.Invoke();
                });
            });
        }

        private void OnValueChange(object sender, ValueChangedEventArgs e)
        {
            //Debug.Log("Update snapshot by value");
            _snapshotUser = e.Snapshot;
        }

        private void OnUserChildChange(object sender, ChildChangedEventArgs e)
        {
            //Debug.Log("Update snapshot by child");
            _snapshotUser = e.Snapshot;
        }

        private void OnAuthStateChange(object sender, EventArgs e)
        {
            CheckUser();
        }

        private DatabaseReference GetRefInUserId(string path)
        {
            return FirebaseDatabase.DefaultInstance.GetReference($"users/{_firebaseUser.UserId}/{path}");
        }

        private DatabaseReference GetSomeRef(string refe) => FirebaseDatabase.DefaultInstance.GetReference(refe);

        public override TarData GetOrNull<T, Tar, TarData>(Tar habObject)
        {
            string id = "";
            if (habObject is HabObject)
                id = (habObject as HabObject).MainDates.GetOrNull<IdContainer>().ID;
            else
                id = ((IIDContainer) habObject).ID;
            var path1 = "SaveDataProfile";
            var path2 = id;
            
            if (_snapshotUser.HasChild(path1)==false)
            {
                //Debug.Log($"path1 ({path1}) none - {habObject}");
                return null;
            }
            if (_snapshotUser.Child(path1).HasChild(path2)==false)
            {
                //Debug.Log($"path2 ({path2}) none - {habObject}");
                return null;
            }
            //Debug.Log(_snapshot.Child(path1).Child(path2));
            var json = _snapshotUser.Child(path1).Child(path2).GetRawJsonValue();
            //Debug.Log($"Load id - {id} - result = {json}");
            return JsonConvert.DeserializeObject<TarData>(json);
        }

        public override void Save<T, Tar, TarData>(Tar habObject, TarData dataToSave)
        {
            string id = "";
            if (habObject is HabObject)
                id = (habObject as HabObject).MainDates.GetOrNull<IdContainer>().ID;
            else
                id = ((IIDContainer) habObject).ID;
            var reference = GetRefInUserId($"SaveDataProfile/{id}");
            var saveData = JsonConvert.SerializeObject(dataToSave);
           // Debug.Log($"Save data {saveData}");
            reference.SetRawJsonValueAsync(saveData).ContinueWithOnMainThread(x =>
            {
                //Debug.Log("Saved");
                if (x.IsFaulted || x.IsCanceled) Debug.Log($"some gond wrong with save with this id {id},\n {saveData}");
            });
        }

        public override void Save(ISaveData data)
        {
            var path1 = "ISaveData";
            var path2 = data.NameFile;
            var reference = GetRefInUserId($"{path1}/{path2}");
            var saveData = JsonConvert.SerializeObject(data);
            reference.SetRawJsonValueAsync(saveData).ContinueWithOnMainThread(x =>
            {
                if (x.IsFaulted || x.IsCanceled) Debug.Log($"some gond wrong with save with this id {data.NameFile},\n {saveData}");
            });
        }

        public override T GetOrDefault<T>()
        {
            var path1 = "ISaveData";
            var def = new T();
            var path2 = def.NameFile;
            if (!_snapshotUser.HasChild(path1))
                return def;
            if (!_snapshotUser.Child(path1).HasChild(path2))
                return def;
            var json = _snapshotUser.Child(path1).Child(path2).GetRawJsonValue();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public void SendFeedBack(string mes)
        {
            var feedRef = GetSomeRef("Feedbacks").Push();
            var data = new FeedbackData();
            data.Message = mes;
            data.FromEditor = Application.isEditor;
            data.IDUsers = _firebaseUser.UserId;
            data.Data = DateTime.Now.ToString();
            feedRef.SetRawJsonValueAsync(JsonConvert.SerializeObject(data));
        }

        public AdIdRewarded GetRewardedID(string name)
        {
            var reference = GetSomeRef("SettingApp/AdIdRewarded");
            var fullref = "AdIdRewarded/" + name;
            if (_snapshotSettingAp.HasChild("AdIdRewarded"))
            {
                if (_snapshotSettingAp.Child("AdIdRewarded").HasChild(name))
                {
                    var result = JsonConvert.DeserializeObject<AdIdRewarded>(_snapshotSettingAp.Child("AdIdRewarded").Child(name).GetRawJsonValue());
                    if (result == null)
                        throw new Exception("Null from BaseData!!!");
                    return result;
                }
            }
            
            {
                var defaul = new AdIdRewarded(name);
                GetSomeRef("SettingApp/"+fullref).SetRawJsonValueAsync(JsonConvert.SerializeObject(defaul)).ContinueWithOnMainThread(x =>
                {
                    if (x.IsFaulted || x.IsCanceled) Debug.Log($"some gond wrong with save with this id,\n {defaul}");
                });
                return defaul;
            }
        }

        public AdIdRewardedWithNameAndCount GetExtendedRewardedId(string name)
        {
            var reference = GetSomeRef("SettingApp/AdIdRewardedWithNameAndCount");
            var fullRef = "AdIdRewardedWithNameAndCount/" + name;
            if (_snapshotSettingAp.HasChild("AdIdRewardedWithNameAndCount"))
            {
                if (_snapshotSettingAp.Child("AdIdRewardedWithNameAndCount").HasChild(name))
                {
                    var result = JsonConvert.DeserializeObject<AdIdRewardedWithNameAndCount>(_snapshotSettingAp.Child("AdIdRewardedWithNameAndCount").Child(name).GetRawJsonValue());
                    if (result == null)
                        throw new Exception("Null from BaseData!!!");
                    return result;
                }
            }
            
            {
                var defaul = new AdIdRewardedWithNameAndCount(name);
                GetSomeRef("SettingApp/"+fullRef).SetRawJsonValueAsync(JsonConvert.SerializeObject(defaul)).ContinueWithOnMainThread(x =>
                {
                    if (x.IsFaulted || x.IsCanceled) Debug.Log($"some gond wrong with save with this id,\n {defaul}");
                });
                return defaul;
            }
        }

        private class FeedbackData
        {
            public string Message;
            public string Data;
            public bool FromEditor;
            public string IDUsers;
        }
    }
}