using studentmanagementsystem.Models;

namespace studentmanagementsystem.Interface
{
    public interface IStudentRepository
    {
        public Task<List<StudentInfo>> GetAll(int page, int pageSize);
        public Task<StudentInfo> GetById(int id);
        public Task<StudentInfo> SaveStudent(StudentInfo student);
        public Task<StudentInfo> UpdateStudent(StudentInfo student);
        public Task<StudentInfo> DeleteStudent(int id, bool status);
        public Task<int> TotalCount();
        public Task<bool> Save();
    }
}
