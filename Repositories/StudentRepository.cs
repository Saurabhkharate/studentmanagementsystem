using Microsoft.EntityFrameworkCore;
using studentmanagementsystem.DatabaseContext;
using studentmanagementsystem.Interface;
using studentmanagementsystem.Models;

namespace studentmanagementsystem.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDatabaseContext dbContext;

        public StudentRepository(AppDatabaseContext context)
        {
            dbContext = context;
        }

        public async Task<List<StudentInfo>> GetAll(int page, int pageSize)
        {
            return await dbContext.Students
                .Include(x => x.Country)
                .Include(x => x.State)
                .Include(x => x.City)
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> TotalCount()
        {
            return await dbContext.Students.CountAsync();
        }

        public async Task<StudentInfo?> GetById(int id)
        {
            return await dbContext.Students.FindAsync(id);
        }

        public async Task<StudentInfo> SaveStudent(StudentInfo student)
        {
            await dbContext.Students.AddAsync(student);
            return student;
        }

        public async Task<StudentInfo> UpdateStudent(StudentInfo student)
        {
            dbContext.Students.Update(student);
            return student;
        }

        public async Task<StudentInfo> DeleteStudent(int id, bool status)
        {
            StudentInfo student = await dbContext.Students.FindAsync(id);
            student.Status = status; 
            return student;
        }

        public async Task<bool> Save()
        {
            await dbContext.SaveChangesAsync();
            return true;
        }
    }

}
