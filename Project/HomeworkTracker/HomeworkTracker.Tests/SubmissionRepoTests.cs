using Xunit;
using HomeworkTracker.Models;
using HomeworkTracker.Repositories;
using System.Linq;

namespace HomeworkTracker.Tests;

public class SubmissionRepoTests
{
    // ==========================================
    // ТЕСТИ НА ДОДАВАННЯ (ADD)
    // ==========================================

    [Fact]
    public void Add_ShouldIncreaseCount_AndSetId()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { AssignmentId = 1, Content = "Домашка 1" });
        
        var result = repo.GetByAssignment(0).ToList();
        Assert.Single(result);
        Assert.Equal(1, result.First().Id);
    }

    [Fact]
    public void Add_MultipleItems_ShouldAssignUniqueIds()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { Content = "Перша" });
        repo.Add(new Submission { Content = "Друга" });

        var all = repo.GetByAssignment(0).ToList();
        Assert.Equal(2, all.Count);
        Assert.Equal(1, all[0].Id);
        Assert.Equal(2, all[1].Id);
    }

    // ==========================================
    // ТЕСТИ НА ПОШУК (GET BY ID)
    // ==========================================

    [Fact]
    public void GetById_ExistingId_ShouldReturnCorrectSubmission()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { Content = "Шукай мене" });

        var result = repo.GetById(1);
        Assert.NotNull(result);
        Assert.Equal("Шукай мене", result.Content);
    }

    [Fact]
    public void GetById_NonExistingId_ShouldReturnNull()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { Content = "Тест" });

        var result = repo.GetById(999);
        Assert.Null(result);
    }

    // ==========================================
    // ТЕСТИ НА ФІЛЬТРАЦІЮ (GET BY ASSIGNMENT)
    // ==========================================

    [Fact]
    public void GetByAssignment_SpecificId_ShouldReturnOnlyMatching()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { AssignmentId = 5, Content = "Математика" });
        repo.Add(new Submission { AssignmentId = 5, Content = "Математика 2" });
        repo.Add(new Submission { AssignmentId = 8, Content = "Фізика" });

        var mathResults = repo.GetByAssignment(5).ToList();
        Assert.Equal(2, mathResults.Count);
        Assert.All(mathResults, s => Assert.Equal(5, s.AssignmentId));
    }

    [Fact]
    public void GetByAssignment_Zero_ShouldReturnAllSubmissions()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { AssignmentId = 1 });
        repo.Add(new Submission { AssignmentId = 2 });

        var allResults = repo.GetByAssignment(0).ToList();
        Assert.Equal(2, allResults.Count);
    }

    [Fact]
    public void GetByAssignment_NoMatchingId_ShouldReturnEmptyList()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { AssignmentId = 1 });

        var results = repo.GetByAssignment(99).ToList();
        Assert.Empty(results);
    }

    // ==========================================
    // ТЕСТИ НА ОНОВЛЕННЯ (UPDATE)
    // ==========================================

    [Fact]
    public void Update_ExistingSubmission_ShouldChangeStatusAndGrade()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { Content = "Початкова", Status = "Нова" });

        repo.Update(new Submission { Id = 1, Grade = 100, Status = "Перевірено" });
        
        var result = repo.GetById(1);
        Assert.Equal(100, result!.Grade);
        Assert.Equal("Перевірено", result!.Status);
    }

    [Fact]
    public void Update_NonExistingSubmission_ShouldNotThrowException()
    {
        var repo = new SubmissionRepo();
        var exception = Record.Exception(() => 
            repo.Update(new Submission { Id = 999, Grade = 50 })
        );

        Assert.Null(exception);
    }

    [Fact]
    public void Update_ShouldNotChangeOtherProperties()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { AssignmentId = 3, Content = "Не чіпай текст" });

        repo.Update(new Submission { Id = 1, Grade = 85, Status = "Ок" });
        
        var result = repo.GetById(1);
        Assert.Equal("Не чіпай текст", result!.Content);
        Assert.Equal(3, result.AssignmentId);
    }
    
    // ==========================================
    // ДОДАТКОВІ ТЕСТИ (РОЗШИРЕНЕ ПОКРИТТЯ)
    // ==========================================

    [Fact]
    public void Add_SubmissionWithEmptyContent_ShouldSaveCorrectly()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { Content = "" });
        var result = repo.GetById(1);
        Assert.Equal("", result!.Content);
    }

    [Fact]
    public void Add_SubmissionWithNullContent_ShouldSaveCorrectly()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { Content = null! });
        var result = repo.GetById(1);
        Assert.Null(result!.Content);
    }

    [Fact]
    public void Add_SubmissionWithMaxIntAssignmentId_ShouldSave()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { AssignmentId = int.MaxValue, Content = "Максимальний ID" });
        var result = repo.GetByAssignment(int.MaxValue).ToList();
        Assert.Single(result);
    }

    [Fact]
    public void Add_SubmissionWithNegativeAssignmentId_ShouldSave()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { AssignmentId = -5, Content = "Від'ємний ID" });
        var result = repo.GetByAssignment(-5).ToList();
        Assert.Single(result);
    }

    [Fact]
    public void Add_FiveSubmissions_ShouldHaveCountFive()
    {
        var repo = new SubmissionRepo();
        for (int i = 0; i < 5; i++)
        {
            repo.Add(new Submission { Content = $"Робота {i}" });
        }
        var all = repo.GetByAssignment(0).ToList();
        Assert.Equal(5, all.Count);
    }

    [Fact]
    public void GetById_NegativeId_ShouldReturnNull()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { Content = "Тест" });
        var result = repo.GetById(-1);
        Assert.Null(result);
    }

    [Fact]
    public void GetById_ZeroId_ShouldReturnNull()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { Content = "Тест" });
        var result = repo.GetById(0);
        Assert.Null(result);
    }

    [Fact]
    public void GetByAssignment_NegativeId_ShouldReturnEmptyList()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { AssignmentId = 1 });
        var results = repo.GetByAssignment(-10).ToList();
        Assert.Empty(results);
    }

    [Fact]
    public void GetByAssignment_ShouldNotReturnOtherAssignments()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { AssignmentId = 1, Content = "Завдання 1" });
        repo.Add(new Submission { AssignmentId = 2, Content = "Завдання 2" });
        
        var results = repo.GetByAssignment(1).ToList();
        Assert.DoesNotContain(results, s => s.AssignmentId == 2);
    }

    [Fact]
    public void Update_GradeToZero_ShouldSaveCorrectly()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { Grade = 50 });
        repo.Update(new Submission { Id = 1, Grade = 0 });
        var result = repo.GetById(1);
        Assert.Equal(0, result!.Grade);
    }

    [Fact]
    public void Update_GradeToMaxInt_ShouldSaveCorrectly()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { Grade = 10 });
        repo.Update(new Submission { Id = 1, Grade = int.MaxValue }); 
        var result = repo.GetById(1);
        Assert.Equal(int.MaxValue, result!.Grade);
    }

    [Fact]
    public void Update_StatusToEmptyString_ShouldSaveCorrectly()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { Status = "Нова" });
        repo.Update(new Submission { Id = 1, Status = "" }); 
        var result = repo.GetById(1);
        Assert.Equal("", result!.Status);
    }

    [Fact]
    public void Update_StatusToNull_ShouldSaveCorrectly()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { Status = "Нова" });
        repo.Update(new Submission { Id = 1, Status = null! }); 
        var result = repo.GetById(1);
        Assert.Null(result!.Status);
    }

    [Fact]
    public void Update_WithSameValues_ShouldNotThrowException()
    {
        var repo = new SubmissionRepo();
        var sub = new Submission { Content = "Текст", Status = "Нова", Grade = 100 };
        repo.Add(sub);
        
        var exception = Record.Exception(() => repo.Update(new Submission { Id = 1, Content = "Текст", Status = "Нова", Grade = 100 }));
        Assert.Null(exception);
    }

    [Fact]
    public void GetByAssignment_MultipleMatches_ShouldReturnAll()
    {
        var repo = new SubmissionRepo();
        repo.Add(new Submission { AssignmentId = 3 });
        repo.Add(new Submission { AssignmentId = 3 });
        repo.Add(new Submission { AssignmentId = 3 });
        
        var results = repo.GetByAssignment(3).ToList();
        Assert.Equal(3, results.Count);
    }
}