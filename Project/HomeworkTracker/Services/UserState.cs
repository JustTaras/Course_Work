namespace HomeworkTracker.Services;

public class UserState
{
    // Ролі: "Guest" (Гість), "Teacher" (Викладач), "Student" (Студент)
    public string Role { get; private set; } = "Guest";
    public int CurrentUserId { get; private set; }
    public string CurrentUserName { get; private set; } = string.Empty;

    public bool IsLoggedIn => Role != "Guest";

    public event Action? OnChange;

    public void Login(string role, int userId, string userName)
    {
        Role = role;
        CurrentUserId = userId;
        CurrentUserName = userName;
        NotifyStateChanged();
    }

    public void Logout()
    {
        Role = "Guest";
        CurrentUserId = 0;
        CurrentUserName = string.Empty;
        NotifyStateChanged();
    }

    // Залишаємо для зворотної сумісності
    public void SetRole(string role)
    {
        Role = role;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}