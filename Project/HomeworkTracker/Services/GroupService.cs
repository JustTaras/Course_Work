using HomeworkTracker.Models;
using HomeworkTracker.Repositories;

namespace HomeworkTracker.Services;

public class GroupService
{
    private readonly IUnitOfWork _unitOfWork;

    public GroupService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Group> CreateGroup(string name)
    {
        var group = new Group { Name = name };
        await _unitOfWork.Groups.AddGroupAsync(group);
        await _unitOfWork.CommitAsync();
        return group;
    }

    public async Task<List<Group>> GetAllGroups()
    {
        return await _unitOfWork.Groups.GetAllAsync();
    }

    public async Task<Group?> GetGroupById(int id)
    {
        return await _unitOfWork.Groups.GetByIdAsync(id);
    }

    public async Task AddStudentToGroup(int groupId, Student student)
    {
        var group = await _unitOfWork.Groups.GetByIdAsync(groupId);
        if (group == null) return;

        group.AddStudent(student);
        await _unitOfWork.CommitAsync();
    }

    public async Task<List<Assignment>> GetAssignmentsForGroup(int groupId)
    {
        return await _unitOfWork.Assignments.GetByGroupIdAsync(groupId);
    }
}
