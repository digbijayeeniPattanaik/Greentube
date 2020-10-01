# Greentube

The web application is created using .net core 3.1.

## API to use Forgot Password and Reset Password

### Users API

- POST - ​/Users​/forgotPassword - Forgot password
- POST - ​/Users​/resetPassword​/{token} - Reset password

[![UsersAPI.png](https://i.postimg.cc/0NThJ17r/UsersAPI.png)](https://postimg.cc/cKcFVj9N)

3.	If time permits: what possible vulnerabilities to you see in this service (i.e. do you see any potential of abuse by callers of the service)? (conceptually, not in your specific implementation). If yes, what would you do / change to prevent such abuse?

  -	It would be nice to have a ResetCounter for the user. The counter will be incremented every time the ResetUrl is being called.
  We can have a threshold for the counter which would restrict the user to call it more than the threshhold.
  After that the ResetUrl will be invalid and we can request the user to do Admin reset or Multifactor authentication.
  -	Whenever the user is calling Forgot Password – a ResetFlag should be set to false and it will only set to true once it is reset.
  -	I have added a Reset code to the token to verify its uniqueness.
  -	ResetCodeTime should be set when a successful link is sent to the user in Forgot Password endpoint.
  -	The email address in Forgot password should be validated with DB value if the user exists.
  -	The token email address needs to be validated in Reset Password.
  - Cover more Unit Tests
