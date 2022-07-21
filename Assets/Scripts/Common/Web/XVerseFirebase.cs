using Firebase;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class XVerseFirebase : MonoBehaviour
{
    private static XVerseFirebase _instance = null;
    public static XVerseFirebase Instance
    {
        get => _instance;
    }

    public string Username;
    public string Email;
    public string Pass;

    //cache token
    private string token;
    public string UserToken
    {
        get
        {
            if (!string.IsNullOrEmpty(token)) return token;
            else
            {
                Debug.LogError("you must refresh the token first");
                return null;
            }
        }
    }
    private FirebaseAuth auth;
    private FirebaseUser user;

    private void Awake()
    {
        if (_instance is null) _instance = this;
    }

    private void Start()
    {
        StartCoroutine(CheckAndFixDependanies());
        
    }

    private IEnumerator CheckAndFixDependanies()
    {
        var checkAndFixDependanciesTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(predicate: () => checkAndFixDependanciesTask.IsCompleted);

        var dependancyResult = checkAndFixDependanciesTask.Result;

        if (dependancyResult == DependencyStatus.Available)
        {
            Debug.Log("Successfully started firebase");
            InitializeFirebase();
        }
        else { Debug.LogError($"Could not resolve all Firebase dependancies: {dependancyResult}"); }
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != null)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed Out");
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log($"Signed In: {user.DisplayName}");
            }
        }
    }


    private void OnGUI()
    {
        //test code
        if (GUI.Button(new Rect(20, 20, 100, 20), "register")) StartCoroutine(RegisterLogic(Username, Email, Pass));
        if (GUI.Button(new Rect(20, 45, 100, 20), "Ping Server"))
        {
            if (string.IsNullOrEmpty(UserToken))
            {
                Debug.LogError("you must first refresh the token before sending a web request");
                return;
            }
            StartCoroutine(XVerseWeb.MakeWebRequest(
            XVerseWeb.XVerseServiceEndPoint + "/auth", XVerseWeb.XVerseRequestTypes.GET, 
            (res) => Debug.Log(res.ResponseString),
            (fail) => Debug.LogWarning(fail.Message),
            UserToken
        ));
        }
        if (GUI.Button(new Rect(20, 70, 100, 20), "signIn")) StartCoroutine(LoginLogic(Email, Pass));
        if (GUI.Button(new Rect(20, 95, 100, 30), "refresh token")) RefreshToken();
    }

    // this sets the token cache field by fetching the firebase token.
    // should be called whenever first logged in or auth info has changed.
    public void RefreshToken()
    {
        GetUserToken().ContinueWith((task) => {
            Debug.Log("refreshed token");
            token = task.Result;
        });
    }

    public async Task<string> GetUserToken()
    {

        FirebaseUser user = auth.CurrentUser;
        string result = await user.TokenAsync(true);

        return result;
    }
    

    #region SIGNOUT
    public void SignOut()
    {
        if (auth.CurrentUser != null) { auth.SignOut(); }
    }
    #endregion
    #region SIGNIN
    private IEnumerator LoginLogic(string email, string password)
    {
        Credential credential = EmailAuthProvider.GetCredential(email, password);
        var loginTask = auth.SignInWithCredentialAsync(credential);

        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            FirebaseException firevaseException = (FirebaseException)loginTask.Exception.GetBaseException();
            AuthError error = (AuthError)firevaseException.ErrorCode;
            string output = "Unkown Error, Please Try Again";

            switch (error)
            {
                case AuthError.MissingEmail:
                    output = "Please Enter Your Email";
                    break;
                case AuthError.MissingPassword:
                    output = "Please Enter Your Password";
                    break;
                case AuthError.InvalidEmail:
                    output = "Invalid Email";
                    break;
                case AuthError.WrongPassword:
                    output = "Wrond Password";
                    break;
                case AuthError.UserNotFound:
                    output = "Account Does Not Exist";
                    break;
            }
        }
        else
        {
            Debug.Log($"signed in as {auth.CurrentUser.Email}");
        }
    }
    #endregion
    #region REGISTER
    private IEnumerator RegisterLogic(string username, string email, string password)
    {
        if (username == "") { Debug.LogError("Please Enter A Username"); }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                FirebaseException firevaseException = (FirebaseException)registerTask.Exception.GetBaseException();
                AuthError error = (AuthError)firevaseException.ErrorCode;
                string output = "Unkown Error, Please Try Again";

                switch (error)
                {
                    case AuthError.InvalidEmail:
                        output = "Invalid Email";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        output = "Email Already In Use";
                        break;
                    case AuthError.WeakPassword:
                        output = "Weak Password";
                        break;
                    case AuthError.MissingEmail:
                        output = "Please Enter Your Email";
                        break;
                    case AuthError.MissingPassword:
                        output = "Please Enter Your Password";
                        break;
                }
            }
            else
            {
                UserProfile profile = new UserProfile
                {
                    DisplayName = username,
                    PhotoUrl = new System.Uri("https://t4.ftcdn.net/jpg/03/40/12/49/360_F_340124934_bz3pQTLrdFpH92ekknuaTHy8JuXgG7fi.jpg"),
                };

                var defaultUserTask = user.UpdateUserProfileAsync(profile);

                yield return new WaitUntil(predicate: () => defaultUserTask.IsCompleted);

                if (defaultUserTask.Exception != null)
                {
                    user.DeleteAsync();
                    FirebaseException firebaseException = (FirebaseException)defaultUserTask.Exception.GetBaseException();
                    AuthError error = (AuthError)firebaseException.ErrorCode;
                    string output = "Unkown Error, Please Try Again";

                    switch (error)
                    {
                        case AuthError.Cancelled:
                            output = "Update User Cancelled";
                            break;
                        case AuthError.SessionExpired:
                            output = "Session Expired";
                            break;

                    }
                }
                else
                {
                    Debug.Log($"Firebase User Created Successfully: {user.DisplayName} ({user.UserId})");
                    //StartCoroutine(SendVerificationEmail());
                }
            }
        }
    }
    #endregion
}
