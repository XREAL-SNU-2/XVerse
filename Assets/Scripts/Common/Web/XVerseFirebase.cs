using Firebase;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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
        //StartCoroutine(CheckAutoLogin());

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

    private string registerOutputText = "see outputs here";

    private void OnGUI()
    {
        GUI.Label(new Rect(150, 50, 300, 50), registerOutputText);
        if (GUI.Button(new Rect(20, 20, 100, 20), "register")) StartCoroutine(RegisterLogic(Username, Email, Pass));
        if (GUI.Button(new Rect(20, 45, 100, 20), "signOut")) StartCoroutine("SignOut");
        if (GUI.Button(new Rect(20, 70, 100, 20), "signIn")) StartCoroutine(LoginLogic(Email, Pass));
        if (GUI.Button(new Rect(20, 95, 100, 30), "GET KEY"))
        {
            XVerseWeb.Start();
        }
    }
    
    public async Task<string> GetUserToken()
    {
        Debug.Log("entered GetUserToken");

        FirebaseUser user = auth.CurrentUser;
        await Task.Delay(2000);
        string result = await user.TokenAsync(true);
        Debug.Log("exiting GetUserToken");

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
            registerOutputText = output;
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
        if (username == "") { registerOutputText = "Please Enter A Username"; }
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
                registerOutputText = output;
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
                    registerOutputText = output;
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
