using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scripts.Serializables.Responses;
using Scripts.Serializables.Story;
using Scripts.Serializables.User;
using Scripts;
using Scripts.UISystem;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Functions;
using Firebase.RemoteConfig;
using GeneratedUI;
using MiniJSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace Scripts.Managers
{
    public class FirebaseManager : GameSingleton<FirebaseManager>
    {
        private FirebaseAuth _auth;
        private FirebaseUser _user;
        private FirebaseApp _app;
        private FirebaseFirestore _db;
        private User _userData;
        
        private FirebaseFunctions _functions;
        public System.Action<bool> OnInitialized;
        public System.Action<bool> OnAuth;
        public System.Action<bool, Story> OnStoryLoaded;
        public System.Action<User> OnUserDataLoaded;
        public System.Action<bool> OnRegister;
        
        public void InitializeFirebase()
        {
            
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    _app = FirebaseApp.DefaultInstance;
                    _db = FirebaseFirestore.DefaultInstance;
                    _functions = FirebaseFunctions.DefaultInstance;
                    OnInitialized.Invoke(true);
                }
                else
                {
                    OnInitialized.Invoke(false);
                    Debug.LogError(System.String.Format(
                        "Could not resolve all Firebase dependencies: {0}",dependencyStatus));
                }
            });
        }

        public void InitializeAuth()
        {
            _auth = FirebaseAuth.DefaultInstance;
            _auth.StateChanged += AuthStateChanged;
        }

        // public void SetUserForVerification()
        // {
        //     _auth = FirebaseAuth.DefaultInstance;
        //     _user = _auth.CurrentUser;
        // }
        private void AuthStateChanged(object sender, System.EventArgs eventArgs)
        {
            Debug.LogWarning("AUTH STATE CHANGED");
            if (_auth.CurrentUser != _user)
            {
                bool signedIn = _user != _auth.CurrentUser && _auth.CurrentUser != null;
                if (!signedIn && _user != null)
                {
                    Debug.Log("Signed out " + _user.UserId);
                    OnAuth?.Invoke(false);
                }
                
                _user = _auth.CurrentUser;
                if (signedIn)
                {
                    Debug.Log($"Signed in as {_user.DisplayName}, email: {_user.Email}, id: {_user.UserId}");
                    if (!_user.IsEmailVerified)
                    {
                        // Calling Reload Async on user because even if user was authenticated before and verified it returns IsEmailVerified FALSE at first.
                        _user.ReloadAsync().ContinueWithOnMainThread(task =>
                        {
                            OnAuth?.Invoke(true);
                            if (!_user.IsEmailVerified)
                            {
                                WindowsManager.Instance.CreateWindow<EmailVerificationWindow>();
                                Debug.Log("User has not verified his email");
                            }
                        });
                    }
                    else
                    {
                        Debug.Log("User has verified his email");
                        OnAuth?.Invoke(true);
                    }
                }
            } else if (_auth.CurrentUser == null)
            {
                OnAuth?.Invoke(false);
            }
        }
        
        public void Login(string email, string password)
        {
            _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    OnAuth?.Invoke(false);
                    return;
                }

                _user = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    _user.DisplayName, _user.UserId);
                OnAuth?.Invoke(true);
            });
            return;
        }

        public void SignOut()
        {
            _auth.SignOut();
        }
        public void Register(string email, string password, string username)
        {
            Dictionary<string, object> userData = new Dictionary<string, object>();
            userData["email"] = email;
            userData["password"] = password;
            userData["username"] = username;
            var createUserFunction = _functions.GetHttpsCallable("createUser");
            createUserFunction.CallAsync(userData).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("Register was canceled.");
                }

                if (task.IsFaulted)
                {
                    FunctionsException e = null;
                    foreach (var inner in task.Exception.InnerExceptions) { 
                        if (inner is Firebase.Functions.FunctionsException) { 
                            e = (FunctionsException)inner;
                            Debug.Log($"<color=red>!! Registration Failed !!</color>\n<color=yellow>ErrorCode:</color> {e.ErrorCode}, <color=yellow>Message:</color> {e.Message} ");
                            break; 
                        } 
                    }
                    
                    if (e == null) { 
                        // We didn't get a proper Functions Exception. 
                        throw task.Exception; 
                    } 
                    
                    if (e.ErrorCode != Firebase.Functions.FunctionsErrorCode.AlreadyExists) { 
                        // The code wasn't right. 
                        throw new Exception(String.Format("Error {0}: {1}", e.ErrorCode, e.Message)); 
                    }
                    
                    OnRegister?.Invoke(false);
                    return;
                }
                if (task.IsCompleted)
                {
                    //string jsonResponse = Json.Serialize(task.Result.Data);
                    string jsonResponse = task.Result.Data.ToString();
                    RegisterResponse response = JsonUtility.FromJson<RegisterResponse>(jsonResponse);
                    if (response.success)
                    {
                        Debug.Log("Successfully Registered User and created a record in Firestore");
                        OnRegister?.Invoke(true);
                    }
                }
            });
        }
        
        private void UpdateUsername(string username)
        {
            _db.Collection("Users").Document(_user.UserId).UpdateAsync("username", username).ContinueWithOnMainThread(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UsernameUpdate was canceled.");
                    return;
                }
            
                if (task.IsFaulted)
                {
                    Debug.LogError("UsernameUpdate encountered an error: " + task.Exception);
                    return;
                }
            });
        }

        public void SendVerificationEmail()
        {
            Debug.Log("Sending Email Verification");
            _user.SendEmailVerificationAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SendVerification Email was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("SendVerification Email encountered an error: " + task.Exception);
                    return;
                }
                Debug.Log("Email Verification Sent");
            });
            
        }

        public void GetUserData()
        {
            _db.Collection("users").Document(_user.UserId).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("GetUserData was canceled.");
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("GetUserData encountered an error: " + task.Exception);
                }
                
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Dictionary<string, object> dict = snapshot.ToDictionary();                       
                    User user = JsonUtility.FromJson<User>(Json.Serialize(dict));

                    List<StoryProgress> progress = new List<StoryProgress>();
                    if(dict.ContainsKey("progress"))
                    {    
                        string[] arrayString = ((System.Collections.IEnumerable)dict["progress"]).Cast<object>().Select(x => x.ToString()).ToArray();
                                             
                        for(int i = 0; i < arrayString.Length; i++)
                        {
                            try
                            {
                                StoryProgress storyProgress = JsonConvert.DeserializeObject<StoryProgress>(arrayString[i]);
                                progress.Add(storyProgress);
                            }
                            catch
                            {
                                Debug.LogWarning($"<color=red>Can't deserialize arrayString{i}</color>:\n{arrayString[i]}");
                            }                            
                        }                     

                    }
                    user.progress = new List<StoryProgress>(progress);

                    OnUserDataLoaded(user);
                } else {
                    Debug.Log("No data for such user");
                }
            });
        }

        public void UpdateUserData()
        {            
            //await _db.Collection("users").Document(_user.UserId).SetAsync(GameManager.Instance.userData, SetOptions.MergeAll);
            UpdateUserCurrencies();
            UpdateUserProgress();
        }
        
        public void UpdateUserCurrencies()
        {
            if(_auth.CurrentUser == null)//_auth.CurrentUser != _user
                return;
            var currencies = GameManager.Instance.userData.currencies;
            Dictionary<string, object> currenciesDict = new Dictionary<string, object>
            {
                { "cash", currencies.cash },
                { "cocktails", currencies.cocktails},
                { "elixirs", currencies.elixirs}
            };
            _db.Collection("users").Document(_user.UserId).UpdateAsync("currencies", currenciesDict);//.ContinueWithOnMainThread(task => {});
        }

        public void UpdateUserProgress()
        {
            if(_auth.CurrentUser == null)//_auth.CurrentUser != _user
                return;
            var storyProgress = GameManager.Instance.userData.progress;
            _db.Collection("users").Document(_user.UserId).UpdateAsync("progress", FieldValue.Delete);             
            for(int i = 0; i < storyProgress.Count; i++)
            {                
                string jsonStory = JsonUtility.ToJson(storyProgress[i]);

                Dictionary<string, object> partialProgressDict = new Dictionary<string, object>
                {
                    { "qlines", storyProgress[i].qlines },
                    { "reputation", storyProgress[i].reputation },
                    { "lastActReputation", storyProgress[i].lastActReputation }
                };

                string secordPart = JsonConvert.SerializeObject(partialProgressDict); 
                JObject jobjectMain = JObject.Parse(jsonStory);
                JObject jobjectSecond = JObject.Parse(secordPart);

                var settings = new JsonMergeSettings {MergeArrayHandling = MergeArrayHandling.Union};
                jobjectMain.Merge(jobjectSecond, settings);

                _db.Collection("users").Document(_user.UserId).UpdateAsync("progress", FieldValue.ArrayRemove(jobjectMain.ToString()));
                _db.Collection("users").Document(_user.UserId).UpdateAsync("progress", FieldValue.ArrayUnion(jobjectMain.ToString()));
            }                
            
        }
        
        public void LoadStory(string storyId)
        {
            _db.Collection("stories").Document(storyId).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogWarning("Task was cancelled");
                    OnStoryLoaded?.Invoke(false, new Story());
                }

                if (task.IsFaulted)
                {
                    Debug.LogWarning("Task was faulted \n" + task.Exception);
                    OnStoryLoaded?.Invoke(false, new Story());
                }
                DocumentSnapshot snapshot = task.Result;
                if (task.IsCompleted && snapshot.Exists)
                {
                    Dictionary<string, object> dict = snapshot.ToDictionary();
                    Story story = JsonUtility.FromJson<Story>(Json.Serialize(dict));
                    OnStoryLoaded?.Invoke(true, story);
                }
            });
        }

        #region fetching config

        public Task FetchRemoteConfig()
        {
            Task fetchTask =
                FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync();
            return fetchTask;
        }

        #endregion

        public void CheckEmailVerified(Action<bool> onUserEmailVerified)
        {
            if (_user.IsEmailVerified)
            {
                onUserEmailVerified(true);
                return;
            }
            _user.ReloadAsync().ContinueWithOnMainThread( task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UsernameUpdate was canceled.");
                }
            
                if (task.IsFaulted)
                {
                    Debug.LogError("UsernameUpdate encountered an error: " + task.Exception);
                }
                onUserEmailVerified(_user.IsEmailVerified);
            });   
        }

        private void OnApplicationQuit()
        {
            UpdateUserData();
        }
    }    
}