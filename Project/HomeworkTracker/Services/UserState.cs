namespace HomeworkTracker.Services;

public class UserState
{
    // Ролі: "Guest" (Гість), "Teacher" (Викладач), "Student" (Студент)
    public string Role { get; private set; } = "Guest"; 

    public event Action? OnChange;

    public void SetRole(string role)
    {
        Role = role;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}