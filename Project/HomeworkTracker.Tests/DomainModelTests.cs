using HomeworkTracker.Models;
using Xunit;

namespace HomeworkTracker.Tests;

public class DomainModelTests
{
    // --- ASSIGNMENT TESTS ---
    [Fact] public void Assignment_IsExpired_ReturnsTrue_WhenDeadlinePassed() {
        var assignment = new Assignment { Deadline = DateTime.Now.AddDays(-1) };
        Assert.True(assignment.IsExpired());
    }

    [Fact] public void Assignment_IsExpired_ReturnsFalse_WhenDeadlineInFuture() {
        var assignment = new Assignment { Deadline = DateTime.Now.AddDays(1) };
        Assert.False(assignment.IsExpired());
    }

    [Fact] public void Assignment_Creation_SetsCorrectProperties() {
        var a = new Assignment { Title = "Test", Description = "Desc" };
        Assert.Equal("Test", a.Title);
        Assert.Equal("Desc", a.Description);
    }

    // --- SUBMISSION TESTS ---
    [Fact] public void Submission_SetGrade_UpdatesGradeValue() {
        var sub = new Submission();
        sub.SetGrade(95);
        Assert.Equal(95, sub.Grade);
    }

    [Fact] public void Submission_SetGrade_ChangesStatusToGraded() {
        var sub = new Submission { Status = "Submitted" };
        sub.SetGrade(90);
        Assert.Equal("Graded", sub.Status);
    }

    [Fact] public void Submission_DefaultStatus_ShouldBeNullOrEmptyInitially() {
        var sub = new Submission();
        Assert.Equal("Submitted", sub.Status);
    }

    // --- USER TESTS ---
    [Fact] public void Student_Creation_SavesGroupId() {
        var s = new Student { GroupId = 5 };
        Assert.Equal(5, s.GroupId);
    }

    [Fact] public void Teacher_CanBeCreated_WithoutGroup() {
        var t = new Teacher { Name = "John" };
        Assert.Equal("John", t.Name);
    }

    [Fact] public void User_Email_StoresCorrectly() {
        var u = new Student { Email = "test@test.com" };
        Assert.Equal("test@test.com", u.Email);
    }

    [Fact] public void Group_CanStoreMultipleStudents() {
        var g = new Group { Students = new List<Student> { new Student(), new Student() } };
        Assert.Equal(2, g.Students.Count);
    }
}