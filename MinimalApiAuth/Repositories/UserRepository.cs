namespace MinimalApiAuth
{
  public static class UserRepository
  {
    public static User Get(string username, string password)
    {
      var users = new List<User> {
        new User { Id = 1, Username = "batman", Password = "robin"},
        new User { Id = 2, Username = "robin", Password = "batman"},
      };
      return users.Where(x =>
        x.Username.ToLower() == username.ToLower()
        && x.Password.ToLower() == password.ToLower()
      ).First();
    }

  }
}