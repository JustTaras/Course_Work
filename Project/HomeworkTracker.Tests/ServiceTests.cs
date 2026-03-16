using HomeworkTracker.Models;
using HomeworkTracker.Repositories;
using HomeworkTracker.Services;
using Moq;
using Xunit;

namespace HomeworkTracker.Tests;

public class ServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUoW;
    private readonly AssignmentService _assignmentService;
    private readonly SubmissionService _submissionService;

    public ServiceTests()
    {
        _mockUoW = new Mock<IUnitOfWork>();
        _mockUoW.Setup(u => u.Assignments).Returns(new Mock<IAssignmentRepository>().Object);
        _mockUoW.Setup(u => u.Submissions).Returns(new Mock<ISubmissionRepository>().Object);
        _assignmentService = new AssignmentService(_mockUoW.Object);
        _submissionService = new SubmissionService(_mockUoW.Object);
    }

    // --- ASSIGNMENT SERVICE TESTS (10 tests) ---
    [Fact] public async Task CreateAssignment_CallsAddAssignmentAsync() {
        var mockRepo = new Mock<IAssignmentRepository>();
        _mockUoW.Setup(u => u.Assignments).Returns(mockRepo.Object);
        await _assignmentService.CreateAssignment("T", "D", DateTime.Now, 1, 1);
        mockRepo.Verify(r => r.AddAssignmentAsync(It.IsAny<Assignment>()), Times.Once);
    }

    [Fact] public async Task CreateAssignment_CallsCommitAsync() {
        await _assignmentService.CreateAssignment("T", "D", DateTime.Now, 1, 1);
        _mockUoW.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact] public async Task CreateAssignment_ReturnsCreatedAssignment() {
        var res = await _assignmentService.CreateAssignment("T", "D", DateTime.Now, 1, 1);
        Assert.NotNull(res);
        Assert.Equal("T", res.Title);
    }

    [Fact] public async Task CreateAssignment_AssignsCorrectGroupId() {
        var res = await _assignmentService.CreateAssignment("T", "D", DateTime.Now, 42, 1);
        Assert.Equal(42, res.GroupId);
    }

    [Fact] public async Task CreateAssignment_AssignsCorrectTeacherId() {
        var res = await _assignmentService.CreateAssignment("T", "D", DateTime.Now, 1, 99);
        Assert.Equal(99, res.TeacherId);
    }

    [Fact] public async Task GetActiveAssignments_ReturnsOnlyFutureDeadlines() {
        var mockRepo = new Mock<IAssignmentRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Assignment> {
            new Assignment { Id = 1, Deadline = DateTime.Now.AddDays(1) },
            new Assignment { Id = 2, Deadline = DateTime.Now.AddDays(-1) }
        });
        _mockUoW.Setup(u => u.Assignments).Returns(mockRepo.Object);
        
        var res = await _assignmentService.GetActiveAssignments();
        Assert.Single(res);
        Assert.Equal(1, res.First().Id);
    }

    [Fact] public async Task GetActiveAssignments_ReturnsEmpty_IfAllExpired() {
        var mockRepo = new Mock<IAssignmentRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Assignment> {
            new Assignment { Deadline = DateTime.Now.AddDays(-1) }
        });
        _mockUoW.Setup(u => u.Assignments).Returns(mockRepo.Object);
        var res = await _assignmentService.GetActiveAssignments();
        Assert.Empty(res);
    }

    [Fact] public async Task GetActiveAssignments_HandlesEmptyList() {
        var mockRepo = new Mock<IAssignmentRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Assignment>());
        _mockUoW.Setup(u => u.Assignments).Returns(mockRepo.Object);
        var res = await _assignmentService.GetActiveAssignments();
        Assert.Empty(res);
    }

    [Fact] public async Task GetActiveAssignments_CallsGetAllAsync() {
        var mockRepo = new Mock<IAssignmentRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Assignment>());
        _mockUoW.Setup(u => u.Assignments).Returns(mockRepo.Object);
        await _assignmentService.GetActiveAssignments();
        mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact] public async Task CreateAssignment_DoesNotThrowOnPastDeadline() {
        var exception = await Record.ExceptionAsync(() => _assignmentService.CreateAssignment("T", "D", DateTime.Now.AddDays(-5), 1, 1));
        Assert.Null(exception);
    }

    // --- SUBMISSION SERVICE TESTS (10 tests) ---
    [Fact] public async Task Submit_CallsAddSubmissionAsync() {
        var mockRepo = new Mock<ISubmissionRepository>();
        _mockUoW.Setup(u => u.Submissions).Returns(mockRepo.Object);
        await _submissionService.Submit(1, 1, "Content");
        mockRepo.Verify(r => r.AddSubmissionAsync(It.IsAny<Submission>()), Times.Once);
    }

    [Fact] public async Task Submit_CallsCommitAsync() {
        await _submissionService.Submit(1, 1, "Content");
        _mockUoW.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact] public async Task Submit_SetsStatusToSubmitted() {
        var res = await _submissionService.Submit(1, 1, "C");
        Assert.Equal("Submitted", res.Status);
    }

    [Fact] public async Task Submit_SetsCurrentDate() {
        var res = await _submissionService.Submit(1, 1, "C");
        Assert.True((DateTime.Now - res.SubmitDate).TotalSeconds < 5);
    }

    [Fact] public async Task Submit_ReturnsCreatedSubmission() {
        var res = await _submissionService.Submit(5, 10, "Code");
        Assert.Equal(5, res.AssignmentId);
        Assert.Equal(10, res.StudentId);
    }

    [Fact] public async Task Grade_FindsSubmissionById() {
        var mockRepo = new Mock<ISubmissionRepository>();
        _mockUoW.Setup(u => u.Submissions).Returns(mockRepo.Object);
        await _submissionService.Grade(99, 100);
        mockRepo.Verify(r => r.GetByIdAsync(99), Times.Once);
    }

    [Fact] public async Task Grade_SetsGradeIfFound() {
        var sub = new Submission { Id = 1 };
        var mockRepo = new Mock<ISubmissionRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(sub);
        _mockUoW.Setup(u => u.Submissions).Returns(mockRepo.Object);
        
        await _submissionService.Grade(1, 95);
        Assert.Equal(95, sub.Grade);
    }

    [Fact] public async Task Grade_ChangesStatusIfFound() {
        var sub = new Submission { Id = 1, Status = "Submitted" };
        var mockRepo = new Mock<ISubmissionRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(sub);
        _mockUoW.Setup(u => u.Submissions).Returns(mockRepo.Object);
        
        await _submissionService.Grade(1, 95);
        Assert.Equal("Graded", sub.Status);
    }

    [Fact] public async Task Grade_CallsCommitIfFound() {
        var sub = new Submission { Id = 1 };
        var mockRepo = new Mock<ISubmissionRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(sub);
        _mockUoW.Setup(u => u.Submissions).Returns(mockRepo.Object);
        
        await _submissionService.Grade(1, 95);
        _mockUoW.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact] public async Task Grade_DoesNotCallCommitIfNotFound() {
        var mockRepo = new Mock<ISubmissionRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Submission?)null);
        _mockUoW.Setup(u => u.Submissions).Returns(mockRepo.Object);
        
        await _submissionService.Grade(1, 95);
        _mockUoW.Verify(u => u.CommitAsync(), Times.Never); // Тут Times.Never, бо UoW викликався в конструкторі
    }

}