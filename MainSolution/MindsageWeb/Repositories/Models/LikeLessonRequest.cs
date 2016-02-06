using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories.Models
{
    /// <summary>
    /// ข้อมูลการ like lesson
    /// </summary>
    public class LikeLessonRequest
    {
        #region Properties

        /// <summary>
        /// รหัส course
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// รหัส lesson
        /// </summary>
        public string LessonId { get; set; }

        /// <summary>
        /// ชื่อบัญชีผู้ใช้ที่ดำเนินการ
        /// </summary>
        public string UserProfileId { get; set; }

        #endregion Properties
    }
}
