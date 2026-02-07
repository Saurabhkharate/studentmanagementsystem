namespace studentmanagementsystem.Helpers
{
    public static class FileHelper
    {
        public static async Task<string?> SaveImageAsync(IFormFile file, string webRootPath)
        {
            if (file == null || file.Length == 0)
                return null;

            // allowed extensions
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(ext))
                throw new Exception("Only jpg, jpeg, png files allowed");

            // create uploads folder
            string uploadsFolder = Path.Combine(webRootPath, "Uploads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // unique filename
            string fileName = Guid.NewGuid().ToString() + ext;
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/Uploads/" + fileName;
        }
    }
}
