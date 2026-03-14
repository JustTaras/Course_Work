using HomeworkTracker.Models;
using HomeworkTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeworkTracker.Services
{
    public class AssignmentService
    {
        private readonly AppDbContext _context;

        public AssignmentService(AppDbContext context)
        {
            _context = context;
        }

        public List<Assignment> GetList()
        {
            return _context.Assignments.Include(a => a.Group).ToList();
        }

        public void AddAssignment(Assignment assignment)
        {
            _context.Assignments.Add(assignment);
            _context.SaveChanges();
        }
    }
}