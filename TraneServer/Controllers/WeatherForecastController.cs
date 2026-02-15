using Firebase.Auth;

using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
namespace TraneServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TraneServerController : ControllerBase
    {
        private readonly IConfiguration _config;
        FirebaseAuthLink _authLink;
   [HttpPost("auth-anonim")] 
        public async Task<IActionResult> SignAuthAnonim()
        {
            try
            {
                string firebaseApiKey = _config["FirebaseApiKey"];

                // ÈÑÏÎËÜÇÓÅÌ åãî
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(firebaseApiKey));
                var auth = await authProvider.SignInAnonymouslyAsync();
                return Ok(new {authcode = auth.User.LocalId,refreshentoken = auth.RefreshToken,firebasetoken = auth.FirebaseToken});
            }
            catch (Exception ex) 
            {
                return BadRequest(new {error = ex.Message});
            }

        }
        [HttpPost("GetIdToken")]
        public async Task<IActionResult> GetIdToken([FromBody] FirebaseAuthAnonim authin)
        {
            try
            {
                var auth = new FirebaseAuth
                {
                    RefreshToken = authin.RefreshToken,
                    FirebaseToken = authin.FirebaseToken,
                    User = new User { LocalId = authin.UserId }
                };

                if (authin.RefreshToken != null && authin.UserId != null)
                {

                    string firebaseApiKey = _config["FirebaseApiKey"];

                    // ÈÑÏÎËÜÇÓÅÌ åãî
                    var authProvider = new FirebaseAuthProvider(new FirebaseConfig(firebaseApiKey));
                    _authLink = await authProvider.RefreshAuthAsync(auth);
                    return Ok(new { success = true, FirebaseToken = _authLink.FirebaseToken });
                }
                else
                {
                    return BadRequest(new { success = false, error = "Îøèáêà íóëë" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
    public class FirebaseAuthAnonim
    {
        public string RefreshToken { get; set; }
        public string UserId { get; set; }
        public string FirebaseToken { get; set; }
    }
}

